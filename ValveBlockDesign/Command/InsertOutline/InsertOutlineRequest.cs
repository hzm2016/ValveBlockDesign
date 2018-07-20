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
    internal class InsertOutlineRequest:ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;
        private Face m_insertFace;
        private iFeature m_selectiFeature;
        private Inventor.Point2d m_Point1;
        private Inventor.Point2d m_Point2;

        public InsertOutlineRequest(Inventor.Application application,Face face,iFeature ifeature,Point2d point1,Point2d point2)
        {
            m_inventorApplication = application;
            m_insertFace = face;
            m_selectiFeature = ifeature;
            m_Point1 = point1;
            m_Point2 = point2;
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            PartDocument oPartDocument;
            oPartDocument = (PartDocument)m_inventorApplication.ActiveDocument;
            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDocument.ComponentDefinition;
            DrowSketch(m_insertFace,m_selectiFeature.Name);
        }

        public void DrowSketch(Face oSelectFace, string sketchName)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveEditObject;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            //WorkPlane oworkplane;
            //oworkplane = oPartCompDef.WorkPlanes.AddByPlaneAndOffset(oSelectFace,0,true);

            int i = JudgeFaceUcs(oSelectFace);

            //添加新的草图
            PlanarSketch oSketch;
            //oSketch = oPartDoc.ComponentDefinition.Sketches.Add(oworkplane, false);
            oSketch = oPartDoc.ComponentDefinition.Sketches.AddWithOrientation(oSelectFace, oPartCompDef.UserCoordinateSystems[i].XAxis, true, false, oPartCompDef.UserCoordinateSystems[i].Origin, false);
            
            //创建临时几何图形对象，以便后续程序进行引用
            TransientGeometry oTransGeo;
            oTransGeo = m_inventorApplication.TransientGeometry;

            //画一个矩形
            
            oSketch.SketchLines.AddAsTwoPointRectangle(m_Point1,m_Point2);
            //string sText;
            //sText = "P";
            //Inventor.TextBox sTextBox = oSketch.TextBoxes.AddByRectangle(oTransGeo.CreatePoint2d(50, 50), oTransGeo.CreatePoint2d(55, 55), sText);

            oSketch.Name = "Outline" + sketchName;
            Inventor.Color red;
            red = m_inventorApplication.TransientObjects.CreateColor(255, 0, 0);
            red.Opacity = 0.3;
            oSketch.Color = red;
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
    }
}
