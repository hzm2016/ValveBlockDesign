using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Inventor;
using System.Windows.Forms;
using System.Data.OleDb;


namespace ValveBlockDesign
{
    internal partial class RotateCmd:ValveBlockDesign.Command
    {
        private RotateForm m_rotateForm;
        private Face m_selectFace;
        private iFeature m_selectiFeature;
        private UserCoordinateSystem m_UCS;

        private HighlightSet m_highlightSet;

        public RotateCmd()
        {
            m_rotateForm = null;
            m_selectFace = null;
            m_selectiFeature = null;
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
            m_rotateForm = new RotateForm(m_inventorApplication, this);

            if (m_rotateForm != null)
            {
                m_rotateForm.Activate();
                m_rotateForm.TopMost = true;
                m_rotateForm.ShowInTaskbar = false;
                m_rotateForm.Show();
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
            m_rotateForm.Hide();
            m_rotateForm.Dispose();
            m_rotateForm = null;
            m_buttonDefinition.Pressed = false;


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
            if (m_rotateForm.checkBoxChoose.Checked)
            {
                //set the selection filer to a cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceCylindricalFilter);

                //set a cursor to specify face selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message
                m_interactionEvents.StatusBarText = "请选择需要旋转的孔";
            }
        }

        public override void DisableInteraction()
        {
            base.DisableInteraction();

            m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow, null, null);
        }

        public void UpdateCommandStatus()
        {
            m_rotateForm.btnsure.Enabled = false;

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

            

            if (m_selectFace != null)
            {
                //update the preview
                //UpdatePreviewGraphics();

                //enable the OK button on dialog
                m_rotateForm.btnsure.Enabled = true;
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

                if (m_rotateForm.checkBoxChoose.Checked)
                {
                    m_selectFace = (Face)selectedEntity;

                    m_rotateForm.checkBoxChoose.Checked = false;
                }

                this.GetSelectiFeature(m_selectFace, ref m_selectiFeature);

                m_selectEvents.AddToSelectedEntities(m_selectiFeature);

                DisableInteraction();

                UpdateCommandStatus();
            }
        }
        public override void ExecuteCommand()
        {
            double angle = 0.0;
            switch (m_rotateForm.comboBoxAngle.Text)
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
            RotateRequest m_rotateRequest = new RotateRequest(m_inventorApplication,m_selectFace,m_selectiFeature,angle);
            base.ExecuteChangeRequest(m_rotateRequest, "AppRotateiFeatureChgDef", m_inventorApplication.ActiveDocument);
        }
    }
}
