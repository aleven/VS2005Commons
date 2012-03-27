using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VS2005Commons.Dialogs
{
    public partial class PasswordDialog : BaseDialog
    {
        private String passwordInserita;
        private String checkPassword;

        public DialogResult ShowDialog(String checkPassword)
        {

            this.checkPassword = checkPassword;
            return this.ShowDialog();

        }

        public PasswordDialog()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            passwordInserita = textBox1.Text;

            if (passwordInserita == checkPassword)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                FormsHelper.Error("Password Errata!", this.Text);
            }
        }

        private void PasswordDialog_Load(object sender, EventArgs e)
        {
            if (StringUtils.isNullOrEmpty(checkPassword))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}