using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;

namespace ValveBlockDesign
{
    internal abstract class Command
    {
        //Inventor application object
        protected Application m_inventorApplication;

        //button definition object
        protected ButtonDefinition m_buttonDefinition;

        private ButtonDefinitionSink_OnExecuteEventHandler m_buttonDefinition_OnExecute_Delegate;

        //Interaction object
        protected ValveBlockDesign.Interaction m_interaction;

        //InteractionEvents object
        protected InteractionEvents m_interactionEvents;

        //SelectEvents object
        protected SelectEvents m_selectEvents;

        //MouseEvents object
        protected MouseEvents m_mouseEvents;

        //TriadEvents object
        protected TriadEvents m_triadEvents;

        //Command status
        protected bool m_commandIsRunning;

        public Inventor.ButtonDefinition ButtonDefinition
        {
            get
            {
                return m_buttonDefinition;
            }
        }

        public Command()
        {
            m_inventorApplication = null;

            m_buttonDefinition = null;

            m_interaction = null;

            m_commandIsRunning = false;
        }

        public void CreateButton(Application application, string displayName, string internalName, CommandTypesEnum commandType, object clientId, string description, string toolTip, object standardIcon, object largeIcon, ButtonDisplayEnum buttonType, bool autoAddToGUI)
        {
            //store the Inventor application object
            m_inventorApplication = application;

            object oIPictureDispStandard = standardIcon;
            object oIPictureDispLarge = largeIcon;

            //create the button definition
            m_buttonDefinition = m_inventorApplication.CommandManager.ControlDefinitions.AddButtonDefinition(
                displayName,
                internalName,
                commandType,
                clientId,
                description,
                toolTip,
                oIPictureDispStandard,
                oIPictureDispLarge,
                buttonType);

            //connect the button events sink
            m_buttonDefinition_OnExecute_Delegate = new ButtonDefinitionSink_OnExecuteEventHandler(ButtonDefinition_OnExecute);
            m_buttonDefinition.OnExecute += m_buttonDefinition_OnExecute_Delegate;

            //display button by default if specified
            if (autoAddToGUI)
            {
                m_buttonDefinition.AutoAddToGUI();
            }
        }

        public void CreateButtonNoIcon(Application application, string displayName, string internalName, CommandTypesEnum commandType, object clientId, string description, string toolTip, object standardIcon, object largeIcon, ButtonDisplayEnum buttonType, bool autoAddToGUI)
        {
            //store the Inventor application object
            m_inventorApplication = application;

            //create the button definition
            m_buttonDefinition = m_inventorApplication.CommandManager.ControlDefinitions.AddButtonDefinition(
                displayName,
                internalName,
                commandType,
                clientId,
                description,
                toolTip,
                standardIcon,
                largeIcon,
                buttonType);

            //connect the button events sink
            m_buttonDefinition_OnExecute_Delegate = new ButtonDefinitionSink_OnExecuteEventHandler(ButtonDefinition_OnExecute);
            m_buttonDefinition.OnExecute += m_buttonDefinition_OnExecute_Delegate;

            //display button by default if specified
            if (autoAddToGUI)
            {
                m_buttonDefinition.AutoAddToGUI();
            }
        }

        virtual protected void ButtonDefinition_OnExecute(NameValueMap context)
        {
            //if command was already started, stop it first
            if (m_commandIsRunning)
            {
                StopCommand();
            }

            //start new command
            StartCommand();  
        }

        public void RemoveButton()
        {
            //disconnect button events sink
            if (m_buttonDefinition != null)
            {
                m_buttonDefinition.OnExecute -= m_buttonDefinition_OnExecute_Delegate;
                m_buttonDefinition = null;
            }
        }

