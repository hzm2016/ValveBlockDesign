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
    internal class EditNetCmd:ValveBlockDesign.Command
    {
        private EditNetForm m_editNetForm;
        private Face m_selectFace;
        private iFeature m_selectiFeature;

        private UserCoordinateSystem m_UCS;
        private HighlightSet m_highlightSet;
        private ConnectToAccess m_connectToAccess;
        private ConnectToAccess m_connectToaccess;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;

        public  EditNetCmd()
        {
            m_editNetForm = null;
            m_selectFace = null;
            m_selectiFeature = null;

            m_UCS = null;
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

            //this.InitializePreviewGraphics();

            //create and display the dialog
            m_editNetForm = new EditNetForm(m_inventorApplication, this);

            if (m_editNetForm!= null)
            {
                m_editNetForm.Activate();
                m_editNetForm.TopMost = true;
                m_editNetForm.ShowInTaskbar = false;
                m_editNetForm.Show();
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
            m_editNetForm.Hide();
            m_editNetForm.Dispose();
            m_editNetForm = null;

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
            if (m_editNetForm.checkBoxChoose.Checked)
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
            string oldNet = m_editNetForm.dataportNET.SelectedRows[0].Cells[1].Value.ToString();
            string portName = m_editNetForm.dataportNET.SelectedRows[0].Cells[0].Value.ToString();
            string newPortName = m_selectiFeature.Name + "-" + portName;
            string netName = m_editNetForm.comboBNETs.Text;
            //EditPortNet(oldNet, netName, newPortName);
            EditNetRequest m_editNetRequest;
            m_editNetRequest = new EditNetRequest(m_inventorApplication,m_selectiFeature,portName,netName);
            base.ExecuteChangeRequest(m_editNetRequest, "AppEditNetChgDef", m_inventorApplication.ActiveDocument);
        }

        private void EditPortNet(string oldNet,string newNet,string portName)
        {
            m_connectToAccess = new ConnectToAccess(deFaultpath, "项目数据库");
            string update = "update NETList set " + oldNet + "='' where PortName ='" + portName + "'";
            if (m_connectToAccess.UpdateInformation(update))
            { };
            string updatenew = "update NETList set " + newNet + "='" + portName + "' where PortName ='" + portName + "'";
            if (m_connectToAccess.UpdateInformation(updatenew))
            { };
        }

        private void ClearHighlight()
        {
            m_highlightSet = null;
        }

        public void UpdateCommandStatus()
        {
            m_editNetForm.btnedit.Enabled = false;

            if (m_UCS != null)
            {
                m_UCS.Visible = false;
            }

            if (m_selectFace != null)
            {
                this.ShowUCS();
            }

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            if (m_selectiFeature != null)
            {
                m_editNetForm.dataportNET.Rows.Clear();
                this.HighlightSelectEntity(m_selectiFeature);
                m_editNetForm.groupBport.Text = m_selectiFeature.Name;
                PortNet(m_selectiFeature);
                
                //AttributeSets Ports = m_selectiFeature.AttributeSets;
                //AttributeSet myPorts = Ports["MyPorts"];
                //Inventor.Attribute AttPortNumber = myPorts["MyPortsNumber"];
                //int PortNumber = AttPortNumber.Value;
                //int i=0;
                //string num;
                //while (i < PortNumber)
                //{
                //    num = (++i).ToString();
                //    Inventor.Attribute Port = myPorts["Port"+num];
                //    string PortNet = Port.Value;
                //    i =m_editNetForm.dataportNET.Rows.Add();
                //    m_editNetForm.dataportNET.Rows[i].Cells[1].Value = PortNet;
                //    m_editNetForm.dataportNET.Rows[i].Cells[0].Value = ++i;
                //}
            }
            
        }

        public void PortNet(iFeature oifeature)
        {
            AttributeSets atr = oifeature.AttributeSets;

            AttributeSet abs = atr["MyAttribSet"];

            Inventor.Attribute att = abs["InternalName"];
            Inventor.Attribute footprint = abs["Footprint"];
            string footprintCheck = footprint.Value;
            Inventor.Attribute indexAttribute = abs["IndexName"];
            string indexName = indexAttribute.Value;
            Inventor.Attribute codeNameAttribute = abs["CodeName"];
            string codeName = codeNameAttribute.Value;
            Inventor.Attribute codeNumberAttribute = abs["CodeNumber"];
            string codeNumber = codeNumberAttribute.Value;
            m_connectToaccess = new ConnectToAccess(deFaultpath, "CavityLibrary", codeName, indexName, codeNumber);
            string sql = @"select * from ComponentsDb where ComponentsDb.IndexName='" + indexName + "'";
            string CavityType = m_connectToaccess.GetSingleInformation(sql, "CavityType");
            if (footprintCheck == "No")
            {
                AttributeSet myPorts = atr["MyPorts"];
                Inventor.Attribute AttPortNumber = myPorts["MyPortsNumber"];
                int PortNumber = AttPortNumber.Value;
                int i = 0;
                string num;
                while (i < PortNumber)
                {
                    num = (++i).ToString();
                    Inventor.Attribute Port = myPorts["Port" + num];
                    string PortNet = Port.Value;
                    i = m_editNetForm.dataportNET.Rows.Add();
                    m_editNetForm.dataportNET.Rows[i].Cells[1].Value = PortNet;
                    m_editNetForm.dataportNET.Rows[i].Cells[0].Value = ++i;
                }
            }
            else if (footprintCheck == "Yes" && CavityType == "二通插装孔")
            {
                AttributeSet myPorts = atr["MyPorts"];
                Inventor.Attribute AttPortNumber = myPorts["MyPortsNumber"];
                int PortNumber = AttPortNumber.Value;
                int i = 0;
                string num;
                while (i < PortNumber)
                {
                    num = (++i).ToString();
                    Inventor.Attribute Port = myPorts["Port" + num];
                    string PortNet = Port.Value;
                    i = m_editNetForm.dataportNET.Rows.Add();
                    m_editNetForm.dataportNET.Rows[i].Cells[1].Value = PortNet;
                    m_editNetForm.dataportNET.Rows[i].Cells[0].Value = ++i;
                }
                Inventor.Attribute portCountAttribute = myPorts["PortsCount"];
                int portCount = portCountAttribute.Value;
                int j = 1;
                while (j <= portCount)
                {
                    i = m_editNetForm.dataportNET.Rows.Add();
                    string portName = m_connectToaccess.SelectConnectToAccess("PortName" + j.ToString());
                    Inventor.Attribute PortsOther = myPorts[portName];
                    string NetName = PortsOther.Value;
                    m_editNetForm.dataportNET.Rows[i].Cells[1].Value = NetName;
                    m_editNetForm.dataportNET.Rows[i].Cells[0].Value = portName;
                    j++;
                }
            }
            else
            {
                AttributeSet myPorts = atr["MyPorts"];
                Inventor.Attribute portCountAttribute = myPorts["PortsCount"];
                int portCount = portCountAttribute.Value;
                int j = 1;
                int i = 0;
                while (j <= portCount)
                {
                    i = m_editNetForm.dataportNET.Rows.Add();
                    string portName = m_connectToaccess.SelectConnectToAccess("PortName" + j.ToString());
                    Inventor.Attribute PortsOther = myPorts[portName];
                    string NetName = PortsOther.Value;
                    m_editNetForm.dataportNET.Rows[i].Cells[1].Value = NetName;
                    m_editNetForm.dataportNET.Rows[i].Cells[0].Value = portName;
                    j++;
                }
            }
        }

        public void UpdateData()
        {
            if (m_selectiFeature != null && (m_editNetForm.dataportNET.SelectedRows.Count > 0) && (m_editNetForm.comboBNETs.Text.Length > 0))
            {
                m_editNetForm.btnedit.Enabled = true;
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

                if (m_editNetForm.checkBoxChoose.Checked)
                {
                    m_selectFace = (Face)selectedEntity;

                    m_editNetForm.checkBoxChoose.Checked = false;
                }

                this.GetSelectiFeature(m_selectFace, ref m_selectiFeature);

                m_selectEvents.AddToSelectedEntities(m_selectiFeature);

                DisableInteraction();

                UpdateCommandStatus();
            }
        }
    }
}
