using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using System.Windows.Forms;

namespace ValveBlockDesign
{
    internal class MoveRequest : ValveBlockDesign.ChangeRequest
    {
        private Inventor.Application m_inventorApplication;

        private Face m_selectFace;
        private iFeature m_selectiFeature;

        private double m_xOffset;
        private double m_yOffset;

        public MoveRequest(Inventor.Application application, Face face, iFeature ifeature, double xOffset, double yOffset)
        {
            m_inventorApplication = application;
            m_selectFace = face;
            m_selectiFeature = ifeature;
            m_xOffset = xOffset;
            m_yOffset = yOffset;
        }

        public override void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)document;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            AttributeSets atr =m_selectiFeature.AttributeSets;

            AttributeSet abs = atr["MyAttribSet"];
            Inventor.Attribute att = abs["InternalName"];
            Inventor.Attribute footprint = abs["Footprint"];
            string name = att.Value;
            string footprintCheck = footprint.Value;
            string thisiFeatureName = name;
            //string oXParamName = thisiFeatureName + ":x";
            //string oYParamName = thisiFeatureName + ":y";

            foreach (iFeatureInput oInput in m_selectiFeature.iFeatureDefinition.iFeatureInputs)
            {
                try
                {
                    if (oInput.Name == thisiFeatureName + ":x" || oInput.Name == thisiFeatureName + ":x:2")
                    {
                        iFeatureParameterInput oParamInput;
                        oParamInput = (iFeatureParameterInput)oInput;
                        Double newValue = oParamInput.Parameter.Value + m_xOffset;
                        oParamInput.Parameter.Value = newValue;
                    }

                    if (oInput.Name == thisiFeatureName + ":y" || oInput.Name == thisiFeatureName + ":y:2")
                    {
                        iFeatureParameterInput oParamInput;
                        oParamInput = (iFeatureParameterInput)oInput;
                        Double newValue = oParamInput.Parameter.Value + m_yOffset;
                        oParamInput.Parameter.Value = newValue;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            if (footprintCheck == "Yes")
            {
                MoveSketch();
            }
            oPartDoc.Update2();
            //MessageBox.Show(m_selectiFeature.Name);
        }

        public void MoveSketch()
        {
            PartDocument oPartDoc;
            oPartDoc = (PartDocument)m_inventorApplication.ActiveEditObject;

            PartComponentDefinition oPartCompDef;
            oPartCompDef = oPartDoc.ComponentDefinition;

            PlanarSketch oSketch = oPartCompDef.Sketches["Footprint"+m_selectiFeature.Name];
            
            Vector2d ovector = m_inventorApplication.TransientGeometry.CreateVector2d(m_yOffset, m_xOffset);

            ObjectCollection oSketchObjects;
            oSketchObjects = m_inventorApplication.TransientObjects.CreateObjectCollection();


            foreach (SketchEntity oSketchEntity in oSketch.SketchEntities)
            {
                oSketchObjects.Add(oSketchEntity);
            }
            oSketch.MoveSketchObjects(oSketchObjects, ovector);
            oSketchObjects.Clear();

            try
            {
                PlanarSketch oSketchOutline = oPartCompDef.Sketches["Outline" + m_selectiFeature.Name];
                foreach (SketchEntity oSketchEntity in oSketchOutline.SketchEntities)
                {
                    oSketchObjects.Add(oSketchEntity);
                }
                oSketchOutline.MoveSketchObjects(oSketchObjects, ovector);
            }
            catch
            {

            }
        }

    }
}
