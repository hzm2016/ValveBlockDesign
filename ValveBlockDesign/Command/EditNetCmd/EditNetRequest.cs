using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class EditNetRequest:ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;

        private string  m_selectportName;
        private iFeature m_selectiFeature;
        private string m_netName;

        public EditNetRequest(Inventor.Application application,iFeature ifeature,string portName,string netName)
        {
            m_selectiFeature = ifeature;
            m_inventorApplication = application;
            m_selectportName = portName;
            m_netName = netName;
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            PartDocument oPartDoc = (PartDocument)m_inventorApplication.ActiveDocument;
            SelectSet oSelectSet = oPartDoc.SelectSet;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;
            try
            {
                int Portname = int.Parse(m_selectportName);
                string portEditName;
                portEditName = m_selectiFeature.Name + "-" + m_selectportName;
                ExtrudeFeature portFaceExtru;
                portFaceExtru = oPartCompDef.Features.ExtrudeFeatures[portEditName];
                Asset asset = null;
                foreach (Asset asset1 in oPartDoc.Assets)
                {
                    if (asset1.DisplayName == m_netName)
                        asset = asset1;
                }
                portFaceExtru.Appearance = asset;
                string coneEditName;
                if (m_selectportName == "1")
                {
                    coneEditName = m_selectiFeature.Name + "-" + m_selectportName + "cone";
                    ExtrudeFeature coneFaceExtru;
                    coneFaceExtru = oPartCompDef.Features.ExtrudeFeatures[coneEditName];
                    coneFaceExtru.Appearance = asset;
                }
                AttributeSets Ports = m_selectiFeature.AttributeSets;
                AttributeSet myPorts = Ports["MyPorts"];
                Inventor.Attribute Port = myPorts["Port" + m_selectportName];
                Port.Value = m_netName;
            }
            catch
            {
                string portEditName;
                portEditName = m_selectiFeature.Name + "-" + m_selectportName;
                ExtrudeFeature portFaceExtru;
                portFaceExtru = oPartCompDef.Features.ExtrudeFeatures[portEditName];
                Asset asset = null;
                foreach (Asset asset1 in oPartDoc.Assets)
                {
                    if (asset1.DisplayName == m_netName)
                        asset = asset1;
                }
                portFaceExtru.Appearance = asset;
                string coneEditName;
                coneEditName = m_selectiFeature.Name + "-" + m_selectportName + "cone";
                ExtrudeFeature coneFaceExtru;
                coneFaceExtru = oPartCompDef.Features.ExtrudeFeatures[coneEditName];
                coneFaceExtru.Appearance = asset;

                AttributeSets Ports = m_selectiFeature.AttributeSets;
                AttributeSet myPorts = Ports["MyPorts"];
                Inventor.Attribute Port = myPorts[m_selectportName];
                Port.Value = m_netName;
            }
            
        }
    }
}
