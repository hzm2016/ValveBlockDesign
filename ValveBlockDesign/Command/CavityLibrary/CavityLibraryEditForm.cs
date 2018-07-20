﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ValveBlockDesign
{
    internal partial class CavityLibraryEditForm : Form
    {
        private ConnectToAccess m_connectToAccess;
        private CavityLibraryEditDataForm m_cavityLibraryEditDataForm;
        private CavityLibraryEditCmd m_cavityLibraryEditCmd;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;
        public CavityLibraryEditForm()
        {
            InitializeComponent();
        }
        public CavityLibraryEditForm(CavityLibraryEditCmd cavityLibraryEditCmd)
        {
            InitializeComponent();
            m_cavityLibraryEditCmd = cavityLibraryEditCmd;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName + "\\CavityLibrary";
        }

        private void btnscan_Click(object sender, EventArgs e)
        {
            if (this.lv1.SelectedItems.Count != 1)
            {
                MessageBox.Show("请选择要浏览的元件类型");
            }
            else
            {
                string indexname = this.lv1.SelectedItems[0].SubItems[0].Text.ToString();
                string sql = @"select * from " + indexname;
                m_connectToAccess = new ConnectToAccess(deFaultpath , "CavityLibrary");
                m_connectToAccess.GetAddInformation(sql, lv2, "编码", "索引编号");
            }
        }

        private void btnsure_Click(object sender, EventArgs e)
        {
            string coding1 = this.lv2.SelectedItems[0].SubItems[0].Text.ToString();
            string coding2 = this.lv2.SelectedItems[0].SubItems[1].Text.ToString();
            string filepath = tb1.Text;
            string indexname = this.lv1.SelectedItems[0].SubItems[0].Text.ToString();
            m_cavityLibraryEditDataForm = new CavityLibraryEditDataForm(coding1, coding2, filepath, indexname, this);
            m_cavityLibraryEditDataForm.Show();
            this.Hide();
        }

        private void btnbrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {                                                                                  //显示浏览文件夹的窗体
                string filepath = fbd.SelectedPath;                                            //返回所选中文件夹所在的路径
                tb1.Text = filepath;                                                           //将文件夹的路径显示在界面的指定文本框中
                System.IO.DirectoryInfo pathname = new System.IO.DirectoryInfo(filepath);      //为了获得该路径的根目录
                string filename = pathname.Name;                                               //获得该根目录下文件夹的名称
                m_connectToAccess = new ConnectToAccess(filepath, filename);
                string sql = @"select * from ComponentsDb";
                m_connectToAccess.GetAddInformation(sql, lv1, "IndexName", "Name");
            }
        }

        private void CavityLibraryEditForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                m_cavityLibraryEditCmd.StopCommand();
            }
        }

        private void CavityLibraryEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_cavityLibraryEditCmd.StopCommand();
        }


    }
}
