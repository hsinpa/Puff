using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffTextMsgPage : PuffMsgInnerPage
    {
        [SerializeField]
        private InputField msgText;

        [SerializeField]
        private Button submitBtn;

        public delegate void OnPuffMsgSend(string content);

        public void SetUp(OnPuffMsgSend onPuffMsgSendEvent) {

            msgText.text = "";

            this.submitBtn.onClick.RemoveAllListeners();
            this.submitBtn.onClick.AddListener(() => {

                if (string.IsNullOrEmpty(msgText.text)) return;

                onPuffMsgSendEvent(msgText.text);
                msgText.text = "";
            });
        }
    }
}