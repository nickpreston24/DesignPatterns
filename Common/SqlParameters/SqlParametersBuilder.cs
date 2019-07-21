using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Shared
{
    public class SqlUtilities
    {
        public static Dictionary<Type, Func<string, SqlParameter>> Parameters { get; }
            = new Dictionary<Type, Func<string, SqlParameter>>()
        {
            { typeof(bool), propertyName => new SqlParameter(propertyName, SqlDbType.Bit) },
            { typeof(int),  propertyName => new SqlParameter(propertyName, SqlDbType.Int) },
            { typeof(double), propertyName => new SqlParameter(propertyName, SqlDbType.Float) },
            { typeof(float), propertyName => new SqlParameter(propertyName, SqlDbType.Float) },
            { typeof(decimal), propertyName => new SqlParameter(propertyName, SqlDbType.Decimal) },
            { typeof(DateTime), propertyName => new SqlParameter(propertyName, SqlDbType.DateTime) },
            { typeof(string), propertyName => new SqlParameter(propertyName, SqlDbType.VarChar) },
        };
    }

    public class SqlParamsBuilder
    {
        public SqlParameter[] GetSqlParams<T>() => typeof(T)
                .GetProperties()
                .Select(property =>
                    GetSqlParam(property.PropertyType, (Attribute.IsDefined(property, typeof(DatabaseAliasAttribute)))
                    ? property.GetCustomAttributes(inherit: false)
                        .OfType<DatabaseAliasAttribute>()
                        .Single().Alias
                    : property.Name)).ToArray();

        public SqlParameter GetSqlParam<T>(T propertyValue, string propertyName)
        {
            var parameter = SqlUtilities.Parameters?[typeof(T)](propertyName);
            parameter.Value = propertyValue;
            return parameter;
        }

        private static SqlParameter GetSqlParam(Type type, string propertyName) => SqlUtilities.Parameters?[type](propertyName);

        private static ICollection<SqlParameter> GetSqlParamsFromTable(string connectionString, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new NullReferenceException(nameof(tableName) + " cannot be empty!");

            var dbTypes = new List<SqlDbType>();
            var table = new DataTable();

            using (var connection = new SqlConnection(connectionString))
            using (var cmd = connection.CreateCommand())
            {
                connection.Open();
                cmd.CommandText = $"SET FMTONLY ON; select * from dbo.{tableName}; SET FMTONLY OFF";
                var reader = cmd.ExecuteReader();
                table = reader.GetSchemaTable();
            }

            var before = new List<Tuple<string, SqlDbType, int>>();
            foreach (var row in table.Rows.Cast<DataRow>())
            {
                var dbType = (SqlDbType)(int.Parse(row["ProviderType"].ToString()));
                int size = int.Parse(row["ColumnSize"].ToString());
                string name = row["ColumnName"].ToString();

                var tuple = new Tuple<string, SqlDbType, int>(name, dbType, size);
                before.Add(tuple);
            }

            var sqlParams = new List<SqlParameter>();
            foreach (var item in before)
            {
                // TODO: Update Tuples to conform to C# 7.0
                var next = new SqlParameter
                {
                    ParameterName = item.Item1,
                    SqlDbType = item.Item2,
                    Size = item.Item3
                };

                sqlParams.Add(next);
            }

            return sqlParams;
        }

        private static string CreateInsertQuery(ICollection<SqlParameter> sqlparameters, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new NullReferenceException(nameof(tableName));

            var query = new StringBuilder($"INSERT INTO dbo.{tableName}\n");

            query.Append("(");

            foreach (var parameter in sqlparameters)
                query.Append($"{parameter.ParameterName}, ");

            query.Length -= 2; //removes extra comma
            query.Append(")\nVALUES\n(");

            foreach (var parameter in sqlparameters)
                query.Append($"@{parameter.ParameterName}, ");

            query.Length -= 2; //removes extra comma
            query.Append(")");

            return query.ToString();
        }

        private static string CreateDeleteQuery(ICollection<SqlParameter> sqlparameters, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new NullReferenceException(nameof(tableName) + " cannot be empty!");

            var query = new StringBuilder($"DELETE FROM dbo.{tableName}\n");

            query.Append("\nWHERE\n");

            foreach (var parameter in sqlparameters)
            {
                GetEqualsCase(parameter, query, SqlQueryOperator.AND);
            }

            query.Length -= 4; //removes extra AND

            return query.ToString();
        }

        private static DataTable FindDuplicateRows(string connectionString
            , string tablename
            , ICollection<string> excludedColumns = null
            , int limit = 1)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var nonPKeyColumnNames = new List<string>();

            string database = builder.InitialCatalog;
            string commandText = $"SET FMTONLY ON; select * from [{database}].dbo.[{tablename}]; SET FMTONLY OFF";

            //
            /// GET SCHEMA and NON-UNIQUEIDENTIFIER COLUMN NAMES
            ////

            var schema = new DataTable($"{tablename} Schema");

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(commandText, connection))
            {
                try
                {
                    command.Connection.Open();
                    var reader = command.ExecuteReader();
                    schema = reader.GetSchemaTable();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            foreach (var row in schema.Rows.Cast<DataRow>())
            {
                var type = (SqlDbType)int.Parse(row["ProviderType"].ToString());
                if (type != SqlDbType.UniqueIdentifier)
                    nonPKeyColumnNames.Add(row["ColumnName"].ToString());
            }

            if (excludedColumns != null)
                nonPKeyColumnNames
                    .RemoveAll(column => excludedColumns.Contains(column));

            //
            /// RETRIEVE DUPLICATES
            //// 

            var duplicates = new DataTable($"{tablename} Duplicates");
            var sql = new StringBuilder();
            var nextColumnNames = new List<string>();

            foreach (var name in nonPKeyColumnNames)
            {
                if (!name.EndsWith(","))
                    nextColumnNames.Add(name + ",");
                else nextColumnNames.Add(name);
            }

            nextColumnNames[nextColumnNames.Count - 1] = nextColumnNames
                .Last()
                .Replace(",", " ")
                .Trim();

            sql.Append("SELECT ");

            foreach (string columnName in nextColumnNames)
                sql.Append(string.Format("{0} ", columnName));

            sql.Append(", count (*) as APPEARANCES ");
            sql.AppendLine(string.Format(" \nFROM {0}.dbo.{1}", database, tablename));
            sql.AppendLine(" Group By ");

            //Add Columns Names again:
            foreach (string columnName in nextColumnNames)
                sql.Append(string.Format("{0} ", columnName));

            sql.AppendLine(" having count (*) > " + limit);

            using (var adapter = new SqlDataAdapter(sql.ToString(), connectionString))
            {
                adapter.SelectCommand.CommandTimeout = 180;
                adapter.Fill(duplicates);

                //string message = string.Format("--Number of Duplicate rows (including original) in table {0}.dbo.{1}: {2}", database, tablename, duplicates.Rows.Count);

                //if (duplicates.Rows.Count > limit)
                //{
                //Debug.WriteLine(message);
                //Debug.WriteLine(sql.ToString());                    
                //}
            }

            return duplicates;
        }

        //TODO: Finish implementing the cases using a indexer (dictionary used as a switch statement)!
        private static void GetEqualsCase(SqlParameter parameter, StringBuilder query, SqlQueryOperator sqlOperator)
        {
            switch (parameter.SqlDbType)
            {
                case SqlDbType.BigInt:
                    break;
                case SqlDbType.Binary:
                    break;
                case SqlDbType.Bit:
                    break;
                case SqlDbType.Char:
                    break;
                case SqlDbType.DateTime:
                    break;
                case SqlDbType.Decimal:
                    break;
                case SqlDbType.Float:
                    break;
                case SqlDbType.Image:
                    break;
                case SqlDbType.Int:
                    query.Append($"{parameter.ParameterName} = {(int)parameter.Value} {sqlOperator.ToString()} ");
                    break;
                case SqlDbType.Money:
                    break;
                case SqlDbType.NChar:
                    break;
                case SqlDbType.NText:
                    break;
                case SqlDbType.NVarChar:
                    break;
                case SqlDbType.Real:
                    break;
                case SqlDbType.UniqueIdentifier:
                    break;
                case SqlDbType.SmallDateTime:
                    break;
                case SqlDbType.SmallInt:
                    break;
                case SqlDbType.SmallMoney:
                    break;
                case SqlDbType.Text:
                    break;
                case SqlDbType.Timestamp:
                    break;
                case SqlDbType.TinyInt:
                    break;
                case SqlDbType.VarBinary:
                    break;
                case SqlDbType.VarChar:
                    query.Append($"{parameter.ParameterName} = '{parameter.Value.ToString()}' {sqlOperator.ToString()} ");
                    break;
                case SqlDbType.Variant:
                    break;
                case SqlDbType.Xml:
                    break;
                case SqlDbType.Udt:
                    break;
                case SqlDbType.Structured:
                    break;
                case SqlDbType.Date:
                    break;
                case SqlDbType.Time:
                    break;
                case SqlDbType.DateTime2:
                    break;
                case SqlDbType.DateTimeOffset:
                    break;
                default:
                    break;
            }
        }

    }
}