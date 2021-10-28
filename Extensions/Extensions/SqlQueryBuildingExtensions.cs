using Dapper;
using Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Extensions
{
	public static partial class SqlExtensions
	{
		//private static SqlTransaction Execute(this SqlTransaction transaction, string sql, object paramSet = null)
		//{
		//	transaction?.Connection.Execute(sql, paramSet);
		//	return transaction;
		//}

		/// <summary>
		/// Builds a new table in the target database for type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public static SqlTransaction CreateTable<T>(this SqlTransaction transaction, bool isTempTable = false)
		{
			Type type = typeof(T);
			List<System.Reflection.PropertyInfo> props = type.GetProperties().ToList();
			string tableName = type.Name;
			string schemaName = "dbo";
			string query = new StringBuilder(
				$@"
				IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{schemaName}].[{tableName}]') AND type in (N'U'))
				BEGIN
				CREATE TABLE ")
				.Append($"{(!isTempTable ? tableName : $"#{tableName} ")} ( ")
				.Set(builder =>
				{
					foreach (System.Reflection.PropertyInfo field in props)
					{
						Type fieldType = field.PropertyType;
						Func<string, string> dbType = typeMap[fieldType];
						builder.Append($"[{field.Name}] {dbType}, ");
					}
				})
				.Set(builder => { builder.Length--; })
				.AppendLine(") END")
				.ToString();

			SqlConnection connection = transaction?.Connection;
			int? affectedRows = connection?.Execute(query, transaction: transaction);
			return transaction;
		}

		/// <summary>
		/// Chain Execution of an existing SQLConnection
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sql"></param>
		/// <returns></returns>
		public static SqlConnection ExecuteNonQuery(this SqlConnection connection, string sql)
		{
			using (SqlCommand command = new SqlCommand(sql, connection))
			{
				command.CommandText = command.CommandText.NoNoLock();
				if (connection.State == ConnectionState.Closed)
				{
					connection.Open();
				}
				command.ExecuteNonQuery();
			}

			return connection;
		}

		private static Dictionary<Type, Func<string, string>> typeMap = new Dictionary<Type, Func<string, string>>()
		{
			[typeof(byte)] = (_) => DbType.Byte.ToString(),
			[typeof(sbyte)] = (_) => DbType.SByte.ToString(),
			[typeof(short)] = (_) => DbType.Int16.ToString(),
			[typeof(ushort)] = (_) => DbType.UInt16.ToString(),
			[typeof(int)] = (_) => DbType.Int32.ToString(),
			[typeof(uint)] = (_) => DbType.UInt32.ToString(),
			[typeof(long)] = (_) => DbType.Int64.ToString(),
			[typeof(ulong)] = (_) => DbType.UInt64.ToString(),
			[typeof(float)] = (_) => DbType.Single.ToString(),
			[typeof(double)] = (_) => DbType.Double.ToString(),
			[typeof(decimal)] = (_) => DbType.Decimal.ToString(),
			[typeof(bool)] = (_) => DbType.Boolean.ToString(),
			[typeof(string)] = (size) => string.IsNullOrWhiteSpace(size) ? $"Varchar({size})" : "Varchar(50)",
			[typeof(char)] = (_) => DbType.StringFixedLength.ToString(),
			[typeof(Guid)] = (_) => DbType.Guid.ToString(),
			[typeof(DateTime)] = (_) => DbType.DateTime.ToString(),
			[typeof(DateTimeOffset)] = (_) => DbType.DateTimeOffset.ToString(),
			[typeof(byte[])] = (_) => DbType.Binary.ToString(),
			[typeof(byte?)] = (_) => DbType.Byte.ToString(),
			[typeof(sbyte?)] = (_) => DbType.SByte.ToString(),
			[typeof(short?)] = (_) => DbType.Int16.ToString(),
			[typeof(ushort?)] = (_) => DbType.UInt16.ToString(),
			[typeof(int?)] = (_) => DbType.Int32.ToString(),
			[typeof(uint?)] = (_) => DbType.UInt32.ToString(),
			[typeof(long?)] = (_) => DbType.Int64.ToString(),
			[typeof(ulong?)] = (_) => DbType.UInt64.ToString(),
			[typeof(float?)] = (_) => DbType.Single.ToString(),
			[typeof(double?)] = (_) => DbType.Double.ToString(),
			[typeof(decimal?)] = (_) => DbType.Decimal.ToString(),
			[typeof(bool?)] = (_) => DbType.Boolean.ToString(),
			[typeof(char?)] = (_) => DbType.StringFixedLength.ToString(),
			[typeof(Guid?)] = (_) => DbType.Guid.ToString(),
			[typeof(DateTime?)] = (_) => DbType.DateTime.ToString(),
			//  [typeof(DateTimeOffset?)] = (_) => DbType.DateTimeOffset.ToString(),
			//[typeof(System.Data.Linq.Binary)] = DbType.Binary, // fix: https://stackoverflow.com/questions/44291757/equivalent-system-data-linq-binary-in-net-core
		};
	}
}

