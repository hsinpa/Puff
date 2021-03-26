using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Puff.View
{
    public class PuffMsgAddModulePanel : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Button[] buttons;

        public System.Action<Button> OnButtonClick;

        public bool isActive => canvasGroup.interactable;

        public void Show(bool isShow) {
            canvasGroup.DOKill();
            canvasGroup.DOFade((isShow) ? 1 : 0, 0.1f);
            canvasGroup.interactable = isShow;
            canvasGroup.blocksRaycasts = isShow;


        }

        public void SetUp(System.Action<Button> buttonCallback) {

            int buttonCount = buttons.Length;

            for (int i = 0; i < buttonCount; i++) {
                var btn = buttons[i];
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => buttonCallback(btn));
            }
        }

        public void ResetButtons() {
            int buttonCount = buttons.Length;

            for (int i = 0; i < buttonCount; i++)
            {
                //buttons[i].onClick.RemoveAllListeners();
                buttons[i].interactable = true;
            }
        }

    }
}