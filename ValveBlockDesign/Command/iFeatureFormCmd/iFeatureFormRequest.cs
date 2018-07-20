using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inventor;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using stdole;
using Microsoft.Win32;
using System.Drawing;

namespace ValveBlockDesign
{
    internal class iFeatureFormRequest:ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;
        private Face m_selsectFace;
        private string filepath;
        private string filename;
        private string codename;
        private string indexname;
        private string codenumber;
        private string checkfootprint;
        //private string m_ClientId = "c29d5be2-c9f7-4783-9191-5070d4944568";
        private string m_iFeatureName;

        private Inventor.Point m_Point;
        private double Xposition;
        private double Yposition;
        private ConnectToAccess m_connectToaccess;
        private iFeature m_iFeature;
        private int ucsNumber;
        private double rotateAngle;
        private DataGridView m_dataportView;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;

        public iFeatureFormRequest(Inventor.Application application, Face face, string filepath, string filename, string codename, string codenumber, string indexname, string checkfootprint, double xdistance, double ydistance, Inventor.Point point, double angle,DataGridView dataport)
        {
            m_inventorApplication = application;
            m_selsectFace = face;
            //---------------------------------------------------------------------------
            //数据库连接参数
            this.filepath = filepath;
            this.filename = filename;
            this.codename = codename;
            this.codenumber = codenumber;
            this.indexname = indexname;
            this.checkfootprint = checkfootprint;
            //---------------------------------------------------------------------------
            //插入点的信息
            this.Xposition = xdistance;
            this.Yposition = ydistance;

            this.m_Point = point;
            this.rotateAngle = angle;
            this.m_dataportView = dataport;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName;
            //---------------------------------------------------------------------------
            m_connectToaccess = new ConnectToAccess(filepath, filename, codename, indexname, codenumber);
        }

        //public iFeatureFormRequest(Inventor.Application application, Face face, string filepath, string filename, string codename, string codenumber, string indexname, string checkfootprint, double xdistance, double ydistance, Inventor.Point point, double angle)
        //{
        //    m_inventorApplication = application;
        //    m_selsectFace = face;
        //    //---------------------------------------------------------------------------
        //    //数据库连接参数
        //    this.filepath = filepath;
        //    this.filename = filename;
        //    this.codename = codename;
        //    this.codenumber = codenumber;
        //    this.indexname = indexname;
        //    this.checkfootprint = checkfootprint;
        //    //---------------------------------------------------------------------------
        //    this.Xposition = xdistance;
        //    this.Yposition = ydistance;
        //    //this.Netname = netname;

        //    this.m_Point = point;
        //    this.rotateAngle = angle;
        //}

        public void AddInformation(string name)
        {
            m_iFeatureName = name;
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


        private void insertCavFeature(Face selectFace,string indexnumber)//用于插入单一孔特征元件
        {
            string oInputName;
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;
            Face oSelectFace;
            oSelectFace = selectFace;
            PartFeatures oFeatures;
            oFeatures = oPartCompDef.Features;
            iFeatureDefinition oiFeatureDef;
            oiFeatureDef = oFeatures.iFeatures.CreateiFeatureDefinition(filepath+"\\"+indexnumber + "Cavity.ide");
            int i = JudgeFaceUcs(oSelectFace);
            foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
            {
                if (oInput.Name == "放置平面" || oInput.Name == "x轴" || oInput.Name == "y轴" || oInput.Name == "y" || oInput.Name == "x")
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
                        case "y":
                        iFeatureParameterInput oInputx;
                        oInputx = (iFeatureParameterInput)oInput;
                        oInputx.Value=Yposition;
                        break;
                        case "x":
                        iFeatureParameterInput oInputy;
                        oInputy = (iFeatureParameterInput)oInput;
                        oInputy.Value=Xposition;
                        break;
                    }
                }
                else
                {
                    oInputName = oInput.Name;
                    iFeatureParameterInput oParameXInput;
                    oParameXInput = (iFeatureParameterInput)oInput;
                    oParameXInput.Expression = m_connectToaccess.SelectConnectToAccess(oInputName);
                }
            }

