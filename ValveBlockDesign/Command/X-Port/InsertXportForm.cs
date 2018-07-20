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
     partial class InsertXportForm : Form
    {
        private Inventor.Application m_inventorApplication;
        private InsertXportCmd m_insertXportCmd;
        public InsertXportForm()
        {
            InitializeComponent();
        }
        public InsertXportForm(Inventor.Application application,InsertXportCmd insertXportCmd)
        {
            InitializeComponent();
            m_inventorApplication = application;
            m_insertXportCmd = insertXportCmd;
            radiobtn2.Checked = true;
        }

        private void InsertXportForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                m_insertXportCmd.UpdateCommandStatus();
            }
        }

        private void InsertXportForm_FormClosed(object sender, EventArgs e)
        {
            m_insertXportCmd.StopCommand();
        }

        private void btnsec_Click(object sender, EventArgs e)
        {
            m_insertXportCmd.StopCommand();
        }

        private void btnsure_Click(object sender, EventArgs e)
        {
            m_insertXportCmd.ExecuteCommand();
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            if (tbID.Text.Length > 0)
                btnsure.Enabled = true;
        }

        private void radiobtn1_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn1.Checked == true)
            {
                m_insertXportCmd.AddInformation();
            }
        }

        private void tbID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tby_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
