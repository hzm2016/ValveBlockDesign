using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Inventor;

namespace ValveBlockDesign
{
    internal partial class InsertOutlineForm : Form
    {
        private Inventor.Application m_inventorApplication;
        private InsertOutlineCmd m_insertOutlineCmd;
        private ConnectToAccess m_connectToaccess;
        private string m_filepath = @"F:\CavityLibrary";
        private string m_filename = "CavityLibrary";

        public InsertOutlineForm()
        {
            InitializeComponent();
        }
        public InsertOutlineForm(Inventor.Application application,InsertOutlineCmd insertOutlineCmd)
        {
            InitializeComponent();
            m_inventorApplication = application;
            m_insertOutlineCmd = insertOutlineCmd;

            checkBoxChoose.Checked = true;
            btninsert.Enabled = false;
        }

        private void InsertOutlineForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_insertOutlineCmd.StopCommand();
        }

        private void InsertOutlineForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_insertOutlineCmd.UpdateCommandStatus();
        }

        private void btnsec_Click(object sender, EventArgs e)
        {
            m_insertOutlineCmd.StopCommand();
        }

        private void checkBoxChoose_Click(object sender, EventArgs e)
        {
            if (this.checkBoxChoose.Checked)
            {
                m_insertOutlineCmd.EnableInteraction();
            }
            else
            {
                m_insertOutlineCmd.DisableInteraction();
            }
        }

        private void btnsure_Click(object sender, EventArgs e)
        {
            m_connectToaccess = new ConnectToAccess(m_filepath,m_filename);
            if (listBoxName.SelectedItem.ToString() != null)
            {
                string name = listBoxName.SelectedItem.ToString();
                string sql = @"select * from Outlines where Outlines.生产厂家='" + name + "'";
                string[] getresult = new string[25];
                m_connectToaccess.GetInformation(sql, "型号", out getresult);
                listBoxName.Items.Clear();
                int i = 0;
                while (getresult[i] != null)
                {
                    listBoxName.Items.Add(getresult[i]);
                    i++;
                }
            }
            else
            {
                MessageBox.Show("请选择元件的生产厂家");
            }
        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            if (listBoxName.SelectedItem.ToString() != null)
            {
                m_insertOutlineCmd.ExecuteCommand();
            }
            else
            {
                MessageBox.Show("请选择外形型号");
            }
        }
    }
}
