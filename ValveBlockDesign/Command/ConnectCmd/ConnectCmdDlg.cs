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
    partial class ConnectCmdDlg : Form
    {
        private Inventor.Application m_inventorApplication;
        private ConnectCmd m_connectCmd;

        public ConnectCmdDlg(Inventor.Application application, ConnectCmd connectCmd)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            m_inventorApplication = application;
            m_connectCmd = connectCmd;

            connectOkButton.Enabled = false;
            checkBoxCav.Checked = true;
        }

        //-----------------------------------------------------------------------------
        //----- Implementation of SelectEvents callbacks
        //-----------------------------------------------------------------------------
        private void ConnectCmdDlg_Load(object sender, EventArgs e)
        {
            this.numerUPX.Enabled = false;
            this.numerUPY.Enabled = false;
        }

        private void checkBoxCav_Click(object sender, EventArgs e)
        {
            checkBoxToCav.Checked = false;
            //if selected then start selection
            if (checkBoxCav.Checked)
            {
                m_connectCmd.EnableInteraction();
            }
            else
            {
                m_connectCmd.DisableInteraction();
            }
        }

        private void checkBoxToCav_Click(object sender, EventArgs e)
        {
            checkBoxCav.Checked = false;

            if (checkBoxToCav.Checked)
            {
                m_connectCmd.EnableInteraction();
            }
            else
            {
                m_connectCmd.DisableInteraction();
            }

        }

        private void ConnectCmdDlg_KeyPress(object sender, KeyPressEventArgs e)
        {
            //"Esc" key cancels command
            if (e.KeyChar == 27)
            {
                m_connectCmd.StopCommand();
            }
        }

        private void OnClose(object sender, System.EventArgs e)
        {
            m_connectCmd.StopCommand();
        }

        private void connectCancelButton_Click(object sender, EventArgs e)
        {
            m_connectCmd.StopCommand();
        }

        private void connectOkButton_Click(object sender, EventArgs e)
        {
            m_connectCmd.ExecuteCommand();
        }

        private void centerRadioButton_Click(object sender, EventArgs e)
        {
            m_connectCmd.UpdateCommandStatus();
        }

        private void onesideRadioButton_Click(object sender, EventArgs e)
        {
            m_connectCmd.UpdateCommandStatus();
        }

        private void othersideRadioButton_Click(object sender, EventArgs e)
        {
            m_connectCmd.UpdateCommandStatus();
        }

        private void numerUPX_ValueChanged(object sender, EventArgs e)
        {
            m_connectCmd.UpdateCommandStatus();
        }

        private void numerUPY_ValueChanged(object sender, EventArgs e)
        {
            m_connectCmd.UpdateCommandStatus();
        }

        private void portIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (portIndexComboBox.Text != "")
            {
                m_connectCmd.UpdateCommandStatus();
            }
        }
        
    }
}
