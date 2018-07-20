using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class ConnectCmd : ValveBlockDesign.Command
    {
        private ConnectCmdDlg m_connectCmdDlg;

        //Command parameters
        private ExtrudeFeature m_thisSurface;
        private ExtrudeFeature m_connectToSurface;

        private iFeature m_thisCav;
        private iFeature m_connectToCav;

        private UserCoordinateSystem m_UCS;
        private HighlightSet m_highlightSet;

        private double m_Xposition;
        private double m_Yposition;
        private double m_Dia;
        private int m_portNumber;
        private int m_portIndex;

        private ConnectAlignTypeEnum m_connectAlignType;

        public enum ConnectAlignTypeEnum
        {
            kNoneType,
            kCenterAlignType,
            kOnesideAlignType,
            kOthersideAlignType,
        }

        public ConnectCmd()
        {
            m_connectCmdDlg = null;

            m_thisSurface = null;
            m_connectToSurface = null;

            m_thisCav = null;
            m_connectToCav = null;

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
            m_connectCmdDlg = new ConnectCmdDlg(m_inventorApplication, this);

            if (m_connectCmdDlg != null)
            {
                m_connectCmdDlg.Activate();
                m_connectCmdDlg.TopMost = true;
                m_connectCmdDlg.ShowInTaskbar = false;
                m_connectCmdDlg.Show();
            }

            //initialize data members
            m_thisSurface = null;
            m_connectToSurface = null;

            m_thisCav = null;
            m_connectToCav = null;

            m_UCS = null;

            //enable interaction
            EnableInteraction();
        }

        public override void StopCommand()
        {
            //Terminate this preview graphic
            TerminatePreviewGraphics();

            //destroy the command dialog
            m_connectCmdDlg.Hide();
            m_connectCmdDlg.Dispose();
            m_connectCmdDlg = null;

            if (m_highlightSet != null)
            {
                this.ClearHighlight();
            }

            if (m_UCS != null)
            {
                m_UCS.Visible = false;                             
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

            if (m_connectCmdDlg.checkBoxCav.Checked)
            {
                m_connectToSurface = null;
                m_connectToCav = null;
                if (m_UCS != null)
                {
                    m_UCS.Visible = false;
                }
                m_selectEvents.Enabled = true;

                //set the selection filer to a cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartSurfaceFeatureFilter);

                //set a cursor to specify face selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message
                m_interactionEvents.StatusBarText = "请选择孔";
            }
            else
            {
                if (m_thisSurface != null)
                {
                    m_selectEvents.AddToSelectedEntities(m_thisSurface);
                }

                m_selectEvents.Enabled = true;

                //set the selection filter to cylinder face
                m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartSurfaceFeatureFilter);

                //set a cursor to specify edge selection
                m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair, null, null);

                //set the status bar message                        
                m_interactionEvents.StatusBarText = "请选择与其相连接的孔";
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

            ConnectRequest connectRequest = new ConnectRequest(m_inventorApplication, m_connectAlignType, m_thisSurface, m_connectToSurface, m_thisCav, m_connectToCav, m_portIndex);

            base.ExecuteChangeRequest(connectRequest, "AppConnectChgDef", m_inventorApplication.ActiveDocument);
        }

        public void UpdateCommandStatus()
        {
            m_connectCmdDlg.connectOkButton.Enabled = false;           

            //更新连接对齐类型
            if (m_connectCmdDlg.centerRadioButton.Checked == false && m_connectCmdDlg.onesideRadioButton.Checked == false && m_connectCmdDlg.othersideRadioButton.Checked == false)
            {
                m_connectAlignType = ConnectAlignTypeEnum.kNoneType;
            }
            else
            {
                if (m_connectCmdDlg.centerRadioButton.Checked == true)
                {
                    m_connectAlignType = ConnectAlignTypeEnum.kCenterAlignType;
                }
                else
                {
                    if (m_connectCmdDlg.onesideRadioButton.Checked == true)
                    {
                        m_connectAlignType = ConnectAlignTypeEnum.kOnesideAlignType;
                    }
                    else
                    {
                        m_connectAlignType = ConnectAlignTypeEnum.kOthersideAlignType;
                    }
                }
            }

            //更新元件1的UCS使其显示，并将其选中port加入高亮显示集
            if (m_thisSurface != null)
            {                
                if (m_highlightSet != null)
                {
                    this.ClearHighlight();
                }
                this.HightghtSelectSurface(m_thisSurface);

                //若两元件均已选择完毕，则判断两元件是否平行
                if (m_connectToSurface != null)
                {
                    m_connectCmdDlg.connectOkButton.Enabled = true;

                    if (IsCavParallel())
                    {
                        MessageBox.Show("无法连接平行油孔！请选择其他油孔。");

                        m_connectToSurface = null;
                        m_connectToCav = null;

                        if (m_highlightSet != null)
                        {
                            this.ClearHighlight();
                        }

                        m_connectCmdDlg.connectOkButton.Enabled = false;
                        return;
                    }

                    this.HightghtSelectSurface(m_connectToSurface);

                    UpdatePreviewGraphics();
                }
                else
                {
                    m_connectCmdDlg.connectOkButton.Enabled = false;
                }
            }
                       
            //更新元件1相对位置
            UnitsOfMeasure unitsOfMeasure = m_inventorApplication.ActiveDocument.UnitsOfMeasure;
            string unitExpre = unitsOfMeasure.GetStringFromType(unitsOfMeasure.LengthUnits);
            m_Xposition = unitsOfMeasure.ConvertUnits(double.Parse(m_connectCmdDlg.numerUPX.Text), unitExpre, "cm");
            m_Yposition = unitsOfMeasure.ConvertUnits(double.Parse(m_connectCmdDlg.numerUPY.Text), unitExpre, "cm");

            //更新元件2 portIndex
            if (m_connectCmdDlg.portIndexComboBox.Text != "")
            {
                m_portIndex = int.Parse(m_connectCmdDlg.portIndexComboBox.Text);
            }

            //更新元件1 port1的Dia
            if (m_connectCmdDlg.diaTextBox.Text != "")
            {
                m_Dia = unitsOfMeasure.ConvertUnits(double.Parse(m_connectCmdDlg.diaTextBox.Text), unitExpre, "cm");
            }
        }

        private void HightghtSelectSurface(ExtrudeFeature thisSurface, ExtrudeFeature connectToSurface)
        {
            m_highlightSet = m_inventorApplication.ActiveDocument.CreateHighlightSet();

            Inventor.Color green;
            green = m_inventorApplication.TransientObjects.CreateColor(0, 255, 0);
            green.Opacity = 0.3;

            m_highlightSet.Color = green;

            m_highlightSet.AddItem(thisSurface);
            m_highlightSet.AddItem(connectToSurface);
        }

        private void HightghtSelectSurface(ExtrudeFeature surfaceFeature)
        {
            m_highlightSet = m_inventorApplication.ActiveDocument.CreateHighlightSet();

            Inventor.Color green;
            green = m_inventorApplication.TransientObjects.CreateColor(0, 255, 0);
            green.Opacity = 0.3;

            m_highlightSet.Color = green;

            m_highlightSet.AddItem(surfaceFeature);
        }

        private void ClearHighlight()
        {
            m_highlightSet = null;
        }

        public void UpdatePreviewGraphics()
        {
        }

        void TerminatePreviewGraphics()
        {
        }

        //-----------------------------------------------------------------------------
        //------------ Implementation of SelectEvents callbacks
        //-----------------------------------------------------------------------------
        public override void OnPreSelect(object preSelectEntity, out bool doHighlight, ObjectCollection morePreSelectEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            doHighlight = false;

            if (preSelectEntity is ExtrudeFeature)
            {
                ExtrudeFeature preSelectFeature = (ExtrudeFeature)preSelectEntity;

                if (preSelectFeature.Faces[1].SurfaceType == SurfaceTypeEnum.kCylinderSurface)
                { 
                    doHighlight = true; 
                }
            }            
        }

        public override void OnSelect(ObjectsEnumerator justSelectedEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, Inventor.View view)
        {
            int nombSelectedEntities = justSelectedEntities.Count;

            if (nombSelectedEntities > 0)
            {
                object selectedEntity = justSelectedEntities[1];

                if (m_connectCmdDlg.checkBoxCav.Checked)
                {
                    m_thisSurface = (ExtrudeFeature )selectedEntity;
                    m_connectCmdDlg.checkBoxCav.Checked = false;

                    //获取所选IFeature的位置和直径，并显示
                    GetiFeature(m_thisSurface, ref m_thisCav);
                    if (m_thisCav != null)
                    {
                        ClearUCS();
                        ShowUCS();

                        try
                        {
                            GetiFeatureData(m_thisCav, ref m_Xposition, ref m_Yposition, ref m_Dia);        //显示位置
                            ShowLocation(m_Xposition, m_Yposition);

                            m_connectCmdDlg.groupBox3.Enabled = true;
                            m_connectCmdDlg.checkBoxToCav.Checked = true;
                            ShowDia(m_Dia);     //显示直径
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("发生错误！ " + ex.ToString());
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("未选中元件，请重新选择！");
                        return;
                    }

                    m_connectToSurface = null;
                    EnableInteraction();                    
                }

                if (m_connectCmdDlg.checkBoxToCav.Checked && (selectedEntity != m_thisSurface))
                {
                    m_connectToSurface = (ExtrudeFeature)selectedEntity;
                    m_connectCmdDlg.checkBoxToCav.Checked = false;

                    GetiFeature(m_connectToSurface, ref m_connectToCav);
                    GetPortInformation(m_connectToSurface, ref m_portNumber, ref m_portIndex);
                    try
                    {
                        ResetComboBox(m_portNumber, m_portIndex);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("获取port时发生错误！ " + ex.ToString());
                        return;
                    }
                                            
                    DisableInteraction();
                    m_selectEvents.Enabled = false;
                }
            }           
        }

        //由选择曲面获取iFeature特征
        private void GetiFeature(ExtrudeFeature surfaceFeature, ref iFeature ifeature)
        {
            string ifeatureName;
            try
            {
                AttributeSets surfAttributeSets;
                surfAttributeSets = surfaceFeature.AttributeSets;

                AttributeSet surfAttibSet;
                surfAttibSet = surfaceFeature.AttributeSets["MyAttribSet"];

                ifeatureName = (string)surfAttibSet["iFeatureName"].Value;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取属性集失败！  " + ex.ToString());
                return;
            }

            //获取iFeature
            PartDocument oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;
            iFeatures oiFeatures = oPartDoc.ComponentDefinition.Features.iFeatures;
            ifeature = oiFeatures[ifeatureName];
        }

        //获取单独孔IFeature的相对坐标和Dia
        private void GetiFeatureData(iFeature ifeature, ref double Xposition, ref double Yposition, ref double dia8)
        {
            string iFeatureInterName;
            try
            {
                AttributeSets oAttributeSets;
                oAttributeSets = ifeature.AttributeSets;

                AttributeSet oAttibSet;
                oAttibSet = ifeature.AttributeSets["MyAttribSet"];

                iFeatureInterName = (string)oAttibSet["InternalName"].Value;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取属性集失败！  " + ex.ToString());
                return;
            }

            //bool flag1 = false;
            //bool flag2 = false;
            //bool flag3 = false;
            foreach (iFeatureInput oInput in ifeature.iFeatureDefinition.iFeatureInputs)
            {                
                if (oInput.Name == iFeatureInterName + ":x")
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    Xposition = oiFeatureParaInput.Parameter.Value;
                    //flag1 = true;
                }
                if (oInput.Name == iFeatureInterName + ":y")
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    Yposition = oiFeatureParaInput.Parameter.Value;
                    //flag2 = true;
                }
                if (oInput.Name == iFeatureInterName + ":Dia8")
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    dia8 = oiFeatureParaInput.Parameter.Value;
                    //flag3 = true;
                }
                //if (flag1 && flag2 && flag3)
                //    break;              
            }
        }

        //使能面板上的位置显示控件，并显示位置
        private void ShowLocation(double Xposition, double Yposition)
        {
            m_connectCmdDlg.numerUPX.Enabled = true;
            m_connectCmdDlg.numerUPY.Enabled = true;

            UnitsOfMeasure unitsOfMeasure = m_inventorApplication.ActiveDocument.UnitsOfMeasure;
            string unitExpre = unitsOfMeasure.GetStringFromType(unitsOfMeasure.LengthUnits);

            m_connectCmdDlg.numerUPX.Text = (unitsOfMeasure.ConvertUnits(Xposition, "cm", unitExpre)).ToString();
            m_connectCmdDlg.numerUPY.Text = (unitsOfMeasure.ConvertUnits(Yposition, "cm", unitExpre)).ToString();
        }

        private void ShowDia(double dia)
        {
             //get the active document
            Document activeDocument = m_inventorApplication.ActiveDocument;
            //get the unit of measure object
            UnitsOfMeasure unitsOfMeasure = activeDocument.UnitsOfMeasure;

            string unitExpre = unitsOfMeasure.GetStringFromType(unitsOfMeasure.LengthUnits);

            m_connectCmdDlg.diaTextBox.Text = (unitsOfMeasure.ConvertUnits(dia, "cm", unitExpre)).ToString();            
        }

        private void GetPortInformation(ExtrudeFeature surfaceFeature, ref int portNumber, ref int portIndex)
        { 
            try
            {
                AttributeSets surfAttributeSets;
                surfAttributeSets = surfaceFeature.AttributeSets;

                AttributeSet surfAttibSet;
                surfAttibSet = surfaceFeature.AttributeSets["MyAttribSet"];

                portNumber = (int)surfAttibSet["PortNumber"].Value;
                portIndex = (int)surfAttibSet["PortIndex"].Value;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取属性集失败！  " + ex.ToString());
                return;
            }
        }

        private void ResetComboBox(int portNumber, int portIndex)
        {
            m_connectCmdDlg.portIndexComboBox.Items.Clear();
            for (int i = 1; i <= m_portNumber; i++)
            {
                m_connectCmdDlg.portIndexComboBox.Items.Add(i);
            }
            m_connectCmdDlg.portIndexComboBox.Text = portIndex.ToString();
        }

        private bool IsCavParallel()
        {
            bool isCavParallel = false;

            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            //获取m_thisCav中心轴向量
            WorkAxis oThisAxis;
            oThisAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_thisSurface.Faces[1], true);
            Line oThisAxisLine;
            oThisAxisLine = oThisAxis.Line;
            UnitVector thisAxisVector = oThisAxisLine.Direction;

            //获取m_connectToSurface中心轴向量
            WorkAxis oConneToAxis;
            oConneToAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_connectToSurface.Faces[1], true);
            Line oConneToAxisLine;
            oConneToAxisLine = oConneToAxis.Line;
            UnitVector conneToAxisVector = oConneToAxisLine.Direction;

            isCavParallel = thisAxisVector.IsParallelTo(conneToAxisVector, 0.01);
            return isCavParallel;
        }

        private void GetUCSIndex(ExtrudeFeature surfaceFeature, ref int i)//用于判断选择的面特征的用户坐标系
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            int portIndex;
            try
            {
                AttributeSets surfAttributeSets;
                surfAttributeSets = surfaceFeature.AttributeSets;

                AttributeSet surfAttibSet;
                surfAttibSet = surfaceFeature.AttributeSets["MyAttribSet"];

                portIndex = (int)surfAttibSet["PortIndex"].Value;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取属性集失败！  " + ex.ToString());
                return;
            }

            #region Use "Line.Direction" to get the UCS
            WorkAxis workAxis;
            workAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(surfaceFeature.Faces[1], true);
            Line oAxisLine;
            oAxisLine = workAxis.Line;
            UnitVector oAxisVector;
            oAxisVector = oAxisLine.Direction;

            Double[] coords = new Double[3];
            oAxisVector.GetUnitVectorData(ref coords);

            string strCoords;
            if (portIndex == 1)
            {
                strCoords = (-coords[0]).ToString() + (-coords[1]).ToString() + (-coords[2]).ToString();
            }
            else
            {
                strCoords = coords[0].ToString() + coords[1].ToString() + coords[2].ToString();
            }

            switch (strCoords)
            {
                case "001":
                    i = 1;
                    break;
                case "0-10":
                    i = 2;
                    break;
                case "100":
                    i = 3;
                    break;
                case "010":
                    i = 4;
                    break;
                case "-100":
                    i = 5;
                    break;
                case "00-1":
                    i = 6;
                    break;
            }
            #endregion
        }

        private void ClearUCS()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            oPartCompDef.UserCoordinateSystems[1].Visible = false;
            oPartCompDef.UserCoordinateSystems[2].Visible = false;
            oPartCompDef.UserCoordinateSystems[3].Visible = false;
            oPartCompDef.UserCoordinateSystems[4].Visible = false;
            oPartCompDef.UserCoordinateSystems[5].Visible = false;
            oPartCompDef.UserCoordinateSystems[6].Visible = false;
        }

        private void ShowUCS()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            int index = 0;
            GetUCSIndex(m_thisSurface, ref index);
            UserCoordinateSystems oUCSs;
            oUCSs = oPartCompDef.UserCoordinateSystems;

            if (index == 0)
            {
                MessageBox.Show("发生错误！");
                return;
            }
            m_UCS = oUCSs[index];            

            m_UCS.Visible = true;
            m_UCS.XAxis.Visible = false;
            m_UCS.YAxis.Visible = false;
            m_UCS.ZAxis.Visible = false;
            m_UCS.XYPlane.Visible = false;
            m_UCS.XZPlane.Visible = false;
            m_UCS.YZPlane.Visible = false;
        }
    }
}
