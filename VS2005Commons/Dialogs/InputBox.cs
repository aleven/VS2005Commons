using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VS2005Commons.Dialogs
{
    public partial class InputBox : BaseDialog
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public String TestoInserito
        {
            get
            {
                return this.txtInput.Text;
            }
        }

        public String Domanda
        {
            set
            {
                this.lblDomanda.Text = value;
            }
        }

        public String Titolo
        {
            set
            {
                this.Text = value;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {

        }

        private void InputBox_Load(object sender, EventArgs e)
        {

        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}