using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;
using System.IO;

namespace ValveBlockDesign
{
    internal class InsertXportRequest:ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;
        private int m_insertPlane;
        private Inventor.Face m_insertFace;

        private double xdistance;
        private double ydistance;
        private string filepath;
        private string codenumber;
        private string codename;
        private string indexname;
        private string XPortID;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;

        private ConnectToAccess m_connectToaccess;

        public InsertXportRequest(Inventor.Application application,int insertPlane,double xdis,double ydis,string codenumber,string codename,string indexname,string xportID)
        {
            m_inventorApplication = application;
            m_insertPlane = insertPlane;
            xdistance = xdis;
            ydistance = ydis;
            this.codename = codename;
            this.codenumber = codenumber;
            this.indexname = indexname;
            this.XPortID = xportID;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName;
            filepath = deFaultpath + "\\CavityLibrary";
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;
            insertXport(oPartDoc, oPartCompDef);
        }

        private void getInsertFace(PartComponentDefinition oPartCompDef)
        {
            switch (m_insertPlane)
            {
                case 1:
                    m_insertFace = oPartCompDef.Features.ExtrudeFeatures["拉伸1"].Faces[5];
                    break;
                case 2:
                    m_insertFace = oPartCompDef.Features.ExtrudeFeatures["拉伸1"].Faces[4];
                    break;
                case 3:
                    m_insertFace = oPartCompDef.Features.ExtrudeFeatures["拉伸1"].Faces[3];
                    break;
                case 4:
                    m_insertFace = oPartCompDef.Features.ExtrudeFeatures["拉伸1"].Faces[2];
                    break;
                case 5:
                    m_insertFace = oPartCompDef.Features.ExtrudeFeatures["拉伸1"].Faces[1];
                    break;
                case 6:
                    m_insertFace = oPartCompDef.Features.ExtrudeFeatures["拉伸1"].Faces[6];
                    break;
            }
        }

