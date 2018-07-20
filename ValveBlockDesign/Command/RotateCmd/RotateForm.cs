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
    internal partial class RotateForm : Form
    {
        private Inventor.Application m_inventorApplication;
        private RotateCmd m_rotateCmd;

        public RotateForm()
        {
            InitializeComponent();
        }
        public RotateForm(Inventor.Application application,RotateCmd rotateCmd)
        {
            InitializeComponent();
            m_inventorApplication = application;
            m_rotateCmd = rotateCmd;
            checkBoxChoose.Checked = true;
            btnsure.Enabled = false;
            comboBoxAngle.Text = "0";
        }

        private void RotateForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                m_rotateCmd.UpdateCommandStatus();
            }
        }

        private void RotateForm_FormClosed(object sender,EventArgs e)
        {
            m_rotateCmd.StopCommand();
        }

        private void btnsure_Click(object sender, EventArgs e)
        {
            m_rotateCmd.ExecuteCommand();
        }

        private void btnsec_Click(object sender, EventArgs e)
        {
            m_rotateCmd.StopCommand();
        }
    }
}
