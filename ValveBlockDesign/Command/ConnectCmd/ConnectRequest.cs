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
    internal class ConnectRequest : ValveBlockDesign.ChangeRequest
    {
        //private ConnectCmdDlg m_connectCmdlg;
        private Inventor.Application m_inventorApplication;

        private ExtrudeFeature m_aSurf;
        private ExtrudeFeature m_bSurf;
        private iFeature m_aCav;
        private iFeature m_bCav;

        private UserCoordinateSystem m_UCSa;
        private UserCoordinateSystem m_UCSb;

        private int m_bPortIndex;
        private int m_connctType;

        private string m_aInterName;
        private string m_bInterName;
        private string m_aIndexName;
        private string m_bIndexName;
        private string m_filepath;
        private string m_filename;

        private string m_aCavType;
        private string m_bCavType;
        private string m_aFootPrint;
        private string m_bFootPrint;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;

        private WorkPlane m_judgeRefPlane; //用作判断直接延伸还是移动连接的基准面

        private ConnectCmd.ConnectAlignTypeEnum m_connctAlignType;

        public ConnectRequest(Inventor.Application application, ConnectCmd.ConnectAlignTypeEnum connctAlignType, ExtrudeFeature thisSurf,
                              ExtrudeFeature connectToSurf, iFeature thisCav, iFeature connectToCav, int connctToPortIndex)
        {
            m_inventorApplication = application;
            m_connctAlignType = connctAlignType;
            m_aSurf = thisSurf;
            m_bSurf = connectToSurf;
            m_aCav = thisCav;
            m_bCav = connectToCav;

            m_bPortIndex = connctToPortIndex;

            SetFilepathAndName();
        }

        //执行函数，其中选中的孔为孔a，后选中的孔为孔b
        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            double cavADia = 0;     //孔a直径
            double cavADepth0 = 0;      //孔a深度Depth0
            double cavBDia = 0;     //孔b直径
            double cavBDepth0 = 0;      //孔b深度
            double cavBStopDepth = 0;       //孔b深度

            int conneTypeByCavityType = 0;

            //获取两孔iFeature的Intername和indexname
            GetiFeatureIntNameAndIndName();
            //获取两孔的类型及是否有footprint
            GetBothCavType(m_aIndexName, m_bIndexName);
            //两孔连接类型分类
            Classification();
            //设置基准面
            SetJudgeRefPlane();

            conneTypeByCavityType = GetConnecType();

            //单孔a和单孔b连接
            if (conneTypeByCavityType > 0)
            {
                if (conneTypeByCavityType == 1)
                {
                    //从单独孔获取数据
                    GetCavData(m_aCav, m_aInterName, "Dia8", ref cavADepth0, ref cavADia);//从孔a的iFeature获取数据
                    GetCavData(m_bCav, m_bInterName, "Depth8", "Dia8", ref cavBDepth0, ref cavBStopDepth, ref cavBDia);//从孔b的iFeature获取数据

                    if (m_bPortIndex == 1)
                    {
                        bool extentOrMove = DeterminExtentOrMove(cavADia, cavBDia);

                        if (extentOrMove)
                        {
                            ExecuteExtent(cavADepth0, cavBDepth0, "Depth8", "Depth8",cavBStopDepth, cavADia);
                        }
                        else
                        {
                            ExecuteCenterConnect(cavBStopDepth, cavADepth0, cavADia, cavBDepth0);
                        }
                    }
                    else
                    {
                        ExecuteCenterConnect(cavBStopDepth, cavADepth0, cavADia, cavBDepth0);
                    }
                }

                //板式阀通孔a和单孔b连接
                if (conneTypeByCavityType == 2)
                {
                    AttributeSets oAttributeSets;
                    oAttributeSets = m_aSurf.AttributeSets;

                    AttributeSet oAttributeSet;                    
                    oAttributeSet = oAttributeSets["MyAttribSet"];
                    string aportName = oAttributeSet["PortName"].Value;

                    GetCavData(m_aCav, m_aInterName, aportName + "dmax", ref cavADepth0, ref cavADia);//从孔a的iFeature获取数据
                    GetCavData(m_bCav, m_bInterName, "Depth8", "Dia8", ref cavBDepth0, ref cavBStopDepth, ref cavBDia);//从孔b的iFeature获取数据

                    if (m_bPortIndex == 1)
                    {
                        bool extentOrMove = DeterminExtentOrMove(cavADia, cavBDia);
                        if (extentOrMove)
                        {
                            ExecuteExtent(aportName, aportName + "depth", cavBDepth0, "Depth8", cavBStopDepth, cavADia);
                        }
                        else
                        {
                            ExecuteCenterConnect(cavBStopDepth, cavADia, cavBDepth0);
                        }
                    }
                    else
                    {
                        ExecuteCenterConnect(cavBStopDepth, cavADia, cavBDepth0);
                    }
                }

                //单孔a和板式阀通孔b连接
                if (conneTypeByCavityType == 3)
                {
                    AttributeSets oAttributeSets;
                    oAttributeSets = m_bSurf.AttributeSets;

                    AttributeSet oAttributeSet;
                    oAttributeSet = oAttributeSets["MyAttribSet"];
                    string bportName = oAttributeSet["PortName"].Value;

                    GetCavData(m_aCav, m_aInterName, "Dia8", ref cavADepth0, ref cavADia);//从孔a的iFeature获取数据
                    GetCavData(m_bCav, m_bInterName, bportName + "depth", bportName + "dmax", ref cavBDepth0, ref cavBStopDepth, ref cavBDia);//从孔b的iFeature获取数据

                    bool extentOrMove = DeterminExtentOrMove(cavADia, cavBDia);
                    if (extentOrMove)
                    {
                        ExecuteExtent(cavADepth0, "Depth8", bportName, bportName + "depth", cavBStopDepth);
                    }
                    else
                    {
                        ExecuteCenterConnect(cavBStopDepth, cavADepth0);
                    }
                }

                //板式阀通孔a和板式阀通孔b连接
                if (conneTypeByCavityType == 4)
                {
                    AttributeSets aAttributeSets;
                    aAttributeSets = m_aSurf.AttributeSets;
                    AttributeSet aAttributeSet;
                    aAttributeSet = aAttributeSets["MyAttribSet"];
                    string aportName = aAttributeSet["PortName"].Value;

                    AttributeSets bAttributeSets;
                    bAttributeSets = m_bSurf.AttributeSets;
                    AttributeSet bAttributeSet;
                    bAttributeSet = bAttributeSets["MyAttribSet"];
                    string bportName = bAttributeSet["PortName"].Value;

                    GetCavData(m_aCav, m_aInterName, aportName + "dmax", ref cavADepth0, ref cavADia);
                    GetCavData(m_bCav, m_bInterName, bportName + "depth", bportName + "dmax", ref cavBDepth0, ref cavBStopDepth, ref cavBDia);

                    bool extentOrMove = DeterminExtentOrMove(cavADia, cavBDia);
                    if (extentOrMove)
                    {
                        ExecuteExtent(aportName, bportName, aportName + "depth", bportName + "depth");
                    }
                    else
                    {
                        ExecuteCenterConnect(cavBStopDepth);
                    }
                }
            }
            else
            {
                MessageBox.Show("连接分类发生错误！");
            }
        }

        private int GetConnecType()
        {
            int conneTypeByCavityType = 0;
            if (m_aFootPrint == "No" && m_bFootPrint == "No")
            {
                conneTypeByCavityType = 1; //两个单独油孔端口相连
                return conneTypeByCavityType;
            }
            else
            {
                if (m_aFootPrint == "Yes" && m_bFootPrint == "No")
                {
                    AttributeSets oAttributeSets;
                    oAttributeSets = m_aSurf.AttributeSets;

                    AttributeSet oAttributeSet;
                    try
                    {
                        oAttributeSet = oAttributeSets["MyAttribSet"];
                        string portName = oAttributeSet["PortName"].Value;
                        conneTypeByCavityType = 2;
                        return conneTypeByCavityType;
                    }
                    catch
                    {
                        conneTypeByCavityType = 1;
                        return conneTypeByCavityType;
                    }
                }
                else
                {
                    if (m_aFootPrint == "No" && m_bFootPrint == "Yes")
                    {
                        AttributeSets oAttributeSets;
                        oAttributeSets = m_bSurf.AttributeSets;

                        AttributeSet oAttributeSet;
                        try
                        {
                            oAttributeSet = oAttributeSets["MyAttribSet"];
                            string portName = oAttributeSet["PortName"].Value;
                            conneTypeByCavityType = 3;
                            return conneTypeByCavityType;
                        }
                        catch
                        {
                            conneTypeByCavityType = 1;
                            return conneTypeByCavityType;
                        }
                    }
                    else
                    {
                        string aType = "";
                        string bType = "";
                        AttributeSets aAttributeSets;
                        aAttributeSets = m_aSurf.AttributeSets;

                        AttributeSet aAttributeSet;
                        try
                        {
                            aAttributeSet = aAttributeSets["MyAttribSet"];
                            string portName = aAttributeSet["PortName"].Value;
                            aType = "1";
                        }
                        catch
                        {
                            aType = "2";
                        }

                        AttributeSets bAttributeSets;
                        bAttributeSets = m_bSurf.AttributeSets;

                        AttributeSet bAttributeSet;
                        try
                        {
                            bAttributeSet = bAttributeSets["MyAttribSet"];
                            string portName = bAttributeSet["PortName"].Value;
                            bType = "1";
                        }
                        catch
                        {
                            bType = "2";
                        }

                        switch (aType + bType)
                        {
                            case "11":
                                conneTypeByCavityType = 4;
                                break;
                            case "12":
                                conneTypeByCavityType = 2;
                                break;
                            case "21":
                                conneTypeByCavityType = 3;
                                break;
                            case "22":
                                conneTypeByCavityType = 1;
                                break;
                        }
                        return conneTypeByCavityType;
                    }
                }
            }
        }

        #region 两孔延伸并连接重载函数（不移动）

        //___________________________________________ 两个单孔延伸相连
        private void ExecuteExtent(double aDepth0, double bDepth0, string aStopDepthName, string bStopDepthName, double cavbStopDepth, double adia)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;
            
            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double aStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_UCSa.XYPlane);
            cavbWorkAxis.Delete();
            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double bStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_UCSb.XYPlane);
            cavaWorkAxis.Delete();

            //改变孔着色曲面的长度
            string aPortDepthExp1 = "";
            GetCavDataFromAccess(m_aCav, m_filepath, m_filename, "PortDepth1", ref aPortDepthExp1);
            double aPortDepth = GetValueFromExpression(aPortDepthExp1);            

            string bPortDepthExp1 = "";
            GetCavDataFromAccess(m_bCav, m_filepath, m_filename, "PortDepth1", ref bPortDepthExp1);
            double bPortDepth = GetValueFromExpression(bPortDepthExp1);

            if (aStopDepth - aDepth0 - aPortDepth > 0 && bStopDepth - bDepth0 - bPortDepth > 0)
            {
                oPartCompDef.Features.ExtrudeFeatures[m_aCav.Name + "-1"].Definition.SetDistanceExtent(aStopDepth - aDepth0 - aPortDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);
                oPartCompDef.Features.ExtrudeFeatures[m_bCav.Name + "-1"].Definition.SetDistanceExtent(bStopDepth - bDepth0 - bPortDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);

                EditCavLength(m_aCav, m_aInterName, aStopDepthName, (aStopDepth - aDepth0));
                EditCavLength(m_bCav, m_bInterName, bStopDepthName, (bStopDepth - bDepth0));
                oPartDoc.Update2();
            }
            else
            {                              
                ExecuteCenterConnect(cavbStopDepth, aDepth0, adia, bDepth0);                
            }
        }

        //____________________________________________两个板式阀通油孔延伸相连
        private void ExecuteExtent(string aPortname, string bPortname, string aPortDepthName, string bPortDepthName)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double aStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_UCSa.XYPlane);
            cavbWorkAxis.Delete();
            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double bStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_UCSb.XYPlane);
            cavaWorkAxis.Delete();

            oPartCompDef.Features.ExtrudeFeatures[m_aCav.Name + "-" + aPortname].Definition.SetDistanceExtent(aStopDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);
            oPartCompDef.Features.ExtrudeFeatures[m_bCav.Name + "-" + bPortname].Definition.SetDistanceExtent(bStopDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);

            EditCavLength(m_aCav, m_aInterName, aPortDepthName, aStopDepth);
            EditCavLength(m_bCav, m_bInterName, bPortDepthName, bStopDepth);
            oPartDoc.Update2();
        }


        //____________________________________________一个单油孔a和一个板式阀b通油孔延伸相连
        private void ExecuteExtent(double aDepth0, string aStopDepthName, string bPortname, string bPortDepthName, double cavbStopDepth)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double aStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_UCSa.XYPlane);
            cavbWorkAxis.Delete();
            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double bStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_UCSb.XYPlane);
            cavaWorkAxis.Delete();

            string aPortDepthExp1 = "";
            GetCavDataFromAccess(m_aCav, m_filepath, m_filename, "PortDepth1", ref aPortDepthExp1);
            double aPortDepth = GetValueFromExpression(aPortDepthExp1);

            if (aStopDepth - aDepth0 - aPortDepth > 0)
            {
                oPartCompDef.Features.ExtrudeFeatures[m_aCav.Name + "-1"].Definition.SetDistanceExtent(aStopDepth - aDepth0 - aPortDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);
                oPartCompDef.Features.ExtrudeFeatures[m_bCav.Name + "-" + bPortname].Definition.SetDistanceExtent(bStopDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);

                EditCavLength(m_aCav, m_aInterName, aStopDepthName, (aStopDepth - aDepth0));
                EditCavLength(m_bCav, m_bInterName, bPortDepthName, bStopDepth);
                oPartDoc.Update2();
            }
            else
            {
                ExecuteCenterConnect(cavbStopDepth, aDepth0);
            }
        }

        //____________________________________________板式阀a通油孔和单油孔b延伸相连
        private void ExecuteExtent(string aPortname, string aPortDepthName, double bDepth0, string bStopDepthName, double cavbStopDepth, double adia)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double aStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_UCSa.XYPlane);
            cavbWorkAxis.Delete();
            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double bStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_UCSb.XYPlane);
            cavaWorkAxis.Delete();

            string bPortDepthExp1 = "";
            GetCavDataFromAccess(m_bCav, m_filepath, m_filename, "PortDepth1", ref bPortDepthExp1);
            double bPortDepth = GetValueFromExpression(bPortDepthExp1);

            if (bStopDepth - bDepth0 - bPortDepth > 0)
            {
                oPartCompDef.Features.ExtrudeFeatures[m_aCav.Name + "-" + aPortname].Definition.SetDistanceExtent(aStopDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);
                oPartCompDef.Features.ExtrudeFeatures[m_bCav.Name + "-1"].Definition.SetDistanceExtent(bStopDepth - bDepth0 - bPortDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);

                EditCavLength(m_aCav, m_aInterName, aPortDepthName, aStopDepth);
                EditCavLength(m_bCav, m_bInterName, bStopDepthName, (bStopDepth - bDepth0));
                oPartDoc.Update2();
            }
            else
            {
                ExecuteCenterConnect(cavbStopDepth, adia, bDepth0);
            }
        }
        #endregion

        #region 孔a移动与孔b中心对齐连接重载函数
        //____________________________________________单油孔移动并连接
        private void ExecuteCenterConnect(double cavbStopDepth, double aDepth0, double adia, double bDepth0)
        {
            int coefficient1 = 0;
            int coefficient2 = 0;
            string modifyParaName1 = "";
            string modifyParaName2 = "";

            SetConnectPara(ref coefficient1, ref coefficient2, ref modifyParaName1, ref modifyParaName2);

            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double aStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_UCSa.XYPlane);
            double bToRefDis = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_judgeRefPlane);
            cavbWorkAxis.Delete();

            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double aToBXYDis = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_UCSb.XYPlane);
            double aToRefDis = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_judgeRefPlane);
            cavaWorkAxis.Delete();

            double bportDepth = 0;
            double bportDia = 0;
            if (m_bPortIndex == 1)
            {
                bportDepth = cavbStopDepth + bDepth0;
            }
            else
            {
                string dBReturnDep = "";
                GetCavDataFromAccess(m_bCav, m_filepath, m_filename, "PortDepth" + m_bPortIndex.ToString(), ref dBReturnDep);
                double returnDep = GetValueFromExpression(dBReturnDep);
                bportDepth = returnDep + bDepth0;

                string dBReturnDia = "";
                GetCavDataFromAccess(m_bCav, m_filepath, m_filename, "PortDia" + m_bPortIndex.ToString(), ref dBReturnDia);
                bportDia = GetValueFromExpression(dBReturnDia);             
            }

            //改变a孔着色曲面的长度
            string aPortDepthExp1 = "";
            GetCavDataFromAccess(m_aCav, m_filepath, m_filename, "PortDepth1", ref aPortDepthExp1);
            double aPortDepth = GetValueFromExpression(aPortDepthExp1);

            double moveDis1 = aToBXYDis - bportDepth;
            double moveDis2 = bToRefDis - aToRefDis;

            if (aStopDepth - aDepth0 - aPortDepth > 0)
            {
                oPartCompDef.Features.ExtrudeFeatures[m_aCav.Name + "-1"].Definition.SetDistanceExtent(aStopDepth - aDepth0 - aPortDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);
                EditCavLocaAndDep(m_aCav, m_aInterName, aStopDepth - aDepth0, "Depth8", modifyParaName1, coefficient1, moveDis1, modifyParaName2, coefficient2, moveDis2);
                oPartDoc.Update2();
            }
            else
            {
                //只移动位置，不改变长度
                EditCavLocaAndDep(m_aCav, m_aInterName, 0, "nochange", modifyParaName1, coefficient1, moveDis1, modifyParaName2, coefficient2, moveDis2);
                oPartDoc.Update2();
            }

            if (m_aFootPrint == "Yes")
            {
                if (m_connctType == 1 || m_connctType == 2)
                {
                    MoveSketch(m_aCav, coefficient1 * moveDis1, coefficient2 * moveDis2);
                }
                else
                {
                    MoveSketch(m_aCav, coefficient2 * moveDis2, coefficient1 * moveDis1);
                }
            }

            if (m_bPortIndex != 1 && adia > bportDia)
            {
                EditCavDia(m_aCav, m_aInterName, "Dia8", bportDia);
            }
            oPartDoc.Update2();               
        }

        //板a和孔b
        private void ExecuteCenterConnect(double cavbStopDepth, double adia, double bDepth0)
        {
            int coefficient1 = 0;
            int coefficient2 = 0;
            string modifyParaName1 = "";
            string modifyParaName2 = "";

            SetConnectPara(ref coefficient1, ref coefficient2, ref modifyParaName1, ref modifyParaName2);

            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double aStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_UCSa.XYPlane);
            double bToRefDis = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_judgeRefPlane);
            cavbWorkAxis.Delete();

            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double aToBXYDis = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_UCSb.XYPlane);
            double aToRefDis = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_judgeRefPlane);
            cavaWorkAxis.Delete();

            double bportDepth = 0;
            double bportDia = 0;
            if (m_bPortIndex == 1)
            {
                bportDepth = cavbStopDepth + bDepth0;
            }
            else
            {
                string dBReturnDep = "";
                GetCavDataFromAccess(m_bCav, m_filepath, m_filename, "PortDepth" + m_bPortIndex.ToString(), ref dBReturnDep);
                double returnDep = GetValueFromExpression(dBReturnDep);
                bportDepth = returnDep + bDepth0;

                string dBReturnDia = "";
                GetCavDataFromAccess(m_bCav, m_filepath, m_filename, "PortDia" + m_bPortIndex.ToString(), ref dBReturnDia);
                bportDia = GetValueFromExpression(dBReturnDia);
            }

            AttributeSets aAttributeSets;
            aAttributeSets = m_aSurf.AttributeSets;
            AttributeSet aAttributeSet;
            aAttributeSet = aAttributeSets["MyAttribSet"];
            string aportName = aAttributeSet["PortName"].Value;
            //改变a孔着色曲面的长度            
            oPartCompDef.Features.ExtrudeFeatures[m_aCav.Name + "-" + aportName].Definition.SetDistanceExtent(aStopDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);

            double moveDis1 = aToBXYDis - bportDepth;
            double moveDis2 = bToRefDis - aToRefDis;
            EditCavLocaAndDep(m_aCav, m_aInterName, aStopDepth, aportName + "depth", modifyParaName1, coefficient1, moveDis1, modifyParaName2, coefficient2, moveDis2);
            oPartDoc.Update2();

            if (m_aFootPrint == "Yes")
            {
                if (m_connctType == 1 || m_connctType == 2)
                {
                    MoveSketch(m_aCav, coefficient1 * moveDis1, coefficient2 * moveDis2);
                }
                else
                {
                    MoveSketch(m_aCav, coefficient2 * moveDis2, coefficient1 * moveDis1);
                }
            }

            if (m_bPortIndex != 1 && adia > bportDia)
            {
                EditCavDia(m_aCav, m_aInterName, aportName + "dmax", bportDia);
            }
            oPartDoc.Update2(); 
        }

        //孔a和板b
        private void ExecuteCenterConnect(double cavbStopDepth, double aDepth0)
        {
            int coefficient1 = 0;
            int coefficient2 = 0;
            string modifyParaName1 = "";
            string modifyParaName2 = "";

            SetConnectPara(ref coefficient1, ref coefficient2, ref modifyParaName1, ref modifyParaName2);

            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double aStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_UCSa.XYPlane);
            double bToRefDis = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_judgeRefPlane);
            cavbWorkAxis.Delete();

            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double aToBXYDis = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_UCSb.XYPlane);
            double aToRefDis = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_judgeRefPlane);
            cavaWorkAxis.Delete();

            double bportDepth = cavbStopDepth;
         //   double bportDia = 0;

            //改变a孔着色曲面的长度
            string aPortDepthExp1 = "";
            GetCavDataFromAccess(m_aCav, m_filepath, m_filename, "PortDepth1", ref aPortDepthExp1);
            double aPortDepth = GetValueFromExpression(aPortDepthExp1);

            double moveDis1 = aToBXYDis - bportDepth;
            double moveDis2 = bToRefDis - aToRefDis;

            if (aStopDepth - aDepth0 - aPortDepth > 0)
            {
                oPartCompDef.Features.ExtrudeFeatures[m_aCav.Name + "-1"].Definition.SetDistanceExtent(aStopDepth - aDepth0 - aPortDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);
                EditCavLocaAndDep(m_aCav, m_aInterName, aStopDepth - aDepth0, "Depth8", modifyParaName1, coefficient1, moveDis1, modifyParaName2, coefficient2, moveDis2);
                oPartDoc.Update2();
            }
            else
            {
                //该句代码只改变位置，不改变长度
                EditCavLocaAndDep(m_aCav, m_aInterName, 0, "nochange", modifyParaName1, coefficient1, moveDis1, modifyParaName2, coefficient2, moveDis2);
                oPartDoc.Update2();
            }

            if (m_aFootPrint == "Yes")
            {
                if (m_connctType == 1 || m_connctType == 2)
                {
                    MoveSketch(m_aCav, coefficient1 * moveDis1, coefficient2 * moveDis2);
                }
                else
                {
                    MoveSketch(m_aCav, coefficient2 * moveDis2, coefficient1 * moveDis1);
                }
            }            
            oPartDoc.Update2();   
        }

        //两板式阀通油孔a b相连
        private void ExecuteCenterConnect(double cavbStopDepth)
        {
            int coefficient1 = 0;
            int coefficient2 = 0;
            string modifyParaName1 = "";
            string modifyParaName2 = "";

            SetConnectPara(ref coefficient1, ref coefficient2, ref modifyParaName1, ref modifyParaName2);

            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double aStopDepth = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_UCSa.XYPlane);
            double bToRefDis = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_judgeRefPlane);
            cavbWorkAxis.Delete();

            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double aToBXYDis = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_UCSb.XYPlane);
            double aToRefDis = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_judgeRefPlane);
            cavaWorkAxis.Delete();

            double bportDepth = cavbStopDepth;

            AttributeSets aAttributeSets;
            aAttributeSets = m_aSurf.AttributeSets;
            AttributeSet aAttributeSet;
            aAttributeSet = aAttributeSets["MyAttribSet"];
            string aportName = aAttributeSet["PortName"].Value;

            //改变a孔着色曲面的长度            
            oPartCompDef.Features.ExtrudeFeatures[m_aCav.Name + "-" + aportName].Definition.SetDistanceExtent(aStopDepth, PartFeatureExtentDirectionEnum.kNegativeExtentDirection);

            double moveDis1 = aToBXYDis - bportDepth;
            double moveDis2 = bToRefDis - aToRefDis;
            EditCavLocaAndDep(m_aCav, m_aInterName, aStopDepth, aportName + "depth", modifyParaName1, coefficient1, moveDis1, modifyParaName2, coefficient2, moveDis2);
            oPartDoc.Update2();

            if (m_aFootPrint == "Yes")
            {
                if (m_connctType == 1 || m_connctType == 2)
                {
                    MoveSketch(m_aCav, coefficient1 * moveDis1, coefficient2 * moveDis2);
                }
                else
                {
                    MoveSketch(m_aCav, coefficient2 * moveDis2, coefficient1 * moveDis1);
                }
            }
        }
        #endregion

        private void SetConnectPara(ref int coeff1, ref int coeff2, ref string modifyParaname1, ref string modifyParaname2)
        {           
            switch (m_connctType)
            {
                case 1:
                    coeff1 = 1;
                    coeff2 = 1;
                    modifyParaname1 = "y";
                    modifyParaname2 = "x";
                    break;
                case 2:
                    coeff1 = -1;
                    coeff2 = 1;
                    modifyParaname1 = "y";
                    modifyParaname2 = "x";
                    break;
                case 3:
                    coeff1 = 1;
                    coeff2 = 1;
                    modifyParaname1 = "x";
                    modifyParaname2 = "y";
                    break;
                case 4:
                    coeff1 = -1;
                    coeff2 = 1;
                    modifyParaname1 = "x";
                    modifyParaname2 = "y";
                    break;
            }
        }

        //获取工作轴到工作平面的距离，返回值为双精度
        private double GetDistanceBetwLineAndFace(PartComponentDefinition partCompDef, WorkAxis workAxis, WorkPlane workPlane)
        {
            NameValueMap cont;
            cont = m_inventorApplication.TransientObjects.CreateNameValueMap();

            Double oDistance;
            oDistance = m_inventorApplication.MeasureTools.GetMinimumDistance(workAxis, workPlane, InferredTypeEnum.kNoInference, InferredTypeEnum.kNoInference, cont);

            return oDistance;
        }

        //获取两孔的iFeature的intername
        private void GetiFeatureIntNameAndIndName()
        {
            try
            {
                AttributeSets oAttributeSets;
                oAttributeSets = m_aCav.AttributeSets;

                AttributeSet oAttibSet;
                oAttibSet = m_aCav.AttributeSets["MyAttribSet"];

                m_aInterName = (string)oAttibSet["InternalName"].Value;
                m_aIndexName = (string)oAttibSet["IndexName"].Value;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取孔a属性集Intername失败！  " + ex.ToString());
                return;
            }

            try
            {
                AttributeSets oAttributeSets;
                oAttributeSets = m_bCav.AttributeSets;

                AttributeSet oAttibSet;
                oAttibSet = m_bCav.AttributeSets["MyAttribSet"];

                m_bInterName = (string)oAttibSet["InternalName"].Value;
                m_bIndexName = (string)oAttibSet["IndexName"].Value;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取孔b属性集Intername失败！  " + ex.ToString());
                return;
            }
        }

        private void GetBothCavType(string aCavIndName, string bCavIndName)
        {
            ConnectToAccess connectToaccess = new ConnectToAccess(m_filepath, m_filename);

            string sqla = @"select * from ComponentsDb where ComponentsDb.IndexName='" + aCavIndName + "'";
            m_aCavType = connectToaccess.GetSingleInformation(sqla, "CavityType");
            m_aFootPrint = connectToaccess.GetSingleInformation(sqla, "Footprint");

            string sqlb = @"select * from ComponentsDb where ComponentsDb.IndexName='" + bCavIndName + "'";
            m_bCavType = connectToaccess.GetSingleInformation(sqlb, "CavityType");
            m_bFootPrint = connectToaccess.GetSingleInformation(sqlb, "Footprint");
        }

        //获取连接两孔的局部坐标系
        private void GetBothUCS(int UCSaIndex, int UCSbIndex)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            if (UCSaIndex != 0 && UCSbIndex != 0)
            {
                m_UCSa = oPartCompDef.UserCoordinateSystems[UCSaIndex];
                m_UCSb = oPartCompDef.UserCoordinateSystems[UCSbIndex];
            }
            else
            {
                 MessageBox.Show("局部用户坐标系获取发生失败！");
                return;
            }
        }

        //获取孔当前的Depth0、portDepth以及Dia8的值
        private void GetCavData(iFeature caviFeature, string ifeatureInterName,string depthName, string diaName, ref double depth0, ref double stopDepth, ref double dia)
        {
            //bool flag1 = false;
            //bool flag2 = false;
            //bool flag3 = false;

            foreach (iFeatureInput oInput in caviFeature.iFeatureDefinition.iFeatureInputs)
            {                
                if (oInput.Name.Contains(ifeatureInterName + ":Depth0"))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    depth0 = oiFeatureParaInput.Parameter.Value;
                    //flag1 = true;
                }
                if (oInput.Name.Contains(ifeatureInterName + ":"+ depthName))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    stopDepth = oiFeatureParaInput.Parameter.Value;
                    //flag2 = true;
                }
                if (oInput.Name.Contains(ifeatureInterName + ":" + diaName))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    dia = oiFeatureParaInput.Parameter.Value;
                    //flag3 = true;
                }
                //if (flag1 && flag2 && flag3)
                //    break;              
            }
        }
       
        //获取孔当前的Depth0和Dia8的值
        private void GetCavData(iFeature caviFeature, string ifeatureInterName,string diaName, ref double depth0, ref double dia)
        {
            //bool flag1 = false;
            //bool flag2 = false;
            foreach (iFeatureInput oInput in caviFeature.iFeatureDefinition.iFeatureInputs)
            {
                if (oInput.Name.Contains(ifeatureInterName + ":Depth0"))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    depth0 = oiFeatureParaInput.Parameter.Value;
                    //flag1 = true;
                }

                if (oInput.Name.Contains(ifeatureInterName + ":" + diaName))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    dia = oiFeatureParaInput.Parameter.Value;
                    //flag2 = true;
                }
                //if (flag1 && flag2)
                //    break;              
            }
        }

        //从数据库读取数值
        private void GetCavDataFromAccess(iFeature ifeature, string filepath, string filename, string inquireDataname, ref string returnValue)
        {
            string codename;
            string indexname;
            string codenumber;
            try
            {
                AttributeSets oAttributeSets;
                oAttributeSets = ifeature.AttributeSets;

                AttributeSet oAttibSet;
                oAttibSet = ifeature.AttributeSets["MyAttribSet"];

                codename = (string)oAttibSet["CodeName"].Value;
                indexname = (string)oAttibSet["IndexName"].Value;
                codenumber = (string)oAttibSet["CodeNumber"].Value;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取属性集失败！  " + ex.ToString());
                return;
            }

            ConnectToAccess connectToaccess = new ConnectToAccess(filepath, filename, codename, indexname, codenumber);
            try
            {
                returnValue = connectToaccess.SelectConnectToAccess(inquireDataname);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("从数据库获取数据失败！" + ex.ToString());
                return;
            }
        }

        //修改孔stopDepth值
        private void EditCavLength(iFeature caviFeature, string iFeatureInterName, string stopDepthName, double newStopDepth)
        {
            foreach (iFeatureInput oInput in caviFeature.iFeatureDefinition.iFeatureInputs)
            {
                if (oInput.Name == iFeatureInterName + ":" + stopDepthName)
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    oiFeatureParaInput.Parameter.Value = newStopDepth;
                    break;
                }
            }
        }

        //修改孔最后一阶的直径（Dia）
        private void EditCavDia(iFeature caviFeature, string iFeatureInterName, string diaName, double newDia)
        {
            foreach (iFeatureInput oInput in caviFeature.iFeatureDefinition.iFeatureInputs)
            {
                if (oInput.Name.Contains(iFeatureInterName + ":" + diaName))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    oiFeatureParaInput.Parameter.Value = newDia;
                    break;
                }
            }
        }

        //修改孔位置和stopDepth信息(差值方式)
        private void EditCavLocaAndDep(iFeature caviFeature, string iFeatureInterName, double newStopDepth,string depthName, string locaName1,int coeff1, double newValue1, string locaName2,int coeff2, double newValue2)
        {
           foreach (iFeatureInput oInput in caviFeature.iFeatureDefinition.iFeatureInputs)
            {
                if (oInput.Name.Contains(iFeatureInterName + ":" + depthName))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    oiFeatureParaInput.Parameter.Value = newStopDepth;
                }
                if (oInput.Name.Contains(iFeatureInterName + ":" + locaName1))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    double value = oiFeatureParaInput.Parameter.Value + coeff1 * newValue1;
                    oiFeatureParaInput.Parameter.Value = value;
                }
                if (oInput.Name.Contains(iFeatureInterName + ":" + locaName2))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    double value = oiFeatureParaInput.Parameter.Value + coeff2 * newValue2;
                    oiFeatureParaInput.Parameter.Value = value;
                }
               
            }
        }

        //修改孔位置和stopDepth信息（直接赋值方式）
        private void EditCavLocaAndDep(iFeature caviFeature, string iFeatureInterName, double newStopDepth, double newToX, double newToY)
        {
            foreach (iFeatureInput oInput in caviFeature.iFeatureDefinition.iFeatureInputs)
            {
                if (oInput.Name.Contains(iFeatureInterName + ":Depth8"))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    oiFeatureParaInput.Parameter.Value = newStopDepth;
                }
                if (oInput.Name.Contains(iFeatureInterName + ":y"))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    oiFeatureParaInput.Parameter.Value = newToX;
                }
                if (oInput.Name.Contains(iFeatureInterName + ":x"))
                {
                    iFeatureParameterInput oiFeatureParaInput;
                    oiFeatureParaInput = (iFeatureParameterInput)oInput;
                    oiFeatureParaInput.Parameter.Value = newToY;
                }
            }
        }

        private void GetUCSIndex(ExtrudeFeature surfaceFeature, ref int i)//用于判断选择的面特征的用户坐标系
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            int portIndex;
            try
            {
                AttributeSets surfAttributeSets;
                surfAttributeSets = surfaceFeature.AttributeSets;

                AttributeSet surfAttibSet;
                surfAttibSet = surfaceFeature.AttributeSets["MyAttribSet"];

                portIndex = (int)surfAttibSet["PortIndex"].Value;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取属性集失败！  " + ex.ToString());
                return;
            }

            #region Use "Line.Direction" to get the UCS
            WorkAxis workAxis;
            workAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(surfaceFeature.Faces[1], true);
            Line oAxisLine;
            oAxisLine = workAxis.Line;
            UnitVector oAxisVector;
            oAxisVector = oAxisLine.Direction;

            Double[] coords = new Double[3];
            oAxisVector.GetUnitVectorData(ref coords);

            string strCoords;
            if (portIndex == 1)
            {
                strCoords = (-coords[0]).ToString() + (-coords[1]).ToString() + (-coords[2]).ToString();
            }
            else
            {
                strCoords = coords[0].ToString() + coords[1].ToString() + coords[2].ToString();
            }

            switch (strCoords)
            {
                case "001":
                    i = 1;
                    break;
                case "0-10":
                    i = 2;
                    break;
                case "100":
                    i = 3;
                    break;
                case "010":
                    i = 4;
                    break;
                case "-100":
                    i = 5;
                    break;
                case "00-1":
                    i = 6;
                    break;
            }
            #endregion
        }


        //设置UCSa和UCSb，并确定连接的类型
        private void Classification()
        {
            int UCSaIndex = 0;      //获取孔a所在平面索引号
            GetUCSIndex(m_aSurf, ref UCSaIndex);
            
            int UCSbIndex = 0;      //获取孔b所在平面索引号
            GetUCSIndex(m_bSurf, ref UCSbIndex);

            //获取孔a和孔b所在平面用户坐标系
            GetBothUCS(UCSaIndex, UCSbIndex);

            string oType = UCSaIndex.ToString() + UCSbIndex.ToString();
            switch (oType) 
            {
                case "12":
                case "62":
                case "26":
                case "56":
                case "34":
                    m_connctType = 1;
                    break;
                case "21":
                case "14":
                case "41":
                case "51":
                case "64":
                case "46":
                case "32":
                    m_connctType = 2;
                    break;
                case "13":
                case "31":
                case "65":
                case "23":
                case "45":
                case "52":
                    m_connctType = 3;
                    break;                
                case "15":
                case "63":
                case "36":
                case "43":
                case "54":
                case "25":
                    m_connctType = 4;
                    break;
            }
        }

        private void SetJudgeRefPlane()
        {
            switch (m_connctType)
            {
                case 1:
                    m_judgeRefPlane = m_UCSa.YZPlane;
                    break;
                case 2:
                    m_judgeRefPlane = m_UCSa.YZPlane;
                    break;
                case 3:
                    m_judgeRefPlane = m_UCSa.XZPlane;
                    break;
                case 4:
                    m_judgeRefPlane = m_UCSa.XZPlane;
                    break;
            }
        }

        //判断两孔延伸连接或是移动连接。返回值为true时直接延伸相连，返回值为false时移动孔a并连接。
        //两孔判断的依据是两孔延伸相交后重叠部分的距离是否大于直径较小孔的20%
        private bool DeterminExtentOrMove(double cavadia, double cavbdia)
        {
            bool flag = false;

            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            WorkAxis cavaWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_aSurf.Faces[1], false);
            double aDistance = GetDistanceBetwLineAndFace(oPartCompDef, cavaWorkAxis, m_judgeRefPlane);
            cavaWorkAxis.Delete();
            WorkAxis cavbWorkAxis = oPartCompDef.WorkAxes.AddByRevolvedFace(m_bSurf.Faces[1], false);
            double bDistance = GetDistanceBetwLineAndFace(oPartCompDef, cavbWorkAxis, m_judgeRefPlane);
            cavbWorkAxis.Delete();

            //获取较小直径
            double refDia = 0;
            if (cavadia <= cavbdia)
            {
                refDia = cavadia;
            }
            else
            {
                refDia = cavbdia;
            }

            //判断是否大于20%
            if (((cavadia + cavbdia) / 2 - System.Math.Abs(aDistance - bDistance)) / refDia >= 0.2)
            {
                flag = true;
            }
            else 
            {
                flag = false;
            }
            return flag;
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

        //用于移动footprint草图
        public void MoveSketch(iFeature ifeature, double yoffset, double xoffset)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveEditObject;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            PlanarSketch oSketch = oPartCompDef.Sketches["Footprint" + ifeature.Name];

            Vector2d ovector = m_inventorApplication.TransientGeometry.CreateVector2d(yoffset, xoffset);

            ObjectCollection oSketchObjects;
            oSketchObjects = m_inventorApplication.TransientObjects.CreateObjectCollection();

            foreach (SketchEntity oSketchEntity in oSketch.SketchEntities)
            {
                oSketchObjects.Add(oSketchEntity);
            }
            oSketch.MoveSketchObjects(oSketchObjects, ovector);
        }

        //设置当前数据库路径及数据库名称
        private void SetFilepathAndName()
        {
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName+"\\CavityLibrary";
            m_filepath = deFaultpath;
            m_filename = "CavityLibrary";
        }
    }
}
