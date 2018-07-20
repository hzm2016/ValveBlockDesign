using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class AlignRequest: ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;
        private Face m_thisFace;
        private Face m_withThisFace;
        private int m_direction;

        private iFeature m_thisiFeature;
        private iFeature m_withThisiFeature;

        private UserCoordinateSystem m_UCS;

        private WorkPlane m_refPlane;
        private string m_paraName;

        private int m_typeNum;
        private int m_coeff;

        public AlignRequest(Inventor.Application application, Face thisFace, Face withThisFace, int direction, UserCoordinateSystem ucs,iFeature thisiFeature,iFeature withthisiFeature)
        {
            m_inventorApplication = application;
            m_thisFace = thisFace;
            m_withThisFace = withThisFace;
            m_direction = direction;
            m_UCS = ucs;
            m_thisiFeature = thisiFeature;
            m_withThisiFeature = withthisiFeature;
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)document;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            if (m_direction == 0)
            {
                MessageBox.Show("软件运行出现错误!");
                return;
            }

            switch (m_direction)
            {
                case 1 :
                    m_typeNum = 1;
                    break;
                case 2:
                    m_typeNum = 2;
                    break;
                case 3:;
                    this.GetAlignType(oPartCompDef);
                    break;
            }

            if (m_typeNum == 0)
            {
                MessageBox.Show("软件运行出现错误!");
                return;
            }

            if (m_typeNum == 3)
            {
                this.ExcuteAlign(oPartDoc, m_UCS.YZPlane, "x", 1);
                this.ExcuteAlign(oPartDoc, m_UCS.XZPlane, "y", -1);
            }
            else
            {
                if (m_typeNum == 11)
                {
                    this.ExcuteAlign(oPartDoc, m_UCS.YZPlane, "y", 1);
                    this.ExcuteAlign(oPartDoc, m_UCS.XZPlane, "x", 1);
                }
                else
                {
                    switch (m_typeNum)
                    {
                        case 1:
                            m_refPlane = m_UCS.XZPlane;
                            m_paraName = "y";
                            m_coeff = -1;
                            break;
                        case 2:
                            m_refPlane = m_UCS.YZPlane;
                            m_paraName = "x";
                            m_coeff = -1;
                            break;
                        case 4:
                            m_refPlane = m_UCS.YZPlane;
                            m_paraName = "x";
                            m_coeff = -1;
                            break;
                        case 5:
                            m_refPlane = m_UCS.YZPlane;
                            m_paraName = "x";
                            m_coeff = 1;
                            break;
                        case 6:
                            m_refPlane = m_UCS.YZPlane;
                            m_paraName = "y";
                            m_coeff = -1;
                            break;
                        case 7:
                            m_refPlane = m_UCS.XZPlane;
                            m_paraName = "x";
                            m_coeff = 1;
                            break;
                        case 8:
                            m_refPlane = m_UCS.XZPlane;
                            m_paraName = "y";
                            m_coeff = 1;
                            break;
                        case 9:
                            m_refPlane = m_UCS.XZPlane;
                            m_paraName = "x";
                            m_coeff = -1;
                            break;
                        case 10:
                            m_refPlane = m_UCS.XZPlane;
                            m_paraName = "y";
                            m_coeff = -1;
                            break;
                        case 12:
                            m_refPlane = m_UCS.YZPlane;
                            m_paraName = "y";
                            m_coeff = 1;
                            break;
                    }

                    this.ExcuteAlign(oPartDoc, m_refPlane, m_paraName, m_coeff);
                }
            }
        }

        private void ExcuteAlign(PartDocument partDoc, WorkPlane refPlane, string paraName, int coeff)
        {
            PartComponentDefinition oPartCompDef;
            oPartCompDef = partDoc.ComponentDefinition;

            WorkAxis thisFaceAxis;
            thisFaceAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_thisFace, false);
            Double thisDist = GetDistanceBetwLineAndFace(oPartCompDef, thisFaceAxis, refPlane);
            thisFaceAxis.Delete();

            WorkAxis withThisFaceAxis;
            withThisFaceAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_withThisFace, false);
            Double withThisDist = GetDistanceBetwLineAndFace(oPartCompDef, withThisFaceAxis, refPlane);
            withThisFaceAxis.Delete();

            Double differ;
            differ = thisDist - withThisDist;

            AttributeSets atr = m_thisiFeature.AttributeSets;

            AttributeSet abs = atr["MyAttribSet"];
            Inventor.Attribute att = abs["InternalName"];
            Inventor.Attribute footprint = abs["Footprint"];
            string footprintCheck = footprint.Value;

            string thisiFeatureName = att.Value;
            string oParameterName = thisiFeatureName + ":" + paraName;

            foreach (iFeatureInput oInput in m_thisiFeature.iFeatureDefinition.iFeatureInputs)
            {
                if (oInput.Name == thisiFeatureName + ":" + paraName || oInput.Name == thisiFeatureName + ":" + paraName+":2")
                {
                    iFeatureParameterInput oParamInput;
                    oParamInput = (iFeatureParameterInput)oInput;
                    Double newValue = oParamInput.Parameter.Value + coeff*differ;
                    oParamInput.Parameter.Value = newValue;
                }
            }
            if (footprintCheck == "Yes")
            {
                if (paraName == "y")
                {
                    MoveSketch(0,coeff * differ);
                }
                else
                {
                    MoveSketch(coeff * differ,0);
                }
            }
            partDoc.Update2();
        }

        public void MoveSketch(double m_xOffset, double m_yOffset)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveEditObject;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            PlanarSketch oSketch = oPartCompDef.Sketches["Footprint" + m_thisiFeature.Name];

            Vector2d ovector = m_inventorApplication.TransientGeometry.CreateVector2d(m_yOffset, m_xOffset);

            ObjectCollection oSketchObjects;
            oSketchObjects = m_inventorApplication.TransientObjects.CreateObjectCollection();


            foreach (SketchEntity oSketchEntity in oSketch.SketchEntities)
            {
                oSketchObjects.Add(oSketchEntity);
            }
            oSketch.MoveSketchObjects(oSketchObjects, ovector);
        }

        private double GetDistanceBetwLineAndFace(PartComponentDefinition partCompDef, WorkAxis workAxis,WorkPlane workPlane)
        {
            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(workAxis, workPlane, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);

            return oDistance;
        }

        private void GetInsertPlane(PartComponentDefinition partCompDef, Face face,out int planeNumb)
        {
            WorkAxis workAxis;
            workAxis = partCompDef.WorkAxes.AddByRevolvedFace(face, true);
            Line oAxisLine;
            oAxisLine = workAxis.Line;
            UnitVector oAxisVector;
            oAxisVector = oAxisLine.Direction;

            Double[] coords = new Double[3];
            oAxisVector.GetUnitVectorData(ref coords);

            string strCoords;
            strCoords = coords[0].ToString() + coords[1].ToString() + coords[2].ToString();

            planeNumb = 0;
            switch (strCoords)
            { 
                case "00-1":
                    planeNumb = 1;
                    break;
                case "010":
                    planeNumb = 2;
                    break;
                case "-100":
                    planeNumb = 3;
                    break;
                case "0-10":
                    planeNumb = 4;
                    break;
                case "100":
                    planeNumb = 5;
                    break;
                case "001":
                    planeNumb = 6;
                    break;
            }
        }

        private void GetAlignType(PartComponentDefinition partCompDef)
        {
            int oThisNum;
            this.GetInsertPlane(partCompDef, m_thisFace, out oThisNum);

            int oWithNum;
            this.GetInsertPlane(partCompDef, m_withThisFace, out oWithNum);

            if (oThisNum == 0 || oWithNum ==0)
            {
                MessageBox.Show("软件运行出现错误!");
                return;
            }

            string oType = oThisNum.ToString() + oWithNum.ToString();

            switch (oType)
            {
                case "16":
                    m_typeNum = 3;
                    break;
                case "61":
                    m_typeNum = 3;
                    break;
                case "24":
                    m_typeNum = 3;
                    break;
                case "42":
                    m_typeNum = 3;
                    break;
                case "35":
                    m_typeNum = 11;
                    break;
                case "53":
                    m_typeNum = 11;
                    break;
                case "21":
                    m_typeNum = 4;
                    break;
                case "12":
                    m_typeNum = 4;
                    break;
                case "46":
                    m_typeNum = 4;
                    break;
                case "64":
                    m_typeNum = 4;
                    break;
                case "14":
                    m_typeNum = 5;
                    break;
                case "41":
                    m_typeNum = 5;
                    break;
                case "26":
                    m_typeNum = 5;
                    break;
                case "62":
                    m_typeNum = 5;
                    break;
                case "15":
                    m_typeNum = 6;
                    break;
                case "65":
                    m_typeNum = 6;
                    break;
                case "32":
                    m_typeNum = 7;
                    break;
                case "34":
                    m_typeNum = 7;
                    break;
                case "13":
                    m_typeNum = 8;
                    break;
                case "63":
                    m_typeNum = 8;
                    break;
                case "31":
                    m_typeNum = 8;
                    break;
                case "36":
                    m_typeNum = 8;
                    break;
                case "51":
                    m_typeNum = 9;
                    break;
                case "56":
                    m_typeNum = 9;
                    break;
                case "45":
                    m_typeNum = 10;
                    break;
                case "54":
                    m_typeNum = 10;
                    break;
                case "52":
                    m_typeNum = 10;
                    break;
                case "25":
                    m_typeNum = 10;
                    break;
                case "23":
                    m_typeNum = 12;
                    break;
                case "43":
                    m_typeNum = 12;
                    break;
            }
        }
    }
}
