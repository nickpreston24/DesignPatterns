using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Extensions
{
	public static partial class SqlExtensions
	{
		public static T ExecuteAndReadObject<T>(string connectionString, string storedProcedureName, CommandType commandType, Func<DbDataReader, T> objectMapper, params SqlParameter[] parameters)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
				{
					command.CommandType = commandType;
					foreach (SqlParameter parameter in parameters)
					{
						command.Parameters.Add(parameter);
					}

					connection.Open();
					return command.ExecuteAndReadObject(objectMapper);
				}
			}
		}

		public static T ReadObject<T>(this DbDataReader reader, Func<DbDataReader, T> objectMapper)
		{
			return reader.Read() ? objectMapper(reader) : default;
		}

		public static List<T> ReadList<T>(this DbDataReader reader, Func<DbDataReader, T> objectMapper)
		{
			List<T> list = new List<T>();

			while (reader.Read())
			{
				list.Add(objectMapper(reader));
			}

			return list;
		}

		public static T ExecuteAndReadObject<T>(this DbCommand command, Func<DbDataReader, T> objectMapper)
		{
			using (DbDataReader reader = command.ExecuteReader())
			{
				return reader.ReadObject(objectMapper);
			}
		}

		public static List<T> ExecuteAndReadList<T>(this DbCommand command, Func<DbDataReader, T> objectMapper)
		{
			using (DbDataReader reader = command.ExecuteReader())
			{
				return reader.ReadList(objectMapper);
			}
		}

		public static List<T> ExecuteAndReadList<T>(string connectionString, string storedProcedureName, CommandType commandType, Func<DbDataReader, T> objectMapper, params SqlParameter[] parameters)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
				{
					command.CommandType = commandType;
					foreach (SqlParameter parameter in parameters)
					{
						command.Parameters.Add(parameter);
					}

					connection.Open();
					return command.ExecuteAndReadList(objectMapper);
				}
			}
		}


		/*Reader*/

		public static DateTime GetDateTime(this DbDataReader reader, string name)
		{
			return reader.GetDateTime(reader.GetOrdinal(name));
		}

		public static int GetInt32(this DbDataReader reader, string name)
		{
			return reader.GetInt32(reader.GetOrdinal(name));
		}

		public static string GetString(this DbDataReader reader, string name)
		{
			return reader.GetString(reader.GetOrdinal(name));
		}

		public static bool HasColumn(this IDataRecord dr, string columnName)
		{
			for (int i = 0; i < dr.FieldCount; i++)
			{
				if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}

			return false;
		}

		public static T Field<T>(this DbDataReader reader, string fieldName = null) => reader.HasColumn(fieldName) ? (T)reader.GetValue(reader.GetOrdinal(fieldName)) : default;
	}
}


