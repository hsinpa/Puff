using Hsinpa.Utility;
using Hsinpa.View;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffMsgThumbItem : MonoBehaviour
    {
        [SerializeField]
        private RawImage thumbImage;

        [SerializeField]
        private Button thumbBtn;

        public string texture_id;

        public void SetUp(System.Action<PuffMsgThumbItem> p_action) {
            UtilityMethod.SetSimpleBtnEvent<PuffMsgThumbItem>(thumbBtn, p_action, this);
        }

        public void SetThumbnail(Texture cropImage, string texture_id) {
            this.texture_id = texture_id;
            thumbImage.texture = cropImage;
        }
    }
}
