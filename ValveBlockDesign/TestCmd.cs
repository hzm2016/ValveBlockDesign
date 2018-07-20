using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;
using System.Data;

namespace ValveBlockDesign
{
    internal class TestCmd : ValveBlockDesign.Command
    {
        public TestCmd()
        {
             
        }

        protected override void ButtonDefinition_OnExecute(NameValueMap context)
        {
            this.GetMinimumDistance();
        }

        private void SelectSetTest()
        {
            SelectSet oSelectSet;
            oSelectSet = m_inventorApplication.ActiveDocument.SelectSet;

            Face oFace;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1] is Face)
                {
                    oFace = oSelectSet[1];
                    MessageBox.Show("Surface area: " + oFace.Evaluator.Area + " cm^2");
                }
                else
                {
                    MessageBox.Show("You must select a Face!");
                }
            }
            else
            {
                MessageBox.Show("You must select a Face!");
            }
        }

        private void CreateWorkPoint()
        {
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;

            SelectSet oSelectSet;
            oSelectSet = oPartDocument.SelectSet;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1].Type != (int)ObjectTypeEnum.kEdgeObject)
                {
                    MessageBox.Show("An edge must be selected!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("An edge must be selected!");
                return;
            }

            Edge oEdge;
            oEdge = oSelectSet[1];

            Vertex oStartVertex;
            oStartVertex = oEdge.StartVertex;

            Vertex oStopVertex;
            oStopVertex = oEdge.StopVertex;

            WorkPoint oStartWorkPoint;
            oStartWorkPoint = oPartCompDef.WorkPoints.AddByPoint(oStartVertex, false);
            WorkPoint oStopWorkPoint;
            oStopWorkPoint = oPartCompDef.WorkPoints.AddByPoint(oStopVertex, false);
        }

        private void CreateAxis()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            SelectSet oSelectSet;
            oSelectSet = oPartDoc.SelectSet;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1].Type != (int)ObjectTypeEnum.kFaceObject)
                {
                    MessageBox.Show("A face must be selected!");
                    return;
                }

                if (oSelectSet[1].SurfaceType != (int)SurfaceTypeEnum.kCylinderSurface)
                {
                    MessageBox.Show("A Cylindrical face muse be selected!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("A face muse be selected!");
                return;
            }

            Face oFace;
            oFace = oSelectSet[1];

            WorkAxis oWorkAxis;
            oWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(oFace, false);
        }

        private void CreateAllRoundFillet()
        {
            PartDocument oPartDocument;
            oPartDocument =(PartDocument) m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartComDef;
            oPartComDef = oPartDocument.ComponentDefinition;

            PlanarSketch oSketch;
            oSketch = oPartComDef.Sketches.Add(oPartComDef.WorkPlanes[3]);

            oSketch.SketchLines.AddAsTwoPointRectangle(m_inventorApplication.TransientGeometry.CreatePoint2d(-6, -4), m_inventorApplication.TransientGeometry.CreatePoint2d(6, 4));

            Profile oProfile;
            oProfile = oSketch.Profiles.AddForSolid();

            ExtrudeDefinition oExtrudeDef;
            oExtrudeDef = oPartComDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(oProfile, PartFeatureOperationEnum.kJoinOperation);
            oExtrudeDef.SetDistanceExtent(5, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection);
            ExtrudeFeature oExtrude;
            oExtrude = oPartComDef.Features.ExtrudeFeatures.Add(oExtrudeDef);

            EdgeCollection oEdges = null;

            FilletFeature oFillet;
            oFillet = oPartComDef.Features.FilletFeatures.AddSimple(oEdges, 1, false, true, true, false, true, false);
        }

        public void MoveiFeature()
        {
            PartDocument oPartDocument;
            oPartDocument =(PartDocument) m_inventorApplication.ActiveDocument;

            SelectSet oSelectSet;
            oSelectSet = oPartDocument.SelectSet;

            Face oSelectFace;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1] is Face)
                {                  
                }
                else
                { 
                    return;
                }
            }
            else
            {
                MessageBox.Show("You must select a Face!");
                return;
            }

            oSelectFace = oSelectSet[1];

            PartFeatures oFeatures;
            oFeatures = oPartDocument.ComponentDefinition.Features;

            iFeatures oiFeatures;
            oiFeatures = oFeatures.iFeatures;

            foreach (iFeature oiFeature in oiFeatures)
            {
                Faces oFaces;
                oFaces = oiFeature.Faces;
                foreach( Face oFace in oFaces)
                {
                    if (oFace == oSelectFace)
                    {
                        iFeature oSelectiFeature;
                        oSelectiFeature = oiFeature;

                        string oSelectiFeatureName = oSelectiFeature.Name;
                        string oParameterName = oSelectiFeatureName + ":距离1";

                        iFeatureDefinition oiFeatureDef;
                        oiFeatureDef = oSelectiFeature.iFeatureDefinition;

                        foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
                        {
                            iFeatureParameterInput oParamInput;

                            if (oInput.Name == oParameterName)
                            {
                                    oParamInput = (iFeatureParameterInput)oInput;

                                    oParamInput.Parameter.Value = 2;
                                    oPartDocument.Update2();
                            }
                        }
                        return;
                    }
                }
            }
        }

        public void CreateUCSByThreePoints()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;

            SelectSet oSelectSet;
            oSelectSet = oPartDoc.SelectSet;

            Face oSelectFace;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1] is Face)
                {
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("You must select a Face!");
                return;
            }

            oSelectFace = oSelectSet[1];

            Edges oEdges;
            oEdges = oSelectFace.Edges;

            WorkPoint oUCSOrigin;
            oUCSOrigin = oPartCompDef.WorkPoints.AddByTwoLines(oEdges[3], oEdges[4], true);

            UserCoordinateSystemDefinition oUCSDef;
            oUCSDef = oPartCompDef.UserCoordinateSystems.CreateDefinition();

            oUCSDef.SetByThreePoints(oUCSOrigin, oEdges[3], oEdges[4]);

            UserCoordinateSystem oUCS;
            oUCS = oPartCompDef.UserCoordinateSystems.Add(oUCSDef);

        }

        public void CreateUCSByTransformationMatrix()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;

            Matrix oMatrix;
            oMatrix = oTransGeo.CreateMatrix();

            Double Pi;
            Pi = Math.PI;

            oMatrix.SetToRotation(Pi / 4, oTransGeo.CreateVector(1, 0, 0), oTransGeo.CreatePoint(0, 0, 0));

            Matrix oTransMatrix;
            oTransMatrix = oTransGeo.CreateMatrix();
            oTransMatrix.SetTranslation(oTransGeo.CreateVector(2, 2, 2));

            oMatrix.TransformBy(oTransMatrix);

            UserCoordinateSystemDefinition oUCSDef;
            oUCSDef = oPartCompDef.UserCoordinateSystems.CreateDefinition();

            oUCSDef.Transformation = oMatrix;
 
            UserCoordinateSystem oUCS;
            oUCS = oPartCompDef.UserCoordinateSystems.Add(oUCSDef);

        }

        public void InteractiveMeasureDistance()
        {
            this.Measure(MeasureTypeEnum.kDistanceMeasure);
        }


        private MeasureEvents m_MeasureEvents;

        private Boolean bStillMeasure;
        private MeasureTypeEnum m_measureType;

        private MeasureEventsSink_OnMeasureEventHandler m_Measure_OnMeasure_Delegate;
        private InteractionEventsSink_OnTerminateEventHandler m_Interaction_OnTerminate_Delegate;

        public void Measure(MeasureTypeEnum measureType)
        {
            m_measureType = measureType;

            bStillMeasure = true;

            m_interactionEvents = m_inventorApplication.CommandManager.CreateInteractionEvents();
            m_Interaction_OnTerminate_Delegate = new InteractionEventsSink_OnTerminateEventHandler(InteractionEvents_OnTerminate);
            m_interactionEvents.OnTerminate += m_Interaction_OnTerminate_Delegate;

            m_MeasureEvents = m_interactionEvents.MeasureEvents;

            m_Measure_OnMeasure_Delegate = new MeasureEventsSink_OnMeasureEventHandler(MeasureEvents_OnMeasure);
            m_MeasureEvents.OnMeasure += m_Measure_OnMeasure_Delegate;

            m_interactionEvents.Start();

            if (m_measureType == MeasureTypeEnum.kDistanceMeasure)
            {
                m_MeasureEvents.Measure(MeasureTypeEnum.kDistanceMeasure);
            }

            do
            {
                m_inventorApplication.UserInterfaceManager.DoEvents();
            }
            while (bStillMeasure);

            m_interactionEvents.Stop();

            m_MeasureEvents = null;
            m_interactionEvents = null;

        }

        private void InteractionEvents_OnTerminate()
        {
            bStillMeasure = false;
        }

        private void MeasureEvents_OnMeasure(MeasureTypeEnum MeasureType, double MeasuredValue, NameValueMap Context)
        {
            string strMeasuredValue;

            if (m_measureType == MeasureTypeEnum.kDistanceMeasure)
            {
                strMeasuredValue = m_inventorApplication.ActiveDocument.UnitsOfMeasure.GetStringFromValue(MeasuredValue, UnitsTypeEnum.kDefaultDisplayLengthUnits);
                MessageBox.Show("Distance = " + strMeasuredValue, "Measure Distance");

                bStillMeasure = false;
            }
        }

        public void GetMinimumDistance()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            SelectSet oSelectSet;
            oSelectSet = oPartDoc.SelectSet;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1].Type != (int)ObjectTypeEnum.kFaceObject)
                {
                    MessageBox.Show("A face must be selected!");
                    return;
                }

                if (oSelectSet[1].SurfaceType != (int)SurfaceTypeEnum.kCylinderSurface)
                {
                    MessageBox.Show("A Cylindrical face muse be selected!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("A face muse be selected!");
                return;
            }

            Face oFace;
            oFace = oSelectSet[1];

            WorkAxis oWorkAxis;
            oWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(oFace, true);

            UserCoordinateSystems oUCSs;
            oUCSs = oPartCompDef.UserCoordinateSystems;

            UserCoordinateSystem oUCS;
            oUCS = oUCSs[1];

            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(oWorkAxis, oUCS.XAxis, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);

            MessageBox.Show(oDistance.ToString() + "cm");
        }

        private void GetNormalFromFace()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            SelectSet oSelectSet;
            oSelectSet = oPartDoc.SelectSet;

            Face oSelectFace;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1] is Face)
                {
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("You must select a Face!");
                return;
            }

            oSelectFace = oSelectSet[1];

            Double[] oParam = new Double[2];
            oParam[0] = 0; 
            oParam[1] = 1;
            Double[] oNormal = new Double[3];
            oSelectFace.Evaluator.GetNormal(ref oParam,ref oNormal);
            MessageBox.Show(oParam[0].ToString()+"  "+oParam[1].ToString());
            MessageBox.Show((string)("x:"+ oNormal[0].ToString() +"  y:"+ oNormal[1].ToString() + "  z:"+oNormal[2].ToString()));
        }

        private void GetEdgesFromFace()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            SelectSet oSelectSet;
            oSelectSet = oPartDoc.SelectSet;

            Face oSelectFace;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1] is Face)
                {
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("You must select a Face!");
                return;
            }

            oSelectFace = oSelectSet[1];

            Edges oEdges;
            oEdges = oSelectFace.Edges;

            EdgeCollection oLineEdges;
            oLineEdges = m_inventorApplication.TransientObjects.CreateEdgeCollection();

            int nombEdges = oEdges.Count;

            MessageBox.Show(nombEdges.ToString());

            for (int edgeCt = 1; edgeCt <= nombEdges; edgeCt++)
            {
                Edge oEdge = oEdges[edgeCt];

                CurveTypeEnum oCurveType;
                oCurveType = oEdge.GeometryType;

                if (oCurveType == CurveTypeEnum.kLineSegmentCurve)
                {
                    oLineEdges.Add(oEdge);
                }
            }

            int objNom = oLineEdges.Count;
            MessageBox.Show(objNom.ToString());
        }

        private void GetBasePoint()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            SelectSet oSelectSet;
            oSelectSet = oPartDoc.SelectSet;

            Face oSelectFace;

            if (oSelectSet.Count > 0)
            {
                if (oSelectSet[1] is Face)
                {
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("You must select a Face!");
                return;
            }

            oSelectFace = oSelectSet[1];

            Double[] oparams = new Double[2];
            oparams[0] = 0.5;
            oparams[1] = 0.5;

            Double[] opoint = new Double[3];
            oSelectFace.Evaluator.GetPointAtParam(ref oparams, ref opoint);
            MessageBox.Show((string)("x:" + opoint[0].ToString() + "  y:" + opoint[1].ToString() + "  z:" + opoint[2].ToString()));
            //Point oPoint;
            //oPoint = m_inventorApplication.TransientGeometry.CreatePoint(opoint[0], opoint[1], opoint[2]);

        }

        public override void ExecuteCommand()
        {
        }
    }
}
