using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class ExtractiFeatureCmd : ValveBlockDesign.Command
    {
        public ExtractiFeatureCmd()
        { 
        }

        protected override void ButtonDefinition_OnExecute(NameValueMap context)
        {
            //Get the CommandManager object
            CommandManager oCommandManager;
            oCommandManager = m_inventorApplication.CommandManager;

            //Get control definition for the homeview command
            ControlDefinition oControlDef;
            oControlDef = oCommandManager.ControlDefinitions["PartiFeatureExtractCmd"];

            //Excute the command
            oControlDef.Execute();
        }

        public override void ExecuteCommand()
        {
        }
    }
}
