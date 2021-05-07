using Puff.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        private Quaternion lerpQuaterion;

        public enum DragDir { VerticalUp, VerticalDown, Horizontal, None };
        public enum Face { Front, RightSide, Back, LeftSide };
        public enum GestureEvent { Release, Save, None };

        private float moveXDist => (Input.mousePosition - lastStandPoint).x;
        private float moveYDist => (Input.mousePosition - lastStandPoint).y;
        private float absX => Mathf.Abs(moveXDist);
        private float absY => Mathf.Abs(moveYDist);
        private Vector3 centerPosition => _camera.transform.position + (_camera.transform.forward * 0.75f);

        private float DragThreshold = 0.1f;
        private Camera _camera;
        private RaycastHit[] raycastHits = new RaycastHit[1];

        private PointerEventData eventData;
        private List<RaycastResult> raycastResults = new List<RaycastResult>();

        private System.Func<PuffItemView, bool> SetCurrentSelectedObjectCallback;
        private System.Action<Face> SetFaceCallback;
        private System.Action<GestureEvent> ReleaseObjectCallback;
        private System.Action<DragDir, float, float, Vector3> ProcessVerticalCallback;


        private DragDir dragMode = DragDir.None;

        public PuffInspectorInput(System.Func<PuffItemView, bool> SetCurrentSelectedObjectCallback,
                                System.Action<Face> SetFaceCallback,
                                System.Action<GestureEvent> ReleaseObjectCallback,
                                System.Action<DragDir, float, float, Vector3> ProcessVerticalCallback, float dragThreshold, Camera camera) {
            this.SetCurrentSelectedObjectCallback = SetCurrentSelectedObjectCallback;
            this.SetFaceCallback = SetFaceCallback;
            this.ReleaseObjectCallback = ReleaseObjectCallback;
            this.ProcessVerticalCallback = ProcessVerticalCallback;
            this.DragThreshold = dragThreshold;
            this._camera = camera;

            this.eventData = new PointerEventData(EventSystem.current);
        }

        public void SetInputSelectObject(PuffItemView puffItem) {
            this.SelectedPuffObject = puffItem;

            if (this.SelectedPuffObject == null)
                gestureEvent = GestureEvent.None;
        }

        private void PlaySmoothAnimation() {
            if (SelectedPuffObject == null) return;

            GraduallyRotateToFace(currentFace);
            GraudaulyFlyToCenter();
        }

        public void OnUpdate()
        {
            if (HasHitUIComponent()) {
                PlaySmoothAnimation();
                return;
            }

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
                PlaySmoothAnimation();
                return;
            }

            DragDir dragDirection = FindDragDirection();

            if (dragDirection == DragDir.Horizontal && dragMode != DragDir.VerticalDown) {
                dragMode = DragDir.Horizontal;
                ProcessRotation();
            }

            if ((dragDirection == DragDir.VerticalDown || dragDirection == DragDir.VerticalUp) && dragMode != DragDir.Horizontal )
            {
                dragMode = DragDir.VerticalDown;
                ProcessVertical(dragDirection);
            }

            if (Input.GetMouseButtonUp(0))
            {
                dragMode = DragDir.None;
                hasHitOnPuffObj = false;
                recordRotationY = SelectedPuffObject.transform.eulerAngles.y;
                GeneralFlag.SharedVectorUnit.Set(SelectedPuffObject.transform.eulerAngles.x, recordRotationY, SelectedPuffObject.transform.eulerAngles.z);
                lerpQuaterion = Quaternion.Euler(GeneralFlag.SharedVectorUnit);
                currentFace = FindTheBestFace();
                this.SetFaceCallback(currentFace);

                rotDir = ((int)currentFace) * 90;

                if (rotDir == 0 && recordRotationY >= 180)
                    rotDir = 360;

                this.ReleaseObjectCallback(gestureEvent);
            }
        }

        private DragDir FindDragDirection()
        {
            //Vertical check first
            if (absY > DragThreshold && dragMode != DragDir.Horizontal)
            {
                return (moveYDist > 0) ? DragDir.VerticalUp : DragDir.VerticalDown;
            }

            if (absX > DragThreshold && dragMode != DragDir.VerticalDown)
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

            float offset = Mathf.Clamp((absY - DragThreshold) * 0.005f, -1, 1);
            float ratio = 1 - (Mathf.Abs(offset * 2f) / 5f);
            if (dragDir == DragDir.VerticalDown) offset *= -1;

            ProcessVerticalCallback(dragDir, ratio, offset, centerPosition);

            if (ratio <= 0.85f)
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

            //Rotation
            GeneralFlag.SharedVectorUnit.Set(0, direction, 0);

            SelectedPuffObject.transform.Rotate(GeneralFlag.SharedVectorUnit, Space.Self);

            return (direction > 0) ? 1 : -1;
        }

        private Face FindTheBestFace()
        {
            float angle = GetAngle((_camera.transform.forward));

            float rawYRot = (SelectedPuffObject.transform.rotation.eulerAngles.y + angle) % 360;

            int face = Mathf.RoundToInt(rawYRot / 90f) % 4;

            if (face < 0)
                face = 4 + face;

            return (Face)face;
        }

        private void GraudaulyFlyToCenter()
        {
            Vector3 frontPosition = _camera.transform.position + (_camera.transform.forward * 0.75f);

            SelectedPuffObject.transform.position = Vector3.Lerp(SelectedPuffObject.transform.position, frontPosition, 0.1f);
        }

        private void GraduallyRotateToFace(Face face)
        {
            float angle = GetAngle( (_camera.transform.forward));
            var objectEulerVector = SelectedPuffObject.transform.rotation.eulerAngles;
            GeneralFlag.SharedVectorUnit.Set(objectEulerVector.x, rotDir - angle, objectEulerVector.z);
            lerpQuaterion = Quaternion.Lerp(lerpQuaterion, Quaternion.Euler(GeneralFlag.SharedVectorUnit), 0.1f);

            SelectedPuffObject.transform.rotation = lerpQuaterion;
        }

        private bool HasHitUIComponent() {
            raycastResults.Clear();

            eventData.position = UnityEngine.Input.mousePosition;
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults.Count > 0;
        }

        private float GetAngle(Vector3 p_direction) {
            var angle = Mathf.Atan2(p_direction.z, p_direction.x);   //radians
                                                      // you need to devide by PI, and MULTIPLY by 180:
            float degrees = 180 * (angle / Mathf.PI);  //degrees
            return degrees -90; //round number, avoid decimal fragments
        }
    }
}