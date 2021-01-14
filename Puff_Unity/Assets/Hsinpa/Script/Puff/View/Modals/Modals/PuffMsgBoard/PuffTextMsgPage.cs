using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;
using System.Linq;
using Puff.Ctrl.Utility;

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

        private ColorItemSObj colorSetting;
        private AccountModel accountModel;
        private Dictionary<int, System.Action> TabActionDictTable = new Dictionary<int, System.Action>();

        public delegate void OnPuffMsgSend(JsonTypes.PuffMessageType content);
        private OnPuffMsgSend OnPuffMsgSendCallback;

        private void Start()
        {
            colorSetting = PuffApp.Instance.models.colorSetting;       

            TabActionDictTable.Add((int)Tabs.Event, SetEventLayout);
            TabActionDictTable.Add((int)Tabs.Review, SetReviewLayout);
            TabActionDictTable.Add((int)Tabs.Story, SetStoryLayout);

            typeTabHolder.SetUp(colorSetting, TabActionDictTable);
            privacyTabHolder.SetUp(colorSetting, null);
            durationTabHolder.SetUp(colorSetting, null);

            buttonModule.SetUp(new System.Action[] {

                () => {
                    BackToPreviousPage();
                }
            });
        }

        public void SetUp(AccountModel accountModel, OnPuffMsgSend onPuffMsgSendEvent) {
            this.accountModel = accountModel;
            this.OnPuffMsgSendCallback = onPuffMsgSendEvent;

            CleanContent();
            typeTabHolder.SetClickTab((int)Tabs.Story);
            privacyTabHolder.SetClickTab((int)Privacy.Public);
            durationTabHolder.SetClickTab((int)Duration.Date);

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
        }

        private void SetStoryLayout()
        {
            reviewModule.gameObject.SetActive(false);
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

            if (privacyTabHolder.gameObject.activeSelf) {

                var puffMsgType = PuffMsgBoardHelper.GetCreateMessageType(
                    this.accountModel.puffAccountType._id,
                    this.accountModel.puffAccountType.username,
                    msgText.text,
                    titleText.text,
                    typeTabHolder.CurrentIndex,
                    privacyTabHolder.CurrentIndex,
                    durationTabHolder.CurrentIndex,
                    10
                );

                this.OnPuffMsgSendCallback(puffMsgType);
                msgText.text = "";

                return;
            }

            SetPrivacyLayout();
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
        }
    }
}