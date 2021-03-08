using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;

namespace Puff.View
{
    public class PuffHUDVIew : MonoBehaviour
    {
        [Header("Bottom HUD")]
        [SerializeField]
        private Transform ButtonHUDView;

        [SerializeField]
        private Button SendMsgBtn;

        [SerializeField]
        private Button ProfileBtn;

        [SerializeField]
        private Button CameraTakePicBtn;

        public enum HUDMode {Normal, Camera};

        public void SetCameraEvent(System.Action SendMsgEvent) {
            AssignBtnEvent(CameraTakePicBtn, SendMsgEvent);
        }

        public void SetBottomHUD(System.Action SendMsgEvent, System.Action ProfileOpenEvent) {
            AssignBtnEvent(SendMsgBtn, SendMsgEvent);
            AssignBtnEvent(ProfileBtn, ProfileOpenEvent);
        }

        public void EnableMode(HUDMode p_mode) {
            Modals.instance.CloseAll();

            ButtonHUDView.gameObject.SetActive(p_mode == HUDMode.Normal);
            CameraTakePicBtn.gameObject.SetActive(p_mode == HUDMode.Camera);
        }

        private void AssignBtnEvent(Button btn, System.Action SimpleEvent) {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => SimpleEvent());
        }

    }
}