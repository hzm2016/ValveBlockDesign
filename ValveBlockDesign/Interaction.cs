using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;
using Microsoft.VisualBasic;

namespace ValveBlockDesign
{
    internal class Interaction
    {
        //Interaction Types collection
        private System.Collections.ArrayList m_interactionTypes;

        //InteractionEvents object
        private InteractionEvents m_interactionEvents;

        //SelectEvents object
        private SelectEvents m_selectEvents;

        //MouseEvents object
        private MouseEvents m_mouseEvents;

        //TriadEvents object
        private TriadEvents m_triadEvents;

        //Parent Command object
        private ValveBlockDesign.Command m_parentCmd;

        //Interaction Event Delegates
        private InteractionEventsSink_OnTerminateEventHandler m_interaction_OnTerminate_Delegate;
       // private InteractionEventsSink_OnHelpEventHandler m_interaction_OnHelp_Delegate;

        //Select Event Delegates
        private SelectEventsSink_OnPreSelectEventHandler m_Select_OnPreSelect_Delegate;
        private SelectEventsSink_OnPreSelectMouseMoveEventHandler m_Select_OnPreSelectMouseMove_Delegate;
        private SelectEventsSink_OnStopPreSelectEventHandler m_Select_OnStopPreSelect_Delegate;
        private SelectEventsSink_OnSelectEventHandler m_Select_OnSelect_Delegate;
        private SelectEventsSink_OnUnSelectEventHandler m_Select_OnUnSelect_Delegate;

        //Mouse Event Delegates
        private MouseEventsSink_OnMouseUpEventHandler m_Mouse_OnMouseUp_Delegate;
        private MouseEventsSink_OnMouseDownEventHandler m_Mouse_OnMouseDown_Delegate;
        private MouseEventsSink_OnMouseClickEventHandler m_Mouse_OnMouseClick_Delegate;
        private MouseEventsSink_OnMouseDoubleClickEventHandler m_Mouse_OnMouseDoubleClick_Delegate;
        private MouseEventsSink_OnMouseMoveEventHandler m_Mouse_OnMouseMove_Delegate;
        private MouseEventsSink_OnMouseLeaveEventHandler m_Mouse_OnMouseLeave_Delegate;

        //Triad Event Delegates
        private TriadEventsSink_OnActivateEventHandler m_Triad_OnActivate_Delegate;
        private TriadEventsSink_OnEndMoveEventHandler m_Triad_OnEndMove_Delegate;
        private TriadEventsSink_OnEndSequenceEventHandler m_Triad_OnEndSequence_Delegate;
        private TriadEventsSink_OnMoveEventHandler m_Triad_OnMove_Delegate;
        private TriadEventsSink_OnMoveTriadOnlyToggleEventHandler m_Triad_OnMoveTriadOnlyToggle_Delegate;
        private TriadEventsSink_OnSegmentSelectionChangeEventHandler m_Triad_OnSegmentSelectionChange_Delegate;
        private TriadEventsSink_OnStartMoveEventHandler m_Triad_OnStartMove_Delegate;
        private TriadEventsSink_OnStartSequenceEventHandler m_Triad_OnStartSequence_Delegate;
        private TriadEventsSink_OnTerminateEventHandler m_Triad_OnTerminate_Delegate;

        internal enum InteractionTypeEnum
        {
            kSelection,
            kMouse,
            kKeyboard,
            kTriad
        };

        public Interaction()
        {
            m_interactionTypes = new System.Collections.ArrayList();

            m_interactionEvents = null;
            m_selectEvents = null;
            m_mouseEvents = null;
            m_triadEvents = null;

        }

