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
     internal class CavityLibraryEditCmd:ValveBlockDesign.Command
    {
         private CavityLibraryEditForm m_cavityLibraryEditForm;
         private ConnectToAccess m_connectToAccess;
         private System.Reflection.Assembly assembly;
         private string deFaultpath;

         public CavityLibraryEditCmd()
         {
             m_cavityLibraryEditForm= null;
             assembly = System.Reflection.Assembly.GetExecutingAssembly();
             FileInfo asmFile = new FileInfo(assembly.Location);
             deFaultpath = asmFile.DirectoryName+ "\\CavityLibrary";
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

             m_cavityLibraryEditForm = new CavityLibraryEditForm(this);

             if (m_cavityLibraryEditForm != null)
             {
                 m_cavityLibraryEditForm.Activate();
                 m_cavityLibraryEditForm.TopMost = true;
                 m_cavityLibraryEditForm.ShowInTaskbar = false;
                 m_cavityLibraryEditForm.Show();
                 m_cavityLibraryEditForm.Text = "修改元件";
                 m_cavityLibraryEditForm.tb1.Text = deFaultpath;
                 AddInformation();
             }
         }

         public override void StopCommand()
         {
             //TerminatePreviewGraphics();
             m_inventorApplication.ActiveView.Update();

             //destroy the command dialog
             m_cavityLibraryEditForm.Hide();
             m_cavityLibraryEditForm.Dispose();
             m_cavityLibraryEditForm = null;

             m_buttonDefinition.Pressed = false;

             //m_commandIsRunning = false;
             base.StopCommand();
         }

         public void AddInformation()
         {
             
             string sql = @"select * from ComponentsDb";
             m_connectToAccess = new ConnectToAccess(deFaultpath, "CavityLibrary");
             m_connectToAccess.GetAddInformation(sql, m_cavityLibraryEditForm.lv1, "IndexName", "Name");
         }
         public override void ExecuteCommand()
         {
         }
    }
}
