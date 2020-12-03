using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.View;
using Hsinpa.View;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;
using TMPro;
using System.Diagnostics.Tracing;

namespace Puff.Ctrl {
    public class PuffInspectCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private PuffInspectView puffInspectView;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private PuffItemView SelectedPuffObject;

        [SerializeField, Range(0.1f, 200)]
        private float DragThreshold = 0.1f;

        private RaycastHit[] raycastHits = new RaycastHit[1];

        bool hasHitOnPuffObj = false;
        Vector3 lastStandPoint;

        public enum Face {Front, RightSide, Back, LeftSide};
        public enum DragDir { VerticalUp, VerticalDown, Horizontal, None };
        public enum GestureEvent { Release, Save, None };

        private GestureEvent gestureEvent = GestureEvent.None;
        private Face currentFace = Face.Front;
        private int rotDir = 1;
        private float recordRotationY;

        private float moveXDist => (Input.mousePosition - lastStandPoint).x;
        private float moveYDist => (Input.mousePosition - lastStandPoint).y;
        private float absX => Mathf.Abs(moveXDist);
        private float absY => Mathf.Abs(moveYDist);

        private readonly Vector3 FixedPuffPosition = new Vector3(0, 0, 8);

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event) {
                case EventFlag.Event.GameStart: {
                        SetUp();
                }
                break;
            }
        }

        private void SetUp()
        {
            //SetFaceInfo(currentFace);
            puffInspectView.Show(false);
            SetInspectViewEvent();
        }

        private void SetFaceInfo(Face p_face) {
            puffInspectView.SetUp(SelectedPuffObject.name, currentFace.ToString("g"));
        }

        private void SetInspectViewEvent() {
            puffInspectView.SetReplyEvent(() =>
            {
                PuffApp.Instance.Notify(EventFlag.Event.OpenPuffMsg);
            });
        }

        #region Device Input Handler
        private void Update()
        {
            if (!hasHitOnPuffObj && Input.GetMouseButtonDown(0))
            {
                hasHitOnPuffObj = HasHitPuffObject();

                if (hasHitOnPuffObj)
                {
                    lastStandPoint = Input.mousePosition;
                    SetCurrentSelectedObject(raycastHits[0].transform.GetComponent<PuffItemView>());
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

            if (dragDirection == DragDir.VerticalDown || dragDirection == DragDir.VerticalUp) {
                ProcessVertical(dragDirection);
            }

            if (dragDirection == DragDir.Horizontal)
                ProcessRotation();

            if (Input.GetMouseButtonUp(0))
            {
                hasHitOnPuffObj = false;
                recordRotationY = SelectedPuffObject.transform.eulerAngles.y;
                currentFace = FindTheBestFace();
                SetFaceInfo(currentFace);

                rotDir = ((int)currentFace) * 90;

                if (rotDir == 0 && recordRotationY >= 180)
                    rotDir = 360;

                if (gestureEvent != GestureEvent.None) {
                    ReleaseSelectObject();
                }
            }
        }

        private DragDir FindDragDirection() {
            //Vertical check first
            if (absY > DragThreshold) {
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

        private void ProcessVertical(DragDir dragDir) {

            float offset = Mathf.Clamp((absY - DragThreshold) * 0.02f, -5, 5);
            float ratio = 1 - (Mathf.Abs(offset * 2f) / 5);
            if (dragDir == DragDir.VerticalDown) offset *= -1;

            SelectedPuffObject.transform.position = new Vector3(0, offset, 8);

            puffInspectView.SetFunctionCanvas(ratio);
            puffInspectView.SetSemiText(dragDir == DragDir.VerticalDown ? GeneralFlag.String.SaveToMailbox : GeneralFlag.String.ReleaseBackToSky);

            if (ratio <= 0)
            {
                gestureEvent = dragDir == DragDir.VerticalDown ? GestureEvent.Save : GestureEvent.Release;
            }
            else {
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

        private void GraudaulyFlyToCenter() {
            SelectedPuffObject.transform.position = Vector3.Lerp(SelectedPuffObject.transform.position, new Vector3(0, 0, 8), 0.1f);

        }

        private void GraduallyRotateToFace(Face face)
        {
            recordRotationY = Mathf.Lerp(recordRotationY, rotDir, 0.1f);

            SelectedPuffObject.transform.rotation = Quaternion.Euler(0, recordRotationY, 0);
        }
        #endregion

        private bool SetCurrentSelectedObject(PuffItemView puffItem) {

            if (SelectedPuffObject != null && SelectedPuffObject.name == puffItem.name) {
                return false;
            } 

            Debug.Log("Hit something new");

            SelectedPuffObject = puffItem;
            SelectedPuffObject.CatchToFront();
            puffInspectView.Show(true);
            SetFaceInfo(Face.Front);      

            return true;
        }

        private void ReleaseSelectObject() {
            gestureEvent = GestureEvent.None;
            puffInspectView.Show(false);

            if (SelectedPuffObject != null) {
                SelectedPuffObject.Dismiss();
                SelectedPuffObject = null;
            }

            Debug.Log("Released");
        }
    }
}
