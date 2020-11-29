using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.View;
using Hsinpa.View;

namespace Puff.Ctrl {
    public class PuffInspectCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private PuffInspectView puffInspectView;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private GameObject SelectedPuffObject;

        [SerializeField, Range(0.1f, 2)]
        private float DragThreshold = 0.1f;

        private RaycastHit[] raycastHits = new RaycastHit[1];

        bool hasHitOnPuffObj = false;
        Vector3 lastStandPoint;

        public enum Face {Front, RightSide, Back, LeftSide};
        private Face currentFace = Face.Front;
        private int rotDir = 1;
        private float recordRotationY;

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
            SetFaceInfo(currentFace);
            SetInspectViewEvent();
        }

        private void SetFaceInfo(Face p_face) {
            puffInspectView.SetUp(SelectedPuffObject.name, currentFace.ToString("g"));
        }

        private void SetInspectViewEvent() {
            puffInspectView.SetReplyEvent(() =>
            {
                OnOpenPuffMsg();
            });
        }

        private void OnOpenPuffMsg() {
            PuffMessageModal puffMessageModal = Modals.instance.OpenModal<PuffMessageModal>();
            PuffMsgFrontPage frontPage = puffMessageModal.GetPuffFrontPage();
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
                }
            }

            if (!hasHitOnPuffObj)
            {
                GraduallyRotateToFace(currentFace);
                return;
            }

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
            }
        }

        private bool HasHitPuffObject()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 10, GeneralFlag.Puff.Layer);

            return hitCount > 0;
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

        private void GraduallyRotateToFace(Face face)
        {

            recordRotationY = Mathf.Lerp(recordRotationY, rotDir, 0.1f);

            SelectedPuffObject.transform.rotation = Quaternion.Euler(0, recordRotationY, 0);
        }
        #endregion
    }
}
