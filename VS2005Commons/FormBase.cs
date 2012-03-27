using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VS2005Commons
{
    public partial class FormBase : Form
    {
        protected bool _modificheApportate = false;
        protected bool _disabilitaCheckSalvataggio = false;

        protected bool _erroreCaricamento = false;
        protected bool _solaLettura = false;

        private bool mouseIsWait = false;

        protected string titolo = "FormBase";

        // TAB PRINCIPALE
        private TabControl parentTabCtrl;
        private TabPage parentTabPag;

        protected bool datiCaricati;

        public TabPage ParentTabPag
        {
            get
            {
                return parentTabPag;
            }
            set
            {
                parentTabPag = value;
            }
        }
        public TabControl ParentTabCtrl
        {
            set
            {
                parentTabCtrl = value;
            }
        }

        private bool enableAutoMaximize = true;
        [Category("Atree")]
        [Description("Specifica se si vuole che la finestra si maximizzi automaticamente")]
        public bool EnableAutoMaximize
        {
            get { return enableAutoMaximize; }
            set { enableAutoMaximize = value; }
        }


        private String semeRicerca;
        /// <summary>
        /// Rappresenta un seme di ricerca da ricercare se la finestra viene aperta con un
        /// parametro di ricerca dall'esterno.
        /// E' utile quando devo aggiornare i dati se la ricerca e' lanciata dall'esterno
        /// </summary>
        public String SemeRicerca
        {
            get { return semeRicerca; }
            set { semeRicerca = value; }
        }

        public FormBase()
        {
            InitializeComponent();
        }

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Chiudi();
            }
        }

        protected void Chiudi()
        {
            this.Close();
        }

        protected virtual void salva()
        {
            // MUST BE IMPLEMENTED
        }

        protected void init()
        {

        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            if (enableAutoMaximize)
            {
                //AtreeMaximizing(this);
                this.WindowState = FormWindowState.Maximized;

                //this.MaximizeBox = false;
                //this.MinimizeBox = false;
                //AtreeMaximized(this);
            }
            else
            {

            }
        }

        private void BaseForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Controllare anche il canc o del
            //if (char.IsLetterOrDigit(e.KeyChar))
            //{
            _modificheApportate = true;
            //}
        }

        private void FormBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_modificheApportate && !_disabilitaCheckSalvataggio)
            {
                DialogResult res = MessageBox.Show("Salvare le modifiche?", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (res == DialogResult.Yes)
                {
                    salva();
                }
            }

            // ESCLUSA LA PRINCIPALE
            if (this.Parent != null)
            {
                if (parentTabPag != null)
                {
                    // TAB PRINCIPALE
                    this.parentTabPag.Dispose();
                    if (!parentTabCtrl.HasChildren)
                    {
                        parentTabCtrl.Visible = false;
                    }
                }
            }
        }

        protected MDIParentBase getFormPrincipale()
        {
            return ((MDIParentBase)this.MdiParent);
        }

        //protected Form getFormPrincipale()
        //{
        //    return (this.MdiParent);
        //}

        protected void MouseWait()
        {
            if (mouseIsWait)
            {
                mouseIsWait = !mouseIsWait;
                this.Cursor = Cursors.Default;
            }
            else
            {
                mouseIsWait = !mouseIsWait;
                this.Cursor = Cursors.WaitCursor;
            }
        }

        private void FormBase_Activated(object sender, EventArgs e)
        {
            // ESCLUSA LA PRINCIPALE
            if (this.Parent != null)
            {
                if (parentTabPag != null)
                {
                    // TAB PRINCIPALE
                    parentTabCtrl.SelectedTab = parentTabPag;
                    if (!parentTabCtrl.Visible)
                    {
                        parentTabCtrl.Visible = true;
                    }
                }
            }
        }

        protected void setTabName(string name)
        {
            if (parentTabPag != null)
            {
                parentTabPag.Text = name;
            }
        }

        /// <summary>
        /// Questo metodo server per rinfrescare i dati della form nel caso sia già aperta
        /// nel tab e si voglia aggiornarne il contenuto
        /// </summary>
        public virtual void AggiornaDati(String semeRicerca)
        {

        }

        private void FormBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (getFormPrincipale() != null)
            {
                // Apro la finestra aperta in precedenza
                if (getFormPrincipale().PreviousFormVisible != null)
                {
                    getFormPrincipale().PreviousFormVisible.Select();
                }

            }
        }

        //public delegate void AtreeMaximizingEventHandler(object sender);
        //public event AtreeMaximizingEventHandler AtreeMaximizing;

        //public delegate void AtreeMaximizedEventHandler(object sender);
        //public event AtreeMaximizedEventHandler AtreeMaximized;  


    }
}