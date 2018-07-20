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
    internal class DeleteRequest:ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;
        private iFeature m_selectiFeature;
        private bool m_outlineDelete;
        private string [] m_deleteName;
        private ConnectToAccess m_connectToaccess;
        private string CavityType;
        private System.Reflection.Assembly assembly;
        private string deFaultpath;
        public DeleteRequest(Inventor.Application application, iFeature ifeature,bool outline,string []name)
        {
            m_inventorApplication = application;
            m_selectiFeature = ifeature;
            m_outlineDelete = outline;
            m_deleteName = new string [20];
            m_deleteName = name;
            assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileInfo asmFile = new FileInfo(assembly.Location);
            deFaultpath = asmFile.DirectoryName;
            
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)document;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            int i = 0;
            while(i<20)
            {
                if (m_deleteName[i] != null)
                {
                    m_selectiFeature = oPartCompDef.Features.iFeatures[m_deleteName[i]];
                    AttributeSets atr = m_selectiFeature.AttributeSets;

                    AttributeSet abs = atr["MyAttribSet"];
                    
                    Inventor.Attribute att = abs["InternalName"];
                    Inventor.Attribute footprint = abs["Footprint"];
                    string footprintCheck = footprint.Value;
                    Inventor.Attribute indexAttribute = abs["IndexName"];
                    string indexName = indexAttribute.Value;
                    Inventor.Attribute codeNameAttribute = abs["CodeName"];
                    string codeName = codeNameAttribute.Value;
                    Inventor.Attribute codeNumberAttribute = abs["CodeNumber"];
                    string codeNumber = codeNumberAttribute.Value;
                    m_connectToaccess = new ConnectToAccess(deFaultpath + "\\CavityLibrary", "CavityLibrary", codeName, indexName, codeNumber);
                    string sql = @"select * from ComponentsDb where ComponentsDb.IndexName='"+indexName+"'";
                    CavityType = m_connectToaccess.GetSingleInformation(sql, "CavityType");
                    string num;
                    if (footprintCheck == "Yes" && CavityType == "板式阀通油孔")
                    {
                        PlanarSketch oSketchFootprint = oPartCompDef.Sketches["Footprint" + m_selectiFeature.Name];
                        oSketchFootprint.Delete();
                        AttributeSet port = atr["MyPorts"];
                        Inventor.Attribute portNumber = port["PortsCount"];
                        int portcount = portNumber.Value;
                        int j = 1;
                        while (j <= portcount)
                        {
                            string portName = m_connectToaccess.SelectConnectToAccess("PortName" + j.ToString());
                            oPartCompDef.Features.ExtrudeFeatures[m_selectiFeature.Name + "-" + portName].Delete();
                            j++;
                        }
                    }
                    else if (footprintCheck == "Yes" && CavityType == "二通插装孔")
                    {
                        AttributeSet port = atr["MyPorts"];
                        Inventor.Attribute portNumber = port["MyPortsNumber"];
                        int portnum = portNumber.Value;
                        int j = 1;
                        while (j <= portnum)
                        {
                            num = (j).ToString();
                            oPartCompDef.Features.ExtrudeFeatures[m_selectiFeature.Name + "-" + num].Delete();
                            j++;
                        }
                        PlanarSketch oSketchFootprint = oPartCompDef.Sketches["Footprint" + m_selectiFeature.Name];
                        oSketchFootprint.Delete();
                        Inventor.Attribute portCountAttribute = port["PortsCount"];
                        int portCount = portCountAttribute.Value;
                        j = 1;
                        while (j <= portCount)
                        {
                            string portName = m_connectToaccess.SelectConnectToAccess("PortName" + j.ToString());
                            oPartCompDef.Features.ExtrudeFeatures[m_selectiFeature.Name + "-" + portName].Delete();
                            j++;
                        }
                    }
                    else if ((footprintCheck == "No" && CavityType == "螺纹孔") || (footprintCheck == "No" && CavityType == "工艺油孔"))
                    {
                        oPartCompDef.Features.ExtrudeFeatures[m_selectiFeature.Name + "-1"].Delete();
                    }
                    else
                    {
                        AttributeSet port = atr["MyPorts"];
                        Inventor.Attribute portNumber = port["MyPortsNumber"];
                        int portnum = portNumber.Value;
                        int j = 1;
                        while (j <= portnum)
                        {
                            num = (j).ToString();
                            oPartCompDef.Features.ExtrudeFeatures[m_selectiFeature.Name + "-" + num].Delete();
                            j++;
                        }
                    }

                    m_selectiFeature.Delete();
                    
                    if (m_outlineDelete)
                    {
                        PlanarSketch oSketchOutline = oPartCompDef.Sketches["Footprint" + m_selectiFeature.Name];
                        oSketchOutline.Delete();
                    }
                }
                i++;
            }
            MessageBox.Show("元件删除成功");
        }
    }
}
