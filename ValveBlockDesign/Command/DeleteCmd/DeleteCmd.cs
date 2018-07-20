using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Inventor;

namespace ValveBlockDesign
{
    internal partial class DeleteCmd:ValveBlockDesign.Command
    {
        private Face m_selectFace;
        private iFeature m_selectiFeature;
        private DeleteCavityForm m_deleteCavityForm;
        private HighlightSet m_highlightSet;
        private string[] deleteName;
            

        public DeleteCmd()
        {
            m_selectFace = null;
            m_selectiFeature = null;
            m_deleteCavityForm = null;
            m_highlightSet = null;
            deleteName = new string[20];
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
            m_deleteCavityForm = new DeleteCavityForm(m_inventorApplication, this);

            if (m_deleteCavityForm != null)
            {
                m_deleteCavityForm.Activate();
                m_deleteCavityForm.TopMost = true;
                m_deleteCavityForm.ShowInTaskbar = false;
                m_deleteCavityForm.Show();
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
            m_deleteCavityForm.Hide();
            m_deleteCavityForm.Dispose();
            m_deleteCavityForm = null;

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
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
            if (m_deleteCavityForm.checkBoxChoose.Checked)
            {
                //set the selection filer to a cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceCylindricalFilter);

                //set a cursor to specify face selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message
                m_interactionEvents.StatusBarText = "请选择需要删除的元件";
            }
        }

        public override void DisableInteraction()
        {
            base.DisableInteraction();

            m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow, null, null);
        }

        public override void ExecuteCommand()
        {
            bool outlineDelete;
            if (m_deleteCavityForm.checkBoxChooseOutline.Checked)
            {
                outlineDelete = true;
            }
            else
            {
                outlineDelete = false;
            }
            //StopCommand();
            int i = 0;
            while (i < m_deleteCavityForm.listBCavity.Items.Count)
            {
                deleteName[i] = m_deleteCavityForm.listBCavity.Items[i].ToString();
                i = i + 1;
            }
            DeleteRequest deleteRequest = new DeleteRequest(m_inventorApplication, m_selectiFeature, outlineDelete, deleteName);
            base.ExecuteChangeRequest(deleteRequest, "AppDeleteiFeatureChgDef", m_inventorApplication.ActiveDocument);
        }

        public void UpdateCommandStatus()
        {
            m_deleteCavityForm.btnsure.Enabled = false;

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            this.HighlightSelectEntity(m_selectiFeature);
            
            //if (!m_deleteCavityForm.AllParametersAreValid())
            //{
            //    return;
            //}

            if (m_selectFace != null)
            {
                m_deleteCavityForm.btnsure.Enabled = true;
            }
            //AddInformation();
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
                        //AddInformation(ifeature);
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

                if (m_deleteCavityForm.checkBoxChoose.Checked)
                {
                    m_selectFace = (Face)selectedEntity;

                    m_deleteCavityForm.checkBoxChoose.Checked = false;
                   
                }

                this.GetSelectiFeature(m_selectFace, ref m_selectiFeature);

                m_selectEvents.AddToSelectedEntities(m_selectiFeature);
                //显示将要删除元件的ID号
                

                DisableInteraction();

                UpdateCommandStatus();
            }
        }

        public override void OnMouseUp(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            AddInformation();
        }

        public void AddInformation()
        {
            m_deleteCavityForm.listBCavity.Items.Add(m_selectiFeature.Name);
        }
    }
}
