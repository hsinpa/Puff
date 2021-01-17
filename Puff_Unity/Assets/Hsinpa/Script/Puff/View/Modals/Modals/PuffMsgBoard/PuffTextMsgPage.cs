using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;
using System.Linq;
using Puff.Ctrl.Utility;
using System.Runtime.CompilerServices;

namespace Puff.View
{
    public class PuffTextMsgPage : PuffMsgInnerPage
    {
        [SerializeField]
        private InputField titleText;

        [SerializeField]
        private InputField msgText;

        [SerializeField]
        private Button submitBtn;

        [SerializeField]
        private PuffMsgButtonModule buttonModule;

        [SerializeField]
        private PuffMsgReivewModule reviewModule;

        [SerializeField]
        private PuffMsgCameraModule _cameraModule;
        public PuffMsgCameraModule cameraModule => _cameraModule;

        [SerializeField]
        private PuffMsgSliderModule sliderModule;

        [Header("Tab")]
        [SerializeField]
        private PuffMsgTabModule typeTabHolder;

        [SerializeField]
        private PuffMsgTabModule privacyTabHolder;

        [SerializeField]
        private PuffMsgTabModule durationTabHolder;

        //Privacy is hidden page
        public enum Tabs { Story, Review, Event, Survey, Privacy};
        public enum Privacy { Public, Friend, Private };
        public enum Duration { Date, Week, Month };
        public enum Distance { Near = 0, Medium, Far, World};

        private ColorItemSObj colorSetting;
        private AccountModel accountModel;
        private Dictionary<int, System.Action> TabActionDictTable = new Dictionary<int, System.Action>();

        public delegate void OnPuffMsgSend(JsonTypes.PuffMessageType content, List<byte[]> bytes);
        private OnPuffMsgSend OnPuffMsgSendCallback;

        public override void SetUp()
        {
            colorSetting = PuffApp.Instance.models.colorSetting;       

            TabActionDictTable.Add((int)Tabs.Event, SetEventLayout);
            TabActionDictTable.Add((int)Tabs.Review, SetReviewLayout);
            TabActionDictTable.Add((int)Tabs.Story, SetStoryLayout);

            typeTabHolder.SetUp(colorSetting, TabActionDictTable);
            privacyTabHolder.SetUp(colorSetting, null);
            durationTabHolder.SetUp(colorSetting, null);

            sliderModule.SetSlider(0, 0, 3, true, OnDistanceSliderChange);

            buttonModule.SetUp(new System.Action[] {

                () => {
                    BackToPreviousPage();
                }
            });
        }

        public void SetContent(AccountModel accountModel, OnPuffMsgSend onPuffMsgSendEvent, System.Action OnCameraClick) {
            this.accountModel = accountModel;
            this.OnPuffMsgSendCallback = onPuffMsgSendEvent;

            CleanContent();
            typeTabHolder.SetClickTab((int)Tabs.Story);
            privacyTabHolder.SetClickTab((int)Privacy.Public);
            durationTabHolder.SetClickTab((int)Duration.Date);

            cameraModule.SetUp(OnCameraClick);

            this.submitBtn.onClick.RemoveAllListeners();
            this.submitBtn.onClick.AddListener(() => {
                OnSubmitButtonClick();
            });
        }

        #region Tab Layout
        private void SetEventLayout() { 
        
        }

        private void SetReviewLayout()
        {
            reviewModule.gameObject.SetActive(true);
            _cameraModule.gameObject.SetActive(false);
        }

        private void SetStoryLayout()
        {
            reviewModule.gameObject.SetActive(false);
            _cameraModule.gameObject.SetActive(true);
        }

        private void SetPrivacyLayout() {
            FrontPageBasicLayout(false);

            reviewModule.gameObject.SetActive(false);
        }

        private void FrontPageBasicLayout(bool enable)
        {
            msgText.gameObject.SetActive(enable);
            titleText.gameObject.SetActive(enable);
            typeTabHolder.gameObject.SetActive(enable);

            privacyTabHolder.gameObject.SetActive(!enable);
            durationTabHolder.gameObject.SetActive(!enable);
            buttonModule.gameObject.SetActive(!enable);
            sliderModule.gameObject.SetActive(!enable);

            _cameraModule.gameObject.SetActive(false);
        }

        private void BackToPreviousPage() {
            if (TabActionDictTable.TryGetValue(typeTabHolder.CurrentIndex, out System.Action tabAction)) {
                FrontPageBasicLayout(true);
                tabAction();
            }
        }

        #endregion
        private void OnSubmitButtonClick() {
            if (string.IsNullOrEmpty(msgText.text)) return;


            var allImageBytes = cameraModule.GetTextureBytes();

            //if (allImageBytes.Count > 0)
            //APIHttpRequest.CurlIMGBB(allImageBytes[0]);

            if (privacyTabHolder.gameObject.activeSelf)
            {

                var puffMsgType = PuffMsgBoardHelper.GetCreateMessageType(
                    this.accountModel.puffAccountType._id,
                    this.accountModel.puffAccountType.username,
                    msgText.text,
                    titleText.text,
                    typeTabHolder.CurrentIndex,
                    privacyTabHolder.CurrentIndex,
                    durationTabHolder.CurrentIndex,
                    (int)sliderModule.sliderValue
                );

                this.OnPuffMsgSendCallback(puffMsgType, allImageBytes);
                msgText.text = "";

                return;
            }

            SetPrivacyLayout();
        }

        private void OnDistanceSliderChange(float p_index) {
            int index = (int)p_index;

            Dictionary<int, string> distTable = new Dictionary<int, string>()
            {
                { (int)Distance.Near, string.Format(StringTextAsset.Messaging.DistanceNear, 100) },
                { (int)Distance.Medium, string.Format(StringTextAsset.Messaging.DistanceMedium, 1) },
                { (int)Distance.Far, string.Format(StringTextAsset.Messaging.DistanceFar, 10) },
                { (int)Distance.World, string.Format(StringTextAsset.Messaging.DistanceWorld) }
            };

            if (distTable.TryGetValue(index, out string text)) {
                sliderModule.SetSliderField(text);   
            }
        }
       
        private void CleanContent()
        {
            FrontPageBasicLayout(true);

            msgText.text = "";
            titleText.text = "";
            reviewModule.SetScore(0);

            privacyTabHolder.gameObject.SetActive(false);
            durationTabHolder.gameObject.SetActive(false);
            buttonModule.gameObject.SetActive(false);
            OnDistanceSliderChange(0);

            _cameraModule.CleanUp();
        }
    }
}