using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;

namespace VS2005Commons
{
    public class ComboBoxUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="lista"></param>
        public static void Popola(ref ComboBox comboBox, object lista, string DisplayMember, string ValueMember)
        {
            comboBox.BeginUpdate();

            comboBox.Items.Clear();

            // List<TipoUtensile> tipiUtensili = new TipoUtensileDao(MyApplication.GetConnection()).ListaOrdinata();
            if (lista != null)
            {
                comboBox.DataSource = lista;
                comboBox.DisplayMember = DisplayMember;
                comboBox.ValueMember = ValueMember;
            }
            comboBox.EndUpdate();
        }

        public static long SelectedValueLong(ref ComboBox comboBox)
        {
            return (long)comboBox.SelectedValue;
        }

        //public static List<IComboBoxable> ToList(List<object > list)
        //{

        //}

    }
}
