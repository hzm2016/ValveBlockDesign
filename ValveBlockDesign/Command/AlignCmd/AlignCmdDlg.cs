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
    partial class AlignCmdDlg : Form
    {
        private Inventor.Application m_inventorApplication;
        private AlignCmd m_alignCmd;

        public AlignCmdDlg(Inventor.Application application, AlignCmd alignCmd)
        {
            //
			// Required for Windows Form Designer support
			//
            InitializeComponent();

            m_inventorApplication = application;
            m_alignCmd = alignCmd;

            alignOkButton.Enabled = false;

            checkBoxCavity.Checked = true;

        }


        //-----------------------------------------------------------------------------
        //----- Implementation of SelectEvents callbacks
        //-----------------------------------------------------------------------------
        private void AlignCmdDlg_KeyPress(object sender, KeyPressEventArgs e)
        {
            //"Esc" key cancels command
            if (e.KeyChar == 27)
            {
                m_alignCmd.StopCommand();
            }
        }

        private void OnCheckBoxCavity(object sender, EventArgs e)
        {
            //disable CheckBoxWithThisCavity
            checkBoxWithThisCavity.Checked = false;

            //if selected then start selection
            if (checkBoxCavity.Checked)
            {
                m_alignCmd.EnableInteraction();
            }
            else
            {
                m_alignCmd.DisableInteraction();
            }
        }

        private void OnCheckBoxWithThisCavity(object sender, EventArgs e)
        {
            //disable CheckBoxCavity
            checkBoxCavity.Checked = false;

            //if selected then start selection
            if (checkBoxWithThisCavity.Checked)
            {
                m_alignCmd.EnableInteraction();
            }
            else
            {
                m_alignCmd.DisableInteraction();
            }
        }

        private void AlignOkButton_Click(object sender, EventArgs e)
        {
            m_alignCmd.ExecuteCommand();
        }

        private void AlignCancelButton_Click(object sender, EventArgs e)
        {
            m_alignCmd.StopCommand();
        }

        private void OnClose(object sender, System.EventArgs e)
        {
            m_alignCmd.StopCommand();
        }

        private void OnXDirectRadioButton(object sender, EventArgs e)
        {
            m_alignCmd.UpdateCommandStatus();
        }

        private void OnYDirectRadioButton(object sender, EventArgs e)
        {
            m_alignCmd.UpdateCommandStatus();
        }


    }
}
