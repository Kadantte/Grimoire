﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataCore.Structures;
using System.Linq;

using Daedalus.Structures;

namespace Grimoire.Tabs.Utilities
{
    public class Grid
    {
        Manager tManager;

        public Grid()
        {
            tManager = Manager.Instance;
        }

        public void GenerateColumns()
        {
            DataGridViewTextBoxColumn[] columns;
            Daedalus.Structures.Cell[] fields = new Daedalus.Structures.Cell[tManager.RDBCore.CellCount];

            switch (tManager.Style)
            {
                case Style.RDB:
                    columns = new DataGridViewTextBoxColumn[fields.Length];
                    fields = tManager.RDBCore.CellTemplate;

                    tManager.RDBTab.ProgressMax = fields.Length;

                    for (int i = 0; i < fields.Length; i++)
                    {
                        Daedalus.Structures.Cell field = fields[i];

                        columns[i] = new DataGridViewTextBoxColumn()
                        {
                            Name = field.Name,
                            HeaderText = field.Name,
                            Width = 100,
                            Resizable = DataGridViewTriState.True,
                            Visible = field.Visible, /*Show*/
                            SortMode = DataGridViewColumnSortMode.Programmatic
                        };

                        tManager.RDBTab.ProgressVal = i;
                    }

                    tManager.RDBTab.ResetProgress();
                    tManager.RDBTab.SetColumns(columns);
                    break;
            }
        }

        public void Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            switch (tManager.Style)
            {
                case Style.RDB:
                    {
                        int rowCount = tManager.RDBCore.RowCount;

                        if (e.RowIndex >= rowCount)
                            return;

                        if (tManager.RDBCore.RowCount > 0)
                            e.Value = tManager.RDBCore.Rows[e.RowIndex][e.ColumnIndex];
                        else
                            e.Value = "NULL";

                    }
                    break;

                case Style.DATA:
                   {
                        if (tManager.DataTab.Filtered && tManager.DataTab.Searching || !tManager.DataTab.Filtered && tManager.DataTab.Searching)
                            e.Value = tManager.DataTab.SearchIndex[e.RowIndex].Name;
                        else if (tManager.DataTab.Filtered && !tManager.DataTab.Searching)
                        {
                            if (e.RowIndex < tManager.DataTab.FilterCount)
                                e.Value = tManager.DataTab.FilteredIndex[e.RowIndex].Name;
                        }
                        else
                            e.Value = tManager.DataCore.Index[e.RowIndex].Name;
                    }
                    break;
            }
        }

        public void Grid_CellPushed(object sender, DataGridViewCellValueEventArgs e)
        {
            switch (tManager.Style)
            {
                case Style.RDB:
                    {
                        int rowCount = tManager.RDBCore.RowCount;
                        if (e.RowIndex >= rowCount)
                            return;

                        tManager.RDBCore.Rows[e.RowIndex][e.ColumnIndex] = e.Value;
                    }
                    break;
            }
        }
    }
}
