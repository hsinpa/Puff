using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Hsinpa.View {
    public class HUDToastView : MonoBehaviour
    {

        [SerializeField]
        private RectTransform rectTran;

        [SerializeField]
        private Text messageText;

        private static HUDToastView _instance;

        public static HUDToastView instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<HUDToastView>();
                }
                return _instance;
            }
        }

        private bool isShowed = false;

        public void ShowMessage(string message, float duration) {

            //If current message is showing, then ignore
            if (isShowed) return;
            isShowed = true;

            var rect = rectTran.rect;
            float width = rect.width - 20;

            messageText.text = message;

            rectTran.DOKill();
            rectTran.DOAnchorPosX(-width, 0.3f);

            duration = 0.3f + duration;
            _ = Utility.UtilityMethod.DoDelayWork(duration, Close);
        }

        private void Close() {
            isShowed = false;

            rectTran.DOKill();
            rectTran.DOAnchorPosX(0, 0.2f);

            messageText.text = "";
        }
    }
}
