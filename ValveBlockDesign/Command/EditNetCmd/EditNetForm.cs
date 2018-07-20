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
    internal partial class EditNetForm : Form
    {
        private Inventor.Application m_inventorApplication;
        private EditNetCmd m_editNetCmd;
        public EditNetForm()
        {
            InitializeComponent();
        }
        public EditNetForm(Inventor.Application application,EditNetCmd editNetCmd)
        {
            InitializeComponent();
            m_inventorApplication = application;
            m_editNetCmd = editNetCmd;
            checkBoxChoose.Checked = true;
            btnedit.Enabled = false;
            dataportNET.ClearSelection();
        }

        private void EditNetForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                m_editNetCmd.StopCommand();
            }
        }

        private void checkBoxChoose_Click(object sender, EventArgs e)
        {
            if (checkBoxChoose.Checked == true)
            {
                m_editNetCmd.EnableInteraction();
            }
            else
            {
                m_editNetCmd.DisableInteraction();
            }
        }

        private void OnClose(object sender, System.EventArgs e)
        {
           m_editNetCmd.StopCommand();
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            m_editNetCmd.ExecuteCommand();
            dataportNET.SelectedRows[0].Cells[1].Value = comboBNETs.Text;
        }

        private void btnsure_Click(object sender, EventArgs e)
        {
            m_editNetCmd.StopCommand();
        }

        private void EditNetForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_editNetCmd.StopCommand();
            
        }

        private void dataportNET_SelectionChanged(object sender, EventArgs e)
        {
            m_editNetCmd.UpdateData();
        }

        private void comboBNETs_TextChanged(object sender, EventArgs e)
        {
            m_editNetCmd.UpdateData();
        }
    }
}