        public void StartInteraction()
        {
            try
            {
                //create instance of the Interaction class
                m_interaction = new ValveBlockDesign.Interaction();

                m_interaction.SetParentCmd(this);

                string strButtonDefObjInternalName = m_buttonDefinition.InternalName;

                m_interaction.StartInteraction(m_inventorApplication, strButtonDefObjInternalName, out m_interactionEvents);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        public void StopInteraction()
        {
            //terminate interaction events
            m_interaction.StopInteraction();

            //m_interaction.Release();
            m_interaction = null;

            m_interactionEvents = null;
        }

        virtual public void StartCommand()
        {
            //Start interaction events
            StartInteraction();

            //press the button
            m_buttonDefinition.Pressed = true;

            //set the commnd status to running
            m_commandIsRunning = true;
        }

        virtual public void StopCommand()
        {
            try
            {
                //unsubscribe from the events
                UnsubscribeFromEvents();

                //stop interaction events
                StopInteraction();

                //un-press the button
                m_buttonDefinition.Pressed = false;

                //set the command status to not-running
                m_commandIsRunning = false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        public abstract void ExecuteCommand();

        public void SubscribeToEvent(Interaction.InteractionTypeEnum interactionType)
        {
            //subscribe to the specified event type (selection, mouse etc.)
            object eventType = null;
            m_interaction.SubscribeToEvent(interactionType, ref eventType);

            if (eventType != null)
            {
                if (eventType is SelectEvents)
                {
                    m_selectEvents = (SelectEvents)eventType;
                }

                if (eventType is MouseEvents)
                {
                    m_mouseEvents = (MouseEvents)eventType;
                }

                if (eventType is TriadEvents)
                {
                    m_triadEvents = (TriadEvents)eventType;
                }
            }
        }

        public void UnsubscribeFromEvents()
        {
            //unsubscribe from all event objects (selection, mouse etc.)
            m_interaction.UnsubscribeFromEvents();

            m_selectEvents = null;
            m_mouseEvents = null;
            m_triadEvents = null;
        }

        virtual public void EnableInteraction()
        {
            //enable interaction events
            m_interaction.EnableInteraction();
        }

        virtual public void DisableInteraction()
        {
            //disable interaction events
            m_interaction.DisableInteraction();
        }

        public void ExecuteChangeRequest(ValveBlockDesign.ChangeRequest changeRequest,object changeDefinition, Inventor._Document document)
        {
            changeRequest.Execute(m_inventorApplication, changeDefinition, document);
        }
        //******************************************************************************
        //               Implementation of Interaction Events sink method               
        //******************************************************************************
        //-----------------------------------------------------------------------------
        //               Implementation of Select Events sink method               
        //-----------------------------------------------------------------------------
        virtual public void OnPreSelect(object preSelectEntity, out bool doHighlight, ObjectCollection morePreSelectEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, View view)
        {
            doHighlight = false;
        }

        //-----------------------------------------------------------------------------
        public void OnPreSelectMouseMove(object preSelectEntity, Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        public void OnStopPreSelect(Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        virtual public void OnSelect(ObjectsEnumerator justSelectedEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        public void OnUnSelect(ObjectsEnumerator unSelectedEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        //----- Implementation of Mouse Events sink methods
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        virtual public void OnMouseUp(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        virtual public void OnMouseDown(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        virtual public void OnMouseClick(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        virtual public void OnMouseDoubleClick(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        virtual public void OnMouseMove(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        virtual public void OnMouseLeave(MouseButtonEnum button, ShiftStateEnum shiftKeys, View view)
        {
            // Not implemented
        }

        //-----------------------------------------------------------------------------
        //----- Implementation of Triad Events sink methods
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        virtual public void OnActivate(NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        //-----------------------------------------------------------------------------
        virtual public void OnEndMove(TriadSegmentEnum selectedTriadSegment, ShiftStateEnum shiftKeys, Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        //----------------------------------------------------------------------------
        virtual public void OnMove(TriadSegmentEnum selectedTriadSegment, ShiftStateEnum shiftKeys, Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        //-----------------------------------------------------------------------------
        virtual public void OnEndSequence(bool cancelled, Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        //-----------------------------------------------------------------------------
        virtual public void OnStartSequence(Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        //-----------------------------------------------------------------------------
        virtual public void OnMoveTriadOnlyToggle(bool moveTriadOnly, EventTimingEnum beforeOrAfter, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        //-----------------------------------------------------------------------------
        virtual public void OnStartMove(TriadSegmentEnum selectedTriadSegment, ShiftStateEnum shiftKeys, Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        //-----------------------------------------------------------------------------
        virtual public void OnTerminate(bool cancelled, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        //-----------------------------------------------------------------------------
        virtual public void OnSegmentSelectionChange(TriadSegmentEnum selectedTriadSegment, EventTimingEnum beforeOrAfter, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            // Not implemented
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }		

        //******************************************************************************
        //               Implementation of UserInput Events sink method               
        //******************************************************************************
        virtual public void OnDrag(DragStateEnum DragState, ShiftStateEnum ShiftKeys, Point ModelPosition, Point2d ViewPosition, View View, NameValueMap AdditionalInfo, out HandlingCodeEnum HandlingCode)
        {
            //not implementation
            HandlingCode = HandlingCodeEnum.kEventNotHandled;
        }
        //------------------------------------------------------------------------------

        virtual public void OnHelp(EventTimingEnum beforeOrAfter, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }
    }
}
