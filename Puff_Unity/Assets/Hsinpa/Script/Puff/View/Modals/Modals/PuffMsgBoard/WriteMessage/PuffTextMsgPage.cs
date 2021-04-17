using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;
using System.Linq;
using Puff.Ctrl.Utility;
using System.Runtime.CompilerServices;
using Hsinpa.View;

namespace Puff.View
{
    public class PuffTextMsgPage : PuffMsgInnerPage
    {
        [Header("Puff type selection page")]
        [SerializeField]
        private PuffMsgTypePanel puffMsgTypePanel;

        [Header("Modules")]
        [SerializeField]
        private Transform moduleContainer;

        [SerializeField]
        private InputField titleText;

        [SerializeField]
        private InputField msgText;

        [SerializeField]
        private PuffMsgReivewModule reviewModule;

        [SerializeField]
        private PuffMsgCameraModule _cameraModule;
        public PuffMsgCameraModule cameraModule => _cameraModule;

        [SerializeField]
        private PuffMsgSliderModule sliderModule;

        [Header("Tab")]
        [SerializeField]
        private PuffMsgTabModule privacyTabHolder;

        [SerializeField]
        private PuffMsgDurationModule durationTabHolder;

        [Header("Action Buttons")]
        [SerializeField]
        private CustomButton submitBtn;

        [SerializeField]
        private Button backBtn;

        [SerializeField]
        private Button addModuleBtn;

        [SerializeField]
        private PuffMsgAddModulePanel addModulePanel;

        //Privacy is hidden page
        public enum Privacy { Public, Friend, Private };
        public enum Distance { Near = 0, Medium, Far, World };
        private enum Page {TypeSelection, MainContent, SideContent }

        private Dictionary<int, JsonTypes.GeographicRange> distTable = new Dictionary<int, JsonTypes.GeographicRange>();

        private ColorItemSObj colorSetting;
        private AccountModel accountModel;
        private Page currentPage = Page.TypeSelection;

        public delegate void OnPuffMsgSend(JsonTypes.PuffMessageType content, List<byte[]> bytes);
        private OnPuffMsgSend OnPuffMsgSendCallback;

        public override void SetUp()
        {
            colorSetting = PuffApp.Instance.models.colorSetting;

            privacyTabHolder.SetUp(colorSetting, null);

            sliderModule.SetSlider(0, 0, 3, true, OnDistanceSliderChange);

            addModulePanel.SetUp(OnModuleIsAppended);
            UtilityMethod.SetSimpleBtnEvent(addModuleBtn, OnAddModulePanelExpand);
            UtilityMethod.SetSimpleBtnEvent(backBtn, BackToPreviousPage);

            distTable = new Dictionary<int, JsonTypes.GeographicRange>()
            {
                { (int)Distance.Near, new JsonTypes.GeographicRange(string.Format(StringTextAsset.Messaging.DistanceNear, 100), 0.1f) },
                { (int)Distance.Medium, new JsonTypes.GeographicRange(string.Format(StringTextAsset.Messaging.DistanceMedium, 1), 1) },
                { (int)Distance.Far,  new JsonTypes.GeographicRange(string.Format(StringTextAsset.Messaging.DistanceFar, 10), 10) },
                { (int)Distance.World, new JsonTypes.GeographicRange(StringTextAsset.Messaging.DistanceWorld, 6371) }
            };
        }

        public void SetContent(AccountModel accountModel, OnPuffMsgSend onPuffMsgSendEvent, System.Action OnCameraClick) {
            this.accountModel = accountModel;
            this.OnPuffMsgSendCallback = onPuffMsgSendEvent;

            CleanContent();
            privacyTabHolder.SetClickTab((int)Privacy.Public);
            durationTabHolder.SetValue(append_date: 3);

            cameraModule.SetUp(OnCameraClick);

            this.submitBtn.onClick.RemoveAllListeners();
            this.submitBtn.onClick.AddListener(() => {
                OnSubmitButtonClick();
            });

            //SetStoryLayout();
            SetSelectTypePage();
        }

        #region Tab Layout
        private void SetSelectTypePage() {
            currentPage = Page.TypeSelection;
            submitBtn.SetTitle(StringTextAsset.Messaging.SubmitBtnNext);
            moduleContainer.gameObject.SetActive(false);
            addModuleBtn.gameObject.SetActive(false);
            puffMsgTypePanel.Show(true);
        }

