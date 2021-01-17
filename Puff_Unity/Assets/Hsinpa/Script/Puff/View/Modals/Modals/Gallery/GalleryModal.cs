using Hsinpa.Utility;
using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class GalleryModal : Modal
    {
        [SerializeField]
        private RawImage mainImage;

        //protected override void Start()
        //{
        //    base.Start();
        //}

        public void SetUp(List<Texture> textures, System.Action CloseEvent) {
            if (textures != null && textures.Count > 0)
            {
                DisplayTexture(textures[0]);

                Button btn = mainImage.GetComponent<Button>();
                UtilityMethod.SetSimpleBtnEvent(btn, CloseEvent);
            }
        }

        private void DisplayTexture(Texture texture) {
            if (texture == null) return;

            float ratio = 1;
            Rect holderRect = this.GetComponent<RectTransform>().rect;

            //Debug.Log($"texture.height {texture.height}, texture.width {texture.width}, Screen.height {Screen.height}, Screen.width {Screen.width}");

            if (texture.height > texture.width)
            {
                float offset = holderRect.height - texture.height;
                ratio = (texture.height + offset) / texture.height;
            }
            else {
                float offset = holderRect.width - texture.width;
                ratio = (texture.width + offset) / texture.width;
            }

            mainImage.texture = texture;
            mainImage.rectTransform.sizeDelta = new Vector2(texture.width * ratio, texture.height * ratio);
        }

    }
}