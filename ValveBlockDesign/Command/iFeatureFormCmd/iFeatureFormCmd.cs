using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inventor;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.OleDb;
using System.Windows.Forms;


namespace ValveBlockDesign
{
    internal partial  class iFeatureFormCmd :ValveBlockDesign.Command
    {
        private EditCavityLibrary m_editCavityLibrary;
        private Face m_withThisFace;
        private string filepath;
        private string filename;
        private string codename; 
        private string indexname;
        private string codenumber;
        private string checkfootprint;
        private Inventor.Point m_Point;
        private HighlightSet m_highlightSet;
        private UserCoordinateSystem m_UCS;
        private string m_iFeatureName;
        private bool m_mouseFlag;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;
        private ConnectToAccess m_connectToAccess;
        public iFeatureFormCmd()
        {
            m_editCavityLibrary = null;
            m_withThisFace = null;
            m_highlightSet = null;
            m_Point = null;
            m_UCS = null;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName+"\\CavityLibrary";
        }
        public void AddiFeatureFormCmd(string ofilepath,string ofilename,string ocodename,string ocodenumber,string oindexname,string checkfootprint,out Inventor.Point point)
        {
            this.filename = ofilename;
            this.filepath = ofilepath;
            this.codename = ocodename;
            this.codenumber = ocodenumber;
            this.indexname = oindexname;
            this.checkfootprint = checkfootprint;
            point = this.m_Point;
        }

        public void Addpoint(out Inventor.Point point)
        {
            point = this.m_Point;
        }

        protected override void ButtonDefinition_OnExecute(NameValueMap context)
        {
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
            m_editCavityLibrary = new EditCavityLibrary(m_inventorApplication, this);
            if (m_editCavityLibrary != null)
            {
                m_editCavityLibrary.Activate();
                m_editCavityLibrary.TopMost = true;
                m_editCavityLibrary.ShowInTaskbar = false;
                m_editCavityLibrary.Show();
                m_editCavityLibrary.tb1.Text = deFaultpath;
                AddInformation();
            }
            m_selectEvents.Enabled = false;
            //EnableInteraction();
        }

        public void AddInformation()//输入默认元件库初始信息
        {
            
            string sql = @"select * from ComponentsDb";
            m_connectToAccess = new ConnectToAccess(deFaultpath, "CavityLibrary");
            m_connectToAccess.GetAddInformation(sql, m_editCavityLibrary.lv1, "IndexName", "Name");
        }

        public override void StopCommand()
        {
            //Terminate this preview graphic

            //destroy the command dialog
            m_inventorApplication.ActiveView.Update();
            m_editCavityLibrary.Hide();
            m_editCavityLibrary.Dispose();
            m_editCavityLibrary = null;
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
            base.StopCommand();
            m_withThisFace = null;
        }

        public override void EnableInteraction()
        {
            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            if (m_UCS != null)
            {                                    
                m_UCS.Visible = false;                
            }
            base.EnableInteraction();

            //clear selection filter
            m_selectEvents.ClearSelectionFilter();

            //reset selections
            m_selectEvents.ResetSelections();

            //specify selection filter and cuisor
            if (m_editCavityLibrary.checkBoxFace.Checked)
            {
                m_selectEvents.Enabled = true;
                //set the selection filer to a cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFacePlanarFilter);

                //set a cursor to specify face selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message
                m_interactionEvents.StatusBarText = "请选择平面";
            }
           // m_selectEvents.Enabled = true;
           // //clear selection filter
           // m_selectEvents.ClearSelectionFilter();

           // //reset selections
           // m_selectEvents.ResetSelections();

           // //specify selection filter and cuisor
           //if (m_withThisFace != null)
           //{
           //     m_selectEvents.AddToSelectedEntities(m_withThisFace);
           // }
           //else
           //{
           //     //set the selection filer to a cylinder face
           //     m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceFilter);

           //     //set a cursor to specify face selection
           //     m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

           //     //set the status bar message
           //     m_interactionEvents.StatusBarText = "请选择面";
           // }
        }

        public override void DisableInteraction()
        {
            base.DisableInteraction();
            m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow, null, null);
        }

        public void UpdateCommandStatus()
        {
            if (m_UCS != null)
            {
                m_UCS.Visible = false;
            }

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            if (m_withThisFace != null)
            {
                this.ShowUCS();
                this.HightghtSelectFace(m_withThisFace);
            }
            
        }

        public override void OnSelect(ObjectsEnumerator justSelectedEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            int nomb = justSelectedEntities.Count;

            if (nomb > 0)
            {
                object selectedEntity = justSelectedEntities[1];

                if (m_editCavityLibrary.checkBoxFace.Checked)
                {
                    m_withThisFace = (Face)selectedEntity;

                    m_editCavityLibrary.checkBoxFace.Checked = false;

                    m_mouseFlag = true;
                }
            }

            m_selectEvents.AddToSelectedEntities(m_withThisFace);

            DisableInteraction();

            UpdateCommandStatus();
        }

        public override void OnPreSelect(object preSelectEntity, out bool doHighlight, ObjectCollection morePreSelectEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            doHighlight = false;

            if (preSelectEntity is Face)
            {
                doHighlight = true;
            }
        }

        public override void OnMouseUp(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            if ((m_withThisFace != null) && m_mouseFlag == true)
            {
                m_Point = ProjectPoint(modelPosition);
                m_editCavityLibrary.tby.Text = (GetDistanceBtwPointandLine(m_Point, m_UCS.XAxis) * 10.0).ToString("F2");
                m_editCavityLibrary.tbx.Text = (GetDistanceBtwPointandLine(m_Point, m_UCS.YAxis) * 10.0).ToString("F2");

                UpdateCommandStatus();
                m_mouseFlag = false;
                m_editCavityLibrary.btninsertsure.Enabled = true;
                m_selectEvents.Enabled = false;
            }   
        }