            iFeature oiFeature;
            oiFeature = oFeatures.iFeatures.Add(oiFeatureDef);

            //------------------------------------------------------------------------------编写保存iFeature名称的属性集
            WriteAttribute(oiFeature, m_Point, "No", rotateAngle);

            oiFeature.Name = m_iFeatureName;
            //------------------------------------------------------------------------------用于网络区别的绘制
            ColorAllCavPort(oiFeature, oSelectFace);
            ShowPortColor();
            oPartDocument.Update2();
        }

        private void ColorAllCavPort(iFeature ifeature, Face face)
        {
            AttributeSets oAttributeSets;
            oAttributeSets = ifeature.AttributeSets;

            AttributeSet oAttributeSet;
            try
            {
                oAttributeSet = oAttributeSets["MyPorts"];
            }
            catch
            {
                oAttributeSet = oAttributeSets.Add("MyPorts", false);
            }

            String num = "1";
            int portNum = int.Parse(m_connectToaccess.SelectConnectToAccess("PortNumber"));
            Double PortDepth1 = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("PortDepth" + num));
            Double Depth0 = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("Depth0"));

            string netName = m_dataportView.Rows[0].Cells[1].Value.ToString();
            ColorCavPort1(ifeature, face, PortDepth1, Depth0, netName, portNum);

            oAttributeSet.Add("MyPortsNumber", ValueTypeEnum.kIntegerType, portNum);
            oAttributeSet.Add("Port1", ValueTypeEnum.kStringType, netName);

            int i = 1;
            while (i < portNum)
            {
                num = (++i).ToString();
                Double PortDia = double.Parse(m_connectToaccess.SelectConnectToAccess("PortDia" + num));
                Double PortDepth = double.Parse(m_connectToaccess.SelectConnectToAccess("PortDepth" + num));

                Double portCenterDepth = ConvertUnit(PortDepth, "mm", "cm") + Depth0;
                Double portDia = ConvertUnit(PortDia, "mm", "cm");

                netName = m_dataportView.Rows[--i].Cells[1].Value.ToString();
                ColorCavPort(ifeature, face, portDia, portCenterDepth, netName, portNum, i);
                oAttributeSet.Add("Port" + num, ValueTypeEnum.kStringType, netName);
                i++;
            }
        }

        //没有安装面的阀或油孔的其他port着色函数
        private void ColorCavPort(iFeature ifeature, Face face, Double portDia, Double portCenterDepth, string netName, int portNumber, int portIndex)
        {

            PartDocument oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;
            SelectSet oSelectSet = oPartDoc.SelectSet;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkPlane portCenterPlane;
            portCenterPlane = oPartCompDef.WorkPlanes.AddByPlaneAndOffset(face, -portCenterDepth, true);

            Faces insertiFeatureFaces;
            insertiFeatureFaces = ifeature.Faces;

            string portName = (portIndex + 1).ToString();
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

                        if ((dis1 >= portCenterDepth && dis2 <= portCenterDepth) || (dis2 >= portCenterDepth && dis1 <= portCenterDepth))
                        {
                            PlanarSketch oPortFaceSketch;
                            oPortFaceSketch = oPartCompDef.Sketches.Add(portCenterPlane);

                            oPortFaceSketch.AddByProjectingEntity(oEdges[1]);

                            Profile profile;
                            profile = oPortFaceSketch.Profiles.AddForSurface();

                            ExtrudeDefinition portFaceExtruDef;
                            portFaceExtruDef = oPartCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(profile, PartFeatureOperationEnum.kSurfaceOperation);
                            portFaceExtruDef.SetDistanceExtent(portDia, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection);

                            ExtrudeFeature portFaceExtru;
                            portFaceExtru = oPartCompDef.Features.ExtrudeFeatures.Add(portFaceExtruDef);
                            portFaceExtru.Name = ifeature.Name + "-" + portName;

                            Asset asset = null;
                            foreach (Asset asset1 in oPartDoc.Assets)
                            {
                                if (asset1.DisplayName == netName)
                                    asset = asset1;
                            }
                            portFaceExtru.Appearance = asset;

                            WritePortFaceAttribute(portFaceExtru, ifeature, portNumber, portIndex + 1);
                            break;
                        }
                    }
                }
            }
            ConnectToAccess connectToAccessNET = new ConnectToAccess(@"F:\CavityLibrary", "项目数据库");
            string sql = "insert into NETList(PortName," + netName + ") values('" + ifeature.Name + "-" + portName + "','" + ifeature.Name + "-" + portName + "')";
            if (connectToAccessNET.InsertInformation(sql))
            { }
            else
            {
                MessageBox.Show("保存网络出错！");
            }
        }

        //单个阀或油孔的port1着色函数
        private void ColorCavPort1(iFeature ifeature, Face face, double portDepth1, double depth0, string netName, int portNumber)
        {
            PartDocument oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;
            SelectSet oSelectSet = oPartDoc.SelectSet;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            double portStartDepth = portDepth1 + depth0;
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
                                WritePortFaceAttribute(portFaceExtru, ifeature, portNumber, 1);
                                WritePortFaceAttribute(coneFaceExtru, ifeature, portNumber, 1);
                                break;
                            }
                        }
                    }
                }
            }
            ConnectToAccess connectToAccessNET = new ConnectToAccess(@"F:\CavityLibrary", "项目数据库");
            string sql = "insert into NETList(PortName," + netName + ") values('" + ifeature.Name + "-1','" + ifeature.Name + "-1')";
            if (connectToAccessNET.InsertInformation(sql))
            { }
            else
            {
                MessageBox.Show("保存网络出错！");
            }
        }

        private void ColorAllFootpPort(iFeature ifeature, Face face)
        {
            //创建myPorts属性集用于记录各油孔的网络信息
            AttributeSets oAttributeSets;
            oAttributeSets = ifeature.AttributeSets;

            AttributeSet oAttributeSet;
            try
            {
                oAttributeSet = oAttributeSets["MyPorts"];
            }
            catch
            {
                oAttributeSet = oAttributeSets.Add("MyPorts", false);
            }

            int portCount = int.Parse(m_connectToaccess.SelectConnectToAccess("PortCount"));
            int portNumber = 0;
            try
            {
                portNumber = int.Parse(m_connectToaccess.SelectConnectToAccess("PortNumber"));
            }
            catch
            {
                portNumber = 0;
            }
            oAttributeSet.Add("PortsCount", ValueTypeEnum.kIntegerType, portCount);

            for (int j = portNumber; j < (portCount + portNumber); j++)
            {
                string portName = m_dataportView.Rows[j].Cells[0].Value.ToString();
                string netName = m_dataportView.Rows[j].Cells[1].Value.ToString();
                ColorFootprintPort(ifeature, face, portCount, netName, portName);
                oAttributeSet.Add(portName, ValueTypeEnum.kStringType, netName);
            }    
           
        }

        //用于给具有安装面的元件端口着色
        private void ColorFootprintPort(iFeature ifeature, Face face, int portCount, string netName, string portName)
        {
            PartDocument oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;
            SelectSet oSelectSet = oPartDoc.SelectSet;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            double portDepth = System.Math.Round(GetValueFromExpression(m_connectToaccess.SelectConnectToAccess(portName + "depth")), 2);

            foreach (Face oFace in ifeature.Faces)
            {
                if (oFace.SurfaceType == SurfaceTypeEnum.kCylinderSurface)
                {
                    Edges oEdges;
                    oEdges = oFace.Edges;

                    if (oEdges.Count == 2 && oEdges[1].GeometryType == CurveTypeEnum.kCircleCurve && oEdges[2].GeometryType == CurveTypeEnum.kCircleCurve)
                    {
                        double dis1 = System.Math.Round(GetDistanceBetwEdgeAndFace(oPartCompDef, oEdges[1], face), 2);
                        double dis2 = System.Math.Round(GetDistanceBetwEdgeAndFace(oPartCompDef, oEdges[2], face), 2);

                        if ((dis1 == 0 && dis2 == portDepth) || (dis2 == 0 && dis1 == portDepth))
                        {
                            Edge startEdge;
                            if (dis1 < dis2)
                            {
                                startEdge = oEdges[1];
                            }
                            else
                            {
                                startEdge = oEdges[2];
                            }

                            if (startEdge != null)
                            {
                                WorkPlane startPlane;
                                startPlane = oPartCompDef.WorkPlanes.AddByPlaneAndOffset(face, 0, true);

                                PlanarSketch oPortFaceSketch;
                                oPortFaceSketch = oPartCompDef.Sketches.Add(startPlane);

                                oPortFaceSketch.AddByProjectingEntity(startEdge);

                                Profile profile;
                                profile = oPortFaceSketch.Profiles.AddForSurface();

                                ExtrudeDefinition portFaceExtruDef;
                                portFaceExtruDef = oPartCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(profile, PartFeatureOperationEnum.kSurfaceOperation);
                                portFaceExtruDef.SetDistanceExtent(portDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);

                                ExtrudeFeature portFaceExtru;
                                portFaceExtru = oPartCompDef.Features.ExtrudeFeatures.Add(portFaceExtruDef);
                                portFaceExtru.Name = ifeature.Name+"-"+ portName;

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

                                Double dAngle = double.Parse(m_connectToaccess.SelectConnectToAccess("Angle"));
                                Double rAngle = ConvertUnit(dAngle, "degree", "radian");

                                ExtrudeDefinition coneFaceExtruDef;
                                coneFaceExtruDef = oPartCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(coneProfile, PartFeatureOperationEnum.kSurfaceOperation);
                                coneFaceExtruDef.SetThroughAllExtent(PartFeatureExtentDirectionEnum.kNegativeExtentDirection);
                                coneFaceExtruDef.TaperAngle = rAngle;

                                ExtrudeFeature coneFaceExtru;
                                coneFaceExtru = oPartCompDef.Features.ExtrudeFeatures.Add(coneFaceExtruDef);
                                coneFaceExtru.Name = ifeature.Name+"-"+portName + "cone";

                                Asset asset = null;
                                foreach (Asset asset1 in oPartDoc.Assets)
                                {
                                    if (asset1.DisplayName == netName)
                                    {
                                        asset = asset1;
                                        break;
                                    }
                                }
                                    portFaceExtru.Appearance = asset;
                                    coneFaceExtru.Appearance = asset;
                                    WritePortFaceAttribute(portFaceExtru, ifeature, 1, 1);
                                    WritePortFaceAttribute(portFaceExtru, ifeature, portCount, portName);
                            }
                        }
                    }
                }
            }
            ConnectToAccess connectToAccessNET = new ConnectToAccess(@"F:\CavityLibrary", "项目数据库");
            string sql = "insert into NETList(PortName," + netName + ") values('" + ifeature.Name + "-" + portName + "','" + ifeature.Name + "-" + portName + "')";
            if (connectToAccessNET.InsertInformation(sql))
            { }
            else
            {
                MessageBox.Show("保存网络出错！");
            }

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

        private double GetDistanceBetwEdgeAndFace(PartComponentDefinition partCompDef, Edge edge, Face face)
        {
            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(edge.Geometry.Center, face, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);

            return oDistance;
        }

        public void insertFootFeature(Face selectFace, string indexnumber)//用于插入具有安装面的元件
        {
            string oInputName;
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;
            Face oSelectFace;
            oSelectFace = selectFace;
            
            PartFeatures oFeatures;
            oFeatures = oPartCompDef.Features;
            iFeatureDefinition oiFeatureDef;
            oiFeatureDef = oFeatures.iFeatures.CreateiFeatureDefinition(filepath +"\\"+ indexnumber + "Footprint.ide");
            ucsNumber = JudgeFaceUcs(oSelectFace);
            Inventor.Vector oVector = oPartCompDef.UserCoordinateSystems[ucsNumber].XAxis.Line.Direction.AsVector();
            foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
            {
                if (oInput.Name == "放置平面" || oInput.Name == "x轴" || oInput.Name == "y轴" || oInput.Name == "y" || oInput.Name == "x")
                {
                    switch (oInput.Name)
                    {
                        case "放置平面":
                            iFeatureSketchPlaneInput oPlaneInput;
                            oPlaneInput = (iFeatureSketchPlaneInput)oInput;
                            oPlaneInput.PlaneInput = oSelectFace;
                            oPlaneInput.SetPosition(m_Point,oVector,rotateAngle);
                            break;
                        case "x轴":
                            iFeatureEntityInput oInputXAxis;
                            oInputXAxis = (iFeatureEntityInput)oInput;
                            oInputXAxis.Entity = oPartCompDef.UserCoordinateSystems[ucsNumber].XAxis;
                            break;
                        case "y轴":
                            iFeatureEntityInput oInputYAxis;
                            oInputYAxis = (iFeatureEntityInput)oInput;
                            oInputYAxis.Entity = oPartCompDef.UserCoordinateSystems[ucsNumber].YAxis;
                            break;
                        case "y":
                            iFeatureParameterInput oInputx;
                            oInputx = (iFeatureParameterInput)oInput;
                            oInputx.Value = Yposition;
                            break;
                        case "x":
                            iFeatureParameterInput oInputy;
                            oInputy = (iFeatureParameterInput)oInput;
                            oInputy.Value = Xposition;
                            break;
                    }
                }
                else
                {
                    oInputName = oInput.Name;
                    iFeatureParameterInput oParameXInput;
                    oParameXInput = (iFeatureParameterInput)oInput;
                    oParameXInput.Expression =m_connectToaccess.SelectConnectToAccess(oInputName);
                }
            }
            iFeature oiFeature;
            oiFeature = oFeatures.iFeatures.Add(oiFeatureDef);
            m_iFeature = oiFeature;
            
            
            double disminX =GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("LminX"))/2.0+Xposition;
            double disminY = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("LminY"))/2.0+Yposition;
            double dismaxX = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("LmaxX"))/2.0 + Xposition;
            double dismaxY = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("LmaxY"))/2.0 + Yposition;

                
            DrawSketch(selectFace, Yposition, Xposition, disminY, disminX, dismaxY, dismaxX, m_iFeatureName);
         
            //------------------------------------------------------------------------------编写保存iFeature名称的属性集
            RotateSketch(oPartCompDef,m_iFeatureName,Yposition,Xposition,rotateAngle);
            WriteAttribute(oiFeature, m_Point, "Yes", rotateAngle);
            oiFeature.Name = m_iFeatureName;

            //------------------------------------------------------------------------------拉伸曲面以区分网络
            string sql = @"select * from ComponentsDb where ComponentsDb.IndexName='" + indexname + "'";
            string cavityType = m_connectToaccess.GetSingleInformation(sql, "CavityType");

            if (cavityType == "二通插装孔")
            {
                ColorAllCavPort(oiFeature, oSelectFace);
            }

            ColorAllFootpPort(oiFeature, oSelectFace);
            ShowPortColor();
            oPartDocument.Update2();
        }

        private void RotateSketch(PartComponentDefinition oPartCompDef,string name,double x,double y,double angle)
        {
            ObjectCollection oSketchObjects;
            oSketchObjects = m_inventorApplication.TransientObjects.CreateObjectCollection();

            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;
            Inventor.Point2d centorPoint = oTransGeo.CreatePoint2d(x, y);
            PlanarSketch oSketch = oPartCompDef.Sketches["Footprint"+name];
            foreach (SketchEntity oSketchEntity in oSketch.SketchEntities)
            {
                oSketchObjects.Add(oSketchEntity);
            }
            oSketch.RotateSketchObjects(oSketchObjects, centorPoint, angle, false, true);
        }
        //-----------------------------------------------------------------------------------
        //编写属性集方法用于旋转操作
        public void WriteAttribute(iFeature oiFeature,Inventor.Point mPoint,string YesOrNo,double angle)
        {
            AttributeSets oAttributeSets;
            oAttributeSets = oiFeature.AttributeSets;

            AttributeSet oAttributeSet;
            oAttributeSet = oAttributeSets.Add("MyAttribSet", false);

            Inventor.Attribute oPlaneAttrib;
            oPlaneAttrib = oAttributeSet.Add("InternalName", ValueTypeEnum.kStringType, oiFeature.Name);

            Inventor.Attribute oFootprintAttrib;
            oFootprintAttrib = oAttributeSet.Add("Footprint", ValueTypeEnum.kStringType, YesOrNo);
            double pointx = m_Point.X;
            Inventor.Attribute mPointXAttrib;
            mPointXAttrib = oAttributeSet.Add("PointX",ValueTypeEnum.kDoubleType,pointx);
            double pointy = m_Point.Y;
            Inventor.Attribute mPointYAttrib;
            mPointYAttrib = oAttributeSet.Add("PointY", ValueTypeEnum.kDoubleType, pointy);
            double pointz = m_Point.Z;
            Inventor.Attribute mPointZAttrib;
            mPointZAttrib = oAttributeSet.Add("PointZ", ValueTypeEnum.kDoubleType, pointz);

            Inventor.Attribute IndexNameAttrib;
            IndexNameAttrib = oAttributeSet.Add("IndexName", ValueTypeEnum.kStringType, indexname);

            Inventor.Attribute CodeNameAttrib;
            CodeNameAttrib = oAttributeSet.Add("CodeName", ValueTypeEnum.kStringType, codename);

            Inventor.Attribute CodeNumberAttrib;
            CodeNumberAttrib = oAttributeSet.Add("CodeNumber", ValueTypeEnum.kStringType, codenumber);

            Inventor.Attribute AngleAttrib;
            AngleAttrib = oAttributeSet.Add("Angle", ValueTypeEnum.kDoubleType, angle);
        }

        //-----------------------------------------------------------------------------------
        //为拉伸曲面创建属性集，用于获取端口信息
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

        //板式阀拉伸曲面属性集补充
        private void WritePortFaceAttribute(ExtrudeFeature portFace, iFeature ifeature, int portCount, string portName)
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
            oAttributeSet.Add("PortCount", ValueTypeEnum.kIntegerType, portCount);
            oAttributeSet.Add("PortName", ValueTypeEnum.kStringType, portName);
        }

        //-----------------------------------------------------------------------------------
        //绘制安装面草图
        public void DrawSketch(Face oSelectFace,double baseX,double daseY,double LminX,double LminY,double LmaxX,double LmaxY,string sketchName)//绘制安装面示意图的方法
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveEditObject;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;
            
            int i = JudgeFaceUcs(oSelectFace);

            //添加新的草图
            PlanarSketch oSketch;
            oSketch = oPartDoc.ComponentDefinition.Sketches.AddWithOrientation(oSelectFace, oPartCompDef.UserCoordinateSystems[i].XAxis, true, false, oPartCompDef.UserCoordinateSystems[i].Origin, false);
            //创建临时几何图形对象，以便后续程序进行引用
            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;
            //画一个矩形
            oSketch.SketchLines.AddAsTwoPointCenteredRectangle(oTransGeo.CreatePoint2d(baseX, daseY), oTransGeo.CreatePoint2d(LminX, LminY));
            oSketch.SketchLines.AddAsTwoPointCenteredRectangle(oTransGeo.CreatePoint2d(baseX, daseY), oTransGeo.CreatePoint2d(LmaxX, LmaxY));
          
            oSketch.Name = "Footprint"+sketchName;
            Inventor.Color orange;
            orange = m_inventorApplication.TransientObjects.CreateColor(0, 0, 255);
            orange.Opacity = 0.3;
            oSketch.Color = orange;
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

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            if (this.checkfootprint == "No")
            {
                insertCavFeature(m_selsectFace, codenumber);
            }
            else
            {
                insertFootFeature(m_selsectFace, codenumber);
            }
        }

    }
}
