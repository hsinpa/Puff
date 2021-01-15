using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;

namespace Puff.View
{
    public class PuffMsgCameraModule : MonoBehaviour
    {
        [SerializeField]
        private Button cameraBtn;

        [SerializeField]
        private RectTransform screenshotHolder;

        [SerializeField]
        private Text indictator;

        private int _maxImageCount;

        private List<Texture> holdImages = new List<Texture>();

        private string indicatorText => string.Format(StringTextAsset.Messaging.CameraIndicator, holdImages.Count.ToString(), _maxImageCount.ToString());

        public void SetUp(int maxImageCount, System.Action OnCameraBtnClick) {
            this._maxImageCount = maxImageCount;
            
            cameraBtn.onClick.RemoveAllListeners();
            cameraBtn.onClick.AddListener(() =>
            {
                OnCameraBtnClick();
            });
        }

        public void CleanUp() {
            UtilityMethod.ClearChildObject(screenshotHolder, indictator.name);

            foreach (Texture t in holdImages) { 
                if (t != null)
                    UtilityMethod.SafeDestroy(t);
            }

            holdImages.Clear();

            indictator.text = indicatorText;
        }

        public void AssignRawImage(Texture p_texuture) {

            if (p_texuture != null) { 
            
            }

            holdImages.Add(p_texuture);
            indictator.text = indicatorText;
        }

    }
}