        public void StartInteraction(Inventor.Application application, string interactionName, out InteractionEvents interactionEvents)
        {
            try
            {
                //-----------------------------------------------------
                //------------------InteractionEvents------------------
                //-----------------------------------------------------
                //create the InteractionEvents object
                m_interactionEvents = application.CommandManager.CreateInteractionEvents();

                //define that we want select events rather than mouse events
                m_interactionEvents.SelectionActive = true;

                //set the name for the interaction events
                m_interactionEvents.Name = interactionName;

                //connect the interaction event sink
                m_interaction_OnTerminate_Delegate = new InteractionEventsSink_OnTerminateEventHandler(InteractionEvents_OnTerminate);
                m_interactionEvents.OnTerminate += m_interaction_OnTerminate_Delegate;

                //------------------SelectEvents--------------------
                //set a reference to the select events
                m_selectEvents = m_interactionEvents.SelectEvents;

                //connect the select event sink
                m_Select_OnSelect_Delegate = new SelectEventsSink_OnSelectEventHandler(SelectEvents_OnSelect);
                m_selectEvents.OnSelect += m_Select_OnSelect_Delegate;

                m_Select_OnPreSelect_Delegate = new SelectEventsSink_OnPreSelectEventHandler(SelectEvents_OnPreSelect);
                m_selectEvents.OnPreSelect += m_Select_OnPreSelect_Delegate;

                m_Select_OnPreSelectMouseMove_Delegate = new SelectEventsSink_OnPreSelectMouseMoveEventHandler(SelectEvents_OnPreSelectMouseMove);
                m_selectEvents.OnPreSelectMouseMove += m_Select_OnPreSelectMouseMove_Delegate;

                m_Select_OnStopPreSelect_Delegate = new SelectEventsSink_OnStopPreSelectEventHandler(SelectEvents_OnStopPreSelect);
                m_selectEvents.OnStopPreSelect += m_Select_OnStopPreSelect_Delegate;

                m_Select_OnUnSelect_Delegate = new SelectEventsSink_OnUnSelectEventHandler(SelectEvents_OnUnSelect);
                m_selectEvents.OnUnSelect += m_Select_OnUnSelect_Delegate;

                //------------------MouseEvents--------------------
                //set a reference to the mouse events
                m_mouseEvents = m_interactionEvents.MouseEvents;

                //connect the mouse event sink
                m_Mouse_OnMouseMove_Delegate = new MouseEventsSink_OnMouseMoveEventHandler(MouseEvents_OnMouseMove);
                m_mouseEvents.OnMouseMove += m_Mouse_OnMouseMove_Delegate;

                m_Mouse_OnMouseUp_Delegate = new MouseEventsSink_OnMouseUpEventHandler(MouseEvents_OnMouseUp);
                m_mouseEvents.OnMouseUp += m_Mouse_OnMouseUp_Delegate;

                m_Mouse_OnMouseDown_Delegate = new MouseEventsSink_OnMouseDownEventHandler(MouseEvents_OnMouseDown);
                m_mouseEvents.OnMouseDown += m_Mouse_OnMouseDown_Delegate;

                m_Mouse_OnMouseClick_Delegate = new MouseEventsSink_OnMouseClickEventHandler(MouseEvents_OnMouseClick);
                m_mouseEvents.OnMouseClick += m_Mouse_OnMouseClick_Delegate;

                m_Mouse_OnMouseDoubleClick_Delegate = new MouseEventsSink_OnMouseDoubleClickEventHandler(MouseEvents_OnMouseDoubleClick);
                m_mouseEvents.OnMouseDoubleClick += m_Mouse_OnMouseDoubleClick_Delegate;

                m_Mouse_OnMouseLeave_Delegate = new MouseEventsSink_OnMouseLeaveEventHandler(MouseEvents_OnMouseLeave);
                m_mouseEvents.OnMouseLeave += m_Mouse_OnMouseLeave_Delegate;

                //------------------TriadEvents--------------------
                //set a reference to the triad events
                m_triadEvents = m_interactionEvents.TriadEvents;

                //connect the triad event sink
                m_Triad_OnActivate_Delegate = new TriadEventsSink_OnActivateEventHandler(TriadEvents_OnActivate);
                m_triadEvents.OnActivate += m_Triad_OnActivate_Delegate;

                m_Triad_OnEndMove_Delegate = new TriadEventsSink_OnEndMoveEventHandler(TriadEvents_OnEndMove);
                m_triadEvents.OnEndMove += m_Triad_OnEndMove_Delegate;

                m_Triad_OnEndSequence_Delegate = new TriadEventsSink_OnEndSequenceEventHandler(TriadEvents_OnEndSequence);
                m_triadEvents.OnEndSequence += m_Triad_OnEndSequence_Delegate;

                m_Triad_OnMove_Delegate = new TriadEventsSink_OnMoveEventHandler(TriadEvents_OnMove);
                m_triadEvents.OnMove += m_Triad_OnMove_Delegate;

                m_Triad_OnMoveTriadOnlyToggle_Delegate = new TriadEventsSink_OnMoveTriadOnlyToggleEventHandler(TriadEvents_OnMoveTriadOnlyToggle);
                m_triadEvents.OnMoveTriadOnlyToggle += m_Triad_OnMoveTriadOnlyToggle_Delegate;

                m_Triad_OnSegmentSelectionChange_Delegate = new TriadEventsSink_OnSegmentSelectionChangeEventHandler(TriadEvents_OnSegmentSelectionChange);
                m_triadEvents.OnSegmentSelectionChange += m_Triad_OnSegmentSelectionChange_Delegate;

                m_Triad_OnStartMove_Delegate = new TriadEventsSink_OnStartMoveEventHandler(TriadEvents_OnStartMove);
                m_triadEvents.OnStartMove += m_Triad_OnStartMove_Delegate;

                m_Triad_OnStartSequence_Delegate = new TriadEventsSink_OnStartSequenceEventHandler(TriadEvents_OnStartSequence);
                m_triadEvents.OnStartSequence += m_Triad_OnStartSequence_Delegate;

                m_Triad_OnTerminate_Delegate = new TriadEventsSink_OnTerminateEventHandler(TriadEvents_OnTerminate);
                m_triadEvents.OnTerminate += m_Triad_OnTerminate_Delegate;

                //start the InteractionEvents
                m_interactionEvents.Start();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            finally
            {
                interactionEvents = m_interactionEvents;
            }
        }

        public void StopInteraction()
        {
            //disconnect interaction events sink
            if (m_interactionEvents != null)
            {
                m_interactionEvents.OnTerminate -= m_interaction_OnTerminate_Delegate;

                m_interactionEvents.Stop();

                m_interactionEvents = null;
            }
        }

        public void EnableInteraction()
        {
            //enable events
            int interactionEvtsCount;
            for (interactionEvtsCount = 0; interactionEvtsCount < m_interactionTypes.Count; interactionEvtsCount++)
            {
                switch ((InteractionTypeEnum)m_interactionTypes[interactionEvtsCount])
                {
                    case InteractionTypeEnum.kSelection:

                        //re-subscribe to selection events 
                        if (m_selectEvents == null)
                        {
                            //set a reference to the select events
                            m_selectEvents = m_interactionEvents.SelectEvents;

                            //connect the select event sink
                            m_selectEvents.OnPreSelect += m_Select_OnPreSelect_Delegate;
                            m_selectEvents.OnPreSelectMouseMove += m_Select_OnPreSelectMouseMove_Delegate;
                            m_selectEvents.OnStopPreSelect += m_Select_OnStopPreSelect_Delegate;
                            m_selectEvents.OnSelect += m_Select_OnSelect_Delegate;
                            m_selectEvents.OnUnSelect += m_Select_OnUnSelect_Delegate;

                            //specify burn through
                            m_selectEvents.PreSelectBurnThrough = true;
                        }

                        break;

                    case InteractionTypeEnum.kMouse:

                        //re-subscribe to mouse events 
                        if (m_mouseEvents == null)
                        {
                            //set a reference to the mouse events
                            m_mouseEvents = m_interactionEvents.MouseEvents;

                            //connect the mouse event sink
                            m_mouseEvents.OnMouseUp += m_Mouse_OnMouseUp_Delegate;
                            m_mouseEvents.OnMouseDown += m_Mouse_OnMouseDown_Delegate;
                            m_mouseEvents.OnMouseClick += m_Mouse_OnMouseClick_Delegate;
                            m_mouseEvents.OnMouseDoubleClick += m_Mouse_OnMouseDoubleClick_Delegate;
                            m_mouseEvents.OnMouseMove += m_Mouse_OnMouseMove_Delegate;
                            m_mouseEvents.OnMouseLeave += m_Mouse_OnMouseLeave_Delegate;
                        }

                        break;

                    case InteractionTypeEnum.kTriad:

                        //re-subscribe to triad events 
                        if (m_triadEvents == null)
                        {
                            //set a reference to the triad events
                            m_triadEvents = m_interactionEvents.TriadEvents;

                            //connect the triad event sink
                            m_triadEvents.OnActivate += m_Triad_OnActivate_Delegate;
                            m_triadEvents.OnEndMove += m_Triad_OnEndMove_Delegate;
                            m_triadEvents.OnEndSequence += m_Triad_OnEndSequence_Delegate;
                            m_triadEvents.OnMove += m_Triad_OnMove_Delegate;
                            m_triadEvents.OnMoveTriadOnlyToggle += m_Triad_OnMoveTriadOnlyToggle_Delegate;
                            m_triadEvents.OnSegmentSelectionChange += m_Triad_OnSegmentSelectionChange_Delegate;
                            m_triadEvents.OnStartMove += m_Triad_OnStartMove_Delegate;
                            m_triadEvents.OnStartSequence += m_Triad_OnStartSequence_Delegate;
                            m_triadEvents.OnTerminate += m_Triad_OnTerminate_Delegate;
                        }

                        break;
                }
            }
        }

        public void DisableInteraction()
        {
            //disable subscribed to events
            int interactionEvtsCount;
            for (interactionEvtsCount = 0; interactionEvtsCount < m_interactionTypes.Count; interactionEvtsCount++)
            {
                switch ((InteractionTypeEnum)m_interactionTypes[interactionEvtsCount])
                {
                    case InteractionTypeEnum.kSelection:

                        //un-subscribe and delete selection events
                        if (m_selectEvents != null)
                        {
                            m_selectEvents.OnPreSelect -= m_Select_OnPreSelect_Delegate;
                            m_selectEvents.OnPreSelectMouseMove -= m_Select_OnPreSelectMouseMove_Delegate;
                            m_selectEvents.OnStopPreSelect -= m_Select_OnStopPreSelect_Delegate;
                            m_selectEvents.OnSelect -= m_Select_OnSelect_Delegate;
                            m_selectEvents.OnUnSelect -= m_Select_OnUnSelect_Delegate;

                            m_selectEvents = null;
                        }

                        break;

                    case InteractionTypeEnum.kMouse:

                        //un-subscribe and delete mouse events 
                        if (m_mouseEvents != null)
                        {
                            m_mouseEvents.OnMouseUp -= m_Mouse_OnMouseUp_Delegate;
                            m_mouseEvents.OnMouseDown -= m_Mouse_OnMouseDown_Delegate;
                            m_mouseEvents.OnMouseClick -= m_Mouse_OnMouseClick_Delegate;
                            m_mouseEvents.OnMouseDoubleClick -= m_Mouse_OnMouseDoubleClick_Delegate;
                            m_mouseEvents.OnMouseMove -= m_Mouse_OnMouseMove_Delegate;
                            m_mouseEvents.OnMouseLeave -= m_Mouse_OnMouseLeave_Delegate;

                            m_mouseEvents = null;
                        }

                        break;

                    case InteractionTypeEnum.kTriad:

                        //un-subscribe and delete triad events
                        if (m_triadEvents != null)
                        {
                            m_triadEvents.OnActivate -= m_Triad_OnActivate_Delegate;
                            m_triadEvents.OnEndMove -= m_Triad_OnEndMove_Delegate;
                            m_triadEvents.OnEndSequence -= m_Triad_OnEndSequence_Delegate;
                            m_triadEvents.OnMove -= m_Triad_OnMove_Delegate;
                            m_triadEvents.OnMoveTriadOnlyToggle -= m_Triad_OnMoveTriadOnlyToggle_Delegate;
                            m_triadEvents.OnSegmentSelectionChange -= m_Triad_OnSegmentSelectionChange_Delegate;
                            m_triadEvents.OnStartMove -= m_Triad_OnStartMove_Delegate;
                            m_triadEvents.OnStartSequence -= m_Triad_OnStartSequence_Delegate;
                            m_triadEvents.OnTerminate -= m_Triad_OnTerminate_Delegate;

                            m_triadEvents = null;
                        }

                        break;
                }
            }
        }

        public void SubscribeToEvent(InteractionTypeEnum interactionType, ref object eventType)
        {
            //check if already subscribed to
            int interactionEvtsCount;
            for (interactionEvtsCount = 0; interactionEvtsCount < m_interactionTypes.Count; interactionEvtsCount++)
            {
                if (interactionType == (InteractionTypeEnum)m_interactionTypes[interactionEvtsCount])
                {
                    return;
                }
            }

            //if not already subsribed to then, subscribe
            m_interactionTypes.Add(interactionType);

            switch (interactionType)
            {
                case InteractionTypeEnum.kSelection:

                    //set a reference to the select events
                    m_selectEvents = m_interactionEvents.SelectEvents;

                    //connect the select event sink
                    m_selectEvents.OnPreSelect += m_Select_OnPreSelect_Delegate;
                    m_selectEvents.OnPreSelectMouseMove += m_Select_OnPreSelectMouseMove_Delegate;
                    m_selectEvents.OnStopPreSelect += m_Select_OnStopPreSelect_Delegate;
                    m_selectEvents.OnSelect += m_Select_OnSelect_Delegate;
                    m_selectEvents.OnUnSelect += m_Select_OnUnSelect_Delegate;

                    //specify burn through
                    m_selectEvents.PreSelectBurnThrough = true;

                    eventType = m_selectEvents;
                    break;

                case InteractionTypeEnum.kMouse:

                    //set a reference to the mouse events
                    m_mouseEvents = m_interactionEvents.MouseEvents;

                    //connect the mouse event sink
                    m_mouseEvents.OnMouseUp += m_Mouse_OnMouseUp_Delegate;
                    m_mouseEvents.OnMouseDown += m_Mouse_OnMouseDown_Delegate;
                    m_mouseEvents.OnMouseClick += m_Mouse_OnMouseClick_Delegate;
                    m_mouseEvents.OnMouseDoubleClick += m_Mouse_OnMouseDoubleClick_Delegate;
                    m_mouseEvents.OnMouseMove += m_Mouse_OnMouseMove_Delegate;
                    m_mouseEvents.OnMouseLeave += m_Mouse_OnMouseLeave_Delegate;

                    eventType = m_mouseEvents;
                    break;

                case InteractionTypeEnum.kTriad:

                    //set a reference to the triad events
                    m_triadEvents = m_interactionEvents.TriadEvents;

                    //connect the triad event sink
                    m_triadEvents.OnActivate += m_Triad_OnActivate_Delegate;
                    m_triadEvents.OnEndMove += m_Triad_OnEndMove_Delegate;
                    m_triadEvents.OnEndSequence += m_Triad_OnEndSequence_Delegate;
                    m_triadEvents.OnMove += m_Triad_OnMove_Delegate;
                    m_triadEvents.OnMoveTriadOnlyToggle += m_Triad_OnMoveTriadOnlyToggle_Delegate;
                    m_triadEvents.OnSegmentSelectionChange += m_Triad_OnSegmentSelectionChange_Delegate;
                    m_triadEvents.OnStartMove += m_Triad_OnStartMove_Delegate;
                    m_triadEvents.OnStartSequence += m_Triad_OnStartSequence_Delegate;
                    m_triadEvents.OnTerminate += m_Triad_OnTerminate_Delegate;

                    eventType = m_triadEvents;
                    break;
            }
        }

        public void UnsubscribeFromEvents()
        {
            int interactionEvtsCount;
            for (interactionEvtsCount = 0; interactionEvtsCount < m_interactionTypes.Count; interactionEvtsCount++)
            {
                switch ((InteractionTypeEnum)m_interactionTypes[interactionEvtsCount])
                {
                    case InteractionTypeEnum.kSelection:

                        //disconnect selection events sink
                        if (m_selectEvents != null)
                        {
                            m_selectEvents.OnPreSelect -= m_Select_OnPreSelect_Delegate;
                            m_selectEvents.OnPreSelectMouseMove -= m_Select_OnPreSelectMouseMove_Delegate;
                            m_selectEvents.OnStopPreSelect -= m_Select_OnStopPreSelect_Delegate;
                            m_selectEvents.OnSelect -= m_Select_OnSelect_Delegate;
                            m_selectEvents.OnUnSelect -= m_Select_OnUnSelect_Delegate;

                            m_selectEvents = null;
                        }

                        break;

                    case InteractionTypeEnum.kMouse:

                        //disconnect mouse events sink
                        if (m_mouseEvents != null)
                        {
                            m_mouseEvents.OnMouseUp -= m_Mouse_OnMouseUp_Delegate;
                            m_mouseEvents.OnMouseDown -= m_Mouse_OnMouseDown_Delegate;
                            m_mouseEvents.OnMouseClick -= m_Mouse_OnMouseClick_Delegate;
                            m_mouseEvents.OnMouseDoubleClick -= m_Mouse_OnMouseDoubleClick_Delegate;
                            m_mouseEvents.OnMouseMove -= m_Mouse_OnMouseMove_Delegate;
                            m_mouseEvents.OnMouseLeave -= m_Mouse_OnMouseLeave_Delegate;

                            m_mouseEvents = null;
                        }

                        break;

                    case InteractionTypeEnum.kTriad:

                        //disconnect triad events sink
                        if (m_triadEvents != null)
                        {
                            m_triadEvents.OnActivate -= m_Triad_OnActivate_Delegate;
                            m_triadEvents.OnEndMove -= m_Triad_OnEndMove_Delegate;
                            m_triadEvents.OnEndSequence -= m_Triad_OnEndSequence_Delegate;
                            m_triadEvents.OnMove -= m_Triad_OnMove_Delegate;
                            m_triadEvents.OnMoveTriadOnlyToggle -= m_Triad_OnMoveTriadOnlyToggle_Delegate;
                            m_triadEvents.OnSegmentSelectionChange -= m_Triad_OnSegmentSelectionChange_Delegate;
                            m_triadEvents.OnStartMove -= m_Triad_OnStartMove_Delegate;
                            m_triadEvents.OnStartSequence -= m_Triad_OnStartSequence_Delegate;
                            m_triadEvents.OnTerminate -= m_Triad_OnTerminate_Delegate;

                            m_triadEvents = null;
                        }

                        break;
                }
            }

            m_interactionTypes.Clear();
        }

        public void SetParentCmd(ValveBlockDesign.Command parentCmd)
        {
            //store a copy of the parent command
            m_parentCmd = parentCmd;
        }

        //******************************************************************************
        //               Implementation of Interaction Events sink method               
        //******************************************************************************
        public void InteractionEvents_OnTerminate()
        {
            //terminate the command
            m_parentCmd.StopCommand();
        }
        //------------------------------------------------------------------------------


        //------------------Implementation of Select Events sink method-----------------
        //------------------------------------------------------------------------------
        public void SelectEvents_OnSelect(ObjectsEnumerator justSelectedEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, View view)
        { 
            //not implemented
            m_parentCmd.OnSelect(justSelectedEntities, selectionDevice, modelPosition, viewPosition, view);
        }

        //-----------------------------------------------------------------------------
        public void SelectEvents_OnPreSelect(ref object preSelectEntity, out bool doHighlight, ref ObjectCollection morePreSelectEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, View view)
        {
            m_parentCmd.OnPreSelect(preSelectEntity, out doHighlight, morePreSelectEntities, selectionDevice, modelPosition, viewPosition, view);
        }

        //-----------------------------------------------------------------------------

        public void SelectEvents_OnPreSelectMouseMove(object preSelectEntity, Point modelPosition, Point2d viewPosition, View view)
        {
            m_parentCmd.OnPreSelectMouseMove(preSelectEntity, modelPosition, viewPosition, view);
        }

        //-----------------------------------------------------------------------------
        public void SelectEvents_OnStopPreSelect(Point modelPosition, Point2d viewPosition, View view)
        {
            m_parentCmd.OnStopPreSelect(modelPosition, viewPosition, view);
        }

        //-----------------------------------------------------------------------------
        public void SelectEvents_OnUnSelect(ObjectsEnumerator unSelectedEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, View view)
		{
            m_parentCmd.OnUnSelect(unSelectedEntities, selectionDevice, modelPosition, viewPosition, view);
		}

		//-----------------------------------------------------------------------------

        //------------------Implementation of Mouse Events sink method------------------
        //------------------------------------------------------------------------------
        public void MouseEvents_OnMouseMove(MouseButtonEnum Button, ShiftStateEnum ShiftKeys, Point ModelPosition, Point2d ViewPosition, View View)
        {
            m_parentCmd.OnMouseMove(Button, ShiftKeys, ModelPosition, ViewPosition, View);
        }

        //-----------------------------------------------------------------------------
        public void MouseEvents_OnMouseUp(MouseButtonEnum Button, ShiftStateEnum ShiftKeys, Point ModelPosition, Point2d ViewPosition, View View)
        {
            m_parentCmd.OnMouseUp(Button, ShiftKeys, ModelPosition, ViewPosition, View);
        }

        //-----------------------------------------------------------------------------
        public void MouseEvents_OnMouseDown(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, View view)
        {
            m_parentCmd.OnMouseDown(button, shiftKeys, modelPosition, viewPosition, view);
        }

        //-----------------------------------------------------------------------------
        public void MouseEvents_OnMouseClick(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, View view)
        {
            m_parentCmd.OnMouseClick(button, shiftKeys, modelPosition, viewPosition, view);
        }

        //-----------------------------------------------------------------------------
        public void MouseEvents_OnMouseDoubleClick(MouseButtonEnum button, ShiftStateEnum shiftKeys, Point modelPosition, Point2d viewPosition, View view)
        {
            m_parentCmd.OnMouseDoubleClick(button, shiftKeys, modelPosition, viewPosition, view);
        }

        //-----------------------------------------------------------------------------
        public void MouseEvents_OnMouseLeave(MouseButtonEnum button, ShiftStateEnum shiftKeys, View view)
        {
            m_parentCmd.OnMouseLeave(button, shiftKeys, view);
        }

        //-----------------------------------------------------------------------------

        //----- Implementation of Triad Events sink methods
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        public void TriadEvents_OnActivate(NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnActivate(context, out handlingCode);
        }

        //-----------------------------------------------------------------------------
        public void TriadEvents_OnEndMove(TriadSegmentEnum selectedTriadSegment, ShiftStateEnum shiftKeys, Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnEndMove(selectedTriadSegment, shiftKeys, coordinateSystem, context, out handlingCode);
        }

        //----------------------------------------------------------------------------
        public void TriadEvents_OnMove(TriadSegmentEnum selectedTriadSegment, ShiftStateEnum shiftKeys, Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnMove(selectedTriadSegment, shiftKeys, coordinateSystem, context, out handlingCode);
        }

        //-----------------------------------------------------------------------------
        public void TriadEvents_OnEndSequence(bool cancelled, Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnEndSequence(cancelled, coordinateSystem, context, out handlingCode);
        }

        //-----------------------------------------------------------------------------
        public void TriadEvents_OnStartSequence(Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnStartSequence(coordinateSystem, context, out handlingCode);
        }

        //-----------------------------------------------------------------------------
        public void TriadEvents_OnMoveTriadOnlyToggle(bool moveTriadOnly, EventTimingEnum beforeOrAfter, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnMoveTriadOnlyToggle(moveTriadOnly, beforeOrAfter, context, out handlingCode);
        }

        //-----------------------------------------------------------------------------
        public void TriadEvents_OnStartMove(TriadSegmentEnum selectedTriadSegment, ShiftStateEnum shiftKeys, Matrix coordinateSystem, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnStartMove(selectedTriadSegment, shiftKeys, coordinateSystem, context, out handlingCode);
        }

        //-----------------------------------------------------------------------------
        public void TriadEvents_OnTerminate(bool cancelled, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnTerminate(cancelled, context, out handlingCode);
        }

        //-----------------------------------------------------------------------------
        public void TriadEvents_OnSegmentSelectionChange(TriadSegmentEnum selectedTriadSegment, EventTimingEnum beforeOrAfter, NameValueMap context, out HandlingCodeEnum handlingCode)
        {
            m_parentCmd.OnSegmentSelectionChange(selectedTriadSegment, beforeOrAfter, context, out handlingCode);
        }




        //------------------------------------------------------------------------------


        public void Interactive_Selection(Inventor.Application application)
        {
            m_interactionEvents = application.CommandManager.CreateInteractionEvents();

            //设置提示
            m_interactionEvents.StatusBarText = "Select an cylinder face.";

            //连接到相关选择事件
            m_selectEvents = m_interactionEvents.SelectEvents;

            //清除过滤器并重新定义圆柱面可选择
            m_selectEvents.ClearSelectionFilter();
            m_selectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceCylindricalFilter);

            //单个选择使能
            m_selectEvents.Enabled = true;
            m_selectEvents.SingleSelectEnabled = true;

            m_selectEvents.MouseMoveEnabled = false;

            //设置委托
            m_Select_OnSelect_Delegate = new Inventor.SelectEventsSink_OnSelectEventHandler(this.m_SelectEvents_OnSelect);
            m_selectEvents.OnSelect += m_Select_OnSelect_Delegate;
            m_interaction_OnTerminate_Delegate = new Inventor.InteractionEventsSink_OnTerminateEventHandler(this.m_InteractionEvent_Onterminate);
            m_interactionEvents.OnTerminate += m_interaction_OnTerminate_Delegate;

            //启动选择进程
            m_interactionEvents.Start();

        }

        private void m_SelectEvents_OnSelect(Inventor.ObjectsEnumerator JustSelectedEntities, Inventor.SelectionDeviceEnum SelectionDevice, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
        {
            if (JustSelectedEntities.Count != 0)
            {
                object selectObj1 = JustSelectedEntities[1];
                //object selectObj2 = JustSelectedEntities[2];
                System.Windows.Forms.MessageBox.Show((string)("Selected Entity Type: " + Information.TypeName(selectObj1)) + " , Interactive selection successful!", "Interactive Selection");
            }
        }

        public void CleanUp()
        {
            m_selectEvents.OnSelect -= new Inventor.SelectEventsSink_OnSelectEventHandler(this.m_SelectEvents_OnSelect);
            m_interactionEvents.OnTerminate -= new Inventor.InteractionEventsSink_OnTerminateEventHandler(this.m_InteractionEvent_Onterminate);

            m_selectEvents = null;
            m_interactionEvents = null;
        }

        private void m_InteractionEvent_Onterminate()
        {
            CleanUp();
        }

        public void TransMouseEvents(MouseEvents mouseEvents)
        {
            mouseEvents = m_mouseEvents;
        }
    }
}
