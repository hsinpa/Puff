using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffMsgCameraModule : MonoBehaviour
    {
        [SerializeField]
        private Button cameraBtn;

        [SerializeField]
        private RawImage rawImagePic;

        public void SetUp(System.Action OnCameraBtnClick) {
            cameraBtn.onClick.RemoveAllListeners();
            cameraBtn.onClick.AddListener(() =>
            {
                OnCameraBtnClick();
            });
        }

        public void AssignRawImage(Texture p_texuture) {
            rawImagePic.texture = p_texuture;
        }

    }
}