        private void ShowUCS()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            int i = JudgeFaceUcs(m_withThisFace);
            UserCoordinateSystems oUCSs;
            oUCSs = oPartCompDef.UserCoordinateSystems;
            m_UCS = oUCSs[i];
            if (m_UCS == null)
            {
                MessageBox.Show("发生错误！");
                return;
            }

            m_UCS.Visible = true;
            m_UCS.XAxis.Visible = false;
            m_UCS.YAxis.Visible = false;
            m_UCS.ZAxis.Visible = false;
            m_UCS.XYPlane.Visible = false;
            m_UCS.XZPlane.Visible = false;
            m_UCS.YZPlane.Visible = false;

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

        private double GetDistanceBtwPointandLine(Inventor.Point mouseupPoint,WorkAxis workaxis)
        {
            PartDocument partDoc;
            partDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition partComDef;
            partComDef = partDoc.ComponentDefinition;

            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(mouseupPoint,workaxis,InferredTypeEnum.kNoInference,InferredTypeEnum.kNoInference,cont);
            return oDistance;
        }

        private int JudgeFaceUcs(Face oSelectFace)//用于判断选择的面的用户坐标系
        {
            int i = 0;
            Double[] oParam = new Double[2];
            oParam[0] = 0;
            oParam[1] = 1;
            Double[] oNormal = new Double[3];
            oSelectFace.Evaluator.GetNormal(oParam, ref oNormal);
            switch ((oNormal[0].ToString() + "," + oNormal[1].ToString() + "," + oNormal[2].ToString()))
            {
                case "0,0,1":
                    i = 1;
                    break;
                case "0,-1,0":
                    i = 2;
                    break;
                case "1,0,0":
                    i = 3;
                    break;
                case "0,1,0":
                    i = 4;
                    break;
                case "-1,0,0":
                    i = 5;
                    break;
                case "0,0,-1":
                    i = 6;
                    break;
            }
            return i;
        }

        private void HightghtSelectFace(Face withThisFace)
        {
            m_highlightSet = m_inventorApplication.ActiveDocument.CreateHighlightSet();

            Inventor.Color green;
            green = m_inventorApplication.TransientObjects.CreateColor(0, 255, 0);
            green.Opacity = 0.3;

            m_highlightSet.Color = green;

            m_highlightSet.AddItem(withThisFace);
        }

        private void ClearHighlight()
        {
            m_highlightSet = null;

            //PartDocument oPartDoc;
            //oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            //PartComponentDefinition oPartCompDef;
            //oPartCompDef = oPartDoc.ComponentDefinition;

            //UserCoordinateSystems oUCSs;
            //oUCSs = oPartCompDef.UserCoordinateSystems;
            //foreach (UserCoordinateSystem m_UCS in oUCSs)
            //{
            //    if (m_UCS.Visible == true)
            //        m_UCS.Visible = false;
            //}
        }

        private Point ProjectPoint(Inventor.Point ModelPosition)
        {
            Point ProjectedPoint;
            PartDocument partDoc;
            partDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition partComDef;
            partComDef = partDoc.ComponentDefinition;

            //Set a reference to the camera object
            Inventor.Camera oCamera;
            oCamera = m_inventorApplication.ActiveView.Camera;

            Vector oVector;
            oVector = oCamera.Eye.VectorTo(oCamera.Target);

            Line oLine;
            oLine = m_inventorApplication.TransientGeometry.CreateLine(ModelPosition, oVector);

            //Create a plane parallel to the X-Y plane
            WorkPlane oWPPlane;
            oWPPlane = partComDef.WorkPlanes.AddByPlaneAndOffset(m_withThisFace,0,true);

            Plane plane;
            plane = oWPPlane.Plane;
            ProjectedPoint = plane.IntersectWithLine(oLine);
            return ProjectedPoint;
        }

        public override void ExecuteCommand()//执行命令
        {
            m_inventorApplication.ActiveView.Update();

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            if (m_UCS != null)
            {
                m_UCS.Visible = false;                
            }
            double X = GetValueFromExpression(m_editCavityLibrary.tbx.Text);
            double Y = GetValueFromExpression(m_editCavityLibrary.tby.Text);
            double angle=0.0;
            if (m_editCavityLibrary.checkBoxRotate.Checked)
            {
                switch (m_editCavityLibrary.comBAngle.Text)
                {
                    case "0":
                        angle = 0.0;
                        break;
                    case "90":
                        angle = 1.57;
                        break;
                    case "180":
                        angle = 3.14;
                        break;
                    case "270":
                        angle = 4.71;
                        break;
                }
            }
            m_iFeatureName = m_editCavityLibrary.tbID.Text;
            iFeatureFormRequest ifeatureformRequest = new iFeatureFormRequest(m_inventorApplication, m_withThisFace, filepath, filename, codename, codenumber, indexname, checkfootprint, X, Y,m_Point,angle,m_editCavityLibrary.dataportinformation);
            ifeatureformRequest.AddInformation(m_iFeatureName);
            base.ExecuteChangeRequest(ifeatureformRequest, "AppInsertiFeatureChgDef", m_inventorApplication.ActiveDocument);
            m_withThisFace = null;
            m_UCS = null;
            m_editCavityLibrary.btninsertsure.Enabled = false;
            m_editCavityLibrary.btninsert2.Enabled = false;
            m_selectEvents.ClearSelectionFilter();
            m_selectEvents.ResetSelections();
        }
    }

}
