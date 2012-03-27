using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace VS2005Commons
{
    public class ListViewUtils
    {

        public static void PrepareListView(ref ListView aListView, bool autoSizeContentBased)
        {
            aListView.FullRowSelect = true;
            aListView.HideSelection = false;
            aListView.MultiSelect = false;
            aListView.View = System.Windows.Forms.View.Details;

            if (ListUtils.isNotEmpty(aListView.Columns))
            {
                foreach (ColumnHeader column in aListView.Columns)
                {
                    if (autoSizeContentBased)
                    {
                        column.Width = -1; // Dimensiona su Contenuto
                    }
                    else
                    {
                        column.Width = -2; // Dimensiona su Colonna
                    }

                }
            }
        }

        public static long SelelectedLongIdFromTag(ref ListView aListView)
        {
            long result = 0;

            foreach (ListViewItem item in aListView.Items)
            {
                if (item.Selected)
                {
                    try
                    {
                        result = Convert.ToInt64(item.Tag);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            return result;
        }

        public static int SelelectedIntIdFromTag(ref ListView aListView)
        {
            int result = 0;

            foreach (ListViewItem item in aListView.Items)
            {
                if (item.Selected)
                {
                    try
                    {
                        result = Convert.ToInt32(item.Tag);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            return result;
        }

        public static DateTime SelelectedDateTimeFromTag(ref ListView aListView)
        {
            DateTime result = DateTime.Now;

            foreach (ListViewItem item in aListView.Items)
            {
                if (item.Selected)
                {
                    try
                    {
                        result = Convert.ToDateTime(item.Tag);
                        break;
                    }
                    catch
                    {

                    }
                }
            }

            return result;
        }

        public static void SelezionaIndice(ref ListView aListView, int indiceSelezionato)
        {
            if (indiceSelezionato > -1)
            {
                foreach (ListViewItem item in aListView.Items)
                {
                    if (item.Index == indiceSelezionato)
                    {
                        item.Selected = true;
                    }
                }
            }

        }

        public static void SelezionaElementoDaIdNelTag(ref ListView aListView, long id)
        {
            if (id > -1)
            {
                aListView.SelectedItems.Clear();

                Point a = aListView.AutoScrollOffset;

                foreach (ListViewItem item in aListView.Items)
                {
                    if (item.Tag != null && (long)item.Tag == id)
                    {
                        item.Selected = true;
                        item.Focused = true;

                        item.EnsureVisible();
                    }
                }

                aListView.AutoScrollOffset = a;
            }
        }

        public static void Popola(ref ListView aListView, IList aListOfData)
        {
            aListView.SuspendLayout();
            aListView.BeginUpdate();
            aListView.Items.Clear();

            if (ListUtils.isNotEmpty(aListOfData))
            {
                foreach (object item in aListOfData)
                {
                    if (item is IListViewItemable)
                    {
                        aListView.Items.Add(((IListViewItemable)item).toListViewItem());
                    }
                    else if (item is DateTime)
                    {
                        ListViewItem dateItem = new ListViewItem(item.ToString());
                        dateItem.Tag = item;

                        aListView.Items.Add(dateItem);
                    }
                    else
                    {
                        aListView.Items.Add(((IListViewItemable)item).ToString());
                    }
                }
            }

            aListView.EndUpdate();
            aListView.ResumeLayout();
        }
    }
}
