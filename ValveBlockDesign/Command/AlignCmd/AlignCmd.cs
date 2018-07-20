using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class AlignCmd : ValveBlockDesign.Command
    {
        //Align command dialog
        private AlignCmdDlg m_alignCmdDlg;

        //Align command parameters
        private Face m_thisFace;
        private Face m_withThisFace;
        private int m_direction;

        private iFeature m_thisiFeature;
        private iFeature m_withThisiFeature;

        private UserCoordinateSystem m_UCS;

        private HighlightSet m_highlightSet;

        internal enum AlignDirectionEnum
        {
            xDirection,
            yDirection
        };

        public AlignCmd()
        {
            m_alignCmdDlg = null;

            m_thisFace = null;
            m_withThisFace = null;

            m_thisiFeature = null;
            m_withThisiFeature = null;

            m_UCS = null;

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

            //initialize interaction previewgraphics objects

            //create and display the dialog
            m_alignCmdDlg = new AlignCmdDlg(m_inventorApplication, this);

            if (m_alignCmdDlg != null)
            {
                m_alignCmdDlg.Activate();
                m_alignCmdDlg.TopMost = true;
                m_alignCmdDlg.ShowInTaskbar = false;
                m_alignCmdDlg.Show();
            }

            //initialize this command data members
            m_thisFace = null;
            m_withThisFace = null;

            m_thisiFeature = null;
            m_withThisiFeature = null;

            m_UCS = null;

            //enable interaction
            EnableInteraction();
        }

        public override void StopCommand()
        {
            //Terminate this preview graphic
            TerminatePreviewGraphics();

            //destroy the command dialog
            m_alignCmdDlg.Hide();
            m_alignCmdDlg.Dispose();
            m_alignCmdDlg = null;

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
            if (m_alignCmdDlg.checkBoxCavity.Checked)
            {
                if (m_withThisFace != null)
                {
                    m_selectEvents.AddToSelectedEntities(m_withThisFace);
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
                if (m_thisFace != null)
                {
                    m_selectEvents.AddToSelectedEntities(m_thisFace);
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
            StopCommand();

            //create the request
            AlignRequest alignRequest = new AlignRequest(m_inventorApplication, m_thisFace, m_withThisFace, m_direction, m_UCS,m_thisiFeature,m_withThisiFeature);

            //execute the request
            base.ExecuteChangeRequest(alignRequest, "AppAlignChgDef", m_inventorApplication.ActiveDocument);
        }

        void TerminatePreviewGraphics()
        {
        }

        public void UpdateCommandStatus()
        {
            //by default ,disable the ok button
            m_alignCmdDlg.alignOkButton.Enabled = false;

            UnitVector oThisVector;
            UnitVector oWithThisVector;

            GetVectorFromFaces(out oThisVector, out oWithThisVector);

            Double tolerance = 0.01;

            if (oThisVector.IsEqualTo(oWithThisVector, tolerance))
            {
                m_alignCmdDlg.XDirectRadioButton.Enabled = true;
                m_alignCmdDlg.YDirectRadioButton.Enabled = true;
            }
            else
            {
                m_alignCmdDlg.XDirectRadioButton.Enabled = false;
                m_alignCmdDlg.YDirectRadioButton.Enabled = false;

                m_alignCmdDlg.XDirectRadioButton.Checked = false;
                m_alignCmdDlg.YDirectRadioButton.Checked = false;
            }

            if (m_alignCmdDlg.XDirectRadioButton.Enabled == true && m_alignCmdDlg.XDirectRadioButton.Checked == true)
            {
                m_direction = 1;
            }
            else
            {
                if (m_alignCmdDlg.YDirectRadioButton.Enabled == true && m_alignCmdDlg.YDirectRadioButton.Checked == true)
                {
                    m_direction = 2;
                }
                else
                {
                    m_direction = 3;
                }
            }

            if (m_alignCmdDlg.XDirectRadioButton.Enabled == true && m_alignCmdDlg.YDirectRadioButton.Enabled == true)
            {
                if (m_thisFace != null && m_withThisFace != null)
                {
                    if (m_UCS != null)
                    {
                        m_UCS.Visible = false;
                    }

                    this.ShowUCS();

                    if (m_alignCmdDlg.XDirectRadioButton.Checked == true || m_alignCmdDlg.YDirectRadioButton.Checked == true)
                    {
                        m_alignCmdDlg.alignOkButton.Enabled = true;
                    }
                }
            }
            else
            {
                if (m_thisFace != null && m_withThisFace != null)
                {
                    if (m_UCS != null)
                    {
                        m_UCS.Visible = false;
                    }

                    this.ShowUCS();

                    m_alignCmdDlg.alignOkButton.Enabled = true;
                }
            }

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            this.HightghtSelectFace(m_thisFace, m_withThisFace);
        }

        //-----------------------------------------------------------------------------
        //----------------- AlignCmd implement ----------------------------------------
        //-----------------------------------------------------------------------------
        private void GetVectorFromFaces(out UnitVector thisAxisVector, out UnitVector withThisAxisVector)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis oThisAxis;
            oThisAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_thisFace, true);
            Line oThisAxisLine;
            oThisAxisLine = oThisAxis.Line;

            thisAxisVector = oThisAxisLine.Direction;

            WorkAxis oWithThisAxis;
            oWithThisAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_withThisFace, true);
            Line oWithThisAxisLine;
            oWithThisAxisLine = oWithThisAxis.Line;

            withThisAxisVector = oWithThisAxisLine.Direction;
        }
        
        //Show the UCS for the m_withiFeature
        private void ShowUCS()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            this.GetSelectiFeature(m_thisFace, ref m_thisiFeature);
            this.GetSelectiFeature(m_withThisFace, ref m_withThisiFeature);

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
            WorkAxis workAxis;
            workAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_withThisFace, true);
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
            #endregion

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

        private void HightghtSelectFace(Face thisFace,Face withThisFace)
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
            int nombSelectedEntities = justSelectedEntities.Count;

            if (nombSelectedEntities > 0)
            {
                object selectedEntity = justSelectedEntities[1];

                //if cavity face is selected
                if ((m_alignCmdDlg.checkBoxCavity.Checked) && (selectedEntity != m_withThisFace))
                {
                    m_thisFace = (Face)selectedEntity;

                    m_alignCmdDlg.checkBoxCavity.Checked = false;

                    if (m_withThisFace == null)
                    {
                        m_alignCmdDlg.checkBoxWithThisCavity.Checked = true;

                        EnableInteraction();
                    }
                    else
                    {
                        DisableInteraction();

                        UpdateCommandStatus();
                    }
                }

                if ((m_alignCmdDlg.checkBoxWithThisCavity.Checked) && (selectedEntity != m_thisFace))
                {
                    m_withThisFace = (Face)selectedEntity;

                    m_alignCmdDlg.checkBoxWithThisCavity.Checked = false;

                    if (m_thisFace == null)
                    {
                        m_alignCmdDlg.checkBoxCavity.Checked = true;

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
