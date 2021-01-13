using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;
using System.Linq;

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
        private PuffMsgReivewModule reviewModule;

        [Header("Tab")]
        [SerializeField]
        private PuffMsgTabModule typeTabHolder;
        public enum Tabs { Story, Review, Event, Survey};

        private ColorItemSObj colorSetting;
        private Dictionary<int, System.Action> TabActionDictTable = new Dictionary<int, System.Action>();

        public delegate void OnPuffMsgSend(string content);

        private void Start()
        {
            colorSetting = PuffApp.Instance.models.colorSetting;       

            TabActionDictTable.Add((int)Tabs.Event, SetEventLayout);
            TabActionDictTable.Add((int)Tabs.Review, SetReviewLayout);
            TabActionDictTable.Add((int)Tabs.Story, SetStoryLayout);

            typeTabHolder.SetUp(colorSetting, TabActionDictTable);
        }

        public void SetUp(OnPuffMsgSend onPuffMsgSendEvent) {
            CleanContent();
            typeTabHolder.SetClickTab((int)Tabs.Story);

            this.submitBtn.onClick.RemoveAllListeners();
            this.submitBtn.onClick.AddListener(() => {

                if (string.IsNullOrEmpty(msgText.text)) return;

                onPuffMsgSendEvent(msgText.text);
                msgText.text = "";
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
        #endregion

        private void CleanContent()
        {
            msgText.text = "";
            titleText.text = "";
            reviewModule.SetScore(0);
        }
    }
}