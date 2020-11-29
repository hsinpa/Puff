using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;
using Michsky.UI.ModernUIPack;

namespace Puff.View
{
    public class PuffInspectView : BaseView
    {
        [SerializeField]
        private Text titleTxt;

        [SerializeField]
        private Text functionText;

        [SerializeField]
        private ButtonManagerBasicIcon ReplyButton;

        public void SetUp(string title, string semiTitle) {
            base.Show(true);

            titleTxt.text = title;
            functionText.text = semiTitle;
        }

        public void SetReplyEvent(System.Action p_event) {
            ReplyButton.clickEvent.RemoveAllListeners();
            ReplyButton.clickEvent.AddListener(() => p_event());
        }
    }
}