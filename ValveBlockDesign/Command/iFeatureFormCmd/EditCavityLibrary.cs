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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ValveBlockDesign
{
    internal partial class  EditCavityLibrary : Form
    {
        public string filepath;  //文件夹路径
        public string filename;
        public string filexmlpath;
        public string filexmlname;
        public string indexname;  //索引名称

        public string indexnameref;
        public string checkfootref;
        public string cavityTyperef;
        public string insertname;
        public string codingname;
        public string codingindex;
        private Inventor.Application m_inventorApplication;
        private iFeatureFormCmd m_ifeatureFormCmd;
        private ConnectToAccess m_connectToaccess;
        private string m_ClientId = "c29d5be2-c9f7-4783-9191-5070d4944568";
        private System.Reflection.Assembly assembly;
        
        private string deFaultpath;
        string[] resources;

        public EditCavityLibrary()
        {
            InitializeComponent();
        }
        public EditCavityLibrary(Inventor.Application application, iFeatureFormCmd ifeatureFormCmd)
        {
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName;
            m_inventorApplication = application;
            m_ifeatureFormCmd = ifeatureFormCmd;
            filepath = deFaultpath + "\\CavityLibrary";
            filename = "CavityLibrary";
            InitializeComponent();
            this.btninsert2.Enabled = false;
            this.btninsertsure.Enabled = false;
        }

        private void btnbrowse_Click(object sender, EventArgs e)//找到元件库所在的文件夹
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {                                                                           //显示浏览文件夹的窗体
                filepath = fbd.SelectedPath;                                            //返回所选中文件夹所在的路径
                tb1.Text = filepath;                                                    //将文件夹的路径显示在界面的指定文本框中
                System.IO.DirectoryInfo pathname = new System.IO.DirectoryInfo(filepath);//为了获得该路径的根目录
                filename = pathname.Name;                                                //获得该根目录下文件夹的名称
                this.ConnectToAccess(filepath, filename);
            }
        }

        private void ConnectToAccess(string filepath,string filename)//用于连接到数据库
        {
            
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();//连接到Access数据库
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename+ @".mdb";
            try
            {
                conn.Open();
                OleDbCommand sqlcmd = new OleDbCommand(@"select * from ComponentsDb", conn);
                OleDbDataReader dbreader = sqlcmd.ExecuteReader();
                lv1.Items.Clear();
                while (dbreader.Read()) 
                {
                    ListViewItem li = new ListViewItem();
                    li.SubItems.Clear();
                    li.SubItems[0].Text = dbreader["IndexName"].ToString();
                    li.SubItems.Add(dbreader["Name"].ToString());
                    lv1.Items.Add(li);
                }
            }

            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s,"Failed to connect to data source",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnscan_Click(object sender, EventArgs e)//是读取一类元件下面的所有类型
        {
            
            if (this.lv1.SelectedItems.Count != 1)
            {
                MessageBox.Show("请选择要浏览的元件库");
            }
            else
            {
                indexname = this.lv1.SelectedItems[0].SubItems[0].Text.ToString();
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=" + filepath + @"\" + filename + @".mdb";
                try
                {
                    conn.Open();
                    OleDbCommand sqlcmd2 = new OleDbCommand(@"select * from " + indexname, conn);
                    OleDbDataReader dbreader2 = sqlcmd2.ExecuteReader();
                    lv2.Items.Clear();                                        //重新选择后将原有信息清空
                    while (dbreader2.Read())
                    {
                        ListViewItem lj = new ListViewItem();
                        lj.SubItems.Clear();
                        lj.SubItems[0].Text = dbreader2["编码"].ToString();
                        lj.SubItems.Add(dbreader2["索引编号"].ToString());
                        lv2.Items.Add(lj);
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
        }

        private void btnsure_Click(object sender, EventArgs e)//主要用于将此界面的信息传递到下一界面
        {
            string coding1 = this.lv2.SelectedItems[0].SubItems[0].Text.ToString();
            string coding2 = this.lv2.SelectedItems[0].SubItems[1].Text.ToString();
            ModifyCavity moc = new ModifyCavity(coding1,coding2,filepath,indexname,this);
            moc.Show();
            this.Hide();
        }

        private void EditCavityLibrary_Load(object sender, EventArgs e)
        {
            this.tabConCavity.SelectTab(1);
        }

        private void btnBrowse2_Click(object sender, EventArgs e)//为了搜索二维原理图导入的信息
        {
            OpenFileDialog ofd = new OpenFileDialog();//打开存储二维原理图信息的xml文件
            ofd.Title = "选择原理图信息文件";
            //string directory = @"F:\";
            //ofd.InitialDirectory = directory;
            ofd.Filter = "|*.xml";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                filexmlname = ofd.FileName;
                filexmlpath = ofd.InitialDirectory;
                tb2.Text = filexmlpath + filexmlname;
                AddInformation(filexmlpath + filexmlname);
            }
            else
            {
                MessageBox.Show("请选择原理图文件");
            }
        }
        //检查该原理图中的元件在库里是否存在对应型号
        private string CheckIndexName(string checkIndexnumber,string name,ref string indexname,ref string checkFootprint,ref string cavityType)
        {
            string checkindexname = null;
            System.Data.OleDb.OleDbConnection conncheck = new System.Data.OleDb.OleDbConnection();
            conncheck.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source=F:\CavityLibrary\CavityLibrary.mdb";
            try
            {
                string indexcheck=null;
                string Footprint = null;
                string CavityType = null;
                conncheck.Open();
                string sqlcheck1 = @"select * from ComponentsDb where ComponentsDb.Name='" + name + @"'";
                OleDbCommand sqlcmdcheck1 = new OleDbCommand(sqlcheck1, conncheck);
                OleDbDataReader dbreader1= sqlcmdcheck1.ExecuteReader();
                while (dbreader1.Read())
                {
                    indexcheck = dbreader1["IndexName"].ToString();
                    Footprint = dbreader1["Footprint"].ToString();
                    CavityType = dbreader1["CavityType"].ToString();
                }
                string sqlcheck2 = @"select 索引编号 from " + indexcheck + " where " + indexcheck + ".编码='" + checkIndexnumber + @"'";
                OleDbCommand sqlcmdcheck2 = new OleDbCommand(sqlcheck2,conncheck);
                OleDbDataReader dbreader2 = sqlcmdcheck2.ExecuteReader();
                while (dbreader2.Read())
                {
                    checkindexname = dbreader2["索引编号"].ToString();
                }
                indexname = indexcheck;
                checkFootprint = Footprint;
                cavityType = CavityType;
            }

            catch (OleDbException ex)
            {
                string s = ex.ToString();
                MessageBox.Show(s, "Failed to connect to data source", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conncheck.Close();
            }
            return checkindexname;
        }

        private void AddInformation(string path)//用于加载二维原理图信息显示的函数
        {
            
            trViewcopnent.Nodes[0].Nodes.Clear();
            trViewcopnent.Nodes[1].Nodes.Clear();
            trViewNet.Nodes.Clear();
            trViewcopnent.LabelEdit = true;
            trViewNet.LabelEdit = true;
            string indexnameref = null;
            string checkFootprintref = null;
            string cavityTyperef = null;
            XDocument doc = XDocument.Load(path);
            IEnumerable<XElement> partNos = from cavity in doc.Descendants("Component")
                                            select cavity;
            foreach (var n in partNos)//读取元件清单
            {
                TreeNode node = new TreeNode((string)n.Element("元件标识"));
                trViewcopnent.Nodes[0].Nodes.Add(node);
                TreeNode node1 = new TreeNode((string)n.Element("型号编码"));
                node.Nodes.Add(node1);
                TreeNode node2 = new TreeNode((string)n.Element("元件名称"));
                node.Nodes.Add(node2);
                TreeNode node3 = new TreeNode("元件编号："+(string)n.Element("元件编号"));
                node.Nodes.Add(node3);
                TreeNode node4 = new TreeNode("油孔名称");
                node.Nodes.Add(node4);
                IEnumerable<XElement> partname = n.Element("油口名").Elements("名称");
                foreach (var s in partname)
                {
                    TreeNode node5 = new TreeNode((string)s);
                    node4.Nodes.Add(node5);
                }
                //用于找出原理图中不存在的元件并进行显示
                if (CheckIndexName((string)n.Element("型号编码"), (string)n.Element("元件名称"), ref indexnameref, ref checkFootprintref, ref cavityTyperef) == null)
                {
                    checkNumber.Items.Add((string)n.Element("元件标识"));
                    btnchange.Enabled = true;
                }
                else
                {
                    btnchange.Enabled = false;
                }
            }
            int i = 0;
            while (i < trViewcopnent.Nodes[0].Nodes.Count)
            {
                
                string name = trViewcopnent.Nodes[0].Nodes[i].Nodes[1].Text;
                string indexname = trViewcopnent.Nodes[0].Nodes[i].Nodes[0].Text;
                m_connectToaccess = new ConnectToAccess(@"F:\CavityLibrary", "CavityLibrary");
                string sql = @"select Footprint from ComponentsDb where ComponentsDb.Name='" + name + @"'";
                string result = m_connectToaccess.GetSingleInformation(sql, "Footprint");
                if (result == "Yes")
                {
                    trViewcopnent.Nodes[0].Nodes[i].ImageIndex = 1;
                    trViewcopnent.Nodes[0].Nodes[i].SelectedImageIndex = 3;
                }
                else
                {
                    trViewcopnent.Nodes[0].Nodes[i].ImageIndex = 0;
                    trViewcopnent.Nodes[0].Nodes[i].SelectedImageIndex = 2;
                }
                i++;
            }
            trViewcopnent.Nodes[0].Expand();
            IEnumerable<XElement> partNos1 = from port in doc.Descendants("Port")
                                            select port;
            foreach (var l in partNos1)//读取油孔清单
            {
                TreeNode nodeportnumber = new TreeNode((string)l.Element("油孔标识"));
                trViewcopnent.Nodes[1].Nodes.Add(nodeportnumber);
                
                TreeNode nodeport = new TreeNode((string)l.Element("型号编码"));
                nodeportnumber.Nodes.Add(nodeport);
                TreeNode nodeportname = new TreeNode((string)l.Element("油孔类型"));
                nodeportnumber.Nodes.Add(nodeportname);
                trViewcopnent.Nodes[1].ImageIndex = 0;
                trViewcopnent.Nodes[1].Expand();
            } 
        
            IEnumerable<XElement> partNos2 = from net in doc.Descendants("Net")
                                             select net;
            foreach (var m in partNos2)//读取网络连接清单
            {
                TreeNode nodenet = new TreeNode((string)m.Element("网络名"));
                trViewNet.Nodes.Add(nodenet);
                int nodenetindex;
                string Name = (string)m.Element("网络名");
                switch (Name)
                {
                    case "NET1":
                        nodenetindex = 0;
                        break;
                    case "NET2":
                        nodenetindex = 1;
                        break;
                    case "NET3":
                        nodenetindex = 2;
                        break;
                    case "NET4":
                        nodenetindex = 3;
                        break;
                    case "NET5":
                        nodenetindex = 4;
                        break;
                    case "NET6":
                        nodenetindex = 5;
                        break;
                    case "NET7":
                        nodenetindex = 6;
                        break;
                    case "NET8":
                        nodenetindex = 7;
                        break;
                    case "NET9":
                        nodenetindex = 8;
                        break;
                    case "NET10":
                        nodenetindex = 9;
                        break;
                    case "NET11":
                        nodenetindex = 10;
                        break;
                    case "NET12":
                        nodenetindex = 11;
                        break;
                    case "NET13":
                        nodenetindex = 12;
                        break;
                    case "NET14":
                        nodenetindex = 13;
                        break;
                    case "NULLNET":
                        nodenetindex = 14;
                        break;
                    default:
                        nodenetindex = 14;
                        break;
                }
                nodenet.ImageIndex = nodenetindex;
                IEnumerable<XElement> partnos3 = m.Element("接口名").Elements("名称");
                foreach (var l in partnos3)
                {
                    TreeNode nodenet2 = new TreeNode((string)l);
                    nodenet. Nodes.Add(nodenet2);
                    nodenet2.ImageIndex = 15;
                }   
            }  
        }

        private void IconPictureInitial()
        {
            //get names of all the sources in the assenbly
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            resources = assembly.GetManifestResourceNames();
        }

        private void  AddTreeBrowerNode(ClientNodeResources oRscs, string NetName, int number)
        {
            ClientNodeResource oRsc;
            switch (NetName)
            {
                case "NET1":
                    oRsc = oRscs.ItemById(m_ClientId,2);
                    break;
                case "NET2":
                    oRsc = oRscs.ItemById(m_ClientId, 3);
                    break;
                case "NET3":
                    oRsc = oRscs.ItemById(m_ClientId, 4);
                    break;
                case "NET4":
                    oRsc = oRscs.ItemById(m_ClientId, 5);
                    break;
                case "NET5":
                    oRsc = oRscs.ItemById(m_ClientId, 6);
                    break;
                case "NET6":
                    oRsc = oRscs.ItemById(m_ClientId,7);
                    break;
                case "NET7":
                    oRsc = oRscs.ItemById(m_ClientId,8);
                    break;
                case "NET8":
                    oRsc = oRscs.ItemById(m_ClientId,9);
                    break;
                case "NET9":
                    oRsc = oRscs.ItemById(m_ClientId,10);
                    break;
                case "NET10":
                    oRsc = oRscs.ItemById(m_ClientId,11);
                    break;
                case "NET11":
                    oRsc = oRscs.ItemById(m_ClientId,12);
                    break;
                case "NET12":
                    oRsc = oRscs.ItemById(m_ClientId,13);
                    break;
                default:
                    oRsc = oRscs.ItemById(m_ClientId, 3);
                    break;
            }

            Document oDoc = default(Document);
            oDoc = m_inventorApplication.ActiveDocument;

            BrowserPanes oPanes = default(BrowserPanes);
            oPanes = oDoc.BrowserPanes;
            BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(NetName, number, oRsc);

            Inventor.BrowserNode topNode = oPanes["油路"].TopNode;
            topNode.AddChild(oDef1);
            
        }

        private void AddTreePortName(string NetName, int number,int NetNumber)
        {
            Document oDoc = default(Document);
            oDoc = m_inventorApplication.ActiveDocument;

            BrowserPanes oPanes = default(BrowserPanes);
            oPanes = oDoc.BrowserPanes;

            ClientNodeResources oRscs = oPanes.ClientNodeResources;
            //单一孔特征的图标显示
            ClientNodeResource oRsc = oRscs.ItemById(m_ClientId,14);
            BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(NetName, number, oRsc);

            Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[NetNumber];
            node.AddChild(oDef1);
        }

        private void deleteBrowerNodes()
        {
            Document oDoc = default(Document);
            oDoc = m_inventorApplication.ActiveDocument;

            BrowserPanes oPanes = default(BrowserPanes);
            oPanes = oDoc.BrowserPanes;
            int delete = 17;
            ClientBrowserNodeDefinition deleteoDef = oPanes.GetClientBrowserNodeDefinition(m_ClientId, delete);
            while (deleteoDef != null)
            {
                deleteoDef.Delete();
                delete++;
                deleteoDef = oPanes.GetClientBrowserNodeDefinition(m_ClientId, delete);
            }
            foreach (Inventor.BrowserNode node in oPanes["油路"].TopNode.BrowserNodes)
            {
                if (node.Visible==false)
                {
                    node.Visible = true;
                }
            }
            oPanes["油路"].Update();
            oPanes["油路"].Activate();
        }

        private void btninsert2_Click(object sender, EventArgs e)//用于原理图中筛选好元件的插入每次单击一个元件进行插入
        {
            if (trViewcopnent.SelectedNode != null)
            {
                indexnameref = null;
                checkfootref = null;//用于返回该元件的Footprint的特征
                cavityTyperef = null;
                insertname = trViewcopnent.SelectedNode.Nodes[1].Text;
                codingname = trViewcopnent.SelectedNode.Nodes[0].Text;
                codingindex = CheckIndexName(codingname, insertname, ref indexnameref, ref checkfootref,ref cavityTyperef);//获得选中元件的索引编号
                indexname = indexnameref;//利用形参获得表格的名称
                groupinformation.Text = "选择插入基准";
                taballinformation.SelectTab(1);
                checkBoxFace.Checked = true;
            }
            else if (groupinformation.Text == "插入新元件请输入元件ID")
            {
                groupinformation.Text = "选择插入基准";
                taballinformation.SelectTab(1);
                checkBoxFace.Checked = true;
             }
            else
            {
                MessageBox.Show("请选择插入元件");
                return;
            }
            btninsert2.Enabled = false;
        }

        private void btnchange_Click(object sender, EventArgs e)//用于更改型号不存在的元件
        {
            if (checkNumber.Items != null && checkNumber.SelectedItem != null)
            {
                this.tabConCavity.SelectTab(0);
            }
            else
            {
                MessageBox.Show("请选择需要更改的元件");
            }
        }

        private void btnsurechange_Click(object sender, EventArgs e)//确认更改元件型号
        {
            string changeindexname=this.lv2.SelectedItems[0].SubItems[0].Text.ToString();
            this.tabConCavity.SelectTab(1);
            int j = int.Parse(checkNumber.SelectedItem.ToString());
            trViewcopnent.Nodes[j-1].Nodes[0].Text = changeindexname;
            int i = checkNumber.SelectedIndex;
            checkNumber.Items.RemoveAt(i);
        }

        private void btninsertsure_Click(object sender, EventArgs e)//执行插入元件特征命令
        {
            Inventor.Point projectedpoint;
            System.IO.DirectoryInfo pathnamenew = new System.IO.DirectoryInfo("F:\\CavityLibrary");
            string formname = pathnamenew.Name;
            //string iFeatureName = tbID.Text;
            m_ifeatureFormCmd.AddiFeatureFormCmd(filepath, formname, codingname, codingindex, indexname, checkfootref, out projectedpoint);

            m_ifeatureFormCmd.ExecuteCommand();
            m_ifeatureFormCmd.UpdateCommandStatus();
            checkBoxFace.Checked = false;
            tbID.Clear();
        }
        //---------------------------------------------------------------------------
        //用于从元件库直接插入元件
        private void btninsert_Click(object sender, EventArgs e)
        {
            tbID.Clear();
            tbID.Focus();
            if (lv2.SelectedItems.Count != 0)
            {
                indexnameref = null;
                checkfootref = null;//用于返回该元件的Footprint的特征
                cavityTyperef = null;
                insertname = lv1.SelectedItems[0].SubItems[1].Text;
                codingname = lv2.SelectedItems[0].SubItems[0].Text;
                codingindex = CheckIndexName(codingname, insertname, ref indexnameref, ref checkfootref,ref cavityTyperef);//获得选中元件的索引编号
                indexname = indexnameref;//利用形参获得表格的名称
                groupinformation.Text = "插入新元件请输入元件ID";
                this.tabConCavity.SelectTab(1);
                dataportinformation.Rows.Clear();
                if (cavityTyperef == "端油孔" || cavityTyperef == "工艺油孔")
                {
                    int index = dataportinformation.Rows.Add();
                    dataportinformation.Rows[index].Cells[0].Value = "1";
                    dataportinformation.Rows[index].Cells[1].Value = "NULLNET";
                }
                else if(cavityTyperef=="螺纹插装孔")
                {
                    m_connectToaccess=new ConnectToAccess(filepath, filename, codingname, indexname, codingindex);
                    int portnum = int.Parse(m_connectToaccess.SelectConnectToAccess("PortNumber"));
                    int index=0;
                    while (index <portnum)
                    {
                        index = dataportinformation.Rows.Add();
                        dataportinformation.Rows[index].Cells[1].Value = "NULLNET";
                        dataportinformation.Rows[index].Cells[0].Value = ++index;
                    }
                }
                else if (cavityTyperef == "板式阀通油孔")
                {
                    int index;
                    m_connectToaccess = new ConnectToAccess(filepath, filename, codingname, indexname, codingindex);
                    int portcount = int.Parse(m_connectToaccess.SelectConnectToAccess("PortCount"));
                    int portindex = 1;
                    while (portindex <= portcount)
                    {
                        string portName = m_connectToaccess.SelectConnectToAccess("PortName" + portindex.ToString());
                        portindex++;
                        index = dataportinformation.Rows.Add();
                        dataportinformation.Rows[index].Cells[0].Value = portName;
                        dataportinformation.Rows[index].Cells[1].Value = "NULLNET";
                    }
                }
                else if (cavityTyperef == "二通插装孔")
                {
                    m_connectToaccess = new ConnectToAccess(filepath, filename, codingname, indexname, codingindex);
                    int portnum = int.Parse(m_connectToaccess.SelectConnectToAccess("PortNumber"));
                    int index = 0;
                    while (index < portnum)
                    {
                        index = dataportinformation.Rows.Add();
                        dataportinformation.Rows[index].Cells[1].Value = "NULLNET";
                        dataportinformation.Rows[index].Cells[0].Value = ++index;
                    }
                    int portcount=int.Parse (m_connectToaccess.SelectConnectToAccess("PortCount"));
                    int portindex=1;
                    while (portindex <= portcount)
                    {
                        string portName = m_connectToaccess.SelectConnectToAccess("PortName"+portindex.ToString());
                        portindex++;
                        index = dataportinformation.Rows.Add();
                        dataportinformation.Rows[index].Cells[0].Value = portName;
                        dataportinformation.Rows[index].Cells[1].Value = "NULLNET";
                    }
                }
                else
                {
                    //不包括油孔的单一孔特征
                }
                taballinformation.SelectTab(0);
            }
            else
            {
                MessageBox.Show("请选择插入的元件");
            }
        }

        private void trViewcopnent_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
            taballinformation.SelectTab(0);
            if (trViewcopnent.SelectedNode == trViewcopnent.Nodes[0] || trViewcopnent.SelectedNode == trViewcopnent.Nodes[1])
            {
                trViewcopnent.SelectedNode.Expand();
                groupinformation.Text = "请选择元件！";
                tbID.Text = "";
            }
            else
            {
                groupinformation.Text = trViewcopnent.SelectedNode.Text;
                tbID.Text = trViewcopnent.SelectedNode.Text;
                dataportinformation.Rows.Clear();
                int i = 0;
                int j = 0;
                int m = 0;
                string net = "NULLNET";
                if (trViewcopnent.SelectedNode.Parent == trViewcopnent.Nodes[0])
                {
                    while (i < trViewcopnent.SelectedNode.Nodes[3].Nodes.Count)
                    {
                        int index = dataportinformation.Rows.Add();
                        dataportinformation.Rows[index].Cells[0].Value= trViewcopnent.SelectedNode.Nodes[3].Nodes[i].Text;
                        for (j = 0; j < trViewNet.Nodes.Count; j++)
                        {
                            for (m = 0; m < trViewNet.Nodes[j].Nodes.Count; m++)
                            {
                                if (trViewNet.Nodes[j].Nodes[m].Text == trViewcopnent.SelectedNode.Text+"-"+trViewcopnent.SelectedNode.Nodes[3].Nodes[i].Text)
                                    net = trViewNet.Nodes[j].Text;
                            }
                        }
                        dataportinformation.Rows[index].Cells[1].Value = net;
                        i++;
                    }
                }
                else
                {
                    int index = dataportinformation.Rows.Add();
                    dataportinformation.Rows[index].Cells[0].Value = trViewcopnent.SelectedNode.Text;
                    for (j = 0; j < trViewNet.Nodes.Count; j++)
                    {
                        for (m = 0; m < trViewNet.Nodes[j].Nodes.Count; m++)
                        {
                            if (trViewNet.Nodes[j].Nodes[m].Text == trViewcopnent.SelectedNode.Text)
                                net = trViewNet.Nodes[j].Text;
                        }
                    }
                    dataportinformation.Rows[index].Cells[1].Value = net;
                }
            }
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            if (tbID.Text.Length > 0)
            {
                btninsert2.Enabled = true;
            }
        }

        private void EditCavityLibrary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                m_ifeatureFormCmd.StopCommand();
            }
        }

        private void EditCavityLibrary_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_ifeatureFormCmd.StopCommand();
        }

        private void btnAddNetInformation_Click(object sender, EventArgs e)//加载油路网络信息
        {
            Document oDoc = default(Document);
            oDoc = m_inventorApplication.ActiveDocument;

            BrowserPanes oPanes = default(BrowserPanes);
            oPanes = oDoc.BrowserPanes;
            int delete = 17;
            ClientBrowserNodeDefinition deleteoDef=oPanes.GetClientBrowserNodeDefinition(m_ClientId,delete);
            while(deleteoDef!=null)
            {
                deleteoDef.Delete();
                delete++;
                deleteoDef=oPanes.GetClientBrowserNodeDefinition(m_ClientId,delete);
            }

            ClientNodeResources oRscs = oPanes.ClientNodeResources;
            ClientNodeResource oRsc = oRscs.ItemById(m_ClientId, 14);
            int number = 0;
            int maxId=17;
            //加载油孔网络信息
            while(number<14)
            {
                int j=0;
                number++;
                BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[number];
                number--;
                while (j < trViewNet.Nodes[number].Nodes.Count)
                {
                    BrowserNodeDefinition oDef = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(trViewNet.Nodes[number].Nodes[j].Text, maxId, oRsc);
                    node.AddChild(oDef);
                    maxId++;
                    j++;
                }
                number++;
            }

            foreach (Inventor.BrowserNode node in oPanes["油路"].TopNode.BrowserNodes)
            {
                if (node.BrowserNodes.Count == 0)
                {
                    node.Visible = false;
                }
            }
            oPanes["油路"].Update();
            oPanes["油路"].Activate();
        }
        //-----------------------------------------------------------------------------------
        //从项目数据库加载油路信息
        private void AddNetInformationFrommdb()
        {
            Document oDoc = default(Document);
            oDoc = m_inventorApplication.ActiveDocument;

            BrowserPanes oPanes = default(BrowserPanes);
            oPanes = oDoc.BrowserPanes;
            int delete = 15;
            ClientBrowserNodeDefinition deleteoDef = oPanes.GetClientBrowserNodeDefinition(m_ClientId, delete);
            while (deleteoDef != null)
            {
                deleteoDef.Delete();
                delete++;
                deleteoDef = oPanes.GetClientBrowserNodeDefinition(m_ClientId, delete);
            }

            ClientNodeResources oRscs = oPanes.ClientNodeResources;
            ClientNodeResource oRsc = oRscs.ItemById(m_ClientId, 14);
            string[] getresult = new string[25];
            m_connectToaccess = new ConnectToAccess(deFaultpath + "\\CavityLibrary", "项目数据库");
            string sql = @"select * from NETList";
            int number = 15;
            #region
            m_connectToaccess.GetInformation(sql, "NET1", out getresult);
            int i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[1];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET2", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[2];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET3", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[3];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET4", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[4];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET5", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[5];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET6", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[6];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET7", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[7];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET8", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[8];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET9", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[9];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET10", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[10];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET11", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[11];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NET12", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[12];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }

            m_connectToaccess.GetInformation(sql, "NULLNET", out getresult);
            i = 0;
            while (getresult[i] != null)
            {
                if (getresult[i].ToString().Length > 0)
                {
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition(getresult[i], number, oRsc);
                    Inventor.BrowserNode node = oPanes["油路"].TopNode.BrowserNodes[13];
                    node.AddChild(oDef1);
                    number++;
                }
                i++;
            }
            #endregion//加载油孔网络信息
            foreach (Inventor.BrowserNode node in oPanes["油路"].TopNode.BrowserNodes)
            {
                if (node.BrowserNodes.Count == 0)
                {
                    node.Visible = false;
                }
            }
            oPanes["油路"].Update();
            oPanes["油路"].Activate();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            deleteBrowerNodes();
        }

        private void tbID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((e.KeyChar>='0'&&e.KeyChar<='9')||(e.KeyChar>='A'&&e.KeyChar<='Z')||(e.KeyChar>='a'&&e.KeyChar<='z'))
            {
                e.Handled=false;
            }
            else
            {
                e.Handled=true;
            }
        }

        private void checkBoxFace_Click(object sender, EventArgs e)
        {
            if (checkBoxFace.Checked)
            {
                m_ifeatureFormCmd.EnableInteraction();
            }
            else
            {
                m_ifeatureFormCmd.DisableInteraction();
            }
        }

        private void checkBoxFace_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFace.Checked)
            {
                m_ifeatureFormCmd.EnableInteraction();
            }
            else
            {
                m_ifeatureFormCmd.DisableInteraction();
            }
        }

    }

        
}
