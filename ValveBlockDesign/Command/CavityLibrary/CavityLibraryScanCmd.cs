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
    internal class CavityLibraryScanCmd:ValveBlockDesign.Command
    {
        private CavityLibraryScanForm m_cavityLibraryScanForm;
        private ConnectToAccess m_connectToAccess;
        //private CavityLibraryEditCmd m_cavityLibraryEditCmd;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;
        
        private ConnectToAccess m_connectToaccess;
        

        public CavityLibraryScanCmd()
        {
            m_cavityLibraryScanForm = null;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName + "\\CavityLibrary";
        }
        protected override void ButtonDefinition_OnExecute(NameValueMap context)
        {
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

            m_cavityLibraryScanForm = new CavityLibraryScanForm(this);

            if (m_cavityLibraryScanForm != null)
            {
                m_cavityLibraryScanForm.Activate();
                m_cavityLibraryScanForm.TopMost = true;
                m_cavityLibraryScanForm.ShowInTaskbar = false;
                m_cavityLibraryScanForm.Show();
                m_cavityLibraryScanForm.tb1.Text=deFaultpath;
                AddInformation();
            }
        }

        public override void StopCommand()
        {
            //TerminatePreviewGraphics();
            m_inventorApplication.ActiveView.Update();

            //destroy the command dialog
            m_cavityLibraryScanForm.Hide();
            m_cavityLibraryScanForm.Dispose();
            m_cavityLibraryScanForm = null;
            m_buttonDefinition.Pressed = false;
            
            //m_commandIsRunning = false;
            base.StopCommand();
        }

        public void AddInformation()
        {
            string sql = @"select * from ComponentsDb";
            m_connectToAccess = new ConnectToAccess(deFaultpath, "CavityLibrary");
            m_connectToAccess.GetAddInformation(sql, m_cavityLibraryScanForm.lv1, "IndexName", "Name");
        }

        public override void ExecuteCommand()
        {
            
        }
    }
}
