﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static partial class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this DataTable table)
            where T : class, new()
        {
            var observableCollection = new ObservableCollection<T>();

            if (table == null || table.Rows.Count == 0)
            {
                return observableCollection;
            }
                        
            var properties = PropertyCache[typeof(T)];

            string propertyName = "";

            foreach (var row in table.AsEnumerable())
            {
                var obj = new T();

                foreach (var property in properties ?? Enumerable.Empty<PropertyInfo>())
                {
                    try
                    {
                        propertyName = property.Name;

                        var value = row[propertyName];

                        if (value == DBNull.Value)
                        {
                            property.SetValue(obj, null, null);
                        }
                        else
                        {
                            property.SetValue(obj, Convert.ChangeType(value, property.PropertyType), null);
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        continue;
                    }
                }

                observableCollection.Add(obj);
            }

            return observableCollection;

        }

        public static List<T> AddRowsByAction<T>(this DataTable table, Func<DataRow, T> rowAdditionAction)
            where T : class, new()
        {
            var lines = new List<T>();

            if (table == null || table.Rows.Count == 0)
            {
                return lines;
            }

            var dataRows = table.Rows.Cast<DataRow>();

            try
            {
                foreach (var row in dataRows ?? Enumerable.Empty<DataRow>())
                {
                    var nextLine = rowAdditionAction(row);

                    if (nextLine != null)
                    {
                        lines.Add(nextLine);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return lines;

        }

        public static List<string> GetColumnNames(this DataTable table)
        {
            try
            {
                if (table == null || table.Rows.Count == 0)
                {
                    return new List<string>();
                }

                return table.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<string>();
            }
        }

        public static void SetColumnsOrder(this DataTable table, params string[] orderedColumnNames)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            if (orderedColumnNames == null || orderedColumnNames.Length == 0)
            {
                return;
            }

            int columnIndex = 0;
            var columnNames = table.GetColumnNames();

            foreach (var columnName in orderedColumnNames.Except(columnNames ?? Enumerable.Empty<string>()))
            {
                try
                {
                    table.Columns[columnName].SetOrdinal(columnIndex);
                    columnIndex++;
                }
                catch (Exception ex)
                {
                    string errMsg = string.Format($"{MethodBase.GetCurrentMethod().Name}: {ex.ToString()}");
                    Debug.WriteLine(errMsg);
                }
            }
        }

        public static void RemoveColumn(this DataTable table, string columnName)
        {
            try
            {
                if (table == null || table.Rows.Count == 0)
                {
                    return;
                }

                if (table.Columns.Contains(columnName))
                {
                    table.Columns.Remove(columnName);
                }

            }
            catch (Exception ex)
            {
                string errMsg = string.Format($"{MethodBase.GetCurrentMethod().Name}: {ex.ToString()}");
                Debug.WriteLine(errMsg);
            }
        }

        public static List<T> ToList<T>(this DataTable table)
            where T : class, new()
        {
            var list = new List<T>();

            if (table == null || table.Rows.Count == 0)
            {
                return list;
            }
                        
            var properties = PropertyCache[typeof(T)];

            if (properties == null || properties.Length == 0)
            {
                return list;
            }

            var tableColumnNames = table.GetColumnNames();

            string propertyName = "";

            foreach (var row in table.AsEnumerable())
            {
                var item = new T();

                foreach (var property in properties ?? Enumerable.Empty<PropertyInfo>())
                {
                    try
                    {
                        propertyName = property.Name;

                        if (!tableColumnNames.Contains(propertyName))
                        {
                            continue;
                        }

                        var value = row[propertyName];

                        if (value == DBNull.Value)
                        {
                            property.SetValue(item, null, null);
                        }

                        //TODO: place a condition for handling Enums, here.
                        else if (property.PropertyType.Equals(typeof(SqlDbType)))
                        {
                            var dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), value.ToString(), true);
                            property.SetValue(item, dbType, null);
                        }

                        else
                        {
                            property.SetValue(item, Convert.ChangeType(value, property.PropertyType), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        continue;
                    }
                }

                list.Add(item);

            }

            return list;

        }

        /// <summary>
        /// As Dynamic Enumerable
        /// Turns a datatable into a dynamic enumerable type
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {
            return table == null ? table.AsEnumerable().Select(row => new DynamicRow(row)) : Enumerable.Empty<dynamic>();
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow _row;
            internal DynamicRow(DataRow row) { _row = row; }
            // Interprets a member-access as an indexer-access on the 
            // contained DataRow.
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = retVal ? _row[binder.Name] : null;
                return retVal;
            }
        }

        /**** SQL To Datatable ***/
        /// <summary>
        /// Fill a given DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="connStr"></param>
        /// <param name="selectQuery"></param>
        private static void FillTable(this DataTable table, string connStr, string selectQuery)
        {
            try
            {
                using (var da = new SqlDataAdapter(selectQuery, connStr))
                {
                    da.SelectCommand.CommandTimeout = 180;

                    da.Fill(table);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
                logger.Error(ex);
                throw;
            }
        }

    }

}
