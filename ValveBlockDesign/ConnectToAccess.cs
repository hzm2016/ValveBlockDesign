using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.IO;
using Inventor;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    class ConnectToAccess
    {
        public string filepath=null;//数据库的路径
        public string filename=null;//数据库文件夹的名称
        public string codename=null;//该元件的型号编码
        public string indexname=null;//索引表格名称即类型编号
        public string codenumber=null;//索引编号
        public string checkfootprint=null;

        public ConnectToAccess(string filepath, string filename)
        {
            this.filepath = filepath;
            this.filename = filename;
        }

        public ConnectToAccess(string filepath, string filename, string codename, string indexname, string codenumber)
        {
            this.filepath = filepath;
            this.filename = filename;
            this.codename = codename;
            this.indexname = indexname;
            this.codenumber = codenumber;
        }

        public string SelectConnectToAccess(string oInputName)   //用于连接到数据库返回所要查找的值oInputName为查找的字段名称
        {
            string Valve = null;
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();//连接到Access数据库
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename + @".mdb";
            try
            {
                conn.Open();
                string sql = @"select * from " + indexname + " where " + indexname + ".编码='" + codename + @"'";
                OleDbCommand sqlcmd = new OleDbCommand(sql, conn);
                OleDbDataReader dbreader = sqlcmd.ExecuteReader();
                while (dbreader.Read())
                {
                    Valve = dbreader[oInputName].ToString();
                }
            }

            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                conn.Close();
            }
            return Valve;
        }

        public string GetSingleInformation(string sql, string oInputName)
        {
            string Valve = null;
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();//连接到Access数据库
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename + @".mdb";
            try
            {
                conn.Open();
                OleDbCommand sqlcmd = new OleDbCommand(sql, conn);
                OleDbDataReader dbreader = sqlcmd.ExecuteReader();
                while (dbreader.Read())
                {
                    Valve = dbreader[oInputName].ToString();
                }
            }

            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                conn.Close();
            }
            return Valve;
        }

        public void GetInformation(string sql, string selectName, out string[] selectResult)//用于连接到数据库
        {
            selectResult = new string[25];
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();//连接到Access数据库
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename + @".mdb";
            try
            {
                conn.Open();
                OleDbCommand sqlcmd = new OleDbCommand(sql, conn);
                OleDbDataReader dbreader = sqlcmd.ExecuteReader();
                int i = 0;
                while (dbreader.Read())
                {
                    selectResult[i] = dbreader[selectName].ToString();
                    i++;
                }
            }

            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        public void GetAddInformation(string sql, ListView lv,string dbname1,string dbname2)                              //用于将查询的结果输入到Listview控件中
        {
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();//连接到Access数据库
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename + @".mdb";
            try
            {
                conn.Open();
                OleDbCommand sqlcmd = new OleDbCommand(sql, conn);
                OleDbDataReader dbreader = sqlcmd.ExecuteReader();
                lv.Items.Clear();
                while (dbreader.Read())
                {
                    ListViewItem lj = new ListViewItem();
                    lj.SubItems.Clear();
                    lj.SubItems[0].Text = dbreader[dbname1].ToString();
                    lj.SubItems.Add(dbreader[dbname2].ToString());
                    lv.Items.Add(lj);
                }
            }

            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        public bool InsertInformation(string sql)
        {
            bool saved = true;
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();//连接到Access数据库
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename + @".mdb";
            try
            {
                conn.Open();
                OleDbCommand sqlcmd = new OleDbCommand(sql, conn);
                sqlcmd.ExecuteNonQuery();
            }

            catch (OleDbException ex)
            {
                string s = ex.Message;
                MessageBox.Show(s,"Failed to connect to data source",MessageBoxButtons.OK,MessageBoxIcon.Error);
                saved = false;
            }
            finally
            {
                conn.Close();
            }
            return saved;
        }

        public bool UpdateInformation(string sql)
        {
            bool saved = true;
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();//连接到Access数据库
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename + @".mdb";
            try
            {
                conn.Open();
                OleDbCommand sqlcmd = new OleDbCommand(sql, conn);
                sqlcmd.ExecuteNonQuery();
            }

            catch (OleDbException ex)
            {
                string s = ex.Message;
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
                saved = false;
            }
            finally
            {
                conn.Close();
            }
            return saved;
        }

        public void CreateTable(string sql)
        {
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();//连接到Access数据库
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename + @".mdb";
            try
            {
                conn.Open();
                OleDbCommand sqlcmd = new OleDbCommand(sql, conn);
                sqlcmd.ExecuteNonQuery();
            }

            catch (OleDbException ex)
            {
                string s = ex.Message;
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                conn.Close();
                //MessageBox.Show("创建表格成功");
            }

        }

    }
}
