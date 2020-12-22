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

        [SerializeField]
        private Button cancelBtn;

        public delegate void OnPuffMsgSend(string content);


        public void SetUp(OnPuffMsgSend onPuffMsgSendEvent, System.Action cancelEvent) {
            this.submitBtn.onClick.RemoveAllListeners();
            this.submitBtn.onClick.AddListener(() => {
                onPuffMsgSendEvent(msgText.text);
                msgText.text = "";
            });

            this.cancelBtn.onClick.RemoveAllListeners();
            this.cancelBtn.onClick.AddListener(() => {
                msgText.text = "";
                cancelEvent();
            });
        }
    }
}