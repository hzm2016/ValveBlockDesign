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
    internal class CavityLibraryAddCmd:ValveBlockDesign.Command
    {
        private CavityLibraryAddForm m_cavityLibraryAddForm;
        private ConnectToAccess m_connectToAccess;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;
        public CavityLibraryAddCmd()
        {
            //m_cavityLibraryAddForm = null;
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

            m_cavityLibraryAddForm = new CavityLibraryAddForm(this);

            if (m_cavityLibraryAddForm != null)
            {
                m_cavityLibraryAddForm.Activate();
                m_cavityLibraryAddForm.TopMost = true;
                m_cavityLibraryAddForm.ShowInTaskbar = false;
                m_cavityLibraryAddForm.Show();
                AddIndexName();
            }
        }

        public override void StopCommand()
        {
            //TerminatePreviewGraphics();
            m_inventorApplication.ActiveView.Update();

            //destroy the command dialog
            m_cavityLibraryAddForm.Hide();
            m_cavityLibraryAddForm.Dispose();
            m_cavityLibraryAddForm = null;
            m_buttonDefinition.Pressed = false;

            //m_commandIsRunning = false;
            base.StopCommand();
        }

        private void AddIndexName()
        {
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName + "\\CavityLibrary";
            string sql = @"select * from ComponentsDb";
            string[] getresult = new string[25];
            int i = 0;
            m_connectToAccess = new ConnectToAccess(deFaultpath,"CavityLibrary");
            m_connectToAccess.GetInformation(sql, "IndexName", out getresult);

            while (getresult[i] != null)
            {
                m_cavityLibraryAddForm.cmbIndexName.Items.Add(getresult[i]);
                i++;
            }
        }

        public override void ExecuteCommand()
        {
        }

    }
}
