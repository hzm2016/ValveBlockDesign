using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;

namespace ValveBlockDesign
{
    internal class ChangeRequest
    {
        private ValveBlockDesign.ChangeProcessor m_changeProcessor;

        public ChangeRequest()
        {
            m_changeProcessor = null;
        }

        //OnExecute :the Execute Code
        virtual public void OnExecute(Document document, NameValueMap context, bool succeeded)
        {
            //
        }

        //initial the changeProcessor
        public void Execute(Application application, object changeDefinition, Inventor._Document document)
        {
            //create instance of ChangeProcessor class
            m_changeProcessor = new ValveBlockDesign.ChangeProcessor();

            //set the parent to get the call back when change processor terminates
            m_changeProcessor.SetParentRequest(this);

            //connet change processor
            m_changeProcessor.Connect(application, changeDefinition, document);
        }

        public void Terminate()
        {
            //disconnect change processor
            if (m_changeProcessor != null)
            {
                m_changeProcessor.Disconnect();

                m_changeProcessor = null;
            }
        }
    }
}
