using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View {
    public class PuffMsgHeader : MonoBehaviour
    {

        [SerializeField]
        private Button CloseBtn;

        [SerializeField]
        private Button BackBtn;

        public void HideBackBtn()
        {
            CloseBtn.gameObject.SetActive(true);
            BackBtn.gameObject.SetActive(false);
        }

        public void ShowBackBtn(System.Action p_backEvent) {
            BackBtn.gameObject.SetActive(true);
            CloseBtn.gameObject.SetActive(false);

            BackBtn.onClick.RemoveAllListeners();
            BackBtn.onClick.AddListener(() => p_backEvent());
        }

    }
}