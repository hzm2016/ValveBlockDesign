using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class InsertOutlineCmd:ValveBlockDesign.Command
    {
        private Face m_selectFace;
        private Face m_insertFace;
        private iFeature m_selectiFeature;
        private HighlightSet m_highlightSet;
        private InsertOutlineForm m_insertOutlineForm;
        private UserCoordinateSystem m_UCS;
        private Point m_Point;
        private Point2d m_Point1;
        private Point2d m_Point2;
        private double basex;
        private double basey;
        //--------------------------------------------
        //用于连接到数据库
        private ConnectToAccess m_connectToaccess;
        private string m_filepath = @"F:\CavityLibrary";
        private string m_filename = "CavityLibrary";
        private string m_codename;
        private string m_indexname;
        private string m_codenumber;//索引编号寻找iFeature特征
        

        public InsertOutlineCmd()
        {
            m_highlightSet = null;
            m_insertOutlineForm = null;
            m_selectiFeature = null;
            m_selectFace = null;
            m_UCS = null;
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
            EnableInteraction();
        }

        public override void StartCommand()
        {
            base.StartCommand();

            base.SubscribeToEvent(Interaction.InteractionTypeEnum.kSelection);

            //this.InitializePreviewGraphics();

            //create and display the dialog
            m_insertOutlineForm = new InsertOutlineForm(m_inventorApplication, this);

            if (m_insertOutlineForm != null)
            {
                m_insertOutlineForm.Activate();
                m_insertOutlineForm.TopMost = true;
                m_insertOutlineForm.ShowInTaskbar = false;
                m_insertOutlineForm.Show();
            }

            //initialize this command data members
            m_selectFace = null;
            m_selectiFeature = null;

            //enable interaction
            EnableInteraction();
        }

        public override void StopCommand()
        {
            //TerminatePreviewGraphics();
            m_inventorApplication.ActiveView.Update();

            //destroy the command dialog
            m_insertOutlineForm.Hide();
            m_insertOutlineForm.Dispose();
            m_insertOutlineForm = null;

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
        }

        public override void EnableInteraction()
        {
            base.EnableInteraction();

            //clear selection filter
            m_selectEvents.ClearSelectionFilter();

            //reset selections
            m_selectEvents.ResetSelections();

            //specify selection filter and cuisor
            if (m_insertOutlineForm.checkBoxChoose.Checked)
            {
                //set the selection filer to a cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceCylindricalFilter);

                //set a cursor to specify face selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message
                m_interactionEvents.StatusBarText = "请选择需要插入外形的孔";
            }
        }

        public override void DisableInteraction()
        {
            base.DisableInteraction();

            m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow, null, null);
        }

        public override void ExecuteCommand()
        {
            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;
            m_connectToaccess = new ConnectToAccess(m_filepath, m_filename);

            string name = m_insertOutlineForm.listBoxName.SelectedItem.ToString();
            string sql = @"select * from Outlines where Outlines.型号='" + name + "'";
            double startx =GetValueFromExpression( m_connectToaccess.GetSingleInformation(sql, "StartX"));
            double starty = GetValueFromExpression(m_connectToaccess.GetSingleInformation(sql, "StartY"));
            double endx = GetValueFromExpression(m_connectToaccess.GetSingleInformation(sql, "EndX"));
            double endy = GetValueFromExpression(m_connectToaccess.GetSingleInformation(sql, "EndY"));
            basex = GetDistanceBtwPointandLine(m_Point,m_UCS.YAxis);
            basey = GetDistanceBtwPointandLine(m_Point,m_UCS.XAxis);
            double x1 = basex - startx;
            double y1 = basey - starty;
            double x2 = basex + endx;
            double y2 = basey + endy;
            m_Point1 = oTransGeo.CreatePoint2d(y1,x1);
            m_Point2 = oTransGeo.CreatePoint2d(y2,x2);

            InsertOutlineRequest m_insertOutlineRequest = new InsertOutlineRequest(m_inventorApplication,m_insertFace,m_selectiFeature,m_Point1,m_Point2);
            base.ExecuteChangeRequest(m_insertOutlineRequest, "AppInsertOutlineChgDef", m_inventorApplication.ActiveDocument);
        }

        private double GetDistanceBtwPointandLine(Inventor.Point mouseupPoint, WorkAxis workaxis)
        {
            PartDocument partDoc;
            partDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition partComDef;
            partComDef = partDoc.ComponentDefinition;

            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(mouseupPoint, workaxis, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);
            return oDistance;
        }

        public void UpdateCommandStatus()
        {
            m_insertOutlineForm.btninsert.Enabled = false;

            this.ShowUCS();

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            this.HighlightSelectEntity(m_selectiFeature);

            if (m_selectFace != null )
            {
                //update the preview
                //UpdatePreviewGraphics();

                //enable the OK button on dialog
                m_insertOutlineForm.btninsert.Enabled = true;
            }
        }

        private void ShowUCS()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis workAxis;
            workAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_selectFace, true);
            Line oAxisLine;
            oAxisLine = workAxis.Line;
            UnitVector oAxisVector;
            oAxisVector = oAxisLine.Direction;

            Double[] coords = new Double[3];
            oAxisVector.GetUnitVectorData(ref coords);

            string strCoords;
            strCoords = coords[0].ToString() + coords[1].ToString() + coords[2].ToString();

            switch (strCoords)
            {
                case "00-1":
                    m_UCS = oPartCompDef.UserCoordinateSystems[1];
                    break;
                case "010":
                    m_UCS = oPartCompDef.UserCoordinateSystems[2];
                    break;
                case "-100":
                    m_UCS = oPartCompDef.UserCoordinateSystems[3];
                    break;
                case "0-10":
                    m_UCS = oPartCompDef.UserCoordinateSystems[4];
                    break;
                case "100":
                    m_UCS = oPartCompDef.UserCoordinateSystems[5];
                    break;
                case "001":
                    m_UCS = oPartCompDef.UserCoordinateSystems[6];
                    break;
            }

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

        private void GetSelectiFeature(Face face, ref iFeature ifeature)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            PartFeatures oFeatures;
            oFeatures = oPartCompDef.Features;

            iFeatures oiFeatures;
            oiFeatures = oFeatures.iFeatures;

            foreach (iFeature oiFeature in oiFeatures)
            {
                Faces oFaces;
                oFaces = oiFeature.Faces;
                foreach (Face oFace in oFaces)
                {
                    if (oFace == face)
                    {
                        ifeature = oiFeature;
                    }
                }
            }
        }

        private void HighlightSelectEntity(iFeature ifeature)
        {
            m_highlightSet = m_inventorApplication.ActiveDocument.CreateHighlightSet();

            Inventor.Color green;
            green = m_inventorApplication.TransientObjects.CreateColor(0, 255, 0);
            green.Opacity = 0.3;

            m_highlightSet.Color = green;

            foreach (Face oFace in ifeature.Faces)
            {
                m_highlightSet.AddItem(oFace);
            }
        }

        private void ClearHighlight()
        {
            m_highlightSet = null;
        }

        public override void OnPreSelect(object preSelectEntity, out bool doHighlight, ObjectCollection morePreSelectEntities, SelectionDeviceEnum selectionDevice,Inventor.Point modelPosition, Point2d viewPosition, Inventor.View view)
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

        public override void OnSelect(ObjectsEnumerator justSelectedEntities, SelectionDeviceEnum selectionDevice,Inventor.Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            int nomb = justSelectedEntities.Count;

            if (nomb > 0)
            {
                object selectedEntity = justSelectedEntities[1];

                if (m_insertOutlineForm.checkBoxChoose.Checked)
                {
                    m_selectFace = (Face)selectedEntity;

                    m_insertOutlineForm.checkBoxChoose.Checked = false;
                }

                this.GetSelectiFeature(m_selectFace, ref m_selectiFeature);

                m_selectEvents.AddToSelectedEntities(m_selectiFeature);

                DisableInteraction();
                //this.HighlightSelectEntity(m_selectiFeature);
                UpdateCommandStatus();
            }
        }

        public override void OnMouseUp(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            AddInformation();
            this.HighlightSelectEntity(m_selectiFeature);
        }

        public void AddInformation()
        {
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;
            
            AttributeSets atr = m_selectiFeature.AttributeSets;
            int insertPlane;
            int insertFace;
            this.GetInsertFace(oPartCompDef, m_selectFace, out insertPlane, out insertFace);
            m_insertFace = oPartCompDef.Features.ExtrudeFeatures["拉伸1"].Faces[insertFace];
            AttributeSet abs = atr["MyAttribSet"];
            Inventor.Attribute internalname = abs["InternalName"];
            Inventor.Attribute footprint = abs["Footprint"];
            Inventor.Attribute pointX = abs["PointX"];
            Inventor.Attribute pointY = abs["PointY"];
            Inventor.Attribute pointZ = abs["PointZ"];
            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;
            m_Point = oTransGeo.CreatePoint(pointX.Value, pointY.Value, pointZ.Value);
            Inventor.Attribute indexname = abs["IndexName"];
            m_indexname = indexname.Value;
            Inventor.Attribute codename = abs["CodeName"];
            m_codename = codename.Value;
            Inventor.Attribute codenumber = abs["CodeNumber"];
            m_codenumber = codenumber.Value;
            m_connectToaccess = new ConnectToAccess(m_filepath, m_filename, m_codename, m_indexname, m_codenumber);
            string sql = @"select 生产厂家 from Outlines where Outlines.标准='" + m_indexname + "'";
            string [] result=new string[25];
            m_connectToaccess.GetInformation(sql,"生产厂家",out result);
            int i = 0;
            while (result[i] != null)
            {
                m_insertOutlineForm.listBoxName.Items.Add(result[i]);
                i++;
            }

        }

        private void GetInsertFace(PartComponentDefinition partCompDef, Face face, out int planeNumb, out int faceNumb)
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
            faceNumb = 0;
            switch (strCoords)
            {
                case "00-1":
                    planeNumb = 1;
                    faceNumb = 5;
                    break;
                case "010":
                    planeNumb = 2;
                    faceNumb = 4;
                    break;
                case "-100":
                    planeNumb = 3;
                    faceNumb = 3;
                    break;
                case "0-10":
                    planeNumb = 4;
                    faceNumb = 2;
                    break;
                case "100":
                    planeNumb = 5;
                    faceNumb = 1;
                    break;
                case "001":
                    planeNumb = 6;
                    faceNumb = 6;
                    break;
            }
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
    }
}
