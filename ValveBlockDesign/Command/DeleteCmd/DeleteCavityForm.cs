using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal partial class DeleteCavityForm : Form
    {
        private DeleteCmd m_deleteCmd;
        private Inventor.Application m_inventorApplication;
        public DeleteCavityForm()
        {
            InitializeComponent();
        }
        public DeleteCavityForm(Inventor.Application application,DeleteCmd deleteCmd)
        {
            InitializeComponent();
            m_inventorApplication = application;
            m_deleteCmd = deleteCmd;

            this.btnsure.Enabled = false;
            this.checkBoxChoose.Checked = true;
        }

        private void btnsure_Click(object sender, EventArgs e)
        {
            m_deleteCmd.ExecuteCommand();
            listBCavity.Items.Clear();
        }

        private void btnsec_Click(object sender, EventArgs e)
        {
            m_deleteCmd.StopCommand();
        }

        private void checkBoxChoose_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxChoose.Checked == false)
            {
                //m_deleteCmd.AddInformation();
            }
        }

        private void checkBoxChoose_Click(object sender, EventArgs e)
        {
            if(this.checkBoxChoose.Checked)
            {
                m_deleteCmd.EnableInteraction();
                //m_deleteCmd.AddInformation();
            }
            else
            {
                m_deleteCmd.DisableInteraction();
            }
        }

        private void DeleteCavityForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_deleteCmd.StopCommand();
        }

        private void DeleteCavityForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                m_deleteCmd.UpdateCommandStatus();
            }
        }

    }
}
