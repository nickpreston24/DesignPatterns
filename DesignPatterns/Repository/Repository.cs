using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DesignPatterns
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected IDbConnection database;
        protected List<TEntity> collection;

        protected Repository(string connectionString)
        {
            database = new SqlConnection(connectionString);
        }

        protected Repository(IDbConnection database)
        {
            this.database = database;
        }

        public abstract IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        public abstract TEntity Get(int id);

        public abstract IEnumerable<TEntity> GetAll();

        public abstract TEntity Add(TEntity entity);

        public abstract IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

        public abstract TEntity Remove(TEntity entity);

        public abstract IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities);


        /// <summary>
        /// Todo: Refactor the following to be used by any class derived from this one.
        /// </summary>

        #region REFACTOR to New file
        //public SqlParameter[] GetSqlParams<T>()
        //{
        //    try
        //    {
        //        return typeof(T).GetProperties().Select(p =>
        //                        GetSqlParam(p.PropertyType, (Attribute.IsDefined(p, typeof(DatabaseAliasAttribute)))
        //                        ? p.GetCustomAttributes(inherit: false).OfType<DatabaseAliasAttribute>().Single().Alias
        //                        : p.Name)).ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

        //public SqlParameter GetSqlParam<T>(T propertyValue, string propertyName)
        //{
        //    try
        //    {
        //        var @switch = GetSqlParameterSwitcher(propertyName);

        //        SqlParameter parameter = (SqlParameter)@switch[typeof(T)].Value;
        //        parameter.Value = propertyValue;
        //        return parameter;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name}: {ex.ToString()}");
        //        throw;
        //    }
        //}

        //private static SqlParameter GetSqlParam(Type type, string propertyName)
        //{
        //    try
        //    {
        //        var @switch = GetSqlParameterSwitcher(propertyName);
        //        return @switch[type];
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name}: {ex.ToString()}");
        //        throw;
        //    }

        //}

        //private static IEnumerable<SqlParameter> GetSqlParamsFromTable(string connectionString, string tableName)
        //{
        //    if (string.IsNullOrWhiteSpace(tableName))
        //    {
        //        throw new Exception("No table name provided!");
        //    }

        //    List<SqlDbType> sqlDbTypes = new List<SqlDbType>();
        //    DataTable dt = new DataTable();

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = connection.CreateCommand())
        //        {
        //            connection.Open();
        //            cmd.CommandText = $"SET FMTONLY ON; select * from dbo.{tableName}; SET FMTONLY OFF";
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            dt = reader.GetSchemaTable();
        //        }
        //    }
        //    //
        //    /// Get the name of column, type of column and size and assign to tuples
        //    ////
        //    List<Tuple<string, SqlDbType, int>> sql_pre_param_list = new List<Tuple<string, SqlDbType, int>>();
        //    foreach (var row in dt.Rows.Cast<DataRow>())
        //    {
        //        SqlDbType type = (SqlDbType)(int.Parse(row["ProviderType"].ToString()));
        //        int size = int.Parse(row["ColumnSize"].ToString());
        //        string name = row["ColumnName"].ToString();
        //        //Debug.WriteLine($"type: {type.ToString()}; size: {size}");
        //        Tuple<string, SqlDbType, int> tuple = new Tuple<string, SqlDbType, int>(name, type, size);
        //        sql_pre_param_list.Add(tuple);
        //    }
        //    //
        //    /// Create full parameter list!
        //    ////
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    foreach (var item in sql_pre_param_list)
        //    {
        //        SqlParameter sp = new SqlParameter
        //        {
        //            ParameterName = item.Item1,
        //            SqlDbType = item.Item2,
        //            Size = item.Item3
        //        };
        //        sqlParams.Add(sp);
        //    }

        //    return sqlParams;
        //}

        //private static Dictionary<Type, SqlParameter> GetSqlParameterSwitcher(string propertyName)
        //{
        //    return new Dictionary<Type, SqlParameter>()
        //        {
        //            { typeof(bool), new SqlParameter(propertyName, SqlDbType.Bit) },
        //            { typeof(int), new SqlParameter(propertyName, SqlDbType.Int) },
        //            { typeof(double), new SqlParameter(propertyName, SqlDbType.Float) },
        //            { typeof(float), new SqlParameter(propertyName, SqlDbType.Float) },
        //            { typeof(decimal), new SqlParameter(propertyName, SqlDbType.Decimal) },
        //            { typeof(DateTime), new SqlParameter(propertyName, SqlDbType.DateTime) },
        //            { typeof(string), new SqlParameter(propertyName, SqlDbType.VarChar) },
        //        };
        //}

        //private static string CreateInsertQuery(IEnumerable<SqlParameter> sqlparameters, string tableName)
        //{
        //    if (string.IsNullOrWhiteSpace(tableName))
        //    {
        //        return null;
        //    }

        //    StringBuilder query = new StringBuilder($"INSERT INTO dbo.{tableName}\n");

        //    try
        //    {

        //        query.Append("(");

        //        foreach (var parameter in sqlparameters)
        //            query.Append($"{parameter.ParameterName}, ");

        //        query.Length -= 2; //removes extra comma
        //        query.Append(")\nVALUES\n(");

        //        foreach (var parameter in sqlparameters)
        //            query.Append($"@{parameter.ParameterName}, ");

        //        query.Length -= 2; //removes extra comma
        //        query.Append(")");

        //    }
        //    catch (Exception)
        //    {
        //        throw;

        //    }

        //    return query.ToString();

        //}

        //private static string CreateDeleteQuery(IEnumerable<SqlParameter> sqlparameters, string tableName)
        //{
        //    if (string.IsNullOrWhiteSpace(tableName))
        //    {
        //        throw new Exception("Table Name cannot be empty!");
        //    }

        //    StringBuilder query = new StringBuilder($"DELETE FROM dbo.{tableName}\n");

        //    query.Append("\nWHERE\n");

        //    foreach (var parameter in sqlparameters)
        //    {
        //        GetEqualsCase(parameter, query, SqlQueryOperator.AND);
        //    }

        //    query.Length -= 4; //removes extra AND

        //    return query.ToString();
        //}

        ////TODO: Finish implementing the cases using a switchionary (dictionary used as a switch statement)!
        //private static void GetEqualsCase(SqlParameter parameter, StringBuilder query, SqlQueryOperator op)
        //{
        //    switch (parameter.SqlDbType)
        //    {
        //        case SqlDbType.BigInt:
        //            break;
        //        case SqlDbType.Binary:
        //            break;
        //        case SqlDbType.Bit:
        //            break;
        //        case SqlDbType.Char:
        //            break;
        //        case SqlDbType.DateTime:
        //            break;
        //        case SqlDbType.Decimal:
        //            break;
        //        case SqlDbType.Float:
        //            break;
        //        case SqlDbType.Image:
        //            break;
        //        case SqlDbType.Int:
        //            query.Append($"{parameter.ParameterName} = {(int)parameter.Value} {op.ToString()} ");
        //            break;
        //        case SqlDbType.Money:
        //            break;
        //        case SqlDbType.NChar:
        //            break;
        //        case SqlDbType.NText:
        //            break;
        //        case SqlDbType.NVarChar:
        //            break;
        //        case SqlDbType.Real:
        //            break;
        //        case SqlDbType.UniqueIdentifier:
        //            break;
        //        case SqlDbType.SmallDateTime:
        //            break;
        //        case SqlDbType.SmallInt:
        //            break;
        //        case SqlDbType.SmallMoney:
        //            break;
        //        case SqlDbType.Text:
        //            break;
        //        case SqlDbType.Timestamp:
        //            break;
        //        case SqlDbType.TinyInt:
        //            break;
        //        case SqlDbType.VarBinary:
        //            break;
        //        case SqlDbType.VarChar:
        //            query.Append($"{parameter.ParameterName} = '{parameter.Value.ToString()}' {op.ToString()} ");
        //            break;
        //        case SqlDbType.Variant:
        //            break;
        //        case SqlDbType.Xml:
        //            break;
        //        case SqlDbType.Udt:
        //            break;
        //        case SqlDbType.Structured:
        //            break;
        //        case SqlDbType.Date:
        //            break;
        //        case SqlDbType.Time:
        //            break;
        //        case SqlDbType.DateTime2:
        //            break;
        //        case SqlDbType.DateTimeOffset:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //private static DataTable FindDuplicateRows(string connectionString, string tablename, IEnumerable<string> excludedColumns = null, int max_appearances_allowed = 1)
        //{
        //    SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(connectionString);
        //    string database = csb.InitialCatalog;
        //    string connStr = csb.ConnectionString;
        //    List<string> nonPKeyColumnNames = new List<string>();
        //    string sql_get_schema = $"SET FMTONLY ON; select * from [{database}].dbo.[{tablename}]; SET FMTONLY OFF";
        //    //
        //    /// GET SCHEMA and NON-UNIQUEIDENTIFIER COLUMN NAMES
        //    ////
        //    DataTable dtSchema = new DataTable($"{tablename} Schema");
        //    using (SqlConnection connection = new SqlConnection(connStr))
        //    {
        //        using (SqlCommand command = new SqlCommand(sql_get_schema, connection))
        //        {
        //            try
        //            {
        //                command.Connection.Open();
        //                SqlDataReader reader = command.ExecuteReader();
        //                dtSchema = reader.GetSchemaTable();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
        //            }
        //        }
        //    }
        //    foreach (var row in dtSchema.Rows.Cast<DataRow>())
        //    {
        //        SqlDbType type = (SqlDbType)(int.Parse(row["ProviderType"].ToString()));
        //        if (type != SqlDbType.UniqueIdentifier)
        //            nonPKeyColumnNames.Add(row["ColumnName"].ToString());
        //    }
        //    if (excludedColumns != null) nonPKeyColumnNames.RemoveAll(x => excludedColumns.Contains(x));
        //    //
        //    /// RETRIEVE DUPLICATES
        //    //// 
        //    DataTable dt = new DataTable($"{tablename} Duplicates");
        //    StringBuilder sql = new StringBuilder();
        //    try
        //    {
        //        List<string> correctedColumNames = new List<string>();
        //        foreach (var name in nonPKeyColumnNames)
        //        {
        //            if (!name.EndsWith(","))
        //                correctedColumNames.Add(name + ",");
        //            else correctedColumNames.Add(name);
        //        }
        //        correctedColumNames[correctedColumNames.Count - 1] = correctedColumNames.Last().Replace(",", " ").Trim();
        //        sql.Append("SELECT ");
        //        //Add Columns Names:
        //        foreach (string columnName in correctedColumNames)
        //            sql.Append(string.Format("{0} ", columnName));
        //        sql.Append(", count (*) as APPEARANCES ");
        //        sql.AppendLine(string.Format(" \nFROM {0}.dbo.{1}", database, tablename));
        //        sql.AppendLine(" Group By ");
        //        //Add Columns Names again:
        //        foreach (string columnName in correctedColumNames)
        //            sql.Append(string.Format("{0} ", columnName));
        //        sql.AppendLine(" having count (*) > " + max_appearances_allowed);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
        //    }
        //    using (SqlDataAdapter da = new SqlDataAdapter(sql.ToString(), connStr))
        //    {
        //        try
        //        {
        //            da.SelectCommand.CommandTimeout = 180;
        //            da.Fill(dt);
        //            if (dt.Rows.Count > max_appearances_allowed)
        //            {
        //                Debug.WriteLine(string.Format("--Number of Duplicate rows (including original) in table {0}.dbo.{1}: {2}", database, tablename, dt.Rows.Count));
        //                Debug.WriteLine(sql.ToString());
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
        //        }
        //    }
        //    return dt;
        //}

        //[AttributeUsage(AttributeTargets.Property)]
        //public class DatabaseAliasAttribute : Attribute
        //{
        //    public string Alias { get; set; }
        //    public DatabaseAliasAttribute(string alias)
        //    {
        //        Alias = alias;
        //    }
        //}
        #endregion Refactor

        #region Dispose
        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    database.Dispose();
                }

                isDisposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Repository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion Dispose

    }

    public enum SqlQueryOperator
    {
        AND,
        OR,
    }

    public enum ParameterCreationOption
    {
        FromTable,
        FromClass,
    }
}