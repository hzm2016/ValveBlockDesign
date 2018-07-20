using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;
using System.IO;

namespace ValveBlockDesign
{
    internal class InsertXportCmd:ValveBlockDesign.Command
    {
        private InsertXportForm m_insertXportForm;

        private Face m_firstFace;
        private Face m_secondFace;

        private int m_insertplane;
        private int m_typeNum;
        double xdistance=0;
        double ydistance=0;

        private UserCoordinateSystem m_UCS;
        private HighlightSet m_highlightSet;
        private ConnectToAccess m_connectToaccess;

        private string codenumber;
        private string codename;
        private string indexname;
        private string filepath;
        private string m_xportID;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;

        public InsertXportCmd()
        {
            m_insertXportForm = null;
            m_firstFace = null;
            m_secondFace = null;
            m_highlightSet = null;
            m_UCS = null;
            m_xportID = null;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName;
            filepath = deFaultpath + "\\CavityLibrary";
        }

        protected override void ButtonDefinition_OnExecute(NameValueMap context)
        {
            //make sure that the current environment is "Part Environment"
            Inventor.Environment currEnvironment = m_inventorApplication.UserInterfaceManager.ActiveEnvironment;
            if (currEnvironment.InternalName != "PMxPartEnvironment")
            {
                System.Windows.Forms.MessageBox.Show("该命令必须在零件环境中运行！");
                return;
            }

            //if command was already started, stop it first
            if (m_commandIsRunning)
            {
                StopCommand();
            }

            //start new command
            StartCommand();
        }

        public override void StartCommand()
        {
            base.StartCommand();

            base.SubscribeToEvent(Interaction.InteractionTypeEnum.kSelection);

            //initialize interaction previewgraphics objects

            //create and display the dialog
            m_insertXportForm = new InsertXportForm(m_inventorApplication, this);

            if (m_insertXportForm != null)
            {
                m_insertXportForm.Activate();
                m_insertXportForm.TopMost = true;
                m_insertXportForm.ShowInTaskbar = false;
                m_insertXportForm.Show();
                AddInformation();
            }

            //initialize this command data members
            m_firstFace = null;
            m_secondFace = null;

            m_UCS = null;

            //enable interaction
            EnableInteraction();
        }

        public void AddInformation()
        {
            m_insertXportForm.btnsure.Enabled = false;
            m_insertXportForm.checkfirstFace.Checked = true;
            string[] getresult = new string[25];
            string sql;
            if (m_insertXportForm.radiobtn2.Checked == true)
            {
                sql = @"select * from ExpanderPlugPort";
                //m_insertXportForm.comBXport.Text = "M-20-16";
                codenumber = "1801";
                indexname = "ExpanderPlugPort";
            }
            else
            {
                sql = @"select * from OrificePlugPort";
                //m_insertXportForm.comBXport.Text = "M12x1.75-6H";
                codenumber = "1901";
                indexname = "OrificePlugPort";
            }
            m_connectToaccess = new ConnectToAccess(filepath, "CavityLibrary");
            m_connectToaccess.GetInformation(sql, "编码", out getresult);
            m_insertXportForm.comBXport.Items.Clear();
            int i = 0;
            while (getresult[i] != null)
            {
                m_insertXportForm.comBXport.Items.Add(getresult[i]);
                i++;
            }
            m_insertXportForm.comBXport.Text =m_insertXportForm.comBXport.Items[0].ToString();
        }

        public override void StopCommand()
        {
            //Terminate this preview graphic
            m_inventorApplication.ActiveView.Update();
            //destroy the command dialog
            //m_insertXportForm.Hide();
            m_insertXportForm.Dispose();
            m_insertXportForm = null;

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            if (m_UCS != null)
            {
                if (m_UCS.Visible == true)
                {
                    m_UCS.Visible = false;
                }
            }

            xdistance = 0;
            ydistance = 0;
            m_insertplane = 0;
            m_firstFace = null;
            m_highlightSet = null;
            m_secondFace = null;
            m_xportID = null;
            base.StopCommand();
        }

