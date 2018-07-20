using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal partial class BoltHoleForm : Form
    {
        private BoltHolesFormCmd m_boltHolesFormCmd;
        private Inventor.Application m_inventorApplication;
        private ConnectToAccess m_connectToAccess;

        public BoltHoleForm()
        {
            InitializeComponent();
        }
        public BoltHoleForm(Inventor.Application inventor, BoltHolesFormCmd boltHolesCmd)
        {
            
            m_inventorApplication = inventor;
            m_boltHolesFormCmd = boltHolesCmd;
            InitializeComponent();
        }

        private void BoltHoleForm_Load(object sender, EventArgs e)
        {
            //btnsure.Enabled = false;
            //checkBoxFace.Checked = true;
            //string[] getresult = new string[25];
            //comBLibrary.Text = "BoltHoles";
            //string sql = @"select * from BoltHoles";
            //m_connectToAccess = new ConnectToAccess(@"F:\CavityLibrary", "CavityLibrary");
            //m_connectToAccess.GetInformation(sql,"编码", out getresult);
            //int i = 0;
            //while (getresult[i] != null)
            //{
            //    comBNumber.Items.Add(getresult[i]);
            //    i++;
            //}
        }

        private void tboffset_TextChanged(object sender, EventArgs e)
        {
            if (tboffset.Text.Length > 0)
            {
                if (radioFour.Checked == true || radioTwo.Checked == true)
                {
                    btnsure.Enabled = true;
                }
                else
                {
                    tboffset.Clear();
                }
            }
        }

        private void btnsure_Click(object sender, EventArgs e)
        {
            m_boltHolesFormCmd.ExecuteCommand();
            m_boltHolesFormCmd.StopCommand();
        }

        private void BoltHoleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_boltHolesFormCmd.StopCommand();
        }

        private void BoltHoleForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                m_boltHolesFormCmd.UpdateCommandStatus();
            }
        }

        private void btnsec_Click(object sender, EventArgs e)
        {
            m_boltHolesFormCmd.StopCommand();
        }

        private void tboffset_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z'))
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
