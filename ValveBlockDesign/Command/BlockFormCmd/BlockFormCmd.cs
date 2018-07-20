using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.IO;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class BlockFormCmd: ValveBlockDesign.Command
    {
        //BlockForm dialog
        private BlockForm m_blockForm;

        private double m_length;
        private double m_width;
        private double m_height;

        //preview objects
        private GraphicsNode m_previewClientGraphicsNode;
        private TriangleStripGraphics m_triangleStripGraphics;
        private GraphicsCoordinateSet m_graphicsCoordinateSet;
        private GraphicsColorSet m_graphicsColorSet;
        private GraphicsIndexSet m_graphicsColorIndexSet;

        //--------------------------------------------------------

        public BlockFormCmd()
        {
            m_blockForm = null;
            
            m_previewClientGraphicsNode = null;
            m_triangleStripGraphics = null;
            m_graphicsCoordinateSet = null;
            m_graphicsColorSet = null;
            m_graphicsColorIndexSet = null;
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

        public double GetValueFromExpression(string expression)
        {
            double value = 0.0;

            //get the active document
            Document activeDocument = m_inventorApplication.ActiveDocument;

            //get the unit of measure object
            UnitsOfMeasure unitsOfMeasure = activeDocument.UnitsOfMeasure;

            //get the current length units of the user
            UnitsTypeEnum lengthUnitsType = unitsOfMeasure.LengthUnits;

            //convert the expression to the current length units of user
            try
            {
                object vVal;
                vVal = unitsOfMeasure.GetValueFromExpression(expression, lengthUnitsType);
                value = System.Convert.ToDouble(vVal);
            }
            catch(System.Exception e)
            {
                string strErrorMsg = e.Message;

                value = 0.0;
                return value;
            }
            return value;
        }

        public void InitializePreviewGraphics()
        {
            m_interactionEvents = m_inventorApplication.CommandManager.CreateInteractionEvents();

            InteractionGraphics interactionGraphics = m_interactionEvents.InteractionGraphics;

            ClientGraphics previewClientGraphics = interactionGraphics.PreviewClientGraphics;

            m_previewClientGraphicsNode = previewClientGraphics.AddNode(1);

            m_triangleStripGraphics = m_previewClientGraphicsNode.AddTriangleStripGraphics();

            GraphicsDataSets graphicsDataSets = interactionGraphics.GraphicsDataSets;

            m_graphicsCoordinateSet = graphicsDataSets.CreateCoordinateSet(1);

            m_graphicsColorSet = graphicsDataSets.CreateColorSet(1);

            m_graphicsColorSet.Add(1, 100, 100, 200);
            m_graphicsColorSet.Add(2, 150, 150, 250);

            m_graphicsColorIndexSet = graphicsDataSets.CreateIndexSet(1);

            m_triangleStripGraphics.CoordinateSet = m_graphicsCoordinateSet;
            m_triangleStripGraphics.ColorSet = m_graphicsColorSet;
            m_triangleStripGraphics.ColorIndexSet = m_graphicsColorIndexSet;

            m_triangleStripGraphics.ColorBinding = ColorBindingEnum.kPerItemColors;
            m_triangleStripGraphics.BurnThrough = true;
 
        }

        public void UpdatePreviewGraphics()
        {
            //remove the existing co-ordinates
            m_graphicsCoordinateSet = null;

            InteractionGraphics interactionGraphics = m_interactionEvents.InteractionGraphics;
            GraphicsDataSets graphicsDataSets = interactionGraphics.GraphicsDataSets;

            m_graphicsCoordinateSet = graphicsDataSets.CreateCoordinateSet(1);

            //remove the existing color indices
            m_graphicsColorIndexSet = null;

            m_graphicsColorIndexSet = graphicsDataSets.CreateIndexSet(1);
            m_triangleStripGraphics.CoordinateSet = m_graphicsCoordinateSet;
            m_triangleStripGraphics.ColorIndexSet = m_graphicsColorIndexSet;

            TransientGeometry transientGeometry = m_inventorApplication.TransientGeometry;

            Point point1 = transientGeometry.CreatePoint(0, 0, 0);
            Point point2 = transientGeometry.CreatePoint(m_length, 0, 0);
            Point point3 = transientGeometry.CreatePoint(m_length, m_width, 0);
            Point point4 = transientGeometry.CreatePoint(0, m_width, 0);
            Point point5 = transientGeometry.CreatePoint(0, 0, m_height);
            Point point6 = transientGeometry.CreatePoint(m_length, 0, m_height);
            Point point7 = transientGeometry.CreatePoint(m_length, m_width, m_height);
            Point point8 = transientGeometry.CreatePoint(0, m_width, m_height);

            AddGraphicsPoints(point1);
            AddColorIndex(1);
            AddGraphicsPoints(point6);
            AddColorIndex(1);
            AddGraphicsPoints(point2);
            AddColorIndex(1);
            AddGraphicsPoints(point3);
            AddColorIndex(1);
            AddGraphicsPoints(point1);
            AddColorIndex(2);
            AddGraphicsPoints(point4);
            AddColorIndex(2);
            AddGraphicsPoints(point8);
            AddColorIndex(1);
            AddGraphicsPoints(point3);
            AddColorIndex(1);
            AddGraphicsPoints(point7);
            AddColorIndex(2);
            AddGraphicsPoints(point6);
            AddColorIndex(2);
            AddGraphicsPoints(point8);
            AddColorIndex(1);
            AddGraphicsPoints(point5);
            AddColorIndex(1);
            AddGraphicsPoints(point1);
            AddColorIndex(2);
            AddGraphicsPoints(point6);
            AddColorIndex(2);

            m_inventorApplication.ActiveView.Update();

            //Get the CommandManager object
            CommandManager oCommandManager;
            oCommandManager = m_inventorApplication.CommandManager;

            //Get control definition for the homeview command
            ControlDefinition oControlDef;
            oControlDef = oCommandManager.ControlDefinitions["AppIsometricViewCmd"];

            //Excute the command
            oControlDef.Execute();
        }

        void AddGraphicsPoints(Point cornerPt)
        {
            int nomGraphicsCoordPts = m_graphicsCoordinateSet.Count;

            m_graphicsCoordinateSet.Add(nomGraphicsCoordPts + 1, cornerPt);
        }

        void AddColorIndex(int colorIndex)
        {
            int nomGraphicsColorIndices = m_graphicsColorIndexSet.Count;

            m_graphicsColorIndexSet.Add(nomGraphicsColorIndices + 1, colorIndex);
        }

        public void TerminatePreviewGraphics()
        {
            m_graphicsCoordinateSet.Delete();

            m_graphicsColorSet.Delete();

            m_graphicsColorIndexSet.Delete();

            m_triangleStripGraphics.Delete();

            m_previewClientGraphicsNode.Delete();

            m_graphicsCoordinateSet = null;
            m_graphicsColorSet = null;
            m_graphicsColorIndexSet = null;
            m_triangleStripGraphics = null;
            m_previewClientGraphicsNode = null;
        }

        public void UpdateCommandStatus()
        {
            //get the parameters from the BlockForm dialog
            //by default,disable the OK button on dialog
            m_blockForm.blockGenerateButton.Enabled = false;

            //all paremeters must be valid
            if (!m_blockForm.AllParametersAreValid())
            {
                return;
            }

            //transfer parameters
            m_length = GetValueFromExpression(m_blockForm.m_blockLength);

            m_width = GetValueFromExpression(m_blockForm.m_blockWidth);

            m_height = GetValueFromExpression(m_blockForm.m_blockHeight);

            if (m_length > 0 && m_width > 0 && m_height > 0)
            {
                //update the preview
                UpdatePreviewGraphics();

                //enable the OK button on dialog
                m_blockForm.blockGenerateButton.Enabled = true;
            }
        }

        public override void EnableInteraction()
        {
            //call base command button's Enable Interaction 
            base.EnableInteraction();
        }

        public override void DisableInteraction()
        {
            //call base command button's Disable Interaction 
            base.DisableInteraction();
        }

        public override void StartCommand()
        {
            //set the IsometricView
            //Get the CommandManager object
            CommandManager oCommandManager;
            oCommandManager = m_inventorApplication.CommandManager;

            //Get control definition for the homeview command
            ControlDefinition oControlDef;
            oControlDef = oCommandManager.ControlDefinitions["AppIsometricViewCmd"];

            //Excute the command
            oControlDef.Execute();

            //call base command button's StartCommand (to also start interaction)
            base.StartCommand();

            ////implement this command specific functionality
            //subscribe to desired interaction event(s)
            //base.SubscribeToEvent(Interaction.InteractionTypeEnum.kSelection);

            this.InitializePreviewGraphics();

            //Create and display the Dialog
            m_blockForm = new BlockForm(m_inventorApplication, this);

            if (m_blockForm != null)
            {
                m_blockForm.Activate();
                m_blockForm.TopMost = true;
                m_blockForm.ShowInTaskbar = false;
                m_blockForm.Show();
            }

            //enable interaction
            EnableInteraction();
        }

        public override void StopCommand()
        {
            //implement this command specific functionality
            TerminatePreviewGraphics();
            m_inventorApplication.ActiveView.Update();

            //destroy the command dialog
            m_blockForm.Hide();
            m_blockForm.Dispose();
            m_blockForm = null;

            ////call base command button's StopCommand (to disconnect interaction sinks)
            //base.StopCommand();

            //stop interaction events
            StopInteraction();

            //un-press the button
            m_buttonDefinition.Pressed = false;

            //set the command status to not-running
            m_commandIsRunning = false;
        }

        public override void ExecuteCommand()
        {
            //stop the command(to disconnect interaction events, interaction graphics and dismiss command dialog)
            StopCommand();

            //Create the rack face request
            BlockFormRequest blockFormRequest = new BlockFormRequest(m_inventorApplication, m_length, m_width, m_height);

            //execute the request
            base.ExecuteChangeRequest(blockFormRequest, "AppBlockGenerateChgDef", m_inventorApplication.ActiveDocument);


        }

        public void TransInventorApp(Inventor.Application inventorApplication)
        {
            m_inventorApplication = inventorApplication;

        }

    }
}
