using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class InsertiFeatureCmd : ValveBlockDesign.Command
    {
        public InsertiFeatureCmd()
        { 
        }
        private int JudgeFaceUcs(Face oSelectFace)
        {
            int i = 0;
            Double[] oParam = new Double[2];
            oParam[0] = 0;
            oParam[1] = 1;
            Double[] oNormal = new Double[3];
            oSelectFace.Evaluator.GetNormal(oParam, ref oNormal);
            switch ((oNormal[0].ToString() + "," + oNormal[1].ToString() + "," + oNormal[2].ToString()))
            {
                case "0,0,1":
                    i = 1;
                    break;
                case "0,-1,0":
                    i = 2;
                    break;
                case "1,0,0":
                    i = 3;
                    break;
                case "0,1,0":
                    i = 4;
                    break;
                case "-1,0,0":
                    i = 5;
                    break;
                case "0,0,-1":
                    i = 6;
                    break;
            }
            //MessageBox.Show(i.ToString());
            return i;
        }
        protected override void ButtonDefinition_OnExecute(NameValueMap context)
        {
            /*//Get the CommandManager object
            CommandManager oCommandManager;
            oCommandManager = m_inventorApplication.CommandManager;

            //Get control definition for the homeview command
            ControlDefinition oControlDef;
            oControlDef = oCommandManager.ControlDefinitions["PartiFeatureInsertCmd"];

            //Excute the command
            oControlDef.Execute();*/
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;
            Face oSelectFace;
            oSelectFace = oPartDocument.SelectSet[1];
            PartFeatures oFeatures;
            oFeatures = oPartCompDef.Features;
            iFeatureDefinition oiFeatureDef;
            oiFeatureDef = oFeatures.iFeatures.CreateiFeatureDefinition("F:\\CavityLibrary\\iFeature.ide");
            int i = JudgeFaceUcs(oSelectFace);
            foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
            {
                switch (oInput.Name)
                {
                    case "放置平面":
                        iFeatureSketchPlaneInput oPlaneInput;
                        oPlaneInput = (iFeatureSketchPlaneInput)oInput;
                        oPlaneInput.PlaneInput = oSelectFace;
                        break;
                    case "x轴":
                        iFeatureEntityInput oInputXAxis;
                        oInputXAxis = (iFeatureEntityInput)oInput;
                        oInputXAxis.Entity = oPartCompDef.UserCoordinateSystems[i].XAxis;
                        break;
                    case "y轴":
                        iFeatureEntityInput oInputYAxis;
                        oInputYAxis = (iFeatureEntityInput)oInput;
                        oInputYAxis.Entity = oPartCompDef.UserCoordinateSystems[i].YAxis;
                        break;
                    case "到x轴距离":
                        iFeatureParameterInput oParameXInput;
                        oParameXInput = (iFeatureParameterInput)oInput;
                        oParameXInput.Expression = "30 mm";
                        break;
                    case "到y轴距离":
                        iFeatureParameterInput oParameYInput;
                        oParameYInput = (iFeatureParameterInput)oInput;
                        oParameYInput.Expression = "40 mm";
                        break;
                }
            }
            iFeature oiFeature;
            oiFeature = oFeatures.iFeatures.Add(oiFeatureDef);
        }

        public override void ExecuteCommand()
        {
        }
    }
}
