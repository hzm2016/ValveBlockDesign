using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using stdole;
using Microsoft.Win32;
using System.Drawing;
using System.IO;

namespace ValveBlockDesign
{
    internal class BoltHolesRequest:ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;
        private ConnectToAccess m_connectToaccess;
        private string filepath;
        private string filename = "CavityLibrary";
        private string codename;
        private string indexname;
        private string codenumber="1101";//索引编号
        private double offset;
        private string IDName;
        private Face m_selsectFace;
        private BoltHoleForm m_boltHoleForm;
        private System.Reflection.Assembly assembly;

        public BoltHolesRequest(Inventor.Application application, Face selectface,BoltHoleForm boltHoleForm)
        {
            m_inventorApplication = application;
            m_selsectFace = selectface;
            m_boltHoleForm = boltHoleForm;
            offset = GetValueFromExpression(m_boltHoleForm.tboffset.Text);
            codename = m_boltHoleForm.comBNumber.Text;
            indexname = m_boltHoleForm.comBLibrary.Text;
            IDName = m_boltHoleForm.tbID.Text;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            filepath = asmFile.DirectoryName + "\\CavityLibrary";   
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            if (m_boltHoleForm.radioFour.Checked == true)
            {
                insertBoltHolesFour();
            }
            if (m_boltHoleForm.radioTwo.Checked == true)
            {
                insertBoltHolesTwo();
            }
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
            catch (System.Exception e)
            {
                string strErrorMsg = e.Message;

                value = 0.0;
                return value;
            }
            return value;
        }
        private int JudgeFaceUcs(Face oSelectFace)//用于判断选择的面的用户坐标系
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
            return i;
        }

        public void insertBoltHolesTwo()
        {
            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();
            string oInputName;
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;
            Face oSelectFace;
            oSelectFace = m_selsectFace;
            PartFeatures oFeatures;
            oFeatures = oPartCompDef.Features;
            iFeatureDefinition oiFeatureDef;
            oiFeatureDef = oFeatures.iFeatures.CreateiFeatureDefinition(filepath+"\\1101Two.ide");
            //double offest = GetValueFromExpression(tb1.Text);
            Edge edge4 = oSelectFace.Edges[4];
            Edge edge2 = oSelectFace.Edges[2];
            Edge edge1 = oSelectFace.Edges[1];
            Edge edge3 = oSelectFace.Edges[3];
            double disx = m_inventorApplication.MeasureTools.GetMinimumDistance(edge4, edge2, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont) - offset;
            double disy = m_inventorApplication.MeasureTools.GetMinimumDistance(edge1, edge3, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont) - offset;

            int i = JudgeFaceUcs(oSelectFace);
            foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
            {
                if (oInput.Name == "放置平面" || oInput.Name == "dx1" || oInput.Name == "dy1" || oInput.Name == "dx2" || oInput.Name == "dy2" || oInput.Name == "x轴" || oInput.Name == "y轴")
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
                            if (i == 3 || i == 4)
                            {
                                oInputXAxis.Entity = oPartCompDef.UserCoordinateSystems[i].XAxis;
                            }
                            else if (i == 5)
                            {
                                oInputXAxis.Entity = oSelectFace.Edges[2];
                            }
                            else
                            {
                                oInputXAxis.Entity = oSelectFace.Edges[1];
                            }
                            break;
                        case "y轴":
                            iFeatureEntityInput oInputYAxis;
                            oInputYAxis = (iFeatureEntityInput)oInput;
                            oInputYAxis.Entity = oPartCompDef.UserCoordinateSystems[i].YAxis;
                            if (i == 3 || i == 4)
                            {
                                oInputYAxis.Entity = oPartCompDef.UserCoordinateSystems[i].YAxis;
                            }
                            else if (i == 5)
                            {
                                oInputYAxis.Entity = oSelectFace.Edges[3];
                            }
                            else
                            {
                                oInputYAxis.Entity = oSelectFace.Edges[2];
                            }
                            break;
                        case "dx1":
                            iFeatureParameterInput oInputx;
                            oInputx = (iFeatureParameterInput)oInput;
                            oInputx.Value = offset;
                            break;
                        case "dy1":
                            iFeatureParameterInput oInputy;
                            oInputy = (iFeatureParameterInput)oInput;
                            oInputy.Value = offset;
                            break;
                        case "dx2":
                            iFeatureParameterInput oInputdisy;
                            oInputdisy = (iFeatureParameterInput)oInput;
                            if (i == 3 || i == 5)
                            {
                                oInputdisy.Value = disx;
                            }
                            else
                            {
                                oInputdisy.Value = disy;
                            }
                            break;
                        case "dy2":
                            iFeatureParameterInput oInputdisx;
                            oInputdisx = (iFeatureParameterInput)oInput;
                            if (i == 3 || i == 5)
                            {
                                oInputdisx.Value = disy;
                            }
                            else
                            {
                                oInputdisx.Value = disx;
                            }
                            break;
                    }
                }
                else
                {
                    oInputName = oInput.Name;
                    iFeatureParameterInput oParameXInput;
                    oParameXInput = (iFeatureParameterInput)oInput;
                    m_connectToaccess = new ConnectToAccess(filepath, filename, codename, indexname, codenumber);
                    oParameXInput.Expression = m_connectToaccess.SelectConnectToAccess(oInputName);
                }
            }
            iFeature oiFeature;
            oiFeature = oFeatures.iFeatures.Add(oiFeatureDef);
            WriteAttribute(oiFeature,"No");
            oiFeature.Name = IDName;
        }

        public void insertBoltHolesFour()
        {
            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();
            string oInputName;
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;
            Face oSelectFace;
            oSelectFace = m_selsectFace;
            PartFeatures oFeatures;
            oFeatures = oPartCompDef.Features;
            iFeatureDefinition oiFeatureDef;
            oiFeatureDef = oFeatures.iFeatures.CreateiFeatureDefinition(filepath+"\\1101Four.ide");
            Edge edge4 = oSelectFace.Edges[4];
            Edge edge2 = oSelectFace.Edges[2];
            Edge edge1 = oSelectFace.Edges[1];
            Edge edge3 = oSelectFace.Edges[3];
            double disx = m_inventorApplication.MeasureTools.GetMinimumDistance(edge4, edge2, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont) - 2 * offset;
            double disy = m_inventorApplication.MeasureTools.GetMinimumDistance(edge1, edge3, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont) - 2 * offset;

            int i = JudgeFaceUcs(oSelectFace);
            foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
            {
                if (oInput.Name == "放置平面" || oInput.Name == "dx1" || oInput.Name == "dy1" || oInput.Name == "dx2" || oInput.Name == "dy2" || oInput.Name == "x轴" || oInput.Name == "y轴")
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
                            if (i == 3 || i == 4)
                            {
                                oInputXAxis.Entity = oPartCompDef.UserCoordinateSystems[i].XAxis;
                            }
                            else if (i == 5)
                            {
                                oInputXAxis.Entity = oSelectFace.Edges[2];
                            }
                            else
                            {
                                oInputXAxis.Entity = oSelectFace.Edges[1];
                            }
                            break;
                        case "y轴":
                            iFeatureEntityInput oInputYAxis;
                            oInputYAxis = (iFeatureEntityInput)oInput;
                            oInputYAxis.Entity = oPartCompDef.UserCoordinateSystems[i].YAxis;
                            if (i == 3 || i == 4)
                            {
                                oInputYAxis.Entity = oPartCompDef.UserCoordinateSystems[i].YAxis;
                            }
                            else if (i == 5)
                            {
                                oInputYAxis.Entity = oSelectFace.Edges[3];
                            }
                            else
                            {
                                oInputYAxis.Entity = oSelectFace.Edges[2];
                            }
                            break;
                        case "dx1":
                            iFeatureParameterInput oInputx;
                            oInputx = (iFeatureParameterInput)oInput;
                            oInputx.Value = offset;
                            break;
                        case "dy1":
                            iFeatureParameterInput oInputy;
                            oInputy = (iFeatureParameterInput)oInput;
                            oInputy.Value = offset;
                            break;
                        case "dx2":
                            iFeatureParameterInput oInputdisy;
                            oInputdisy = (iFeatureParameterInput)oInput;
                            if (i == 3 || i == 5)
                            {
                                oInputdisy.Value = disy;
                            }
                            else
                            {
                                oInputdisy.Value = disx;
                            }
                            break;
                        case "dy2":
                            iFeatureParameterInput oInputdisx;
                            oInputdisx = (iFeatureParameterInput)oInput;
                            if (i == 3 || i == 5)
                            {
                                oInputdisx.Value = disx;
                            }
                            else
                            {
                                oInputdisx.Value = disy;
                            }
                            break;
                    }
                }
                else
                {
                    oInputName = oInput.Name;
                    iFeatureParameterInput oParameXInput;
                    oParameXInput = (iFeatureParameterInput)oInput;
                    m_connectToaccess = new ConnectToAccess(filepath, filename, codename, indexname, codenumber);
                    oParameXInput.Expression = m_connectToaccess.SelectConnectToAccess(oInputName);
                }
            }
            iFeature oiFeature;
            oiFeature = oFeatures.iFeatures.Add(oiFeatureDef);
            WriteAttribute(oiFeature, "No");
            oiFeature.Name = IDName;
        }

        public void WriteAttribute(iFeature oiFeature, string YesOrNo)
        {
            AttributeSets oAttributeSets;
            oAttributeSets = oiFeature.AttributeSets;

            AttributeSet oAttributeSet;
            oAttributeSet = oAttributeSets.Add("MyAttribSet", false);

            Inventor.Attribute oPlaneAttrib;
            oPlaneAttrib = oAttributeSet.Add("InternalName", ValueTypeEnum.kStringType, oiFeature.Name);

            Inventor.Attribute oFootprintAttrib;
            oFootprintAttrib = oAttributeSet.Add("Footprint", ValueTypeEnum.kStringType, YesOrNo);
 
            Inventor.Attribute IndexNameAttrib;
            IndexNameAttrib = oAttributeSet.Add("IndexName", ValueTypeEnum.kStringType, indexname);

            Inventor.Attribute CodeNameAttrib;
            CodeNameAttrib = oAttributeSet.Add("CodeName", ValueTypeEnum.kStringType, codename);

            Inventor.Attribute CodeNumberAttrib;
            CodeNumberAttrib = oAttributeSet.Add("CodeNumber", ValueTypeEnum.kStringType, codenumber);

        }
    }
}
