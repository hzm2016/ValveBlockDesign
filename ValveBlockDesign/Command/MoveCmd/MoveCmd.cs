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
    internal class MoveCmd : ValveBlockDesign.Command
    {
        private MoveCmdDlg m_moveCmdDlg;

        private double m_xOffset;
        private double m_yOffset;

        private Face m_selectFace;
        private iFeature m_selectiFeature;

        private UserCoordinateSystem m_UCS;

        private HighlightSet m_highlightSet;

        //preview objects
        private GraphicsNode m_previewClientGraphicsNode;
        private PointGraphics m_pointGraphics;
        private GraphicsCoordinateSet m_graphicsCoordinateSet;
        private GraphicsColorSet m_graphicsColorSet;
        private GraphicsIndexSet m_graphicsColorIndexSet;

        public MoveCmd()
        {
            m_moveCmdDlg = null;

            m_selectFace = null;
            m_selectiFeature = null;

            m_UCS = null;

            m_previewClientGraphicsNode = null;
            m_pointGraphics = null;
            m_graphicsCoordinateSet = null;
            m_graphicsColorSet = null;
            m_graphicsColorIndexSet = null;

            m_highlightSet = null;
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

            //this.InitializePreviewGraphics();

            //create and display the dialog
            m_moveCmdDlg = new MoveCmdDlg(m_inventorApplication, this);

            if (m_moveCmdDlg != null)
            {
                m_moveCmdDlg.Activate();
                m_moveCmdDlg.TopMost = true;
                m_moveCmdDlg.ShowInTaskbar = false;
                m_moveCmdDlg.Show();
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
            m_moveCmdDlg.Hide();
            m_moveCmdDlg.Dispose();
            m_moveCmdDlg = null;

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
            if (m_moveCmdDlg.checkBoxChoose.Checked)
            {
                //set the selection filer to a cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceCylindricalFilter);

                //set a cursor to specify face selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message
                m_interactionEvents.StatusBarText = "请选择需要移动的孔";
            }
        }

        public override void DisableInteraction()
        {
            base.DisableInteraction();

            m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow, null, null);
        }

        public override void ExecuteCommand()
        {
            StopCommand();

            MoveRequest moveRequest = new MoveRequest(m_inventorApplication, m_selectFace, m_selectiFeature, m_xOffset, m_yOffset);

            base.ExecuteChangeRequest(moveRequest, "AppMoveChgDef", m_inventorApplication.ActiveDocument);
        }

        public double GetValueFromExpression(string expression)
        {
            double value = 0.0;

            Document activeDoc = m_inventorApplication.ActiveDocument;

            //get the unit of measure object
            UnitsOfMeasure unitsOfMeature = activeDoc.UnitsOfMeasure;

            //get the current length units of the user
            UnitsTypeEnum lengthUnitsType = unitsOfMeature.LengthUnits;

            //convert the expression to the current length units of user
            try
            {
                object vVal;
                vVal = unitsOfMeature.GetValueFromExpression(expression, lengthUnitsType);
                value = System.Convert.ToDouble(vVal);
            }
            catch(System.Exception e)
            {
                string strErrorMsg = e.Message;

                value = 0.0;
                return value;
            }

            return value;
        }

        public void UpdateCommandStatus()
        {
            m_moveCmdDlg.moveOkButton.Enabled = false;
         
            if (m_UCS != null)
            {
                m_UCS.Visible = false;
            }

            this.ShowUCS();

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            this.HighlightSelectEntity(m_selectiFeature);

            if (!m_moveCmdDlg.AllParametersAreValid())
            {
                return;
            }

            m_xOffset = GetValueFromExpression(m_moveCmdDlg.m_xOffsetExpression);
            m_yOffset = GetValueFromExpression(m_moveCmdDlg.m_yOffsetExpression);

            if (m_selectFace != null && (m_xOffset != 0 || m_yOffset != 0))
            {
                //update the preview
                //UpdatePreviewGraphics();

                //enable the OK button on dialog
                m_moveCmdDlg.moveOkButton.Enabled = true;
            }
        }

        //-----------------------------------------------------------------------------
        //----------------- MoveCmd implement ----------------------------------------
        //-----------------------------------------------------------------------------
        public void InitializePreviewGraphics()
        {
            m_interactionEvents = m_inventorApplication.CommandManager.CreateInteractionEvents();

            InteractionGraphics interactionGraphics = m_interactionEvents.InteractionGraphics;

            ClientGraphics previewClientGraphics = interactionGraphics.PreviewClientGraphics;

            m_previewClientGraphicsNode = previewClientGraphics.AddNode(1);

            m_pointGraphics = m_previewClientGraphicsNode.AddPointGraphics();

            GraphicsDataSets graphicsDateSets = interactionGraphics.GraphicsDataSets;

            m_graphicsCoordinateSet = graphicsDateSets.CreateCoordinateSet(1);

            m_graphicsColorSet = graphicsDateSets.CreateColorSet(1);

            m_graphicsColorSet.Add(1, 255, 0, 0);

            m_graphicsColorIndexSet = graphicsDateSets.CreateIndexSet(1);

            m_pointGraphics.CoordinateSet = m_graphicsCoordinateSet;
            m_pointGraphics.BurnThrough = true;
        }

        public void UpdatePreviewGraphics()
        {
            m_graphicsCoordinateSet = null;

            InteractionGraphics interactionGraphics = m_interactionEvents.InteractionGraphics;
            GraphicsDataSets graphicsDataSets = interactionGraphics.GraphicsDataSets;

            if (graphicsDataSets.Count != 0)
            {
                graphicsDataSets[1].Delete(); 
            }

          //  m_graphicsCoordinateSet = graphicsDataSets.CreateCoordinateSet(1);

            //m_pointGraphics.CoordinateSet = m_graphicsCoordinateSet;

            //TransientGeometry transientGeometry = m_inventorApplication.TransientGeometry;

            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis workAxis;
            workAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_selectFace, true);

            if (m_UCS == null)
            {
                return;
            }

            WorkPoint workPoint;
            workPoint = oPartCompDef.WorkPoints.AddByCurveAndEntity(workAxis, m_UCS.XYPlane);
            Point transPoint = workPoint.Point;

            UnitVector xUnitVector = m_UCS.XAxis.Line.Direction;
            UnitVector yUnitVector = m_UCS.YAxis.Line.Direction;

            Double[] xCoords = new Double[3];
            xUnitVector.GetUnitVectorData(ref xCoords);
            Double[] yCoords = new Double[3];
            yUnitVector.GetUnitVectorData(ref yCoords);

            double xOffset = this.GetValueFromExpression(m_moveCmdDlg.xOffsetText.Text);
            double yOffset = this.GetValueFromExpression(m_moveCmdDlg.yOffsetText.Text);

            Double[] transCoords = new Double[3];
            transCoords[0] = xOffset * xCoords[0] + yOffset * yCoords[0];
            transCoords[1] = xOffset * xCoords[1] + yOffset * yCoords[1];
            transCoords[2] = xOffset * xCoords[2] + yOffset * yCoords[2];

            Vector transVector = m_inventorApplication.TransientGeometry.CreateVector();
            transVector.PutVectorData(transCoords);

            transPoint.TranslateBy(transVector);

            Double[] oPointCoords = new Double[3];
            oPointCoords[0] = transPoint.X;
            oPointCoords[1] = transPoint.Y;
            oPointCoords[2] = transPoint.Z;

            m_graphicsCoordinateSet = graphicsDataSets.CreateCoordinateSet(1);

            m_graphicsCoordinateSet.PutCoordinates(oPointCoords);

            m_pointGraphics.CoordinateSet = m_graphicsCoordinateSet;

            TransientGeometry transientGeometry = m_inventorApplication.TransientGeometry;

           // m_graphicsCoordinateSet.Add(1, transPoint);

            m_inventorApplication.ActiveView.Update();
        }

        public void TerminatePreviewGraphics()
        {
            m_graphicsCoordinateSet.Delete();

            m_graphicsColorSet.Delete();

            m_graphicsColorIndexSet.Delete();

            m_pointGraphics.Delete();

            m_previewClientGraphicsNode.Delete();

            m_graphicsCoordinateSet = null;
            m_graphicsColorSet = null;
            m_graphicsColorIndexSet = null;
            m_pointGraphics = null;
            m_previewClientGraphicsNode = null;
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

        //-----------------------------------------------------------------------------
        //------------ Implementation of SelectEvents callbacks
        //-----------------------------------------------------------------------------
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
            int nomb = justSelectedEntities.Count;

            if (nomb > 0)
            {
                object selectedEntity = justSelectedEntities[1];

                if (m_moveCmdDlg.checkBoxChoose.Checked)
                {
                    m_selectFace = (Face)selectedEntity;

                    m_moveCmdDlg.checkBoxChoose.Checked = false;
                }

                this.GetSelectiFeature(m_selectFace, ref m_selectiFeature);

                m_selectEvents.AddToSelectedEntities(m_selectiFeature);

                DisableInteraction();

                UpdateCommandStatus();
            }
        }

    }
}
