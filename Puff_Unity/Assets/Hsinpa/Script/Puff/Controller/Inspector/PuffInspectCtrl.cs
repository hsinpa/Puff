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
using Puff.Model;

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
        private PuffModel _puffModel;

        private Vector3 sharedVectorUnit = new Vector3();

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event) {
                case EventFlag.Event.GameStart: {
                        SetUp();
                }
                break;

                case EventFlag.Event.EnterCameraMode:
                {
                        ShowInspectorUI(false);
                }
                break;

                case EventFlag.Event.ExitCameraMode:
                {
                        ShowInspectorUI(true);
                }
                break;
            }
        }

        private void SetUp()
        {
            _puffModel = PuffApp.Instance.models.puffModel;
            _puffInspectorInput = new PuffInspectorInput(SetCurrentSelectedObject, SetFaceInfo, ReleaseSelectObject, ProcessVertical, DragThreshold, _camera);

            puffHUDView.SetBottomHUD(() => {
                puffHUDView.EnableMode(PuffHUDVIew.HUDMode.Normal);
            },
            () => {
                PuffApp.Instance.Notify(EventFlag.Event.OpenSendMsg);
            },
            () => {

            },
            () =>
            {
                PuffApp.Instance.Notify(EventFlag.Event.OnProfileOpen);
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
                JsonTypes.PuffMessageType messageType = this._puffModel.GetMessageTypeByID(SelectedPuffObject.puffID);
                PuffApp.Instance.Notify(EventFlag.Event.OpenPuffMsg, messageType);
            });
        }

        private void Update()
        {
            _puffInspectorInput.OnUpdate();
        }

        private void ProcessVertical(PuffInspectorInput.DragDir dragDir, float ratio, float offset, Vector3 puff_center)
        {
            sharedVectorUnit.Set(puff_center.x, puff_center.y + offset, puff_center.z);

            SelectedPuffObject.transform.position = sharedVectorUnit;
            puffInspectView.SetFunctionCanvasAlpha(ratio);
            puffInspectView.SetSemiText(dragDir == PuffInspectorInput.DragDir.VerticalDown ? GeneralFlag.String.SaveToMailbox : GeneralFlag.String.ReleaseBackToSky);
            puffInspectView.SetSaveSmokeVisualEffect(offset);
            //Debug.Log("Vertical ratio " + ratio + ", offset "+ offset);
        }

        private bool SetCurrentSelectedObject(PuffItemView puffItem) {

            if (SelectedPuffObject != null && SelectedPuffObject.name == puffItem.name) {
                return false;
            } 

            SelectedPuffObject = puffItem;
            SelectedPuffObject.CatchToFront();
            _puffInspectorInput.SetInputSelectObject(puffItem);

            puffInspectView.Show(true);
            SetFaceInfo(PuffInspectorInput.Face.Front);      

            return true;
        }

        private void ReleaseSelectObject(bool releaseBool) {

            if (releaseBool) {
                puffInspectView.Show(false);

                if (SelectedPuffObject != null)
                {
                    SelectedPuffObject.Dismiss();
                    SelectedPuffObject = null;

                    _puffInspectorInput.SetInputSelectObject(SelectedPuffObject);
                }
            }

            //0 = hide
            puffInspectView.SetSaveSmokeVisualEffect(offset: 0);
        }

        private void ShowInspectorUI(bool isShow) {
            puffInspectView.Show(isShow);
            if (SelectedPuffObject != null)
                SelectedPuffObject.gameObject.SetActive(isShow);
        }
    }
}
