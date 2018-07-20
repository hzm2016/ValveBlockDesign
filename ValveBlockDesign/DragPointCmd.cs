using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Inventor;


namespace ValveBlockDesign
{
    internal class DragPointCmd : ValveBlockDesign.Command
    {
        private UserInputEvents m_userInputEvents;

        //InteractionGraphics objects
        private InteractionGraphics m_interactionGrapgics;

        //WorkPoint object
        private WorkPoint m_workPoint;

        private UserInputEventsSink_OnDragEventHandler m_UserInput_OnDrag_Delegate;

        public DragPointCmd()
        {
            m_userInputEvents = null;

            m_interactionGrapgics = null;

            m_workPoint = null;
        }

        protected override void ButtonDefinition_OnExecute(NameValueMap context)
        {
            this.Initialize();
 //           this.StartInteraction();
        }

        public void Initialize()
        {
            m_userInputEvents = m_inventorApplication.CommandManager.UserInputEvents;
            m_UserInput_OnDrag_Delegate = new UserInputEventsSink_OnDragEventHandler(OnDrag);
            m_userInputEvents.OnDrag += m_UserInput_OnDrag_Delegate;
        }

        public override void OnDrag(DragStateEnum DragState, ShiftStateEnum ShiftKeys, Point ModelPosition, Point2d ViewPosition, View View, NameValueMap AdditionalInfo, out HandlingCodeEnum HandlingCode)
        {
 
            SelectSet oSelectSet;
            oSelectSet = m_inventorApplication.ActiveDocument.SelectSet;

            if (DragState == DragStateEnum.kDragStateDragHandlerSelection)
            {
                if (oSelectSet.Count == 1 && oSelectSet[1].Type == (int)ObjectTypeEnum.kWorkPointObject)
                {
                    m_workPoint = oSelectSet[1];

                    if (m_workPoint.DefinitionType == WorkPointDefinitionEnum.kFixedWorkPoint)
                    {
                        HandlingCode = HandlingCodeEnum.kEventCanceled;

                        this.StartInteraction();

                        m_interactionEvents.MouseEvents.MouseMoveEnabled = true;

                        m_interactionGrapgics = m_interactionEvents.InteractionGraphics;
                        m_interactionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCommonSketchDrag);
                        m_interactionEvents.Start();
                    }
                    else
                    {
                        base.OnDrag(DragState, ShiftKeys, ModelPosition, ViewPosition, View, AdditionalInfo, out HandlingCode);
                    }
                }
                else
                {
                    base.OnDrag(DragState, ShiftKeys, ModelPosition, ViewPosition, View, AdditionalInfo, out HandlingCode);
                }
            }
            else
            {
                base.OnDrag(DragState, ShiftKeys, ModelPosition, ViewPosition, View, AdditionalInfo, out HandlingCode);
            }
        }

        public override void OnMouseMove(MouseButtonEnum Button, ShiftStateEnum ShiftKeys, Point ModelPosition, Point2d ViewPosition, View View)
        {
            SelectSet oSelectSet;
            oSelectSet = m_inventorApplication.ActiveDocument.SelectSet;

            if (oSelectSet.Count == 1 && oSelectSet[1].Type ==(int) ObjectTypeEnum.kWorkPointObject)
            {
                FixedWorkPointDef oWPDef;
                oWPDef = m_workPoint.Definition;

                Inventor.Point oProjectedPoint;
                this.ProjectPoint(ModelPosition, oWPDef.Point, out oProjectedPoint);

                //Set a reference to the transient geometry object for user later
                TransientGeometry oTransGeo;
                oTransGeo = m_inventorApplication.TransientGeometry;

                //Create a graghics data set object. This object contains all if the information used to define the graphics.
                GraphicsDataSets oGraDataSets;
                oGraDataSets = m_interactionGrapgics.GraphicsDataSets;

                if (oGraDataSets.Count != 0)
                {
                    oGraDataSets[1].Delete();
                }

                //Create a coordinates set
                GraphicsCoordinateSet oGraCoordSet;
                oGraCoordSet = oGraDataSets.CreateCoordinateSet(1);

                //Create an array that contains coordinates that define a set of outwardly spiraling points
                double[] oPointCoords = new double[3];
                //define the x y and z components of the points
                oPointCoords[0] = oProjectedPoint.X;
                oPointCoords[1] = oProjectedPoint.Y;
                oPointCoords[2] = oProjectedPoint.Z;

                //Assign the point to the coordinate set
                oGraCoordSet.PutCoordinates(oPointCoords);

                //create the ClientGraphics object
                ClientGraphics oClientGraphics;
                oClientGraphics = m_interactionGrapgics.PreviewClientGraphics;

                if (oClientGraphics.Count != 0)
                {
                    oClientGraphics[1].Delete();
                }

                //create a new graphics node within the client graphics objects
                GraphicsNode oPointNode;
                oPointNode = oClientGraphics.AddNode(1);

                //Create a PointGraphics object within the node
                PointGraphics oPointGraphics;
                oPointGraphics = oPointNode.AddPointGraphics();

                //Assign the coordinate set to the line graphics
                oPointGraphics.CoordinateSet = oGraCoordSet;
                oPointGraphics.PointRenderStyle = PointRenderStyleEnum.kCrossPointStyle;
                m_inventorApplication.ActiveView.Update();
            }
        }

        public override void OnMouseUp(MouseButtonEnum Button, ShiftStateEnum ShiftKeys, Point ModelPosition, Point2d ViewPosition, View View)
        {
            SelectSet oSelectSet;
            oSelectSet = m_inventorApplication.ActiveDocument.SelectSet;

            if ((oSelectSet.Count == 1) && (oSelectSet[1].Type ==(int) ObjectTypeEnum.kWorkPointObject))
            {
                FixedWorkPointDef oWPDef;
                oWPDef = m_workPoint.Definition;

                Inventor.Point oProjectedPoint;
                this.ProjectPoint(ModelPosition, oWPDef.Point, out oProjectedPoint);

                //Reposition the fixed work point
                oWPDef.Point = oProjectedPoint;
                m_inventorApplication.ActiveDocument.Update();
                m_interactionEvents.Stop();
            }
        }

        private void ProjectPoint(Inventor.Point ModelPosition, Inventor.Point WorkPointPosition, out Inventor.Point ProjectedPoint)
        {
            //Set a reference to the camera object
            Inventor.Camera oCamera;
            oCamera = m_inventorApplication.ActiveView.Camera;
            
            Vector oVector;
            oVector = oCamera.Eye.VectorTo(oCamera.Target);

            Line oLine;
            oLine = m_inventorApplication.TransientGeometry.CreateLine(ModelPosition, oVector);

            //Create the z-axis vector
            Vector oZAxis;
            oZAxis = m_inventorApplication.TransientGeometry.CreateVector(0, 0, 1);

            //Create a plane parallel to the X-Y plane
            Plane oWPPlane;
            oWPPlane = m_inventorApplication.TransientGeometry.CreatePlane(WorkPointPosition, oZAxis);

            ProjectedPoint = oWPPlane.IntersectWithLine(oLine);
        }

        public override void OnPreSelect(object preSelectEntity, out bool doHighlight, ObjectCollection morePreSelectEntities, SelectionDeviceEnum selectionDevice, Point modelPosition, Point2d viewPosition, View view)
        {
            doHighlight = true;
        }

        public override void ExecuteCommand()
        {
           
        }
    }
}
