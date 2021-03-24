using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Hsinpa.Utility;

namespace Hsinpa.View
{
    public class IrrigatePanelView : BaseView
    {
        [SerializeField]
        private Slider expSlider;

        [SerializeField]
        private Text expText;

        [SerializeField]
        private Text subMessasge;

        [SerializeField]
        private Image irrigateSprite;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
            irrigateSprite.color = (isShow) ? GeneralFlag.Colors.DisplayColor : GeneralFlag.Colors.HideColor;
        }

        public void SetContent(string appendExpStr, string subMessageStr, System.Action OnAnimationEndEvent) {
            expText.text = appendExpStr;
            subMessasge.text = subMessageStr;

            this.Show(true, 0.4f);

            irrigateSprite.DOFade(1, 0.4f);

            _ = UtilityMethod.DoDelayWork(4, () =>
            {
                this.Show(false, 0.2f);
                irrigateSprite.DOFade(0, 0.2f);

                if (OnAnimationEndEvent != null)
                    OnAnimationEndEvent();
            });
        }

    }
}