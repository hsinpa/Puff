using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View {
    public class PuffMsgFrontPage : PuffMsgInnerPage
    {
        [SerializeField]
        private Text title;

        [SerializeField]
        private Text semi_title;

        [SerializeField]
        private Text Description;

        [SerializeField]
        private Button ReplyBtn;

        public void SetContent(string p_title, string p_semititle, string p_description, System.Action ReplyBtnEvent) {
            title.text = p_title;
            semi_title.text = p_semititle;
            Description.text = p_description;

            ReplyBtn.onClick.RemoveAllListeners();
            ReplyBtn.onClick.AddListener(() => ReplyBtnEvent());
        }
    }
}
