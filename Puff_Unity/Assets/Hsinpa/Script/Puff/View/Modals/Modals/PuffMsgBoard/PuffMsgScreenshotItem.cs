using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;

namespace Puff.View
{
    public class PuffMsgScreenshotItem : MonoBehaviour
    {
        [SerializeField]
        private Button closeBtn;

        [SerializeField]
        private Button imageBtn;

        [SerializeField]
        private RawImage image;

        public void SetUp(Texture p_texture, System.Action<PuffMsgScreenshotItem> OnImageClick, System.Action<PuffMsgScreenshotItem> OnCloseBtnClick) {

            if (p_texture != null)
                this.image.texture = p_texture;

            UtilityMethod.SetSimpleBtnEvent<PuffMsgScreenshotItem>(imageBtn, OnImageClick, this);
            UtilityMethod.SetSimpleBtnEvent<PuffMsgScreenshotItem>(closeBtn, OnCloseBtnClick, this);
        }

    }
}