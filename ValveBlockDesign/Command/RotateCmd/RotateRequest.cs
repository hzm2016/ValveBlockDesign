using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace ValveBlockDesign
{
    internal class RotateRequest:ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;

        private Face m_selectFace;
        private Face m_insertFace;
        private iFeature m_selectiFeature;
        double m_Angle;
        double m_OldAngle;
        double m_InsertAngle=0.0;
        string m_iFeatureName;
        private Inventor.Point m_Point;
        //-----------------------------------------------------------------------
        //数据库连接参数
        private string m_filepath=@"F:\CavityLibrary";
        private string m_filename= "CavityLibrary";
        private string m_codename;
        private string m_indexname;
        private string m_codenumber;//索引编号寻找iFeature特征
        private string m_checkfootprint;
        //-----------------------------------------------------------------------
        //插入特征
        private ConnectToAccess m_connectToaccess;
        private double Xposition;
        private double Yposition;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;

        public RotateRequest(Inventor.Application application, Face face, iFeature ifeature,double angle)
        {
            m_inventorApplication = application;
            m_selectFace = face;
            m_selectiFeature = ifeature;
            m_Angle = angle;
        }

        public void InsertInformation()
        {
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;

            AttributeSets atr = m_selectiFeature.AttributeSets;
            int insertPlane;
            int insertFace;
            this.GetInsertFace(oPartCompDef, m_selectFace, out insertPlane, out insertFace);
            m_insertFace = oPartCompDef.Features.ExtrudeFeatures["拉伸1"].Faces[insertFace];
            AttributeSet abs = atr["MyAttribSet"];
            Inventor.Attribute internalname = abs["InternalName"];
            Inventor.Attribute footprint = abs["Footprint"];
            Inventor.Attribute pointX = abs["PointX"];
            Inventor.Attribute pointY = abs["PointY"];
            Inventor.Attribute pointZ = abs["PointZ"];
            Inventor.Attribute indexname = abs["IndexName"];
            m_indexname = indexname.Value;
            Inventor.Attribute codename = abs["CodeName"];
            m_codename = codename.Value;
            Inventor.Attribute codenumber = abs["CodeNumber"];
            m_codenumber = codenumber.Value;
            Inventor.Attribute angle = abs["Angle"];
            m_OldAngle = angle.Value;
            m_InsertAngle = m_OldAngle + m_Angle;

            m_checkfootprint = footprint.Value;

            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;
            m_Point = oTransGeo.CreatePoint(pointX.Value,pointY.Value,pointZ.Value);

            Xposition = pointX.Value;
            //GetDistanceBtwPointandLine(m_Point,oPartCompDef.UserCoordinateSystems[insertPlane].XAxis);
            Yposition = pointY.Value;
            //GetDistanceBtwPointandLine(m_Point,oPartCompDef.UserCoordinateSystems[insertPlane].YAxis);
            m_iFeatureName = m_selectiFeature.Name;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName;
            m_filepath = deFaultpath + "\\CavityLibrary";

        }

        private double GetDistanceBtwPointandLine(Inventor.Point mouseupPoint, WorkAxis workaxis)
        {
            PartDocument partDoc;
            partDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition partComDef;
            partComDef = partDoc.ComponentDefinition;

            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(mouseupPoint, workaxis, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);
            return oDistance;
        }
        public void DeleteiFeature()
        {
            m_selectiFeature.Delete();
        }

        public void InsertiFeature(PartComponentDefinition oPartCompDef)
        {
            InsertInformation();
            DeleteiFeature();
            if (m_checkfootprint == "Yes")
            {
                PlanarSketch oSketchFootprint = oPartCompDef.Sketches["Footprint" + m_iFeatureName];
                oSketchFootprint.Delete();
                insertFootFeature(m_insertFace);
            }
            else
            {
                MessageBox.Show("请选择具有安装面的孔特征");
                return;
            }
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;
            InsertiFeature(oPartCompDef);
        }
        //--------------------------------------------------------------------------------------
        //获得插入元件的安装表面
        private void GetInsertFace(PartComponentDefinition partCompDef, Face face, out int planeNumb, out int faceNumb)
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
            faceNumb = 0;
            switch (strCoords)
            {
                case "00-1":
                    planeNumb = 1;
                    faceNumb = 5;
                    break;
                case "010":
                    planeNumb = 2;
                    faceNumb = 4;
                    break;
                case "-100":
                    planeNumb = 3;
                    faceNumb = 3;
                    break;
                case "0-10":
                    planeNumb = 4;
                    faceNumb = 2;
                    break;
                case "100":
                    planeNumb = 5;
                    faceNumb = 1;
                    break;
                case "001":
                    planeNumb = 6;
                    faceNumb = 6;
                    break;
            }
        }

        private void insertCavFeature(Face selectFace, string indexnumber)//用于插入单一孔特征元件
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
            oiFeatureDef = oFeatures.iFeatures.CreateiFeatureDefinition("F:\\CavityLibrary\\" + indexnumber + "Cavity.ide");
            int i = JudgeFaceUcs(oSelectFace);
            foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
            {
                if (oInput.Name == "放置平面" || oInput.Name == "x轴" || oInput.Name == "y轴" || oInput.Name == "x" || oInput.Name == "y")
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
                    m_connectToaccess = new ConnectToAccess(m_filepath, m_filename, m_codename, m_indexname, m_codenumber);
                    oParameXInput.Expression = m_connectToaccess.SelectConnectToAccess(oInputName);
                }
            }
            iFeature oiFeature;
            oiFeature = oFeatures.iFeatures.Add(oiFeatureDef);
            //------------------------------------------------------------------------------用于网络区别的绘制
            //DrawAllPortColor(oiFeature, oSelectFace);
            //oPartDocument.Update2();
            //------------------------------------------------------------------------------编写保存iFeature名称的属性集
            AttributeSets oAttributeSets;
            oAttributeSets = oiFeature.AttributeSets;

            AttributeSet oAttributeSet;
            oAttributeSet = oAttributeSets.Add("MyAttribSet", false);

            Inventor.Attribute oPlaneAttrib;
            oPlaneAttrib = oAttributeSet.Add("InternalName", ValueTypeEnum.kStringType, oiFeature.Name);

            Inventor.Attribute oFootprintAttrib;
            oFootprintAttrib = oAttributeSet.Add("Footprint", ValueTypeEnum.kStringType, "No");

            oiFeature.Name = m_iFeatureName;
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

        private double GetDistanceBetwEdgeAndFace(PartComponentDefinition partCompDef, Edge edge, Face face)
        {
            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(edge.Geometry.Center, face, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);

            return oDistance;
        }

        private void insertFootFeature(Face selectFace)//用于插入具有安装面的元件
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
            oiFeatureDef = oFeatures.iFeatures.CreateiFeatureDefinition(m_filepath +"\\"+m_codenumber+"Footprint.ide");
            int ucsNumber = JudgeFaceUcs(oSelectFace);
            Inventor.Vector oVector = oPartCompDef.UserCoordinateSystems[ucsNumber].XAxis.Line.Direction.AsVector();
            foreach (iFeatureInput oInput in oiFeatureDef.iFeatureInputs)
            {
                if (oInput.Name == "放置平面" || oInput.Name == "x轴" || oInput.Name == "y轴" || oInput.Name == "x" || oInput.Name == "y")
                {
                    switch (oInput.Name)
                    {
                        case "放置平面":
                            iFeatureSketchPlaneInput oPlaneInput;
                            oPlaneInput = (iFeatureSketchPlaneInput)oInput;
                            oPlaneInput.PlaneInput = oSelectFace;
                            oPlaneInput.SetPosition(m_Point, oVector, m_InsertAngle);
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
                    m_connectToaccess = new ConnectToAccess(m_filepath, m_filename, m_codename, m_indexname, m_codenumber);
                    oParameXInput.Expression =m_connectToaccess.SelectConnectToAccess(oInputName);
                }
            }
            iFeature oiFeature;
            oiFeature = oFeatures.iFeatures.Add(oiFeatureDef);
            
            double disminX = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("LminX")) / 2 + Xposition;
            double disminY = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("LminY")) / 2 + Yposition;
            double dismaxX = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("LmaxX")) / 2 + Xposition;
            double dismaxY = GetValueFromExpression(m_connectToaccess.SelectConnectToAccess("LmaxY")) / 2 + Yposition;

            DrowSketch(selectFace, Yposition, Xposition, disminY, disminX, dismaxY, dismaxX, m_iFeatureName);
            RotateSketch(oPartCompDef, m_iFeatureName, Yposition, Xposition, m_InsertAngle);

            WriteAttribute(oiFeature, m_Point, "Yes", m_InsertAngle);
            //一定要在后面执行
            oiFeature.Name = m_iFeatureName;
        }

        private void RotateSketch(PartComponentDefinition oPartCompDef, string name, double x, double y, double angle)
        {
            ObjectCollection oSketchObjects;
            oSketchObjects = m_inventorApplication.TransientObjects.CreateObjectCollection();

            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;
            Inventor.Point2d centorPoint = oTransGeo.CreatePoint2d(x, y);
            PlanarSketch oSketch = oPartCompDef.Sketches["Footprint" + name];
            foreach (SketchEntity oSketchEntity in oSketch.SketchEntities)
            {
                oSketchObjects.Add(oSketchEntity);
            }
            oSketch.RotateSketchObjects(oSketchObjects, centorPoint, angle, false, true);
        }

        public void WriteAttribute(iFeature oiFeature, Inventor.Point mPoint, string YesOrNo, double angle)
        {
            //建立该元件的属性集
            AttributeSets oAttributeSets;
            oAttributeSets = oiFeature.AttributeSets;
            AttributeSet oAttributeSet;
            oAttributeSet = oAttributeSets.Add("MyAttribSet", false);

            //存储元件插入时系统分配的内部名称用于后面的移动操作
            Inventor.Attribute oPlaneAttrib;
            oPlaneAttrib = oAttributeSet.Add("InternalName", ValueTypeEnum.kStringType, oiFeature.Name);

            //存储该元件是否具有安装面
            Inventor.Attribute oFootprintAttrib;
            oFootprintAttrib = oAttributeSet.Add("Footprint", ValueTypeEnum.kStringType, YesOrNo);

            //存储该元件插入点的坐标
            double pointx = m_Point.X;
            Inventor.Attribute mPointXAttrib;
            mPointXAttrib = oAttributeSet.Add("PointX", ValueTypeEnum.kDoubleType, pointx);
            double pointy = m_Point.Y;
            Inventor.Attribute mPointYAttrib;
            mPointYAttrib = oAttributeSet.Add("PointY", ValueTypeEnum.kDoubleType, pointy);
            double pointz = m_Point.Z;
            Inventor.Attribute mPointZAttrib;
            mPointZAttrib = oAttributeSet.Add("PointZ", ValueTypeEnum.kDoubleType, pointz);

            //存储该元件再次插入时连接到数据库需要的信息
            Inventor.Attribute IndexNameAttrib;
            IndexNameAttrib = oAttributeSet.Add("IndexName", ValueTypeEnum.kStringType, m_indexname);
            Inventor.Attribute CodeNameAttrib;
            CodeNameAttrib = oAttributeSet.Add("CodeName", ValueTypeEnum.kStringType, m_codename);
            Inventor.Attribute CodeNumberAttrib;
            CodeNumberAttrib = oAttributeSet.Add("CodeNumber", ValueTypeEnum.kStringType, m_codenumber);

            //存储该元件在插入时已经旋转的角度
            Inventor.Attribute AngleAttrib;
            AngleAttrib = oAttributeSet.Add("Angle", ValueTypeEnum.kDoubleType, angle);
        }
        //----------------------------------------------------------------------------------------------
        //绘制安装面示意图的方法
        public void DrowSketch(Face oSelectFace, double baseX, double daseY, double LminX, double LminY, double LmaxX, double LmaxY, string sketchName)
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
            
            oSketch.Name = "Footprint" + sketchName;
            Inventor.Color blue;
            blue = m_inventorApplication.TransientObjects.CreateColor(0, 0, 255);
            blue.Opacity = 0.3;
            oSketch.Color = blue;
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


    }
}
