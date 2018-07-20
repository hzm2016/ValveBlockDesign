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
    partial class MoveCmdDlg : Form
    {
        private Inventor.Application m_inventorApplication;
        private MoveCmd m_moveCmd;

        public string m_xOffsetExpression;
        public string m_yOffsetExpression;

        public MoveCmdDlg(Inventor.Application application, MoveCmd moveCmd)
        {
            InitializeComponent();

            m_inventorApplication = application;
            m_moveCmd = moveCmd;

            moveOkButton.Enabled = false;

            checkBoxChoose.Checked = true;
        }

        //-----------------------------------------------------------------------------
        //----- Implementation of SelectEvents callbacks
        //-----------------------------------------------------------------------------
        private void MoveCmdDlg_KeyPress(object sender, KeyPressEventArgs e)
        {
            //"Esc" key cancels command
            if (e.KeyChar == 27)
            {
                m_moveCmd.StopCommand();
            }
        }

        private void CheckBoxChoose_Click(object sender, EventArgs e)
        {
            if (checkBoxChoose.Checked)
            {
                m_moveCmd.EnableInteraction();
            }
            else
            {
                m_moveCmd.DisableInteraction();
            }
        }

        private void MoveOkButton_Click(object sender, EventArgs e)
        {
            m_moveCmd.ExecuteCommand();
        }

        private void MoveCancelButton_Click(object sender, EventArgs e)
        {
            m_moveCmd.StopCommand();
        }

        private void OnClose(object sender, System.EventArgs e)
        {
            m_moveCmd.StopCommand();
        }

        private void OnChangeXOffsetEdit(object sender, System.EventArgs e)
        {
            double xOffset = m_moveCmd.GetValueFromExpression(xOffsetText.Text.Trim());

            //reject invalid input
            if (!ValidateDoubleInput(xOffset.ToString()))
            {
                xOffsetText.ForeColor = System.Drawing.Color.Red;
                moveOkButton.Enabled = false;
                MessageBox.Show("输入值无效，请重新输入！");
            }
            else 
            {
                m_xOffsetExpression = xOffsetText.Text;
                m_moveCmd.UpdateCommandStatus();
            }
        }

        private void OnChangeYOffsetEdit(object sender, System.EventArgs e)
        {
            double yOffset = m_moveCmd.GetValueFromExpression(yOffsetText.Text.Trim());

            if (!ValidateDoubleInput(yOffset.ToString()))
            {
                yOffsetText.ForeColor = System.Drawing.Color.Red;
                moveOkButton.Enabled = false;
                MessageBox.Show("输入值无效，请重新输入！");
            }
            else
            {
                m_yOffsetExpression = yOffsetText.Text;
                m_moveCmd.UpdateCommandStatus();
            }
        }

        private bool ValidateDoubleInput(string input)
        {
            try
            {
                double num = double.Parse(input);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool AllParametersAreValid()
        {
            return (
                (xOffsetText.ForeColor != System.Drawing.Color.Red)
                &&
                (yOffsetText.ForeColor != System.Drawing.Color.Red)
                &&
                ((xOffsetText.Text != "0")||(yOffsetText.Text != "0"))
                );
        }

        private void MoveCmdDlg_Load(object sender, EventArgs e)
        {

        }
    }
}
