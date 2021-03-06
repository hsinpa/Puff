﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Hsinpa.View {
    public class BaseView : MonoBehaviour
    {

        [SerializeField]
        protected CanvasGroup canvasGroup;

        public virtual void Show(bool isShow) {
            if (canvasGroup != null) {
                canvasGroup.alpha = (isShow) ? 1 : 0;
                canvasGroup.blocksRaycasts = isShow;
                canvasGroup.interactable = isShow;
            }
        }

        public virtual void Show(bool isShow, float animTime)
        {
            if (canvasGroup != null)
            {
                int targetAlpha = (isShow) ? 1 : 0;

                canvasGroup.DOKill();
                canvasGroup.DOFade(targetAlpha, animTime);
                canvasGroup.blocksRaycasts = isShow;
                canvasGroup.interactable = isShow;
            }
        }

        public bool isShow => canvasGroup.alpha == 1;
    }
}
