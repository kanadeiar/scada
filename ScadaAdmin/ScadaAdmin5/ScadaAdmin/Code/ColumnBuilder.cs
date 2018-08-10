﻿/*
 * Copyright 2018 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaAdminCommon
 * Summary  : Creates columns for a DataGridView control
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using Scada.Admin.Project;
using Scada.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Creates columns for a DataGridView control
    /// <para>Создает столбцы для элемента управления DataGridView</para>
    /// </summary>
    internal class ColumnBuilder
    {
        private readonly ConfigBase configBase; // the reference to the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <remarks>The configuration database is required for creating combo box columns.</remarks>
        public ColumnBuilder(ConfigBase configBase)
        {
            this.configBase = configBase ?? throw new ArgumentNullException("configBase");
        }


        /// <summary>
        /// Creates a new column that hosts text cells.
        /// </summary>
        private DataGridViewTextBoxColumn NewTextBoxColumn(string dataPropertyName)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName
            };
        }

        /// <summary>
        /// Creates a new column that hosts cells which values are selected from a combo box.
        /// </summary>
        private DataGridViewComboBoxColumn NewComboBoxColumn(
            string dataPropertyName, string valueMember, string displayMember, object dataSource)
        {
            return new DataGridViewComboBoxColumn
            {
                Name = dataPropertyName,
                HeaderText = dataPropertyName,
                DataPropertyName = dataPropertyName,
                ValueMember = valueMember,
                DisplayMember = displayMember,
                DataSource = dataSource,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DisplayStyleForCurrentCellOnly = true
            };
        }

        /// <summary>
        /// Creates a new column that hosts cells which values are selected from a combo box.
        /// </summary>
        private DataGridViewComboBoxColumn NewComboBoxColumn<T>(
            string dataPropertyName, string displayMember, IList<T> dataSource, bool addEmptyRow = false)
        {
            return NewComboBoxColumn(dataPropertyName, dataPropertyName, displayMember,
                CreateComboBoxSource(dataSource, dataPropertyName, displayMember, addEmptyRow));
        }

        /// <summary>
        /// Creates a data table for using as a data source of a combo box.
        /// </summary>
        private DataTable CreateComboBoxSource<T>(
            IList<T> list, string valueMember, string displayMember, bool addEmptyRow)
        {
            DataTable dataTable = list.ToDataTable(true);

            if (addEmptyRow)
            {
                DataRow emptyRow = dataTable.NewRow();
                emptyRow[valueMember] = DBNull.Value;
                emptyRow[displayMember] = " ";
                dataTable.Rows.Add(emptyRow);
            }

            dataTable.DefaultView.Sort = displayMember;

            return dataTable;
        }

        /// <summary>
        /// Translates the column headers.
        /// </summary>
        private DataGridViewColumn[] TranslateHeaders(string tableName, DataGridViewColumn[] columns)
        {
            if (Localization.Dictionaries.TryGetValue("Scada.Admin.App.Code.ColumnBuilder." + tableName, 
                out Localization.Dict dict))
            {
                foreach (DataGridViewColumn col in columns)
                {
                    if (dict.Phrases.TryGetValue(col.Name, out string header))
                        col.HeaderText = header;
                }
            }

            return columns;
        }
        

        /// <summary>
        /// Creates columns for the object table.
        /// </summary>
        private DataGridViewColumn[] CreateObjTableColumns()
        {
            return TranslateHeaders("ObjTable", new DataGridViewColumn[]
            {
                    NewTextBoxColumn("ObjNum"),
                    NewTextBoxColumn("Name"),
                    NewTextBoxColumn("Descr")
            });
        }

        /// <summary>
        /// Creates columns for the devices table.
        /// </summary>
        private DataGridViewColumn[] CreateKPTableColumns()
        {
            return TranslateHeaders("KPTable", new DataGridViewColumn[]
            {
                NewTextBoxColumn("KPNum"),
                NewTextBoxColumn("Name"),
                NewComboBoxColumn("KPTypeID", "Name", configBase.KPTypeTable.Items),
                NewTextBoxColumn("Address"),
                NewTextBoxColumn("CallNum"),
                NewComboBoxColumn("CommLineNum", "Name", configBase.CommLineTable.Items, true),
                NewTextBoxColumn("Descr")
            });
        }


        /// <summary>
        /// Creates columns for the specified table
        /// </summary>
        public DataGridViewColumn[] CreateColumns(Type itemType)
        {
            if (itemType == typeof(Obj))
                return CreateObjTableColumns();
            else if (itemType == typeof(KP))
                return CreateKPTableColumns();
            else
                return new DataGridViewColumn[0];
        }
    }
}
