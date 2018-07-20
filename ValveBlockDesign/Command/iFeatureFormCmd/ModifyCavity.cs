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
using Inventor;

namespace ValveBlockDesign
{
    internal partial class ModifyCavity : Form
    {
        private EditCavityLibrary m_EditCavityLibrary;
        string codingname;                                              //编码号
        string codingnumber;                                            //索引号
        string pathname;                                                //文件夹路径名
        string indexname;                                               //分类编号是为了后面界面获得前面信息
        string formname;
        string typeright;
        public ModifyCavity()
        {
            InitializeComponent();
        }

        public ModifyCavity(string codingname,string codingnumber,string pathname,string indexname,EditCavityLibrary formlibrary)//为了参数的传递的构造函数
        {
            InitializeComponent();
            this.codingname = codingname;
            this.codingnumber = codingnumber;
            this.pathname = pathname;
            this.indexname = indexname;

            m_EditCavityLibrary = formlibrary;
        }

        private void CavityLook()//用于多级阶梯孔的阶梯参数显示的函数
        {
            System.IO.DirectoryInfo pathnamenew = new System.IO.DirectoryInfo(pathname);
            formname = pathnamenew.Name;
            System.Data.OleDb.OleDbConnection conn2 = new System.Data.OleDb.OleDbConnection();
            conn2.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + pathname + @"\" + formname + @".mdb";
            try
            {
                conn2.Open();
                string sql4 = @"select * from " + indexname + " where " + indexname + ".编码='" + codingname + @"'";
                OleDbCommand sqlcmd4 = new OleDbCommand(sql4, conn2);
                OleDbDataReader dbreader4 = sqlcmd4.ExecuteReader();
                while (dbreader4.Read())
                {
                    int num = 0;
                    while (num <= 8)
                    {
                        string number = num.ToString();
                        foreach (Control ctr in groBcavitydim.Controls)
                        {
                            if (ctr.Name.Equals("Dia" + number))
                                ctr.Text = dbreader4["Dia" + number].ToString();
                            if (ctr.Name.Equals("Depth" + number))
                                ctr.Text = dbreader4["Depth" + number].ToString();
                            if (ctr.Name.Equals("Angle" + number))
                                ctr.Text = dbreader4["Angle" + number].ToString();
                        }
                        num++;
                    }
                    MaxDrillDia.Text = dbreader4["MaxDrillDia"].ToString();
                    ThreadStep.Text = dbreader4["ThreadStep"].ToString();    //用于孔的螺纹信息的显示
                    ThreadSize.Text = dbreader4["ThreadSize"].ToString();
                    ThreadPith.Text = dbreader4["ThreadPith"].ToString();
                    ThreadClass.Text = dbreader4["ThreadClass"].ToString();
                }
            }
            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn2.Close();
            }
        }
        private void FootprintLook()//用于安装界面参数的显示函数
        {
            System.IO.DirectoryInfo pathnamenew = new System.IO.DirectoryInfo(pathname);
            formname = pathnamenew.Name;
            System.Data.OleDb.OleDbConnection conn2 = new System.Data.OleDb.OleDbConnection();
            conn2.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + pathname + @"\" + formname + @".mdb";
            try
            {
                conn2.Open();
                string sql3 = @"select * from " + codingnumber + "Footprint";
                OleDbCommand sqlcmd3 = new OleDbCommand(sql3, conn2);
                OleDbDataReader dbreader3 = sqlcmd3.ExecuteReader();
                lvface.Items.Clear();
                while (dbreader3.Read())//用于安装界面参数的显示
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems[0].Text = dbreader3["油孔名称"].ToString();
                    li.SubItems.Add(dbreader3["X" + codingname].ToString());
                    li.SubItems.Add(dbreader3["Y" + codingname].ToString());
                    lvface.Items.Add(li);
                }
            }
            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn2.Close();
            }
        }

        private void CartiagePortLook()//用于预览插装孔的油孔的参数显示
        {
            System.IO.DirectoryInfo pathnamenew = new System.IO.DirectoryInfo(pathname);
            formname = pathnamenew.Name;
            System.Data.OleDb.OleDbConnection conn2 = new System.Data.OleDb.OleDbConnection();
            conn2.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + pathname + @"\" + formname + @".mdb";
            try
            {
                conn2.Open();
                string sql2 = @"select * from " + indexname + " where " + indexname + ".编码='" + codingname + @"'";//元件特征表的搜索
                OleDbCommand sqlcmd2 = new OleDbCommand(sql2, conn2);
                OleDbDataReader dbreader2 = sqlcmd2.ExecuteReader();
                while (dbreader2.Read())                                
                {
                    PortNumber.Text = dbreader2["PortNumber"].ToString(); 
                    int j = 1;
                    while (j <= int.Parse(PortNumber.Text))
                    {
                        string i = j.ToString();
                        foreach (Control c in groBcarvalve.Controls)
                        {
                            if (c.Name.Equals("PortDia" + i))
                                c.Text = dbreader2["PortDia" + i].ToString();
                            if (c.Name.Equals("PortDepth" + i))
                                c.Text = dbreader2["PortDepth" + i].ToString();
                            if (c.Name.Equals("PortConnection" + i))
                                c.Text = dbreader2["PortConnection" + i].ToString();
                        }
                        j++;
                    }
                }
            }
            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn2.Close();
            }
        }
        private void FootprintCavityLook()//用于安装面指定单一油孔的参数显示
        {
            System.IO.DirectoryInfo pathnamenew = new System.IO.DirectoryInfo(pathname);
            formname = pathnamenew.Name;
            System.Data.OleDb.OleDbConnection conn2 = new System.Data.OleDb.OleDbConnection();
            conn2.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + pathname + @"\" + formname + @".mdb";
            try
            {
                conn2.Open();
                string sql5 = @"select * from " + indexname + " where " + indexname + ".编码='" + codingname + @"'";//元件特征表的搜索
                OleDbCommand sqlcmd5 = new OleDbCommand(sql5, conn2);
                OleDbDataReader dbreader5 = sqlcmd5.ExecuteReader();
                while (dbreader5.Read())
                {
                                                     //预览视图的油孔名称
                   Dia0.Text = dbreader5["Pdmax"].ToString();
                   Depth0.Text = dbreader5["PortDepth"].ToString();
                   Angle0.Text = dbreader5["Angle"].ToString();
                   MaxDrillDia.Text = dbreader5["MaxDrillDia"].ToString();
                }
            }
            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn2.Close();
            }
        }
        private void CavityPicture()//特征孔的图形显示的函数
        {
            string filecavity = pathname + @"\" + codingnumber + @"Cavity.bmp";
            Bitmap myBitmapcavity = new Bitmap(filecavity);
            picBcavity.Image = myBitmapcavity; 
        }
        private void FootprintPicture()//安装平面的图形显示的函数
        {
            string filefootprint = pathname + @"\" + codingnumber + @"Footprint.bmp";
            Bitmap myBitmapfootp = new Bitmap(filefootprint);
            picBface.Image = myBitmapfootp;
        }
        private void ModifyCavity_Load(object sender, EventArgs e)
        {
            CavityPicture();
            tb1.Text = codingname;//编码号
            tb2.Text = codingnumber;//索引编号
            System.IO.DirectoryInfo pathnamenew = new System.IO.DirectoryInfo(pathname);
            formname = pathnamenew.Name;
            System.Data.OleDb.OleDbConnection conn2 = new System.Data.OleDb.OleDbConnection();
            conn2.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + pathname + @"\" + formname + @".mdb";
            try
            {
                conn2.Open();
                string sql1 =@"select * from ComponentsDb where ComponentsDb.IndexName='" + indexname+@"'";//元件类型表的搜索
                OleDbCommand sqlcmd1 = new OleDbCommand(sql1, conn2);
                OleDbDataReader dbreader1 = sqlcmd1.ExecuteReader();
                while (dbreader1.Read())
                {
                    typeright = dbreader1["CavityType"].ToString();//用于界面上的孔类型的显示
                    tbtype.Text = typeright;
                    if (dbreader1["Footprint"].ToString() == "No")//用于判断安装面界面是否显示
                    {
                        CavityLook();
                        groBface.Enabled = false;
                        groBfacedim.Enabled = false;
                        CartiagePortLook();
                    }
                    else
                    {
                        FootprintPicture();
                        FootprintCavityLook();
                        FootprintLook();
                    }
                }
            }
            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn2.Close();
            }
            if (typeright == "二通插装孔"||typeright=="螺纹插装孔")            //用于判断插装阀组合框是否显示
            {
                groBcarvalve.Enabled = true;
            }
            else
            {
                groBcarvalve.Enabled = false;
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void ModifyCavity_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_EditCavityLibrary.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            m_EditCavityLibrary.Show();
        }

        private void tb2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
