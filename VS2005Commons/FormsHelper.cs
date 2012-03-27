using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VS2005Commons.Dialogs;

namespace VS2005Commons
{
    public class FormsHelper
    {
        private static Dictionary<string, Form> onceForms;

        public static bool Confirm(String message, String title)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static void Inform(String message, String title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Error(String message, String title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Warn(String message, String title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static String Input(String message, String title)
        {
            String res = null;

            InputBox input = new InputBox();
            input.Domanda = message;
            input.Titolo = title;
            if (input.ShowDialog() == DialogResult.OK)
            {
                res = input.TestoInserito;
            }
            return res;
        }

        public static bool ConfirmTimer(String message, String title, int timer)
        {
            return new CountDownConfirm(message, title, timer).ShowDialog() == DialogResult.Yes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Ritorna -1 se non sono stati immessi valori</returns>
        public static int InputNumeric(String domanda, String titolo)
        {
            int res = -1;

            InputBox input = new InputBox();
            input.Titolo = titolo;
            input.Domanda = domanda;
            if (input.ShowDialog() == DialogResult.OK)
            {
                res = Convert.ToInt32(input.TestoInserito);
            }
            // InputBox(domanda, titolo, "0");

            return res;
        }

        private static List<Form> formsAlreadyOpened;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aForm"></param>
        public static void ShowOnce(Form aForm)
        {
            bool result = false;

            //if (formsAlreadyOpened == null)
            //{
            //    formsAlreadyOpened = new List<Form>();
            //}

            //foreach (Form alreadyOpen in formsAlreadyOpened)
            //{
            //    if (alreadyOpen.GetType().Equals(aForm.GetType()))
            //    {
            //        result = true;
            //        alreadyOpen.Select();
            //        break;
            //    }

            //}

            if (onceForms == null)
            {
                onceForms = new Dictionary<string, Form>();
            }

            Form test = null;
            onceForms.TryGetValue(aForm.GetType().FullName, out test);

            if (test != null)
            {
                // formsAlreadyOpened.Add(aForm);
                // aForm.Activate();

                // Devo capire se ho il riferimento all'oggetto ma la form e' chiusa

                if (test.IsHandleCreated)
                {
                    test.Select();
                }
                else
                {
                    // TODO: SISTEMARE !!
                    aForm.Show();
                    aForm.Focus();
                    test = aForm;
                }

            }
            else
            {
                onceForms.Add(aForm.GetType().FullName, aForm);
                aForm.Show();
            }
        }

        public static void ShowOncePassword(Form aForm, string password)
        {
            if (InsertPassword(password))
            {
                ShowOnce(aForm);
            }
        }

        public static bool InsertPassword(String passwordToCheck)
        {
            return (new PasswordDialog().ShowDialog(passwordToCheck) == DialogResult.OK);
        }
    }
}
