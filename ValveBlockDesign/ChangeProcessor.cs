using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;

namespace ValveBlockDesign
{
     internal class ChangeProcessor
    {
         private Inventor.ChangeProcessor m_changeProcessor;

         //parent ChangeRequest object
         private ValveBlockDesign.ChangeRequest m_parentRequest;

         private ChangeProcessorSink_OnExecuteEventHandler m_changeProcessor_OnExecute_Delegate;
         private ChangeProcessorSink_OnTerminateEventHandler m_changeProcessor_OnTerminate_Delegate;

         public ChangeProcessor()
         {
             m_changeProcessor = null;
             m_parentRequest = null;
         }

         public void Connect(Application application, object changeDefinition, Inventor._Document document)
         {
             //get the change manager object
             ChangeManager oChangeManager = application.ChangeManager;

             //get the change definitions collection for this AddIn
             ChangeDefinitions oChangeDefinitions = oChangeManager["{c29d5be2-c9f7-4783-9191-5070d4944568}"];

             //create the change processor associated with the change definition
             m_changeProcessor = oChangeDefinitions[changeDefinition].CreateChangeProcessor();

             //connect event handler in order to receive change processor events
             m_changeProcessor_OnExecute_Delegate = new ChangeProcessorSink_OnExecuteEventHandler(ChangeProcessorEvnets_OnExecute);
             m_changeProcessor.OnExecute += m_changeProcessor_OnExecute_Delegate;

             m_changeProcessor_OnTerminate_Delegate = new ChangeProcessorSink_OnTerminateEventHandler(ChangeProcessorEvnets_OnTerminate);
             m_changeProcessor.OnTerminate += m_changeProcessor_OnTerminate_Delegate;

             //execute the change processor
             m_changeProcessor.Execute(document);
         }

         public void Disconnect()
         {
             //disconnect change processor evnets sink
             if (m_changeProcessor != null)
             {
                 m_changeProcessor.OnExecute -= m_changeProcessor_OnExecute_Delegate;
                 m_changeProcessor.OnTerminate -= m_changeProcessor_OnTerminate_Delegate;

                 m_changeProcessor = null;
             }

         }

         public void SetParentRequest(ValveBlockDesign.ChangeRequest parentRequest)
         {
             //store the parent request object
             m_parentRequest = parentRequest;
         }

        //-----------------------------------------------------------------------------
        //----- Implementation of ChangeProcessor Events sink methods
        //-----------------------------------------------------------------------------
        //-----------------------------------------------------------------------------
         public void ChangeProcessorEvnets_OnExecute(Inventor._Document document, NameValueMap context, ref bool succeeded)
         {
             m_parentRequest.OnExecute(document, context, succeeded);
         }

         public void ChangeProcessorEvnets_OnTerminate()
         {
             m_parentRequest.Terminate();
         }
    }
}
