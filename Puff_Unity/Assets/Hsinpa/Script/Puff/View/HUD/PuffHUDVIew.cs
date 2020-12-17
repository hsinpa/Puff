using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffHUDVIew : MonoBehaviour
    {
        [Header("Bottom HUD")]
        [SerializeField]
        private Transform ButtonHUDView;

        [SerializeField]
        private Button SendMsgBtn;

        public void SetBottomHUD(System.Action SendMsgEvent) {
            AssignBtnEvent(SendMsgBtn, SendMsgEvent);
        }

        private void AssignBtnEvent(Button btn, System.Action SimpleEvent) {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => SimpleEvent());
        }

    }
}