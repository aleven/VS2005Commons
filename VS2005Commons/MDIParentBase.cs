using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VS2005Commons
{
    public partial class MDIParentBase : FormBase
    {
        private int childFormNumber = 0;

        private FormBase currentFormVisible;
        private FormBase previousFormVisible;

        public FormBase PreviousFormVisible
        {
            get { return previousFormVisible; }
            set { previousFormVisible = value; }
        }


        public MDIParentBase()
        {
            InitializeComponent();
        }

        private TabControl aTabControlInMainForm;

        public TabControl ATabControlInMainForm
        {
            get { return aTabControlInMainForm; }
            set { aTabControlInMainForm = value; }
        }


        //private void ShowNewForm(object sender, EventArgs e)
        //{
        //    // Create a new instance of the child form.
        //    Form childForm = new Form();
        //    // Make it a child of this MDI form before showing it.
        //    childForm.MdiParent = this;
        //    childForm.Text = "Window " + childFormNumber++;
        //    childForm.Show();
        //}

        //private void OpenFile(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        //    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        //    {
        //        string FileName = openFileDialog.FileName;
        //        // TODO: Add code here to open the file.
        //    }
        //}

        //private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog();
        //    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        //    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
        //    {
        //        string FileName = saveFileDialog.FileName;
        //        // TODO: Add code here to save the current contents of the form to a file.
        //    }
        //}

        //private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Application.Exit();
        //}

        //private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        //}

        //private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        //}

        //private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // TODO: Use System.Windows.Forms.Clipboard.GetText() or System.Windows.Forms.GetData to retrieve information from the clipboard.
        //}

        //private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        //}

        //private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        //}

        //private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    LayoutMdi(MdiLayout.Cascade);
        //}

        //private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    LayoutMdi(MdiLayout.TileVertical);
        //}

        //private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    LayoutMdi(MdiLayout.TileHorizontal);
        //}

        //private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    LayoutMdi(MdiLayout.ArrangeIcons);
        //}

        //private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    foreach (Form childForm in MdiChildren)
        //    {
        //        childForm.Close();
        //    }
        //}

        /// <summary>
        /// Procedura che si occupa di aprire ed aggiornare il menu delle finestre
        /// </summary>
        /// <param name="aForm"></param>
        public FormBase ShowMDIChildFormWithTab(FormBase aFormToShow)
        {
            FormBase res = SearchChildren(aFormToShow);

            // if (!mdiFormAperta(aFormToShow))
            if (res == aFormToShow)
            {
                if (ATabControlInMainForm != null)
                {
                    aFormToShow.ParentTabCtrl = ATabControlInMainForm;
                    TabPage tp = new TabPage();
                    tp.Parent = ATabControlInMainForm;
                    tp.Text = aFormToShow.Text;
                    tp.Show();
                    aFormToShow.ParentTabPag = tp;

                    childFormNumber++;
                    ATabControlInMainForm.SelectedTab = tp;
                }

                previousFormVisible = currentFormVisible;
                currentFormVisible = aFormToShow;

                aFormToShow.MdiParent = this;
                aFormToShow.Show();
            }
            else
            {
                previousFormVisible = currentFormVisible;
                currentFormVisible = aFormToShow;

                // aFormToShow.Select();
                res.Select();
                res.AggiornaDati(SemeRicerca);
            }

            return res;
        }

        public void ShowMDIChildFormWithTabPassword(FormBase aFormToShow, String password)
        {
            if (FormsHelper.InsertPassword(password))
            {
                ShowMDIChildFormWithTab(aFormToShow);
            }            
        }

        public void ShowMDIChildFormWithTabAndRrefresh(FormBase aFormToShow)
        {
            FormBase res = ShowMDIChildFormWithTab(aFormToShow);

            if (res != aFormToShow)
            {
                res.AggiornaDati(aFormToShow.SemeRicerca);
                res.Refresh();
            }
        }

        private bool mdiFormAperta(FormBase aForm)
        {
            bool res = false;
            if (this.MdiChildren.Length > 0)
            {
                foreach (Form children in this.MdiChildren)
                {
                    if (children.GetType().Equals(aForm.GetType()))
                    {
                        res = true;

                        if ((aForm is IFormDettaglio) &&
                            (children is IFormDettaglio))
                        {
                            if (((IFormDettaglio)aForm).IdentificativoDettaglio != ((IFormDettaglio)children).IdentificativoDettaglio)
                            {
                                res = false;
                            }
                        }

                        //if (res)
                        //{
                        //    if (children.WindowState == FormWindowState.Minimized)
                        //    {
                        //        children.WindowState = FormWindowState.Normal;
                        //    }
                        //    children.Activate();
                        //    break;
                        //}
                        if (res)
                        {
                            children.Select();
                            break;
                        }
                    }
                }
            }
            return res;
        }

        private FormBase SearchChildren(FormBase aForm)
        {
            FormBase res = aForm;
            if (this.MdiChildren.Length > 0)
            {
                foreach (Form children in this.MdiChildren)
                {
                    if (children.GetType().Equals(aForm.GetType()))
                    {
                        res = (FormBase)children;

                        if ((aForm is IFormDettaglio) &&
                            (children is IFormDettaglio))
                        {
                            // Gestisco i Dettagli aperti di ID Diveri
                            if (((IFormDettaglio)aForm).IdentificativoDettaglio != ((IFormDettaglio)children).IdentificativoDettaglio)
                            {
                                res = aForm;
                            }
                        }

                        //if (res)
                        //{
                        //    if (children.WindowState == FormWindowState.Minimized)
                        //    {
                        //        children.WindowState = FormWindowState.Normal;
                        //    }
                        //    children.Activate();
                        //    break;
                        //}
                        //if (res)
                        //{
                        //    children.Select();
                        break;
                        //}
                    }
                }
            }
            return res;
        }

        private void MDIParentBase_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

    }
}