        private void insertXport(PartDocument oPartDoc,PartComponentDefinition oPartCompDef)
        {
            string oInputName;
            PartFeatures oFeatures;
            oFeatures = oPartCompDef.Features;
            getInsertFace(oPartCompDef);
            iFeatureDefinition oiFeatureDef;
            oiFeatureDef = oFeatures.iFeatures.CreateiFeatureDefinition(filepath+"\\"+codenumber+"Cavity.ide");

            foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
            {
                if (oInput.Name == "放置平面" || oInput.Name == "x轴" || oInput.Name == "y轴" || oInput.Name == "y" || oInput.Name == "x")
                {
                    switch (oInput.Name)
                    {
                        case "放置平面":
                            iFeatureSketchPlaneInput oPlaneInput;
                            oPlaneInput = (iFeatureSketchPlaneInput)oInput;
                            oPlaneInput.PlaneInput = m_insertFace;
                            break;
                        case "x轴":
                            iFeatureEntityInput oInputXAxis;
                            oInputXAxis = (iFeatureEntityInput)oInput;
                            oInputXAxis.Entity = oPartCompDef.UserCoordinateSystems[m_insertPlane].XAxis;
                            break;
                        case "y轴":
                            iFeatureEntityInput oInputYAxis;
                            oInputYAxis = (iFeatureEntityInput)oInput;
                            oInputYAxis.Entity = oPartCompDef.UserCoordinateSystems[m_insertPlane].YAxis;
                            break;
                        case "y":
                            iFeatureParameterInput oInputx;
                            oInputx = (iFeatureParameterInput)oInput;
                            oInputx.Value = xdistance;
                            break;
                        case "x":
                            iFeatureParameterInput oInputy;
                            oInputy = (iFeatureParameterInput)oInput;
                            oInputy.Value = ydistance;
                            break;
                    }
                }
                else
                {
                    oInputName = oInput.Name;
                    iFeatureParameterInput oParameXInput;
                    oParameXInput = (iFeatureParameterInput)oInput;
                    m_connectToaccess = new ConnectToAccess(filepath, "CavityLibrary", codename, indexname, codenumber);
                    oParameXInput.Expression = m_connectToaccess.SelectConnectToAccess(oInputName);
                }
            }
            iFeature oiFeature;
            oiFeature = oFeatures.iFeatures.Add(oiFeatureDef);

            AttributeSets oAttributeSets;
            oAttributeSets = oiFeature.AttributeSets;

            AttributeSet oAttributeSet;
            oAttributeSet = oAttributeSets.Add("MyAttribSet", false);

            Inventor.Attribute oPlaneAttrib;
            oPlaneAttrib = oAttributeSet.Add("InternalName", ValueTypeEnum.kStringType, oiFeature.Name);
            Inventor.Attribute oFootprintAttrib;
            oFootprintAttrib = oAttributeSet.Add("Footprint", ValueTypeEnum.kStringType, "No");

            Inventor.Attribute IndexNameAttrib;
            IndexNameAttrib = oAttributeSet.Add("IndexName", ValueTypeEnum.kStringType, indexname);

            Inventor.Attribute CodeNameAttrib;
            CodeNameAttrib = oAttributeSet.Add("CodeName", ValueTypeEnum.kStringType, codename);

            Inventor.Attribute CodeNumberAttrib;
            CodeNumberAttrib = oAttributeSet.Add("CodeNumber", ValueTypeEnum.kStringType, codenumber);


            Double PortDepth1 = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("PortDepth1"));
            Double Depth0 = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("Depth0"));

            oAttributeSet.Add("MyPortsNumber", ValueTypeEnum.kIntegerType, 1);
            oAttributeSet.Add("Port1", ValueTypeEnum.kStringType, "NET1");
            oiFeature.Name = XPortID;
            Drowport(oiFeature, m_insertFace, PortDepth1, Depth0, "NET1");
            ShowPortColor();
        }

        private void ShowPortColor()
        {
            PartDocument oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;
            SelectSet oSelectSet = oPartDoc.SelectSet;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkSurfaces workSurfaces;
            workSurfaces = oPartCompDef.WorkSurfaces;

            foreach (WorkSurface workSurf in workSurfaces)
            {
                workSurf.Translucent = false;
            }
        }

        private void Drowport(iFeature ifeature, Face face, double portDepth1, double depth0, string netName)
        {
           PartDocument oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;
            SelectSet oSelectSet = oPartDoc.SelectSet;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            double portStartDepth = portDepth1;
            WorkPlane portStartPlane;
            portStartPlane = oPartCompDef.WorkPlanes.AddByPlaneAndOffset(face, -portStartDepth, true);

            Faces insertiFeatureFaces;
            insertiFeatureFaces = ifeature.Faces;

            foreach (Face oFace in insertiFeatureFaces)
            {
                if (oFace.SurfaceType == SurfaceTypeEnum.kCylinderSurface)
                {
                    Edges oEdges;
                    oEdges = oFace.Edges;

                    if (oEdges.Count == 2 && oEdges[1].GeometryType == CurveTypeEnum.kCircleCurve && oEdges[2].GeometryType == CurveTypeEnum.kCircleCurve)
                    {
                        double dis1 = GetDistanceBetwEdgeAndFace(oPartCompDef, oEdges[1], face);
                        double dis2 = GetDistanceBetwEdgeAndFace(oPartCompDef, oEdges[2], face);

                        if (dis1 >= portStartDepth || dis2 >= portStartDepth)
                        {
                            Edge startEdge;
                            if (dis1 >= dis2)
                            {
                                startEdge = oEdges[2];
                            }
                            else
                            {
                                startEdge = oEdges[1];
                            }

                            if (startEdge != null)
                            {
                                PlanarSketch oPortFaceSketch;
                                oPortFaceSketch = oPartCompDef.Sketches.Add(portStartPlane);

                                oPortFaceSketch.AddByProjectingEntity(startEdge);

                                Profile profile;
                                profile = oPortFaceSketch.Profiles.AddForSurface();

                                ExtrudeDefinition portFaceExtruDef;
                                portFaceExtruDef = oPartCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(profile, PartFeatureOperationEnum.kSurfaceOperation);

                                //获取最后一阶孔的深度
                                double lastDepth = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("Depth8"));
                                portFaceExtruDef.SetDistanceExtent(lastDepth - portDepth1, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);

                                ExtrudeFeature portFaceExtru;
                                portFaceExtru = oPartCompDef.Features.ExtrudeFeatures.Add(portFaceExtruDef);
                                portFaceExtru.Name = ifeature.Name + "-1";

                                //创建底部圆锥面
                                WorkPoint coneBasePoint;
                                coneBasePoint = oPartCompDef.WorkPoints.AddAtCentroid(portFaceExtru.Faces[1].Edges[1], true);

                                WorkPlane coneStartPlane;
                                coneStartPlane = oPartCompDef.WorkPlanes.AddByPlaneAndPoint(face, coneBasePoint, true);

                                PlanarSketch oConeSketch;
                                oConeSketch = oPartCompDef.Sketches.Add(coneStartPlane);

                                oConeSketch.AddByProjectingEntity(startEdge);

                                Profile coneProfile;
                                coneProfile = oConeSketch.Profiles.AddForSurface();

                                Double dAngle8 = double.Parse(m_connectToaccess.SelectConnectToAccess("Angle8"));
                                Double rAngle8 = ConvertUnit(dAngle8, "degree", "radian");

                                ExtrudeDefinition coneFaceExtruDef;
                                coneFaceExtruDef = oPartCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(coneProfile, PartFeatureOperationEnum.kSurfaceOperation);
                                coneFaceExtruDef.SetThroughAllExtent(PartFeatureExtentDirectionEnum.kNegativeExtentDirection);
                                coneFaceExtruDef.TaperAngle = rAngle8;

                                ExtrudeFeature coneFaceExtru;
                                coneFaceExtru = oPartCompDef.Features.ExtrudeFeatures.Add(coneFaceExtruDef);
                                coneFaceExtru.Name = ifeature.Name + "-1cone";

                                Asset asset = null;
                                foreach (Asset asset1 in oPartDoc.Assets)
                                {
                                    if (asset1.DisplayName == netName)
                                    {
                                        asset = asset1;
                                    }
                                }
                                portFaceExtru.Appearance = asset;
                                coneFaceExtru.Appearance = asset;
                                WritePortFaceAttribute(portFaceExtru, ifeature, 1, 1);
                                WritePortFaceAttribute(coneFaceExtru, ifeature, 1, 1);
                                break;
                            }
                        }
                    }
                }
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
        
        private double ConvertUnit(Double value, String unit1, String unit2)
        {
            double result = 0.0;

            Document activeDocument = m_inventorApplication.ActiveDocument;

            UnitsOfMeasure unitsOfMeasure = activeDocument.UnitsOfMeasure;

            try
            {
                object vVal;
                vVal = unitsOfMeasure.ConvertUnits(value, unit1, unit2);
                result = System.Convert.ToDouble(vVal);
            }
            catch (System.Exception e)
            {
                string strErrorMsg = e.Message;

                result = 0.0;
                return result;
            }
            return result;
        }

        private double GetDistanceBetwEdgeAndFace(PartComponentDefinition partCompDef, Edge edge, Face face)
        {
            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(edge.Geometry.Center, face, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);

            return oDistance;
        }

        private void WritePortFaceAttribute(ExtrudeFeature portFace, iFeature ifeature, int portNumber, int index)
        {
            AttributeSets oAttributeSets;
            oAttributeSets = portFace.AttributeSets;

            AttributeSet oAttributeSet;
            try
            {
                oAttributeSet = oAttributeSets["MyAttribSet"];
            }
            catch
            {
                oAttributeSet = oAttributeSets.Add("MyAttribSet", false);
            }

            oAttributeSet.Add("iFeatureName", ValueTypeEnum.kStringType, ifeature.Name);
            oAttributeSet.Add("PortNumber", ValueTypeEnum.kIntegerType, portNumber);
            oAttributeSet.Add("PortIndex", ValueTypeEnum.kIntegerType, index);
        }
    }
}
