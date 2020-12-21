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

        [SerializeField]
        private CanvasGroup functionalPanelCanvas;

        public void SetUp(string title, string semiTitle) {
            base.Show(true);

            SetFunctionCanvasAlpha(1);

            titleTxt.text = title;
            SetSemiText(semiTitle);

        }

        public void SetSemiText(string semiText) {
            functionText.text = semiText;
        }

        public void SetFunctionCanvasAlpha(float alpha) {
            functionalPanelCanvas.alpha = alpha;

            functionalPanelCanvas.interactable = alpha > 0.95f;
            functionalPanelCanvas.blocksRaycasts = alpha > 0.95f;            
        }

        public void SetReplyEvent(System.Action p_event) {
            ReplyButton.clickEvent.RemoveAllListeners();
            ReplyButton.clickEvent.AddListener(() => p_event());
        }
    }
}