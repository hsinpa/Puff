using DG.Tweening;
using Hsinpa.Utility;
using Hsinpa.View;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View {
    public class PuffMsgFrontPage : PuffMsgInnerPage
    {
        [SerializeField]
        private Text title;

        [SerializeField]
        private Text author;

        [SerializeField]
        private Text create_date;

        [SerializeField]
        private Text Description;

        [SerializeField]
        private InputField _ReplyInputfield;
        private InputField ReplyInputfield => _ReplyInputfield;

        [SerializeField]
        private Transform FrontMsgPage;

        [SerializeField]
        private Transform BackMsgPage;

        [Header("Action Buttons")]
        [SerializeField]
        private Button _ReplyBtn;
        private Button ReplyBtn => _ReplyBtn;

        [SerializeField]
        private Button RotateBtn;

        [SerializeField]
        private Button IrrigateBtn;

        [SerializeField]
        private Button ToLibraryBtn;

        [SerializeField]
        private Button ProfileBtn;

        [Header("Comment")]
        [SerializeField]
        private RectTransform commentsHolder;

        [SerializeField]
        private PuffMsgCommentItem commentItemPrefab;

        [Header("Thumb Photo")]
        [SerializeField]
        private RectTransform thumbHolder;

        [SerializeField]
        private PuffMsgThumbItem thumbItemPrefab;

        [SerializeField]
        private Material cropMat;

        [Header("Other")]
        [SerializeField]
        private Image SaveToLibraryBadge;

        [SerializeField]
        private Image leafImage;

        [SerializeField]
        private Sprite greanLeaf;

        [SerializeField]
        private Sprite waterLeaf;


        private List<PuffMsgThumbItem> cacheThumbItems = new List<PuffMsgThumbItem>();

        private List<RenderTexture> cacheThumbs = new List<RenderTexture>();
        private int thumbSize = 256;

        public override void SetUp() {
            PrepareCacheTexture(maxThumb: 4);
        }

        public void SetContent(JsonTypes.PuffMessageType puffMsgType, bool IsMsgSaveToLibrary, System.Action IrrigateBtnEvent,
            System.Action<JsonTypes.PuffMessageType> SaveToLibraryBtnEvent, System.Action<string, string> ProfileBtnEvent,
            System.Action<string> ReplyBtnEvent) {

            title.text = puffMsgType.title;
            author.text = puffMsgType.author;
            create_date.text = puffMsgType.parseDate.ToString("MM/dd/yyyy hh:mm tt");
            Description.text = puffMsgType.body;
            SaveToLibraryBadge.enabled = IsMsgSaveToLibrary;
            leafImage.sprite = greanLeaf;
            _ReplyInputfield.text = "";

            CleanThumbnails();

            _ReplyBtn.onClick.RemoveAllListeners();
            _ReplyBtn.onClick.AddListener(() => {
                if (!string.IsNullOrEmpty(_ReplyInputfield.text))
                    ReplyBtnEvent(_ReplyInputfield.text);
            });

            if (puffMsgType.comments != null)
                GenerateComments(puffMsgType.comments);

            if (puffMsgType.images != null)
                GenerateThumbnails(puffMsgType.images);

            RotateBtn.onClick.RemoveAllListeners();
            RotateBtn.onClick.AddListener(OnRotation);

            UtilityMethod.SetSimpleBtnEvent(IrrigateBtn, () => {
                IrrigateBtnEvent();
                leafImage.sprite = waterLeaf;
            });

            UtilityMethod.SetSimpleBtnEvent<JsonTypes.PuffMessageType>(ToLibraryBtn, SaveToLibraryBtnEvent, puffMsgType);

            ProfileBtn.onClick.RemoveAllListeners();
            ProfileBtn.onClick.AddListener(() => {
                ProfileBtnEvent(puffMsgType.author_id, puffMsgType.author);
            });
        }

        private void GenerateComments(List<JsonTypes.PuffCommentType> commentList) {
            int maxLength = 3;
            int commentCount = commentList.Count;
            int commentStopIndex = (int)Mathf.Clamp(commentCount - maxLength, 0, commentCount);

            UtilityMethod.ClearChildObject(commentsHolder);

            for (int i = commentCount - 1; i >= commentStopIndex; i--)
            {
                InsertNewComments(commentList[i]);
            }
        }

        public void InsertNewComments(JsonTypes.PuffCommentType comment) {
            PuffMsgCommentItem commentItem = UtilityMethod.CreateObjectToParent<PuffMsgCommentItem>(commentsHolder, commentItemPrefab.gameObject);

            commentItem.name = comment._id;
            commentItem.SetComment(comment);
        }

        public void EnableReplyInput(bool enable) {
            ReplyBtn.interactable = enable;

            if (enable) {
                _ReplyInputfield.text = "";
            }
        }

        private void GenerateThumbnails(List<string> urls) {
            int urlCount = urls.Count;
            int maxCount = cacheThumbItems.Count;


            for (int i = 0; i < urlCount; i++) {
                if (i < maxCount && !string.IsNullOrEmpty(urls[i])) {
                    GenerateThumbnail(cacheThumbItems[i], i, urls[i]);
                }
            }
        }

        private void GenerateThumbnail(PuffMsgThumbItem item, int index, string url)
        {
            TextureUtility.GetTexture(url, (Texture2D texture) => {
                item.gameObject.SetActive(true);

                TextureUtility.TextureStructure textureStructure = TextureUtility.GrabTextureRadius(texture.width, texture.height, 1);
                Texture cropThumbnail = TextureUtility.RotateAndScaleImage(texture, cacheThumbs[index], cropMat, textureStructure, 0);

                item.SetThumbnail(cropThumbnail, url);
            });
        }

        private void CleanThumbnails() {
            foreach (Transform t in thumbHolder.transform) {
                t.gameObject.SetActive(false);
            }
        }

        private void PrepareCacheTexture(int maxThumb)
        {
            for (int i = 0; i < maxThumb; i++) {
                cacheThumbs.Add(
                    TextureUtility.GetRenderTexture(thumbSize)
                );

                PuffMsgThumbItem thumbItem = UtilityMethod.CreateObjectToParent<PuffMsgThumbItem>(thumbHolder, thumbItemPrefab.gameObject);

                thumbItem.SetUp(OnThumbnailClick);
                thumbItem.gameObject.SetActive(false);
                cacheThumbItems.Add(thumbItem);
            }
        }

        private void OnThumbnailClick(PuffMsgThumbItem p_item) {
            GalleryModal gallery = Modals.instance.OpenModal<GalleryModal>();

            TextureUtility.GetTexture(p_item.texture_id, (Texture2D t) => {
                gallery.SetUp(new List<Texture>() { t }, () => {
                    Modals.instance.Close();
                    Modals.instance.OpenModal<PuffMessageModal>(); 
                });
            });
        }

        private void OnRotation() {
            Vector3 startRot = new Vector3(0, 90, 0);
            Vector3 returnRot = new Vector3(0, 0, 0);

            this.transform.DORotate(startRot, 0.3f);

            _ = UtilityMethod.DoDelayWork(0.55f, () =>
            {
                this.transform.DORotate(returnRot, 0.5f);

                this.FrontMsgPage.gameObject.SetActive(!this.FrontMsgPage.gameObject.activeInHierarchy);
                this.BackMsgPage.gameObject.SetActive(!this.BackMsgPage.gameObject.activeInHierarchy);
            });
        }
    }
}