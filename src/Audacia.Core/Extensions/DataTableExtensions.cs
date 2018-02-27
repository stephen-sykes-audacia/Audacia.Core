﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Audacia.Core.Extensions
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Converts a DataTable to a CSV string
        /// </summary>
        /// <param name="dataTable">The DataTable to convert</param>
        /// <param name="delimiter">The delimiter to use in the converted string (defaults to ',')</param>
        /// <returns>The converted string</returns>
        public static string ToCsv(this DataTable dataTable, string delimiter = ",")
        {
            if (delimiter == "\"")
            {
                throw new NotSupportedException("Cannot use \" as a delimiter as it is used in CSV qualification");
            }

            var outputBuilder = new StringBuilder();

            var headers = new HashSet<string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                var header = column.Caption ?? column.ColumnName;

                header = header.Replace("\"", "\"\"");

                headers.Add($"\"{header}\"");
            }

            outputBuilder.AppendLine(string.Join(delimiter, headers));

            foreach (DataRow row in dataTable.Rows)
            {
                var cells = new string[row.ItemArray.Length];

                for (var i = 0; i < row.ItemArray.Length; i++)
                {
                    var cellValue = row.ItemArray.ElementAt(i);

                    // If we have a format string, we can apply it here using string.Format();
                    var cellValueString = cellValue.ToString();
                    cellValueString = cellValueString.Replace("\"", "\"\"");

                    cells[i] = $"\"{cellValueString}\"";
                }

                outputBuilder.AppendLine(string.Join(delimiter, cells));
            }

            return outputBuilder.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }
    }
}