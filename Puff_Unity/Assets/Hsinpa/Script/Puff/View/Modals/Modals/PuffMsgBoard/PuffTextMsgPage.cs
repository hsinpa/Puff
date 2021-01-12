using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;

namespace Puff.View
{
    public class PuffTextMsgPage : PuffMsgInnerPage
    {
        [SerializeField]
        private InputField msgText;

        [SerializeField]
        private Button submitBtn;

        [Header("Tab")]
        [SerializeField]
        private RectTransform tabHolder;

        private PuffMsgTabBtn[] tabButtons;
        public enum Tabs {Event, Review, Story, Survey};


        private Tabs currentTabs = Tabs.Event;
        private ColorItemSObj colorSetting;
        private Dictionary<Tabs, System.Action> TabActionDictTable = new Dictionary<Tabs, System.Action>();

        public delegate void OnPuffMsgSend(string content);

        private void Start()
        {
            colorSetting = PuffApp.Instance.models.colorSetting;
            tabButtons = tabHolder.GetComponentsInChildren<PuffMsgTabBtn>();
            foreach (var t in tabButtons) {
                t.puffButton.onClick.RemoveAllListeners();
                t.puffButton.onClick.AddListener(() =>
                {
                    OnTabClick(t);
                });
            }

            TabActionDictTable.Add(Tabs.Event, SetEventLayout);
            TabActionDictTable.Add(Tabs.Review, SetReviewLayout);
            TabActionDictTable.Add(Tabs.Story, SetStoryLayout);
        }

        public void SetUp(OnPuffMsgSend onPuffMsgSendEvent) {

            msgText.text = "";

            this.submitBtn.onClick.RemoveAllListeners();
            this.submitBtn.onClick.AddListener(() => {

                if (string.IsNullOrEmpty(msgText.text)) return;

                onPuffMsgSendEvent(msgText.text);
                msgText.text = "";
            });
        }

        private void OnTabClick(PuffMsgTabBtn btn) {
            foreach (var tab in tabButtons) {
                tab.puffButton.image.color = colorSetting.TextUnSelectedColor;

                if (btn.tabType == tab.tabType)
                    tab.puffButton.image.color = colorSetting.TextHighlightColor;
            }

            if (TabActionDictTable.TryGetValue(btn.tabType, out System.Action p_action)) {
                p_action();
            }
        }

        #region Tab Layout
        private void SetEventLayout() { 
        
        }

        private void SetReviewLayout()
        {

        }

        private void SetStoryLayout()
        {

        }
        #endregion
    }
}