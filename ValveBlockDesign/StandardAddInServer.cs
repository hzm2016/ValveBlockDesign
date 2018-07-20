using System;
using System.Runtime.InteropServices;
using Inventor;
using Microsoft.Win32;

using System.Windows.Forms;
using stdole;
using System.Drawing;
using System.IO;
using System.Data.OleDb;

namespace ValveBlockDesign
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("c29d5be2-c9f7-4783-9191-5070d4944568")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {
        #region
        // Inventor application object.
        private Inventor.Application m_inventorApplication;

        //get the sources for icons.
        private System.Reflection.Assembly assembly;
        string[] resources;

        //Icon object
        private object oIPictureDispStandard;
        private object oIPictureDispLarge;

        private ValveBlockDesign.BlockFormCmd m_blockFormCmd;
        private ValveBlockDesign.AlignCmd m_alignCmd;
        private ValveBlockDesign.ExtractiFeatureCmd m_extractiFeatureCmd;
        private ValveBlockDesign.iFeatureFormCmd  m_iFeatureFormCmd;
        private ValveBlockDesign.MoveCmd m_moveCmd;
        private ValveBlockDesign.ConnectCmd m_connectCmd;
        private ValveBlockDesign.CavityLibraryEditCmd m_cavityLibraryEditCmd;
        private ValveBlockDesign.CavityLibraryScanCmd m_cavityLibraryScanCmd;
        private ValveBlockDesign.CavityLibraryAddCmd  m_cavityLibraryAddCmd;
        private ValveBlockDesign.BoltHolesFormCmd m_boltHolesFormCmd;
        private ValveBlockDesign.DeleteCmd m_deleteCmd;
        private ValveBlockDesign.RotateCmd m_rotateCmd;
        private ValveBlockDesign.InsertOutlineCmd m_insertOutlineCmd;
        private ValveBlockDesign.InsertXportCmd m_insertXportCmd;
        private ValveBlockDesign.EditNetCmd m_editNetCmd;

        private String m_strAddInGUID;

        //user interface event
        private UserInterfaceEvents m_userInterfaceEvents;

        //event handler delegates
        private Inventor.UserInterfaceEventsSink_OnResetCommandBarsEventHandler UserInterfaceEventsSink_OnResetCommandBarsEventDelegate;
        private Inventor.UserInterfaceEventsSink_OnEnvironmentChangeEventHandler UserInterfaceEventsSink_OnEnvironmentChangeEventDelegate;
        private Inventor.UserInterfaceEventsSink_OnResetRibbonInterfaceEventHandler UserInterfaceEventsSink_OnResetRibbonInterfaceEventDelegate;

        #endregion

        public StandardAddInServer()
        {
            m_inventorApplication = null;
            m_blockFormCmd = null;
            m_alignCmd = null;
            m_extractiFeatureCmd = null;
            m_iFeatureFormCmd = null;
            m_cavityLibraryEditCmd = null;
            m_cavityLibraryAddCmd = null;
            m_cavityLibraryScanCmd = null;
            m_boltHolesFormCmd = null;
            m_deleteCmd = null;
            m_rotateCmd = null;
            m_insertOutlineCmd = null;
            m_insertXportCmd = null;
            m_editNetCmd = null;
        }

        #region ApplicationAddInServer Members

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            try
            {
                // This method is called by Inventor when it loads the addin.
                // The AddInSiteObject provides access to the Inventor Application object.
                // The FirstTime flag indicates if the addin is loaded for the first time.

                #region Initialize AddIn members and event delegate
                // Initialize AddIn members.
                m_inventorApplication = addInSiteObject.Application;

                //Initialize event delegates
                m_userInterfaceEvents = m_inventorApplication.UserInterfaceManager.UserInterfaceEvents;

                UserInterfaceEventsSink_OnResetCommandBarsEventDelegate = new UserInterfaceEventsSink_OnResetCommandBarsEventHandler(UserInterfaceEvents_OnResetCommandBars);
                m_userInterfaceEvents.OnResetCommandBars += UserInterfaceEventsSink_OnResetCommandBarsEventDelegate;

                UserInterfaceEventsSink_OnEnvironmentChangeEventDelegate = new UserInterfaceEventsSink_OnEnvironmentChangeEventHandler(UserInterfaceEvents_OnEnvironmentChange);
                m_userInterfaceEvents.OnEnvironmentChange += UserInterfaceEventsSink_OnEnvironmentChangeEventDelegate;

                UserInterfaceEventsSink_OnResetRibbonInterfaceEventDelegate = new UserInterfaceEventsSink_OnResetRibbonInterfaceEventHandler(UserInterfaceEventsSink_OnResetRibbonInterface);
                m_userInterfaceEvents.OnResetRibbonInterface += UserInterfaceEventsSink_OnResetRibbonInterfaceEventDelegate;
                #endregion

                // TODO: Add ApplicationAddInServer.Activate implementation.
                // e.g. event initialization, command creation etc.

                String strAddInGUID = "{" + ((GuidAttribute)System.Attribute.GetCustomAttribute(typeof(StandardAddInServer), typeof(GuidAttribute))).Value + "}";
                m_strAddInGUID = strAddInGUID;

                #region ������ť.  create the buttons

                //get all the names of the current assembly
                this.IconPictureInitial();
                //-----------------------------------------------------------------------------------
                //create the instance of the "BlockGenerator" command and button
                m_blockFormCmd = new BlockFormCmd();
                this.CreateButton(
                    "ValveBlockDesign.resources.BlockStandard.ico",
                    "ValveBlockDesign.resources.BlockLarge.ico",
                    m_blockFormCmd, 
                    "��",
                    "AppBlockGenerateCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "Generate a block",
                    "Generate a block with parameters entered by user",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "Align" command and button
                m_alignCmd = new AlignCmd();
                this.CreateButton(
                    "ValveBlockDesign.resources.AlignStandard.ico",
                    "ValveBlockDesign.resources.AlignLarge.ico",
                    m_alignCmd,
                    "����",
                    "AppAlignCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "Cavity align",
                    "Cavity align",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "ExtractiFeature" command and button
                m_extractiFeatureCmd = new ExtractiFeatureCmd();
                this.CreateButton(
                     "ValveBlockDesign.resources.ExtractiFeatureStandard.ico",
                     "ValveBlockDesign.resources.ExtractiFeatureLarge.ico",
                     m_extractiFeatureCmd,
                     "��ȡģ��",
                     "AppExtractiFeatureCmd",
                     CommandTypesEnum.kShapeEditCmdType,
                     strAddInGUID,
                     "��ȡ����",
                     "��ȡ����",
                     ButtonDisplayEnum.kDisplayTextInLearningMode,
                     false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "InsertiFeature" command and button
                m_iFeatureFormCmd = new iFeatureFormCmd();
                this.CreateButton(
                    "ValveBlockDesign.resources.InsertiFeatureStandard.ico",
                    "ValveBlockDesign.resources.InsertiFeatureLarge.ico",
                    m_iFeatureFormCmd,
                    "Ԫ������",
                    "AppInsertiFeatureCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "��������",
                    "��������",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "DeleteiFeature" command and button
                m_deleteCmd = new DeleteCmd();
                this.CreateButton(
                    "ValveBlockDesign.resources.DeleteCmdStandard.ico",
                    "ValveBlockDesign.resources.DeleteCmdLarge.ico",
                    m_deleteCmd,
                    "ɾ��",
                    "AppDeleteiFeatureCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "ɾ������",
                    "ɾ������",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "RotateiFeature" command and button
                m_rotateCmd = new RotateCmd();
                this.CreateButton(
                    "ValveBlockDesign.resources.RotateCmdStandard.ico",
                    "ValveBlockDesign.resources.RotateCmdLarge.ico",
                    m_rotateCmd,
                    "��ת",
                    "AppRotateiFeatureCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "��ת����",
                    "��ת����",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "InsertOutline" command and button
                m_insertOutlineCmd = new InsertOutlineCmd();
                this.CreateButton(
                    "ValveBlockDesign.resources.InsertiFeatureStandard.ico",
                    "ValveBlockDesign.resources.OutLineCmdLarge.ico",
                    m_insertOutlineCmd,
                    "��װ����",
                    "AppInsertOutlineCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "���밲װ����",
                    "���밲װ����",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "Move" command and button
                m_moveCmd = new MoveCmd();
                this.CreateButton(
                    "ValveBlockDesign.resources.MoveCmdStandard.ico",
                    "ValveBlockDesign.resources.MoveCmdLarge.ico",
                    m_moveCmd,
                    "�ƶ�",
                    "AppMoveCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "�ƶ���",
                    "�ƶ���",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "Connect" command and button
                m_connectCmd = new ConnectCmd();
                this.CreateButton(
                    "ValveBlockDesign.resources.ConnectStandard.ico",
                    "ValveBlockDesign.resources.ConnectLarge.ico",
                    m_connectCmd,
                    "����",
                    "AppConnectCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "���ӿ�",
                    "���ӿ�",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
                //create the instance of the "Edit" command and button
                m_cavityLibraryEditCmd = new CavityLibraryEditCmd();
               this.CreateButton(
                    "ValveBlockDesign.resources.EditCavityCmdStandard.ico",
                    "ValveBlockDesign.resources.EditCavityCmdLarge.ico",
                     m_cavityLibraryEditCmd,
                    "�޸�Ԫ��",
                    "AppEditLibraryCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "�޸�Ԫ������",
                    "�޸�Ԫ������",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //-----------------------------------------------------------------------------------
               //create the instance of the "Add" command and button
               m_cavityLibraryAddCmd= new CavityLibraryAddCmd();
               this.CreateButton(
                    "ValveBlockDesign.resources.AddCavityCmdStandard.ico",
                    "ValveBlockDesign.resources.AddCavityCmdLarge.ico",
                     m_cavityLibraryAddCmd,
                    "���Ԫ��",
                    "AppAddLibraryCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "�����Ԫ��",
                    "�����Ԫ��",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
               //-----------------------------------------------------------------------------------
               m_cavityLibraryScanCmd = new CavityLibraryScanCmd();
               this.CreateButton(
                    "ValveBlockDesign.resources.ScanCavityCmdStandard.ico",
                    "ValveBlockDesign.resources.ScanCavityCmdLarge.ico",
                     m_cavityLibraryScanCmd,
                    "���Ԫ��",
                    "AppScanLibraryCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "�������Ԫ��",
                    "�������Ԫ��",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
               //-----------------------------------------------------------------------------------
               m_boltHolesFormCmd = new BoltHolesFormCmd();
               this.CreateButton(
                    "ValveBlockDesign.resources.BoltHolesCmdStandard.ico",
                    "ValveBlockDesign.resources.BoltHolesCmdLarge.ico",
                     m_boltHolesFormCmd,
                    "��װ���ƿ�",
                    "AppInsertBoltHolesCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "���밲װ���ƿ�",
                    "���밲װ���ƿ�",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
                //---------------------------------------------------------------------------------
               m_insertXportCmd = new InsertXportCmd();
               this.CreateButton(
                    "ValveBlockDesign.resources.BoltHolesCmdStandard.ico",
                    "ValveBlockDesign.resources.Xport.ico",
                     m_insertXportCmd,
                    "���տ�",
                    "AppInsertXportCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "���빤�տ�",
                    "���빤�տ�",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
               //---------------------------------------------------------------------------------
               m_editNetCmd = new EditNetCmd();
               this.CreateButton(
                    "ValveBlockDesign.resources.BoltHolesCmdStandard.ico",
                    "ValveBlockDesign.resources.EditColor.ico",
                     m_editNetCmd,
                    "�޸�����",
                    "AppEditNetCmd",
                    CommandTypesEnum.kShapeEditCmdType,
                    strAddInGUID,
                    "�޸��Ϳ�����",
                    "�޸��Ϳ�����",
                    ButtonDisplayEnum.kDisplayTextInLearningMode,
                    false);
               //---------------------------------------------------------------------------------
                #endregion

                #region ����ChangeManager��CommandCategory.  Set the ChangeManager and CommandCategory
                //Create change definitions
                //get the change manager
                ChangeManager oChangeManager = m_inventorApplication.ChangeManager;

                //Create the change definition collections
                ChangeDefinitions oChangeDefs = oChangeManager.Add(strAddInGUID);

                //-----------------------------------------------------------------------------------
                //Create the "Block" change definition
                ChangeDefinition oBlockChangeDef = oChangeDefs.Add("AppBlockGenerateChgDef", "��");

                //Create the command category
                CommandCategory oBlockCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("��", "AppBlockGenerateCmdCat", strAddInGUID);
                oBlockCmdCategory.Add(m_blockFormCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oAlignChangeDef = oChangeDefs.Add("AppAlignChgDef", "����");

                CommandCategory oAlignCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("����", "AppAlignCmdCat", strAddInGUID);
                oAlignCmdCategory.Add(m_alignCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oExtractiFeatureChangeDef = oChangeDefs.Add("AppExtractiFeatureChgDef", "��ȡ����");

                CommandCategory oExtractiFeatureCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("��ȡ����", "AppExtractiFeatureCmdCat", strAddInGUID);
                oExtractiFeatureCmdCategory.Add(m_extractiFeatureCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oInsertiFeatureChangeDef = oChangeDefs.Add("AppInsertiFeatureChgDef", "��������");

                CommandCategory oInsertiFeatureCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("��������", "AppInsertiFeatureCmdCat", strAddInGUID);
                oInsertiFeatureCmdCategory.Add(m_iFeatureFormCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oDeleteiFeatureChangeDef = oChangeDefs.Add("AppDeleteiFeatureChgDef", "ɾ������");

                CommandCategory oDeleteiFeatureCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("ɾ������", "AppDeleteiFeatureCmdCat", strAddInGUID);
                oDeleteiFeatureCmdCategory.Add(m_deleteCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oRotateiFeatureChangeDef = oChangeDefs.Add("AppRotateiFeatureChgDef", "��ת����");

                CommandCategory oRotateiFeatureCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("��ת����", "AppRotateiFeatureCmdCat", strAddInGUID);
                oRotateiFeatureCmdCategory.Add(m_rotateCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oMoveChangeDef = oChangeDefs.Add("AppMoveChgDef", "�ƶ���");

                CommandCategory oMoveCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("�ƶ���", "AppMoveCmdCat", strAddInGUID);
                oMoveCmdCategory.Add(m_moveCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oInsertOutlineChangeDef = oChangeDefs.Add("AppInsertOutlineChgDef", "���밲װ����");

                CommandCategory oInsertOutlineCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("���밲װ����", "AppInsertOutlineCmdCat", strAddInGUID);
                oInsertOutlineCmdCategory.Add(m_insertOutlineCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oInsertXportChangeDef = oChangeDefs.Add("AppInsertXportChgDef", "���빤�տ�");

                CommandCategory oInsertXportCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("���빤�տ�", "AppInsertXportCmdCat", strAddInGUID);
                oInsertXportCmdCategory.Add(m_insertXportCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oEditNetChangeDef = oChangeDefs.Add("AppEditNetChgDef", "�޸��Ϳ�����");

                CommandCategory oEitNetCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("�޸��Ϳ�����", "AppEditNetCmdCat", strAddInGUID);
                oEitNetCmdCategory.Add(m_editNetCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oConnectDef = oChangeDefs.Add("AppConnectChgDef", "����");

                CommandCategory oConnectCmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add("����", "AppConnectCmdCat", strAddInGUID);
                oConnectCmdCategory.Add(m_connectCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oEditLibraryDef = oChangeDefs.Add("AppEditLibraryChgDef", "�޸Ĳ���");

                CommandCategory oEditLibraryCategory = m_inventorApplication.CommandManager.CommandCategories.Add("�޸Ĳ���", "AppEditLibraryCmdCat", strAddInGUID);
                oEditLibraryCategory.Add(m_cavityLibraryEditCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oAddLibraryDef = oChangeDefs.Add("AppAddLibraryChgDef", "�������Ԫ��");

                CommandCategory oAddLibraryCategory = m_inventorApplication.CommandManager.CommandCategories.Add("�������Ԫ��", "AppAddLibraryCmdCat", strAddInGUID);
                oAddLibraryCategory.Add(m_cavityLibraryAddCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oScanLibraryDef = oChangeDefs.Add("AppScanLibraryChgDef", "�������Ԫ��");

                CommandCategory oScanLibraryCategory = m_inventorApplication.CommandManager.CommandCategories.Add("�������Ԫ��", "AppScanLibraryCmdCat", strAddInGUID);
                oScanLibraryCategory.Add(m_cavityLibraryScanCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                ChangeDefinition oInsertBoltHolesDef = oChangeDefs.Add("AppInsertBoltHolesChgDef", "���밲װ���ƿ�");

                CommandCategory oInsertBoltHolesCategory = m_inventorApplication.CommandManager.CommandCategories.Add("���밲װ���ƿ�", "AppInsertBoltHolesCmdCat", strAddInGUID);
                oInsertBoltHolesCategory.Add(m_boltHolesFormCmd.ButtonDefinition);
                //-----------------------------------------------------------------------------------
                #endregion

                #region ����Ribbon���. Load the Ribbon Panel.
                if (firstTime)
                {
                    Ribbon partDocRibbon;
                    partDocRibbon = m_inventorApplication.UserInterfaceManager.Ribbons["Part"];

                    RibbonTab myRibbonTab;
                    myRibbonTab = partDocRibbon.RibbonTabs.Add(
                        "�������",
                        "id_TabBlockDesign",
                        strAddInGUID,
                        "",
                        false);

                    RibbonPanels ribbonPanels;
                    ribbonPanels = myRibbonTab.RibbonPanels;

                    RibbonPanel cubePanel;
                    cubePanel = ribbonPanels.Add(
                        "��",
                        "id_Panel_Block",
                        strAddInGUID,
                        "",
                        false);
                    
                    CommandControls blockPanelCtrls;
                    blockPanelCtrls = cubePanel.CommandControls;

                    CommandControl cubePanelCtrl;
                    cubePanelCtrl = blockPanelCtrls.AddButton(
                        m_blockFormCmd.ButtonDefinition, 
                        true, 
                        true, 
                        "", 
                        false);
                    //------------------------------------------------------------------
                    //----------------------------------------------
                    //��iFeature��������
                    RibbonPanel iFeaturePanel;
                    iFeaturePanel = ribbonPanels.Add(
                        "����",
                        "id_Panel_iFeature",
                        strAddInGUID,
                        "",
                        false);

                    CommandControls iFeatureCtrls;
                    iFeatureCtrls = iFeaturePanel.CommandControls;

                    CommandControl InsertiFeatureCtrl;//����Ԫ��
                    InsertiFeatureCtrl = iFeatureCtrls.AddButton(
                        m_iFeatureFormCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);

                    CommandControl InsertOutlineCtrl;//���밲װ����
                    InsertOutlineCtrl = iFeatureCtrls.AddButton(
                        m_insertOutlineCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        true);
                    CommandControl InsertBoltHolesCtrl;//��װ���ƿ�
                    InsertBoltHolesCtrl = iFeatureCtrls.AddButton(
                        m_boltHolesFormCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                    //-------------------------------------------------------------------------------
                    //RibbonPanel connectPanel;
                    //connectPanel = ribbonPanels.Add(
                    //    "����",
                    //    "id_Panel_Connect",
                    //    strAddInGUID,
                    //    "",
                    //    false);

                    //CommandControls connectCtrls;
                    //connectCtrls = connectPanel.CommandControls;

                    RibbonPanel modifyPanel;
                    modifyPanel = ribbonPanels.Add(
                        "�޸�",
                        "id_Panel_Edit",
                        strAddInGUID,
                        "",
                        false);

                    CommandControls modifyCtrls;
                    modifyCtrls = modifyPanel.CommandControls;

                    CommandControl AlignCtrl;
                    AlignCtrl = modifyCtrls.AddButton(
                        m_alignCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);

                    CommandControl MoveCtrl;
                    MoveCtrl = modifyCtrls.AddButton(
                        m_moveCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                    CommandControl DeleteCtrl;//ɾ��Ԫ������
                    DeleteCtrl = modifyCtrls.AddButton(
                        m_deleteCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                    CommandControl EditPortCtrl;//�޸�����
                    EditPortCtrl = modifyCtrls.AddButton(
                        m_editNetCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        true);
                    CommandControl RotateCtrl;//��תԪ������
                    RotateCtrl = modifyCtrls.AddButton(
                        m_rotateCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                    //--------------------------------------------------------
                    RibbonPanel ConnectPanel;
                    ConnectPanel = ribbonPanels.Add(
                        "��ͨ��·",
                        "id_Panel_Connection",
                        strAddInGUID,
                        "",
                        false);

                    CommandControls ConnectCtrls;
                    ConnectCtrls = ConnectPanel.CommandControls;

                    CommandControl ConnectCtrl;
                    ConnectCtrl = ConnectCtrls.AddButton(
                        m_connectCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                    CommandControl InsertXportCtrl;//���빤�տ�
                    InsertXportCtrl = ConnectCtrls.AddButton(
                        m_insertXportCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        true);
                    
                   
                    //------------------------------------------------------------
                    //Ԫ����Ĳ������
                    RibbonPanel CavityLibraryPanel;
                    CavityLibraryPanel = ribbonPanels.Add(
                        "Ԫ����",
                        "id_Panel_CavityLibrary",
                        strAddInGUID,
                        "",
                        false);

                    CommandControls CavityLibraryCtrls;
                    CavityLibraryCtrls = CavityLibraryPanel.CommandControls;

                    CommandControl EditCavityCtrl;
                    EditCavityCtrl = CavityLibraryCtrls.AddButton(
                       m_cavityLibraryEditCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                    CommandControl AddCavityCtrl;
                    AddCavityCtrl = CavityLibraryCtrls.AddButton(
                       m_cavityLibraryAddCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                    CommandControl ScanCavityCtrl;
                    ScanCavityCtrl = CavityLibraryCtrls.AddButton(
                       m_cavityLibraryScanCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                    CommandControl ExtractiFeatureCtrl;
                    ExtractiFeatureCtrl = CavityLibraryCtrls.AddButton(
                        m_extractiFeatureCmd.ButtonDefinition,
                        true,
                        true,
                        "",
                        false);
                }
                #endregion
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }

        }

        public void Deactivate()
        {
            // This method is called by Inventor when the AddIn is unloaded.
            // The AddIn will be unloaded either manually by the user or
            // when the Inventor session is terminated

            // TODO: Add ApplicationAddInServer.Deactivate implementation
            m_userInterfaceEvents.OnResetCommandBars -= UserInterfaceEventsSink_OnResetCommandBarsEventDelegate;
            m_userInterfaceEvents.OnEnvironmentChange -= UserInterfaceEventsSink_OnEnvironmentChangeEventDelegate;
            m_userInterfaceEvents.OnResetRibbonInterface -= UserInterfaceEventsSink_OnResetRibbonInterfaceEventDelegate;

            UserInterfaceEventsSink_OnResetCommandBarsEventDelegate = null;
            UserInterfaceEventsSink_OnEnvironmentChangeEventDelegate = null;
            UserInterfaceEventsSink_OnResetRibbonInterfaceEventDelegate = null;
            m_userInterfaceEvents = null;

            //remove the command button(s)
            m_blockFormCmd.RemoveButton();
            m_alignCmd.RemoveButton();
            m_extractiFeatureCmd.RemoveButton();
            m_iFeatureFormCmd.RemoveButton();
            m_moveCmd.RemoveButton();
            m_connectCmd.RemoveButton();
            m_cavityLibraryEditCmd.RemoveButton();
            m_cavityLibraryScanCmd.RemoveButton();
            m_cavityLibraryAddCmd.RemoveButton();
            m_boltHolesFormCmd.RemoveButton();
            m_deleteCmd.RemoveButton();
            m_rotateCmd.RemoveButton();
            m_insertOutlineCmd.RemoveButton();
            m_insertXportCmd.RemoveButton();
            m_editNetCmd.RemoveButton();

            m_blockFormCmd = null;
            m_alignCmd = null;
            m_extractiFeatureCmd = null;
            m_iFeatureFormCmd = null;
            m_moveCmd = null;
            m_connectCmd = null;
            m_cavityLibraryEditCmd = null;
            m_cavityLibraryAddCmd = null;
            m_cavityLibraryScanCmd = null;
            m_boltHolesFormCmd = null;
            m_deleteCmd = null;
            m_rotateCmd = null;
            m_insertOutlineCmd = null;
            m_insertXportCmd = null;
            m_editNetCmd = null;

            //-----------------------------------------------------------------------------------
            //delete change definitions
            ChangeManager oChangeManager = m_inventorApplication.ChangeManager;

            ChangeDefinitions oChangeDefinition = oChangeManager[m_strAddInGUID];
            //-----------------------------------------------------------------------------------
            ChangeDefinition oBlockFormChangeDefinition = oChangeDefinition["AppBlockGenerateChgDef"];
            oBlockFormChangeDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oAlignChangeDefinition = oChangeDefinition["AppAlignChgDef"];
            oAlignChangeDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oExtractChangeDefinition = oChangeDefinition["AppExtractiFeatureChgDef"];
            oExtractChangeDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oInsertChangeDefinition = oChangeDefinition["AppInsertiFeatureChgDef"];
            oInsertChangeDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oDeleteChangeDefinition = oChangeDefinition["AppDeleteiFeatureChgDef"];
            oDeleteChangeDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oRotateChangeDefinition = oChangeDefinition["AppRotateiFeatureChgDef"];
            oRotateChangeDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oInsertOutlineChangeDefinition = oChangeDefinition["AppInsertOutlineChgDef"];
            oInsertOutlineChangeDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oMoveDefinition = oChangeDefinition["AppMoveChgDef"];
            oMoveDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oInsertXportDefinition = oChangeDefinition["AppInsertXportChgDef"];
            oInsertXportDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oEditNetDefinition = oChangeDefinition["AppEditNetChgDef"];
            oEditNetDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oConnectDefinition = oChangeDefinition["AppConnectChgDef"];
            oConnectDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oEditLibraryDefinition = oChangeDefinition["AppEditLibraryChgDef"];
            oEditLibraryDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oAddLibraryDefinition = oChangeDefinition["AppAddLibraryChgDef"];
            oAddLibraryDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oScanLibraryDefinition = oChangeDefinition["AppScanLibraryChgDef"];
            oScanLibraryDefinition.Delete();
            //-----------------------------------------------------------------------------------
            ChangeDefinition oInsertBoltHolesDefinition = oChangeDefinition["AppInsertBoltHolesChgDef"];
            oInsertBoltHolesDefinition.Delete();
            //-----------------------------------------------------------------------------------
            // Release objects.
            //�ͷŶ���
            Marshal.ReleaseComObject(m_inventorApplication);
            m_inventorApplication = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
        }

        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        #endregion

        private void IconPictureInitial()
        {
            //get names of all the sources in the assenbly
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            resources = assembly.GetManifestResourceNames();
        }

        private void CreateButton(string standardPictureName, string largePictureName, ValveBlockDesign.Command parentCmd, string displayName, string internalName, CommandTypesEnum commandType, object clientId, string description, string toolTip, ButtonDisplayEnum buttonType, bool autoAddToGUI)
        {
            GetIconPicture(standardPictureName, largePictureName);
            parentCmd.CreateButton(
                m_inventorApplication, 
                displayName, 
                internalName, 
                commandType, 
                clientId, 
                description, 
                toolTip, 
                oIPictureDispStandard, 
                oIPictureDispLarge, 
                buttonType, 
                autoAddToGUI);
        }

        private void GetIconPicture(string standardPictureName, string largePictureName)
        {
            oIPictureDispStandard = null;
            oIPictureDispLarge = null;
            
            //get the original picture
            System.IO.Stream oStream_s = assembly.GetManifestResourceStream(standardPictureName);
            System.IO.Stream oStream_l = assembly.GetManifestResourceStream(largePictureName);
            
            //instant icon object,get original source
            System.Drawing.Icon oIcon_s = new System.Drawing.Icon(oStream_s);
            System.Drawing.Icon oIcon_l = new System.Drawing.Icon(oStream_l);

            //call ImageToPictureDisp convertion method
            oIPictureDispStandard = AxHostConverter.ImageToPictureDisp(oIcon_s.ToBitmap());
            oIPictureDispLarge = AxHostConverter.ImageToPictureDisp(oIcon_l.ToBitmap());
        }

        internal class AxHostConverter : AxHost
        {
            private AxHostConverter()
                : base("")
            { 
            }
            //��ͼƬ��ԴתΪPictureDisp��ʽ
            public static stdole.IPictureDisp ImageToPictureDisp(Image image)
            {
                return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
            }
            //��PictureDisp��ʽת��ΪͼƬ��Դ
            public static Image PictureDispToImage(stdole.IPictureDisp pictureDisp)
            {
                return GetPictureFromIPicture(pictureDisp);
            }
        }

        private void UserInterfaceEvents_OnResetCommandBars(ObjectsEnumerator commandBars, NameValueMap context)
        {
            try
            {
                CommandBar commandBar;
                for (int commandBarCt = 1; commandBarCt <= commandBars.Count; commandBarCt++)
                {
                    commandBar = (Inventor.CommandBar)commandBars[commandBarCt];
                    if (commandBar.InternalName == "PMxPartFeatureCmdBar")
                    {
                        //add button back to the part features toolbar
                        commandBar.Controls.AddButton(m_blockFormCmd.ButtonDefinition, 0);

                        return;
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }

        private void UserInterfaceEvents_OnEnvironmentChange(Inventor.Environment environment, EnvironmentStateEnum environmentState, EventTimingEnum beforeOrAfter, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            try
            {
                String envInternalName;
                envInternalName = environment.InternalName;

                if (envInternalName == "PMxPartEnvironment")
                {
                    //enable the button when the part environment is activated or resumed
                    if (environmentState == EnvironmentStateEnum.kActivateEnvironmentState || environmentState == EnvironmentStateEnum.kResumeEnvironmentState)
                        m_blockFormCmd.ButtonDefinition.Enabled = true;


                    //disable the button when the part environment is terminated or suspended
                    if (environmentState == EnvironmentStateEnum.kTerminateEnvironmentState || environmentState == EnvironmentStateEnum.kSuspendEnvironmentState)
                        m_blockFormCmd.ButtonDefinition.Enabled = false;
                }

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        private void UserInterfaceEventsSink_OnResetRibbonInterface(NameValueMap context)
        {
            try
            {
                // Add the button to the Modify panel on Model tab in Part Ribbon
                UserInterfaceManager userInterfaceMgr = m_inventorApplication.UserInterfaceManager;

                // Get Part Ribbon
                Ribbons ribbons = userInterfaceMgr.Ribbons;
                Ribbon partRibbon = ribbons["Part"];

                // Get Modify Panel
                RibbonTab modelRibbonTab = partRibbon.RibbonTabs["id_TabBlockDesign"];
                RibbonPanel modifyRibbonPanel = modelRibbonTab.RibbonPanels["id_Panel_Block"];

                // Add the button to the Panel
                modifyRibbonPanel.CommandControls.AddButton(m_blockFormCmd.ButtonDefinition,
                                                            false,
                                                            true,
                                                            "",
                                                            false);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
    }
}
