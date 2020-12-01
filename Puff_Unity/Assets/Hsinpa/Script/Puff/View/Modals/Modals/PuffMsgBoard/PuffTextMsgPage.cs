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

        public void SetUp(System.Action<string> submitEvent, System.Action cancelEvent) {
            this.submitBtn.onClick.RemoveAllListeners();
            this.submitBtn.onClick.AddListener(() => {
                submitEvent(msgText.text);
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