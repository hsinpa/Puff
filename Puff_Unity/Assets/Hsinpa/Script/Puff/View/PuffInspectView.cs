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
        private Image saveSmokeEffect;

        [SerializeField]
        private ButtonManagerBasicIcon ReplyButton;

        [SerializeField]
        private CanvasGroup functionalPanelCanvas;

        private Vector2 cacheVector = Vector2.zero;
        private Color cacheColor = Color.white;

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

        public void SetSaveSmokeVisualEffect(float offset) {
            if (offset >= 0) {
                cacheVector.y = 0;
                cacheColor.a = 0;
                saveSmokeEffect.rectTransform.anchoredPosition = cacheVector;
                saveSmokeEffect.color = cacheColor;
                return;
            }

            float revertOffset = Mathf.Clamp(Mathf.Abs(offset) / 0.5f, 0 , 1);
            float size = saveSmokeEffect.rectTransform.sizeDelta.y * 0.5f;
            cacheVector.y = (size * revertOffset) - size;
            cacheColor.a = revertOffset;
            saveSmokeEffect.rectTransform.anchoredPosition = cacheVector;
            saveSmokeEffect.color = cacheColor;
        }
    }
}