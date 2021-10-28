using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Extensions
{
	public static partial class DataTableExtensions
	{
		/// <summary>
		/// Convert Table To List of line items
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="table"></param>
		/// <param name="patch"></param>
		/// <returns></returns>
		public static List<T> ToList<T>(this DataTable table, Func<DataRow, T> patch)
		{
			if (table?.Rows?.Count == 0 || patch == null)
				return new List<T>(0);

			List<T> lines = new List<T>();

			List<DataRow> rows = table.Rows.Cast<DataRow>().ToList();

			foreach (DataRow row in rows)
			{
				lines.Add(patch(row));
			}

			return lines;
		}

		/// <summary>
		/// Converts a DataTable to a list with generic objects
		/// </summary>
		/// <typeparam name="T">Generic object</typeparam>
		/// <param name="table">DataTable</param>
		/// <returns>List with generic objects</returns>
		public static List<T> ToList<T>(this DataTable table) where T : class, new()
		{
			List<T> list = new List<T>();
			Type type = typeof(T);
			System.Reflection.PropertyInfo[] props = type.GetProperties();
			foreach (DataRow row in table.Rows.Cast<DataRow>().AsEnumerable() ?? Enumerable.Empty<DataRow>())
			{
				T item = new T();

				foreach (System.Reflection.PropertyInfo property in props)
				{
					System.Reflection.PropertyInfo propertyInfo = type.GetProperty(property.Name);
					object rowValue = row[property.Name];
					if (rowValue == DBNull.Value)
						propertyInfo.SetValue(item, null, null);
					else
						propertyInfo.SetValue(item, rowValue, null);
					//propertyInfo.SetValue(item, Convert.ChangeType(rowValue, propertyInfo.PropertyType), null);
				}

				list.Add(item);
			}

			return list;

		}

		/// <summary>
		/// Get Column Header Names from a given datatable instance
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public static List<string> GetHeaders(this DataTable table)
		{
			return table.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();
		}

		/// <summary>
		/// Sets the Column Headers according to priority (similar to Select id, age, name, * from XYZ)
		/// </summary>
		/// <param name="table">What do you think?</param>
		/// <param name="columnNames">The headers that show up first in the table</param>
		/// <returns></returns>
		public static DataTable SetColumnsOrder(this DataTable table, params string[] columnNames)
		{
			if (table == null)
				return new DataTable("EMPTY");

			int columnIndex = 0;
			foreach (string columnName in columnNames)
			{
				if (table.Columns[columnName] == null)
				{
					return table;
				}
				table.Columns[columnName].SetOrdinal(columnIndex);
				columnIndex++;
			}
			return table;
		}

		/// <summary>
		/// Remove a DataTable's column by name IFF it exists!
		/// </summary>
		/// <param name="table"></param>
		/// <param name="columnName"></param>
		public static DataTable RemoveColumn(this DataTable table, string columnName)
		{
			if (table.Columns.Contains(columnName))
				table.Columns.Remove(columnName);
			return table;
		}

		/// <summary>
		/// Convert a List of a class type to Datatable of same type-schema
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static DataTable ToDatatable<T>(this IEnumerable<T> collection)
		{
			if (collection is null || collection.Count() == 0)
			{
				return new DataTable();
			}

			DataTable dt = new DataTable();

			//Add headers:
			List<System.Reflection.PropertyInfo> properties = collection.First()
				 .GetType()
				 .GetProperties()
				 .ToList();

			properties.ForEach(prop => dt.Columns.Add(prop.Name, prop.PropertyType));

			//Add all the values as new DataRows:
			object[] values = new object[properties.Count];
			foreach (T item in collection ?? Enumerable.Empty<T>())
			{
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = properties[i].GetValue(item);
				}
				dt.Rows.Add(values);
			}

			return dt;
		}

		/// <summary>
		/// As Dynamic Enumerable
		/// Turns a datatable into a dynamic enumerable type
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
		{
			return table.Rows.Cast<DataRow>()
				.AsEnumerable()
				.Select(row => new DynamicRow(row));
		}

		private sealed class DynamicRow : DynamicObject
		{
			private readonly DataRow row;

			internal DynamicRow(DataRow row) { this.row = row; }

			// Interprets a member-access as an indexer-access on the 
			// contained DataRow.
			public override bool TryGetMember(GetMemberBinder binder, out object result)
			{
				bool retVal = row.Table.Columns.Contains(binder.Name);
				result = retVal ? row[binder.Name] : null;
				return retVal;
			}
		}

		/**** SQL To Datatable ***/
		/// <summary>
		/// Fill a given DataTable
		/// </summary>
		/// <param name="table"></param>
		/// <param name="connectionString"></param>
		/// <param name="selectQuery"></param>
		private static DataTable Fill(this DataTable table, string connectionString, string selectQuery)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or whitespace.", nameof(connectionString));
			}

			if (string.IsNullOrWhiteSpace(selectQuery))
			{
				throw new ArgumentException($"'{nameof(selectQuery)}' cannot be null or whitespace.", nameof(selectQuery));
			}

			if (!selectQuery.Contains("SELECT", StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException("Query must contain SELECT statement");
			}

			using (SqlDataAdapter fillerUpper = new SqlDataAdapter(selectQuery.ToString(), connectionString))
			{
				fillerUpper.SelectCommand.CommandTimeout = 180; //make into a Setting?
				fillerUpper.Fill(table);
			}

			return table;
		}

		private static DataTable Fill(this DataTable table, SqlConnection connection, string selectQuery)
		{
			if (connection is null)
			{
				throw new ArgumentNullException(nameof(connection));
			}

			if (string.IsNullOrWhiteSpace(selectQuery))
			{
				throw new ArgumentException($"'{nameof(selectQuery)}' cannot be null or whitespace.", nameof(selectQuery));
			}

			if (!selectQuery.Contains("SELECT", StringComparison.OrdinalIgnoreCase))
			{
				throw new InvalidOperationException("Query must contain SELECT statement");
			}

			if (connection.State != ConnectionState.Open)
				connection.Open();

			using (SqlDataAdapter fillerUpper = new SqlDataAdapter(selectQuery.ToString(), connection))
			{
				fillerUpper.SelectCommand.CommandTimeout = 180; //make into a Setting?
				fillerUpper.Fill(table);
			}

			return table;
		}


		/// <summary>
		/// String.Contains w/ IgnoreCase, etc: https://stackoverflow.com/questions/444798/case-insensitive-containsstring
		/// TODO: Move to RegexExtensions or own class.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="check"></param>
		/// <param name="compare"></param>
		/// <returns></returns>
		public static bool Contains(this string source, string check, StringComparison compare)
		{
			return source?.IndexOf(check, compare) >= 0;
		}

		/// <summary>
		/// Binds the results of a SELECT Query to a given DataGridView
		/// </summary>
		/// <param name="view"></param>
		/// <param name="connectionString"></param>
		/// <param name="selectQuery"></param>
		/// <returns></returns>
		public static DataGridView Bind(this DataGridView view, SqlConnection connection, string selectQuery)
		{
			// Populate a new data table and bind it to the BindingSource.
			DataTable table = new DataTable
			{
				Locale = CultureInfo.InvariantCulture
			}
			.Fill(connection, selectQuery);

			Action uiUpdates = () =>
			{
				view.DataSource = table;
				// Resize the DataGridView columns to fit the newly loaded content.
				view.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
			};

			view.Update(uiUpdates);

			return view;
		}

		public static DataGridView Bind(this DataGridView view, DataTable table)
		{
			view.Update(() =>
			{
				view.DataSource = table;
			});

			return view;
		}

		public static DataGridView Bind<TModel>(this DataGridView view, IEnumerable<TModel> collection)
		{
			DataTable table = collection.ToDatatable()
				.Set(dt => dt.Locale = CultureInfo.InvariantCulture);

			Action uiUpdates = () =>
			{
				view.DataSource = table;
				// Resize the DataGridView columns to fit the newly loaded content.
				view.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
			};

			view.Update(uiUpdates);

			return view;
		}


		/// <summary>
		/// Implicitly checks for Invoke required, preventing UI Thread issues
		/// </summary>
		/// <param name="control"></param>
		/// <param name="uiUpdates"></param>
		/// <returns></returns>
		public static Control Update(this Control control, Action uiUpdates)
		{
			if (control.InvokeRequired)
			{
				control.Invoke(uiUpdates);
			}
			else
			{
				uiUpdates();
			}

			return control;
		}
	}
}