using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.View;
using Hsinpa.View;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;
using TMPro;
using System.Diagnostics.Tracing;
using Puff.Ctrl.Utility;

namespace Puff.Ctrl {
    public class PuffInspectCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private PuffInspectView puffInspectView;

        [SerializeField]
        private PuffHUDVIew puffHUDView;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private PuffItemView SelectedPuffObject;

        [SerializeField, Range(0.1f, 200)]
        private float DragThreshold = 0.1f;

        private PuffInspectorInput _puffInspectorInput;

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
            _puffInspectorInput = new PuffInspectorInput(SetCurrentSelectedObject, SetFaceInfo, ReleaseSelectObject, ProcessVertical, DragThreshold, _camera);

            puffHUDView.SetBottomHUD(() => {
                PuffApp.Instance.Notify(EventFlag.Event.OpenSendMsg);
            });

            puffInspectView.Show(false);
            SetInspectViewEvent();
        }

        private void SetFaceInfo(PuffInspectorInput.Face p_face) {
            puffInspectView.SetUp(SelectedPuffObject.name, p_face.ToString("g"));
        }

        private void SetInspectViewEvent() {
            puffInspectView.SetReplyEvent(() =>
            {
                PuffApp.Instance.Notify(EventFlag.Event.OpenPuffMsg);
            });
        }

        private void Update()
        {
            _puffInspectorInput.OnUpdate();
        }

        private void ProcessVertical(PuffInspectorInput.DragDir dragDir, float ratio, float offset)
        {
            SelectedPuffObject.transform.position = new Vector3(0, offset, 8);
            puffInspectView.SetFunctionCanvasAlpha(ratio);
            puffInspectView.SetSemiText(dragDir == PuffInspectorInput.DragDir.VerticalDown ? GeneralFlag.String.SaveToMailbox : GeneralFlag.String.ReleaseBackToSky);
        }

        private bool SetCurrentSelectedObject(PuffItemView puffItem) {

            if (SelectedPuffObject != null && SelectedPuffObject.name == puffItem.name) {
                return false;
            } 

            Debug.Log("Hit something new");

            SelectedPuffObject = puffItem;
            SelectedPuffObject.CatchToFront();
            _puffInspectorInput.SetInputSelectObject(puffItem);

            puffInspectView.Show(true);
            SetFaceInfo(PuffInspectorInput.Face.Front);      

            return true;
        }

        private void ReleaseSelectObject() {
            puffInspectView.Show(false);

            if (SelectedPuffObject != null) {
                SelectedPuffObject.Dismiss();
                SelectedPuffObject = null;

                _puffInspectorInput.SetInputSelectObject(SelectedPuffObject);
            }

            Debug.Log("Released");
        }
    }
}
