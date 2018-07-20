using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace ValveBlockDesign
{
    internal partial class CavityLibraryAddForm : Form
    {
        private ConnectToAccess m_connectToAccess;
        private CavityLibraryAddCmd m_cavityLibraryAddCmd;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;

        private string IndexName;
        private string codenumber;
        private string codename;

        public CavityLibraryAddForm()
        {
            InitializeComponent();
        }
        public CavityLibraryAddForm(CavityLibraryAddCmd cavityLibraryAddCmd)
        {
            m_cavityLibraryAddCmd = cavityLibraryAddCmd;
            InitializeComponent();
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName + "\\CavityLibrary";
        }
        private void CavityLibraryAddForm_Load(object sender, EventArgs e)
        {

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            m_cavityLibraryAddCmd.StopCommand();
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)     //用于对插入元件特征进行类型分类显示不同输入参数
       {
           if (comboType.Text == "板式阀通油孔" || comboType.Text == "二通插装孔")
           {
               groBfacedim.Enabled = true;
               groBface.Enabled = true;
               if (comboType.Text == "板式阀通油孔")
               {
                   groBcarvalve.Enabled = false;
               }
           }
           else
           {
               groBfacedim.Enabled = false;
               groBface.Enabled = false;
               if (comboType.Text == "螺纹插装孔")
               {
                   groBcarvalve.Enabled = true;
               }
               else
               {
                   groBcarvalve.Enabled = false;
               }
           }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            
            IndexName =cmbIndexName.Text.ToString();
            codenumber =combcodenumber.Text.ToString();
            codename =tb1.Text.ToString();
            string sql = "insert into " + IndexName + "(索引编号,编码) values('" + codenumber + "','" + codename + "')";
            m_connectToAccess = new ConnectToAccess(deFaultpath, "CavityLibrary");
            if (m_connectToAccess.InsertInformation(sql))
            {
                foreach (Control ctr in this.groBcavitydim.Controls)       //输入孔的尺寸参数
               {
                     if (ctr is TextBox)
                     {
                         //TextBox ctr1=new ctr as TextBox;
                         if ((ctr as TextBox).Text.Length!= 0)
                        { 
                            string sqlupdate = "update " + IndexName + " set " + (ctr as TextBox).Name + "='" + (ctr as TextBox).Text + "' where 编码 ='" + codename + "'";
                            if (m_connectToAccess.UpdateInformation(sqlupdate))
                                 { };
                         }
                     }
                }
                foreach (Control ctr in this.groBThread.Controls)           //输入螺纹的参数信息
                {
                    if (ctr is TextBox)
                    {
                        //TextBox ctr1=new ctr as TextBox;
                        if ((ctr as TextBox).Text.Length!= 0)
                        {
                            string sqlupdate = "update " + IndexName + " set " + (ctr as TextBox).Name + "='" + (ctr as TextBox).Text + "' where 编码 ='" + codename + "'";
                            if (m_connectToAccess.UpdateInformation(sqlupdate))
                                { };
                        }
                    }
                }
                foreach (Control ctr in this.groBcarvalve.Controls)        //输入油孔的参数信息
                {
                    if (ctr is TextBox)
                    {
                        //TextBox ctr1=new ctr as TextBox;
                        if ((ctr as TextBox).Text.Length!= 0)
                        {
                            string sqlupdate = "update " + IndexName + " set " + (ctr as TextBox).Name + "='" + (ctr as TextBox).Text + "' where 编码 ='" + codename + "'";
                            if (m_connectToAccess.UpdateInformation(sqlupdate))
                               { };
                        }
                    }
                }
                if (groBfacedim.Enabled == true)                             //输入安装面的油孔位置信息
                {
                    int i = 0;
                    while (i < combcodenumber.Items.Count && combcodenumber.Text.ToString() != combcodenumber.Items[i].ToString())
                    {
                        i++;
                    }
                    if (combcodenumber.Text.ToString() == combcodenumber.Items[i].ToString())
                    {
                        EditGrouFaceDim();
                    }
                    else
                    {
                        AddNewGrouFaceDim();
                    }
                }
                MessageBox.Show("插入新元件成功");
            }
        }

        private void btninsertpicture_Click(object sender, EventArgs e)//用于插入安装面示意图图片
        {
            OpenFileDialog ofdpictfoot = new OpenFileDialog();
            ofdpictfoot.Title = "选择安装面示意图";
            ofdpictfoot.Filter = "|*.bmp";
            
            string IndexName=combcodenumber.Text.ToString();
            if (ofdpictfoot.ShowDialog()==DialogResult.OK)
            {
                string filexmlname = ofdpictfoot.FileName;
                string filexmlpath = ofdpictfoot.InitialDirectory;
                string path = filexmlpath + filexmlname;
                string filefootprint = path;
                Bitmap myBitmapfootp = new Bitmap(filefootprint);
                picBface.Image = myBitmapfootp;
                picBface.Image.Save(deFaultpath + IndexName+"Footprint.bmp");
            }
            else
            {
                MessageBox.Show("请选择图片文件");
            }
            
        }

        private void btninsertpicture2_Click(object sender, EventArgs e)//用于插入单一孔侧视图示意图图片
        {
            OpenFileDialog ofdpictcavity = new OpenFileDialog();
            ofdpictcavity.Title = "选择单一孔侧视图示意图";
            ofdpictcavity.Filter = "|*.bmp";

            string IndexName = combcodenumber.Text.ToString();
            if (ofdpictcavity.ShowDialog() == DialogResult.OK)
            {
                string filexmlname = ofdpictcavity.FileName;
                string filexmlpath = ofdpictcavity.InitialDirectory;
                string path = filexmlpath + filexmlname;
                string filecavity = path;
                Bitmap myBitmapcavity = new Bitmap(filecavity);
                picBcavity.Image = myBitmapcavity;
                picBcavity.Image.Save(deFaultpath + IndexName + "Cavity.bmp");
            }
            else
            {
                MessageBox.Show("请选择图片文件");
            }
        }

        private void CavityLibraryAddForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                m_cavityLibraryAddCmd.StopCommand();
            }
        }

        private void CavityLibraryAddForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_cavityLibraryAddCmd.StopCommand();
        }

        private void cmbIndexName_TextChanged(object sender, EventArgs e)
        {
            string IndexName = cmbIndexName.Text.ToString();
            string sql = @"select 索引编号 from " + IndexName + " group by 索引编号";
            string[] getresult = new string[25];
            int i = 0;
            m_connectToAccess = new ConnectToAccess(deFaultpath, "CavityLibrary");
            m_connectToAccess.GetInformation(sql, "索引编号", out getresult);
            while (getresult[i] != null)
            {
                combcodenumber.Items.Add(getresult[i]);
                i++;
            }
        }

        private void combcodenumber_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            while(i<combcodenumber.Items.Count)
            {
                if(combcodenumber.Text.ToString()==combcodenumber.Items[i].ToString())
                {
                    if (groBface.Enabled ==true)
                    {
                        Bitmap myBitmapfootp = new Bitmap(deFaultpath + combcodenumber.Text.ToString() + "Footprint.bmp");
                        picBface.Image = myBitmapfootp;
                    }
                    Bitmap myBitmapcavity = new Bitmap(deFaultpath  + combcodenumber.Text.ToString() + "Cavity.bmp");
                    picBcavity.Image = myBitmapcavity;
                }
                i++;
            }
        }

        private void AddNewGrouFaceDim()//创建一个新的油孔位置信息表
        {
            string sqlcreate = @"CREATE TABLE " + codenumber + "Footprint(ID AUTOINCREMENT,油孔名称 TEXT(50),Dia" + codename + " TEXT(50),X" + codename + " TEXT(50),Y" + codename + " TEXT(50))";
            m_connectToAccess.CreateTable(sqlcreate);
            foreach (DataGridViewRow dr in dataGridViewdim.Rows)
            {
                dataGridViewdim.AllowUserToAddRows = false;
                string sqlupdate1 = "update " + IndexName + " set " + dr.Cells[0].Value.ToString() + "dmax='" + dr.Cells[1].Value.ToString() + "' where 编码 ='" + codename + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdate1))
                { };
                string sqlupdate2 = "update " + IndexName + " set " + dr.Cells[0].Value.ToString() + "coordX='" + dr.Cells[2].Value.ToString() + "' where 编码 ='" + codename + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdate2))
                { };
                string sqlupdate3 = "update " + IndexName + " set " + dr.Cells[0].Value.ToString() + "coordY='" + dr.Cells[3].Value.ToString() + "' where 编码 ='" + codename + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdate3))
                { };
                string sqlinsert = "insert into " + codenumber + "Footprint(油孔名称,Dia" + codename + ",X" + codename + ",Y" + codename + ") values('" + dr.Cells[0].Value.ToString() + "','" + dr.Cells[1].Value.ToString() + "','" + dr.Cells[2].Value.ToString() + "','" + dr.Cells[3].Value.ToString() + "')";
                if (m_connectToAccess.InsertInformation(sqlinsert))
                {
                    MessageBox.Show("坐标位置信息添加成功");
                }
            }
        }

        private void EditGrouFaceDim()//在原来的油孔位置信息表中添加数据
        {
            string sqlcreate = @"ALTER TABLE " + codenumber + "Footprint ADD Dia" + codename + " TEXT(50),X" + codename + " TEXT(50),Y" + codename + " TEXT(50))";
            m_connectToAccess.CreateTable(sqlcreate);
            foreach (DataGridViewRow dr in dataGridViewdim.Rows)
            {
                dataGridViewdim.AllowUserToAddRows = false;
                string sqlupdate1 = "update " + IndexName + " set " + dr.Cells[0].Value.ToString() + "dmax='" + dr.Cells[1].Value.ToString() + "' where 编码 ='" + codename + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdate1))
                { };
                string sqlupdate2 = "update " + IndexName + " set " + dr.Cells[0].Value.ToString() + "coordX='" + dr.Cells[2].Value.ToString() + "' where 编码 ='" + codename + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdate2))
                { };
                string sqlupdate3 = "update " + IndexName + " set " + dr.Cells[0].Value.ToString() + "coordY='" + dr.Cells[3].Value.ToString() + "' where 编码 ='" + codename + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdate3))
                { };
                string sqlupdateDia = "update " + codenumber + "Footprint set Dia" + codename + "='" + dr.Cells[1].Value.ToString() + "' where 油孔名称 ='" + dr.Cells[0].Value.ToString() + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdateDia))
                {
                    MessageBox.Show("坐标位置信息添加成功");
                }
                string sqlupdateX = "update " + codenumber + "Footprint set X" + codename + "='" + dr.Cells[2].Value.ToString() + "' where 油孔名称 ='" + dr.Cells[0].Value.ToString() + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdateX))
                {
                    MessageBox.Show("坐标位置信息添加成功");
                }
                string sqlupdateY = "update " + codenumber + "Footprint set Y" + codename + "='" + dr.Cells[3].Value.ToString() + "' where 油孔名称 ='" + dr.Cells[0].Value.ToString() + "'";
                if (m_connectToAccess.UpdateInformation(sqlupdateY))
                {
                    MessageBox.Show("坐标位置信息添加成功");
                }
            }
        }
    }
}