        public override void EnableInteraction()
        {
            base.EnableInteraction();

            //clear selection filter
            m_selectEvents.ClearSelectionFilter();

            //reset selections
            m_selectEvents.ResetSelections();

            //specify selection filter and cuisor
            if (m_insertXportForm.checkfirstFace.Checked)
            {
                if (m_firstFace != null)
                {
                    m_selectEvents.AddToSelectedEntities(m_firstFace);
                }

                //set the selection filer to a cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceCylindricalFilter);

                //set a cursor to specify face selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message
                m_interactionEvents.StatusBarText = "请选择孔";
            }
            else
            {
                if (m_secondFace != null)
                {
                    m_selectEvents.AddToSelectedEntities(m_secondFace);
                }

                //set the selection filter to cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceCylindricalFilter);

                //set a cursor to specify edge selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message                        
                m_interactionEvents.StatusBarText = "请选择对齐到孔";
            }
        }

        public override void DisableInteraction()
        {
            base.DisableInteraction();

            m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow, null, null);
        }

        public override void ExecuteCommand()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;
            if (xdistance == 0 || ydistance == 0)
            {
                //GetInsertInformation(oPartCompDef);
                if (xdistance == 0)
                {
                    xdistance =GetValueFromExpression(m_insertXportForm.tbx.Text);
                }
                if (ydistance == 0)
                {
                    ydistance = GetValueFromExpression(m_insertXportForm.tby.Text);
                }
            }
            else
            {
                UnitVector xVector;
                xVector = oPartCompDef.UserCoordinateSystems[m_insertplane].XAxis.Line.Direction;
                UnitVector ofirstVector;
                UnitVector osecondVector;
                GetVectorFromFaces(out ofirstVector, out osecondVector);
                WorkAxis firstAxis;
                firstAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_firstFace, true);
                WorkAxis secondAxis;
                secondAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_secondFace, true);

                Double tolerance = 0.01;
                if (ofirstVector.IsParallelTo(xVector, tolerance))
                {
                    xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[m_insertplane].XZPlane);
                    ydistance = GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[m_insertplane].YZPlane);
                }
                else
                {
                    xdistance = GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[m_insertplane].XZPlane);
                    ydistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[m_insertplane].YZPlane);
                }
                
            }
            if (m_insertXportForm.comBXport.Text != null)
            {
                codename = m_insertXportForm.comBXport.Text;
            }
            else
            {
                MessageBox.Show("请选择工艺孔型号");
                return;
            }

            //StopCommand();
            m_xportID = m_insertXportForm.tbID.Text;
            //create the request
            InsertXportRequest m_insertXportRequest;
            m_insertXportRequest = new InsertXportRequest(m_inventorApplication, m_insertplane, xdistance, ydistance, codenumber, codename, indexname, m_xportID);
            //execute the request
            base.ExecuteChangeRequest(m_insertXportRequest, "AppInsertXportChgDef", m_inventorApplication.ActiveDocument);
        }

        public double GetValueFromExpression(string expression)
        {
            double value = 0.0;

            //get the active document
            Document activeDocument = m_inventorApplication.ActiveDocument;

            //get the unit of measure object
            UnitsOfMeasure unitsOfMeasure = activeDocument.UnitsOfMeasure;

            //get the current length units of the user
            UnitsTypeEnum lengthUnitsType = unitsOfMeasure.LengthUnits;

            //convert the expression to the current length units of user
            try
            {
                object vVal;
                vVal = unitsOfMeasure.GetValueFromExpression(expression, lengthUnitsType);
                value = System.Convert.ToDouble(vVal);
            }
            catch (System.Exception e)
            {
                string strErrorMsg = e.Message;

                value = 0.0;
                return value;
            }
            return value;
        }

        private void GetInsertInformation(PartComponentDefinition oPartCompDef)
        {
            WorkAxis firstAxis;
            firstAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_firstFace, true);
            WorkAxis secondAxis;
            secondAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_secondFace, true);
            switch (m_typeNum)
            {
                case 13:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[1].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[1].XZPlane))
                    {
                        m_insertplane = 5;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[1].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[1].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[1].YZPlane))
                    {
                        m_insertplane = 2;
                        ydistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[1].YZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
                case 14:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[2].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[2].XZPlane))
                    {
                        m_insertplane = 5;
                        xdistance=GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[2].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[2].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[2].YZPlane))
                    {
                        m_insertplane = 6;
                        ydistance=GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[2].YZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
                case 15:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[3].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[3].XZPlane))
                    {
                        m_insertplane = 6;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[6].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[3].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[3].YZPlane))
                    {
                        m_insertplane = 2;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[2].XZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
                case 16:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[4].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[4].XZPlane))
                    {
                        m_insertplane = 5;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[4].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[4].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[4].YZPlane))
                    {
                        m_insertplane = 6;
                        ydistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[4].YZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
                case 17:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[5].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[5].XZPlane))
                    {
                        m_insertplane = 2;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[5].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[5].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[5].YZPlane))
                    {
                        m_insertplane = 6;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[5].YZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
                case 18:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[6].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[6].XZPlane))
                    {
                        m_insertplane = 5;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[6].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[6].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[6].YZPlane))
                    {
                        m_insertplane = 2;
                        ydistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[6].YZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
                case 19:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[6].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[6].XZPlane))
                    {
                        m_insertplane = 5;
                        ydistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[6].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[1].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[1].YZPlane))
                    {
                        m_insertplane = 2;
                        ydistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[1].YZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
                case 20:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[5].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[5].XZPlane))
                    {
                        m_insertplane = 2;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[5].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[5].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[5].YZPlane))
                    {
                        m_insertplane = 6;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[5].YZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
                case 21:
                    if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[4].XZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[4].XZPlane))
                    {
                        m_insertplane = 5;
                        xdistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[4].XZPlane);
                    }
                    else if (GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[4].YZPlane) == GetDistanceBetwLineAndFace(oPartCompDef, secondAxis, oPartCompDef.UserCoordinateSystems[4].YZPlane))
                    {
                        m_insertplane = 6;
                        ydistance = GetDistanceBetwLineAndFace(oPartCompDef, firstAxis, oPartCompDef.UserCoordinateSystems[4].YZPlane);
                    }
                    else
                    {
                        MessageBox.Show("请先执行对齐操作");
                        return;
                    }
                    break;
            }
        }

        private double GetDistanceBetwLineAndFace(PartComponentDefinition partCompDef, WorkAxis workAxis, WorkPlane workPlane)
        {
            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(workAxis, workPlane, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);

            return oDistance;
        }

        private void HightghtSelectFace(Face thisFace, Face withThisFace)
        {
            m_highlightSet = m_inventorApplication.ActiveDocument.CreateHighlightSet();

            Inventor.Color green;
            green = m_inventorApplication.TransientObjects.CreateColor(0, 255, 0);
            green.Opacity = 0.3;

            m_highlightSet.Color = green;

            m_highlightSet.AddItem(thisFace);
            m_highlightSet.AddItem(withThisFace);
        }

        private void ClearHighlight()
        {
            m_highlightSet = null;
        }

        private void ShowUCS(int insertplane)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            #region Use "AttribSet" to get the UCS
            //AttributeSet oAttibSet;
            //oAttibSet = m_withThisiFeature.AttributeSets["MyAttribSet"];

            //Inventor.Attribute oPlaneAttribe;
            //oPlaneAttribe = oAttibSet["Plane"];

            //int oAttribValue = (int)oPlaneAttribe.Value;

            //switch (oAttribValue)
            //{
            //    case 1:
            //        m_UCS = oPartCompDef.UserCoordinateSystems[1];
            //        break;
            //    case 2:
            //        m_UCS = oPartCompDef.UserCoordinateSystems[2];
            //        break;
            //    case 3:
            //        m_UCS = oPartCompDef.UserCoordinateSystems[3];
            //        break;
            //    case 4:
            //        m_UCS = oPartCompDef.UserCoordinateSystems[4];
            //        break;
            //    case 5:
            //        m_UCS = oPartCompDef.UserCoordinateSystems[5];
            //        break;
            //    case 6:
            //        m_UCS = oPartCompDef.UserCoordinateSystems[6];
            //        break;
            //}
            #endregion

            #region Use "Line.Direction" to get the UCS
            if (insertplane != 0)
            {
                m_UCS = oPartCompDef.UserCoordinateSystems[insertplane];
            }
            if (m_UCS == null)
            {
                MessageBox.Show("发生错误！");
                return;
            }
            #endregion

            m_UCS.Visible = true;
            m_UCS.XAxis.Visible = false;
            m_UCS.YAxis.Visible = false;
            m_UCS.ZAxis.Visible = false;
            m_UCS.XYPlane.Visible = false;
            m_UCS.XZPlane.Visible = false;
            m_UCS.YZPlane.Visible = false;
        }

        private void GetAlignType()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            int oThisNum;
            this.GetInsertPlane(oPartCompDef, m_firstFace, out oThisNum);

            int oWithNum;
            this.GetInsertPlane(oPartCompDef, m_secondFace, out oWithNum);

            if (oThisNum == 0 || oWithNum == 0)
            {
                MessageBox.Show("软件运行出现错误!");
                return;
            }

            string oType = oThisNum.ToString() + oWithNum.ToString();

            switch (oType)
            {
                case "12":
                    m_typeNum = 1;
                    break;
                case "21":
                    m_typeNum = 1;
                    break;
                case "13":
                    m_typeNum = 2;
                    break;
                case "31":
                    m_typeNum = 2;
                    break;
                case "14":
                    m_typeNum = 3;
                    break;
                case "41":
                    m_typeNum = 3;
                    break;
                case "15":
                    m_typeNum = 4;
                    break;
                case "51":
                    m_typeNum = 4;
                    break;
                case "25":
                    m_typeNum = 5;
                    break;
                case "52":
                    m_typeNum = 5;
                    break;
                case "26":
                    m_typeNum = 6;
                    break;
                case "62":
                    m_typeNum = 6;
                    break;
                case "23":
                    m_typeNum = 7;
                    break;
                case "32":
                    m_typeNum = 7;
                    break;
                case "34":
                    m_typeNum = 8;
                    break;
                case "43":
                    m_typeNum = 8;
                    break;
                case "36":
                    m_typeNum = 9;
                    break;
                case "63":
                    m_typeNum = 9;
                    break;
                case "45":
                    m_typeNum = 10;
                    break;
                case "54":
                    m_typeNum = 10;
                    break;
                case "46":
                    m_typeNum = 11;
                    break;
                case "64":
                    m_typeNum = 11;
                    break;
                case "56":
                    m_typeNum = 12;
                    break;
                case "65":
                    m_typeNum = 12;
                    break;
                case "11":
                    m_typeNum = 13;
                    break;
                case "22":
                    m_typeNum = 14;
                    break;
                case "33":
                    m_typeNum = 15;
                    break;
                case "44":
                    m_typeNum = 16;
                    break;
                case "55":
                    m_typeNum = 17;
                    break;
                case "66":
                    m_typeNum = 18;
                    break;
                case "16":
                    m_typeNum = 19;
                    break;
                case "61":
                    m_typeNum = 19;
                    break;
                case "35":
                    m_typeNum = 20;
                    break;
                case "53":
                    m_typeNum = 20;
                    break;
                case "42":
                    m_typeNum = 21;
                    break;
                case "24":
                    m_typeNum = 21;
                    break;
            }
        }

        private void GetInsertPlane(PartComponentDefinition partCompDef, Face face, out int planeNumb)
        {
            WorkAxis workAxis;
            workAxis = partCompDef.WorkAxes.AddByRevolvedFace(face, true);
            Line oAxisLine;
            oAxisLine = workAxis.Line;
            UnitVector oAxisVector;
            oAxisVector = oAxisLine.Direction;

            Double[] coords = new Double[3];
            oAxisVector.GetUnitVectorData(ref coords);

            string strCoords;
            strCoords = coords[0].ToString() + coords[1].ToString() + coords[2].ToString();

            planeNumb = 0;
            switch (strCoords)
            {
                case "00-1":
                    planeNumb = 1;
                    break;
                case "010":
                    planeNumb = 2;
                    break;
                case "-100":
                    planeNumb = 3;
                    break;
                case "0-10":
                    planeNumb = 4;
                    break;
                case "100":
                    planeNumb = 5;
                    break;
                case "001":
                    planeNumb = 6;
                    break;
            }
        }

        public void UpdateCommandStatus()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;
            //by default ,disable the ok button
            m_insertXportForm.btnsure.Enabled = false;
            m_insertXportForm.tbID.Enabled = false;
            UnitVector oThisVector;
            UnitVector oWithThisVector;
            GetVectorFromFaces(out oThisVector, out oWithThisVector);
            //获得插入工艺孔平面
            GetAlignType();
            if (m_typeNum == 2 || m_typeNum == 4 || m_typeNum == 9 || m_typeNum == 12 )
            {
                m_insertplane = 2;
                m_insertXportForm.tbx.Enabled = false;
                m_insertXportForm.tby.Enabled = false;
                xdistance = 5.0;
                ydistance = 5.0;
            }
            else if (m_typeNum == 1 || m_typeNum == 3 || m_typeNum == 6 || m_typeNum == 11 )
            {
                m_insertplane = 5;
                m_insertXportForm.tbx.Enabled = false;
                m_insertXportForm.tby.Enabled = false;
                xdistance = 5.0;
                ydistance = 5.0;
            }
            else if (m_typeNum == 5 || m_typeNum == 7 || m_typeNum == 8 || m_typeNum == 10)
            {
                m_insertplane = 6;
                m_insertXportForm.tbx.Enabled = false;
                m_insertXportForm.tby.Enabled = false;
                xdistance = 5.0;
                ydistance = 5.0;
            }
            else
            {
                m_insertplane = 0;
                GetInsertInformation(oPartCompDef);
                if (xdistance!=0)
                {
                    m_insertXportForm.tbx.Enabled = false;
                }
                if (ydistance !=0)
                {
                    m_insertXportForm.tby.Enabled=false;
                }
            }
            if (m_firstFace != null && m_secondFace != null)
            {
                    if (m_UCS != null)
                    {
                        m_UCS.Visible = false;
                    }
                    //this.ShowUCS(m_insertplane);
                    m_insertXportForm.tbID.Enabled = true;
             }

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            this.HightghtSelectFace(m_firstFace, m_secondFace);
        }

        private void GetVectorFromFaces(out UnitVector thisAxisVector, out UnitVector withThisAxisVector)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis oThisAxis;
            oThisAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_firstFace, true);
            Line oThisAxisLine;
            oThisAxisLine = oThisAxis.Line;

            thisAxisVector = oThisAxisLine.Direction;

            WorkAxis oWithThisAxis;
            oWithThisAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_secondFace, true);
            Line oWithThisAxisLine;
            oWithThisAxisLine = oWithThisAxis.Line;

            withThisAxisVector = oWithThisAxisLine.Direction;
        }

        public override void OnPreSelect(object preSelectEntity, out bool doHighlight, ObjectCollection morePreSelectEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            doHighlight = false;

            if (preSelectEntity is Face)
            {
                Face preSelectFace = (Face)preSelectEntity;

                if (preSelectFace.SurfaceType == SurfaceTypeEnum.kCylinderSurface)
                {
                    PartDocument oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

                    iFeatures oiFeatures = oPartDoc.ComponentDefinition.Features.iFeatures;

                    foreach (iFeature oiFeature in oiFeatures)
                    {
                        Faces oFaces;
                        oFaces = oiFeature.Faces;
                        foreach (Face oFace in oFaces)
                        {
                            if (oFace == preSelectFace)
                            {
                                doHighlight = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public override void OnSelect(ObjectsEnumerator justSelectedEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            int nombSelectedEntities = justSelectedEntities.Count;

            if (nombSelectedEntities > 0)
            {
                object selectedEntity = justSelectedEntities[1];

                //if cavity face is selected
                if ((m_insertXportForm.checkfirstFace.Checked) && (selectedEntity != m_secondFace))
                {
                    m_firstFace = (Face)selectedEntity;

                    m_insertXportForm.checkfirstFace.Checked = false;

                    if (m_secondFace == null)
                    {
                        m_insertXportForm.checksecondFace.Checked = true;

                        EnableInteraction();
                    }
                    else
                    {
                        DisableInteraction();

                        UpdateCommandStatus();
                    }
                }

                if ((m_insertXportForm.checksecondFace.Checked) && (selectedEntity != m_firstFace))
                {
                    m_secondFace = (Face)selectedEntity;

                    m_insertXportForm.checksecondFace.Checked = false;

                    if (m_firstFace == null)
                    {
                        m_insertXportForm.checkfirstFace.Checked = true;

                        EnableInteraction();
                    }
                    else
                    {
                        DisableInteraction();

                        UpdateCommandStatus();
                    }
                }
            }
        }
    }
}
