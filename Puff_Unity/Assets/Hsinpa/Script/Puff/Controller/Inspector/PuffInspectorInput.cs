using Puff.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.Ctrl.Utility
{
    public class PuffInspectorInput
    {
        private bool hasHitOnPuffObj = false;
        private Vector3 lastStandPoint;
        private PuffItemView SelectedPuffObject;

        private GestureEvent gestureEvent = GestureEvent.None;
        private Face currentFace = Face.Front;
        private int rotDir = 1;
        private float recordRotationY;

        public enum DragDir { VerticalUp, VerticalDown, Horizontal, None };
        public enum Face { Front, RightSide, Back, LeftSide };
        public enum GestureEvent { Release, Save, None };

        private float moveXDist => (Input.mousePosition - lastStandPoint).x;
        private float moveYDist => (Input.mousePosition - lastStandPoint).y;
        private float absX => Mathf.Abs(moveXDist);
        private float absY => Mathf.Abs(moveYDist);

        private float DragThreshold = 0.1f;
        private Camera _camera;
        private RaycastHit[] raycastHits = new RaycastHit[1];

        private System.Func<PuffItemView, bool> SetCurrentSelectedObjectCallback;
        private System.Action<Face> SetFaceCallback;
        private System.Action ReleaseObjectCallback;
        private System.Action<DragDir, float, float> ProcessVerticalCallback;


        public PuffInspectorInput(System.Func<PuffItemView, bool> SetCurrentSelectedObjectCallback, 
                                System.Action<Face> SetFaceCallback,
                                System.Action ReleaseObjectCallback,
                                System.Action<DragDir, float, float> ProcessVerticalCallback, float dragThreshold, Camera camera) {
            this.SetCurrentSelectedObjectCallback = SetCurrentSelectedObjectCallback;
            this.SetFaceCallback = SetFaceCallback;
            this.ReleaseObjectCallback = ReleaseObjectCallback;
            this.ProcessVerticalCallback = ProcessVerticalCallback;
            this.DragThreshold = dragThreshold;
            this._camera = camera;
        }

        public void SetInputSelectObject(PuffItemView puffItem) {
            this.SelectedPuffObject = puffItem;

            if (this.SelectedPuffObject == null)
                gestureEvent = GestureEvent.None;
        }

        public void OnUpdate()
        {
            if (!hasHitOnPuffObj && Input.GetMouseButtonDown(0))
            {
                hasHitOnPuffObj = HasHitPuffObject();

                if (hasHitOnPuffObj)
                {
                    lastStandPoint = Input.mousePosition;

                    if (SelectedPuffObject == null)
                        this.SetCurrentSelectedObjectCallback(raycastHits[0].transform.GetComponent<PuffItemView>());
                }
            }

            if (SelectedPuffObject == null) return;

            if (!hasHitOnPuffObj)
            {
                GraduallyRotateToFace(currentFace);
                GraudaulyFlyToCenter();
                return;
            }

            DragDir dragDirection = FindDragDirection();

            if (dragDirection == DragDir.VerticalDown || dragDirection == DragDir.VerticalUp)
            {
                ProcessVertical(dragDirection);
            }

            if (dragDirection == DragDir.Horizontal)
                ProcessRotation();

            if (Input.GetMouseButtonUp(0))
            {
                hasHitOnPuffObj = false;
                recordRotationY = SelectedPuffObject.transform.eulerAngles.y;
                currentFace = FindTheBestFace();
                this.SetFaceCallback(currentFace);

                rotDir = ((int)currentFace) * 90;

                if (rotDir == 0 && recordRotationY >= 180)
                    rotDir = 360;

                if (gestureEvent != GestureEvent.None)
                {
                    this.ReleaseObjectCallback();
                }
            }
        }

        private DragDir FindDragDirection()
        {
            //Vertical check first
            if (absY > DragThreshold)
            {
                return (moveYDist > 0) ? DragDir.VerticalUp : DragDir.VerticalDown;
            }

            if (absX > DragThreshold)
                return DragDir.Horizontal;

            return DragDir.None;
        }

        private bool HasHitPuffObject()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 100, GeneralFlag.Puff.Layer);

            return hitCount > 0;
        }

        private void ProcessVertical(DragDir dragDir)
        {

            float offset = Mathf.Clamp((absY - DragThreshold) * 0.02f, -5, 5);
            float ratio = 1 - (Mathf.Abs(offset * 2f) / 5);
            if (dragDir == DragDir.VerticalDown) offset *= -1;

            ProcessVerticalCallback(dragDir, ratio, offset);

            if (ratio <= 0)
            {
                gestureEvent = dragDir == DragDir.VerticalDown ? GestureEvent.Save : GestureEvent.Release;
            }
            else
            {
                gestureEvent = GestureEvent.None;
            }
        }

        private int ProcessRotation()
        {
            Vector3 currentStandPoint = Input.mousePosition;
            float direction = (currentStandPoint - lastStandPoint).x;
            direction = Mathf.Clamp(direction, -5, 5);

            Vector3 rotation = new Vector3(0, direction, 0);

            SelectedPuffObject.transform.Rotate(rotation, Space.Self);

            return (direction > 0) ? 1 : -1;
        }

        private Face FindTheBestFace()
        {
            float rawYRot = SelectedPuffObject.transform.rotation.eulerAngles.y;
            int face = Mathf.RoundToInt(rawYRot / 90f) % 4;

            return (Face)face;
        }

        private void GraudaulyFlyToCenter()
        {
            SelectedPuffObject.transform.position = Vector3.Lerp(SelectedPuffObject.transform.position, new Vector3(0, 0, 8), 0.1f);

        }

        private void GraduallyRotateToFace(Face face)
        {
            recordRotationY = Mathf.Lerp(recordRotationY, rotDir, 0.1f);

            SelectedPuffObject.transform.rotation = Quaternion.Euler(0, recordRotationY, 0);
        }




    }
}