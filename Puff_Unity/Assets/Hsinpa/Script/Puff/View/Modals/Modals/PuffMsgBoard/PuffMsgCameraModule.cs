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

        [SerializeField]
        private PuffMsgScreenshotItem PrefabScreenshotItem;

        [SerializeField]
        private Texture DebugTexture;

        private const int _maxImageCount = 3;

        private List<Texture> _textureList = new List<Texture>();
        public List<Texture> textureList => _textureList;

        private string indicatorText => string.Format(StringTextAsset.Messaging.CameraIndicator, _textureList.Count.ToString(), _maxImageCount.ToString());

        public bool isTakePhotoAvailable => _textureList.Count < _maxImageCount;

        public void SetUp(System.Action OnCameraBtnClick) {            
            cameraBtn.onClick.RemoveAllListeners();
            cameraBtn.onClick.AddListener(() =>
            {
                OnCameraBtnClick();
            });
        }

        public void CleanUp() {
            UtilityMethod.ClearChildObject(screenshotHolder, indictator.name);

            foreach (Texture t in _textureList) { 
                if (t != null)
                    UtilityMethod.SafeDestroy(t);
            }

            _textureList.Clear();

            indictator.text = indicatorText;
        }

        public void AssignRawImage(Texture p_texuture) {

            PuffMsgScreenshotItem screenshotItem = UtilityMethod.CreateObjectToParent<PuffMsgScreenshotItem>(screenshotHolder, PrefabScreenshotItem.gameObject);

            screenshotItem.SetUp(p_texuture, OnImageItemClick, OnImageItemRemove);

            screenshotItem.transform.SetSiblingIndex(0);
            
            _textureList.Add(p_texuture);
            indictator.text = indicatorText;
        }

        private void OnImageItemClick(PuffMsgScreenshotItem p_item) {
            int index = p_item.transform.GetSiblingIndex();
        }

        private void OnImageItemRemove(PuffMsgScreenshotItem p_item) {
            int index = p_item.transform.GetSiblingIndex();

            _textureList.RemoveAt(index);

            UtilityMethod.SafeDestroy(p_item.gameObject);

            indictator.text = indicatorText;
        }

        public List<byte[]> GetTextureBytes() {
            List<byte[]> bytesList = new List<byte[]>();

            foreach (Texture t in textureList) {
                if (t != null)
                    bytesList.Add(TextureUtility.TextureToTexture2D(t).EncodeToJPG());
            }

            //bytesList.Add(TextureUtility.TextureToTexture2D(DebugTexture).EncodeToJPG());

            return bytesList;
        }

    }
}