        private void SetStoryLayout() {
            currentPage = Page.MainContent;
            puffMsgTypePanel.Show(false);
            moduleContainer.gameObject.SetActive(true);
            reviewModule.gameObject.SetActive(reviewModule.score > 0);
            _cameraModule.gameObject.SetActive(_cameraModule.textureList.Count > 0);
            submitBtn.SetTitle(StringTextAsset.Messaging.SubmitBtnNext);

            addModuleBtn.gameObject.SetActive(true);
            addModulePanel.ResetButtons();
        }

        private void SetPrivacyLayout() {
            currentPage = Page.SideContent;
            FrontPageBasicLayout(false);
        
            reviewModule.gameObject.SetActive(false);
            _cameraModule.gameObject.SetActive(false);
            submitBtn.SetTitle(StringTextAsset.Messaging.SubmitBtnShare);
        }

        private void FrontPageBasicLayout(bool enable)
        {
            msgText.gameObject.SetActive(enable);
            titleText.gameObject.SetActive(enable);

            privacyTabHolder.gameObject.SetActive(!enable);
            durationTabHolder.gameObject.SetActive(!enable);
            sliderModule.gameObject.SetActive(!enable);
            addModuleBtn.gameObject.SetActive(enable);
            backBtn.gameObject.SetActive(!enable);
        }

        private void BackToPreviousPage() {
            FrontPageBasicLayout(true);
            SetStoryLayout();
        }

        #endregion
        private void OnSubmitButtonClick() {


            switch (currentPage) {

                case Page.TypeSelection:
                {
                    SetStoryLayout();
                }
                break;

                case Page.MainContent:
                {
                    if (string.IsNullOrEmpty(msgText.text) || string.IsNullOrEmpty(titleText.text))
                    {
                            HUDToastView.instance.Toast(StringTextAsset.Messaging.WarningNoMessageOrTitle, 4, GeneralFlag.Colors.ToastColorNormal);

                        return;
                    }

                    SetPrivacyLayout();
                }
                break;

                case Page.SideContent:
                    {
                        var allImageBytes = cameraModule.GetTextureBytes();

                        GPSLocationService.GetGPS(this, false, (GPSLocationService.LocationInfo locationInfo) =>
                        {
                            if (distTable.TryGetValue((int)sliderModule.sliderValue, out JsonTypes.GeographicRange gRange))
                            {
                                var puffMsgType = PuffMsgBoardHelper.GetCreateMessageType(
                                    (int)this.puffMsgTypePanel.SelectedType,
                                    this.accountModel.puffAccountType._id,
                                    this.accountModel.puffAccountType.username,
                                    msgText.text,
                                    titleText.text,
                                    privacyTabHolder.CurrentIndex,
                                    durationTabHolder.date,
                                    gRange.kilometer_radius,
                                    locationInfo
                                );

                                this.OnPuffMsgSendCallback(puffMsgType, allImageBytes);
                                msgText.text = "";
                            }
                        });

                    }
                    break;
            }
        }

        private void OnDistanceSliderChange(float p_index) {
            int index = (int)p_index;

            if (distTable.TryGetValue(index, out JsonTypes.GeographicRange gRange)) {
                sliderModule.SetSliderField(gRange.name);
            }
        }

        private void OnAddModulePanelExpand()
        {
            bool isExpand = !addModulePanel.isActive;

            addModulePanel.Show(isExpand);
        }

        private void OnModuleIsAppended(Button btn)
        {
            btn.interactable = false;

            switch (btn.name) {
                case EventFlag.ModuleActionButton.CameraBtn:
                    cameraModule.gameObject.SetActive(true);
                    break;

                case EventFlag.ModuleActionButton.ReviewBtn:
                    reviewModule.gameObject.SetActive(true);
                    break;
            }

            addModulePanel.Show(false);
        }

        private void CleanContent()
        {
            FrontPageBasicLayout(true);

            msgText.text = "";
            titleText.text = "";
            reviewModule.SetScore(0);

            privacyTabHolder.gameObject.SetActive(false);
            durationTabHolder.gameObject.SetActive(false);
            //buttonModule.gameObject.SetActive(false);
            OnDistanceSliderChange(0);
            puffMsgTypePanel.SetPuffType(JsonTypes.PuffTypes.FloatSeed);

            addModulePanel.Show(false);
            addModulePanel.ResetButtons();
            _cameraModule.CleanUp();
        }
    }
}