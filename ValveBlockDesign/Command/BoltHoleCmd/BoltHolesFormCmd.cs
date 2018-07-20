using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inventor;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ValveBlockDesign
{
    internal class BoltHolesFormCmd:ValveBlockDesign.Command
    {
        private BoltHoleForm m_boltHoleForm;
        private Face m_withThisFace;
        private HighlightSet m_highlightSet;
        private ConnectToAccess m_connectToAccess;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;

        public BoltHolesFormCmd()
        {
            m_boltHoleForm = null;
            m_withThisFace = null;
            m_highlightSet = null;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName + "\\CavityLibrary";    
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
            m_boltHoleForm = new BoltHoleForm(m_inventorApplication, this);

            if (m_boltHoleForm != null)
            {
                m_boltHoleForm.Activate();
                m_boltHoleForm.TopMost = true;
                m_boltHoleForm.ShowInTaskbar = false;
                m_boltHoleForm.Show();
                m_boltHoleForm.comBLibrary.Text = "BoltHoles";
                AddInformation();
            }

            //initialize this command data members
            m_withThisFace = null;

            //enable interaction
            EnableInteraction();
        }

        public void AddInformation()
        {
            
            m_boltHoleForm.radioFour.Checked = true;
            m_boltHoleForm.btnsure.Enabled = false;
            m_boltHoleForm.checkBoxFace.Checked = true;
            string[] getresult = new string[25];
            
            string indexname=m_boltHoleForm.comBLibrary.Text;
            string sql = @"select * from "+indexname;
            m_connectToAccess = new ConnectToAccess(deFaultpath, "CavityLibrary");
            m_connectToAccess.GetInformation(sql, "编码", out getresult);
            int i = 0;
            while (getresult[i] != null)
            {
                m_boltHoleForm.comBNumber.Items.Add(getresult[i]);
                i++;
            }
        }

        public override void StopCommand()
        {
            //Terminate this preview graphic

            //destroy the command dialog
            m_inventorApplication.ActiveView.Update();
            m_boltHoleForm.Hide();
            m_boltHoleForm.Dispose();
            m_boltHoleForm = null;

            base.StopCommand();
            m_buttonDefinition.Pressed = false;

            ////set the command status to not-running
            //m_commandIsRunning = false;
        }

        public override void EnableInteraction()
        {
            base.EnableInteraction();

            //clear selection filter
            m_selectEvents.ClearSelectionFilter();

            //reset selections
            m_selectEvents.ResetSelections();

            //specify selection filter and cuisor
            if (m_withThisFace != null)
            {
                m_selectEvents.AddToSelectedEntities(m_withThisFace);
            }
            else
            {
                //set the selection filer to a cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceFilter);

                //set a cursor to specify face selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message
                m_interactionEvents.StatusBarText = "请选择面";
            }
        }

        public override void DisableInteraction()
        {
            base.DisableInteraction();
            m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow, null, null);
        }

        public void UpdateCommandStatus()
        {
            m_selectEvents.ClearSelectionFilter();
            //reset selections
            m_selectEvents.ResetSelections();
            if (m_withThisFace != null)
            {
                m_withThisFace = null;
            }
            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }
            DisableInteraction();
        }

        public override void OnSelect(ObjectsEnumerator justSelectedEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            if (m_boltHoleForm.checkBoxFace.Checked == true)
            {
                if (m_withThisFace == null)
                {
                    m_withThisFace = justSelectedEntities[1];
                    this.HightghtSelectFace(m_withThisFace);
                }
                else
                {
                    this.ClearHighlight();
                    m_withThisFace = justSelectedEntities[1];
                    this.HightghtSelectFace(m_withThisFace);
                }
                m_boltHoleForm.checkBoxFace.Checked = false;
            }
        }

        public override void OnPreSelect(object preSelectEntity, out bool doHighlight, ObjectCollection morePreSelectEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            doHighlight = false;

            if (preSelectEntity is Face)
            {
                doHighlight = true;
            }
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

            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            UserCoordinateSystems oUCSs;
            oUCSs = oPartCompDef.UserCoordinateSystems;
            foreach (UserCoordinateSystem m_UCS in oUCSs)
            {
                if (m_UCS.Visible == true)
                    m_UCS.Visible = false;
            }
        }

        public override void ExecuteCommand()
        {
            BoltHolesRequest m_boltHolesRequest = new BoltHolesRequest(m_inventorApplication,m_withThisFace, m_boltHoleForm);
            base.ExecuteChangeRequest(m_boltHolesRequest, "AppInsertBoltHolesChgDef", m_inventorApplication.ActiveDocument);
        }
    }
}
