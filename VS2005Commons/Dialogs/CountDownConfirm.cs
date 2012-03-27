using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VS2005Commons.Dialogs
{
    public partial class CountDownConfirm : BaseDialog
    {
        public CountDownConfirm()
        {
            InitializeComponent();
        }

        public CountDownConfirm(String domanda, String titolo, int timer)
            : this()
        {
            this.lblDomanda.Text = domanda;
            this.Text = titolo;
            this.timer = timer;
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

        private int timer = 0;
        public int Timer
        {
            set { timer = value; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void InputBox_Load(object sender, EventArgs e)
        {

        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            timer = timer - 1;
            btnOk.Text = "OK" + " (" + timer + ")";

            if (timer < 0)
            {
                this.DialogResult = DialogResult.Yes;
            }
            else
            {
                timer1.Enabled = true;
            }
        }
    }
}