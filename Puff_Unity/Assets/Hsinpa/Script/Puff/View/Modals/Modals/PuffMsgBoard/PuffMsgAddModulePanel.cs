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

        public void Show(bool isShow) {
            canvasGroup.DOKill();
            canvasGroup.DOFade((isShow) ? 1 : 0, 0.1f);
            canvasGroup.interactable = isShow;
            canvasGroup.blocksRaycasts = isShow;
        }

        public void SetUp(System.Action<Button> buttonCallback) {

            int buttonCount = buttons.Length;

            for (int i = 0; i < buttonCount; i++) {
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() => buttonCallback(buttons[i]));
            }
        }

        public void ResetButtons() {
            int buttonCount = buttons.Length;

            for (int i = 0; i < buttonCount; i++)
            {
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].interactable = true;
            }
        }

    }
}