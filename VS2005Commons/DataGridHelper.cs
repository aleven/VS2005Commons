using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NLog;

namespace VS2005Commons
{
    public class DataGridHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static DataGridHelper getIstanza()
        {
            return new DataGridHelper();
        }

        /// <summary>
        /// Aggiungere la chiamata a questo metodo in _EditingControlShowing del DataGridView interessato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GestioneNumericPad(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox innerTextBox;
            if (e.Control is TextBox)
            {
                innerTextBox = e.Control as TextBox;
                // innerTextBox.KeyDown -= new KeyEventHandler(innerTextBox_KeyDown);
                // innerTextBox.KeyDown += new KeyEventHandler(innerTextBox_KeyDown);

                // Succede che si aggancia 2 volte l'evento e poi si duplica il carattere
                innerTextBox.KeyPress -= new KeyPressEventHandler(innerTextBox_KeyPress);
                innerTextBox.KeyPress += new KeyPressEventHandler(innerTextBox_KeyPress);
            }            
        }

        private void innerTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Succede che si aggancia 2 volte l'evento e poi si duplica il carattere
            if (!e.Handled)
            {
                if (e.KeyChar == 46)
                {
                    SendKeys.Send(","); //manda la virgola al posto del .
                    e.Handled = true; //fa in modo che il . non vada nell'input
                }
            }
        }

        private void innerTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Decimal)
            //{
            //    SendKeys.Send(","); //manda la virgola al posto del .
            //    e.SuppressKeyPress = true; //fa in modo che il . non vada nell'input
            //}
        }

        public static void Configure(ref DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToDeleteRows = false;

            dgv.AllowUserToResizeRows = false;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        public static void ConfigureEditable(ref DataGridView dgv)
        {
            Configure(ref dgv);

            dgv.ReadOnly = false;
        }

        public static void ConfigureNoAutoColumn(ref DataGridView dgv)
        {
            Configure(ref dgv);

            dgv.AutoGenerateColumns = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="indiceColonnaID"></param>
        /// <returns>-1 if no item is selected</returns>
        public static int IDSelezionato(DataGridView dgv, int indiceColonnaID)
        {
            int res = -1;

            if (dgv != null && dgv.CurrentRow != null)
            {
                Object val = dgv[indiceColonnaID, dgv.CurrentRow.Index].Value;
                if (val != null ) {
                    try
                    {
                        res = int.Parse(val.ToString());
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
            }

            return res;
        }

    }
}
