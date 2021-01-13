using System.Collections;
using System.Collections.Generic;
using Hsinpa.Utility;
using UnityEngine;

namespace Puff.View
{
    public class PuffMsgTabModule : MonoBehaviour
    {
        private PuffMsgTabBtn[] tabButtons;
        private int currentIndex;

        private ColorItemSObj colorSetting;

        private Dictionary<int, System.Action> _tabActionDictTable = new Dictionary<int, System.Action>();

        public void SetUp(ColorItemSObj colorSetting, Dictionary<int, System.Action> tabActionDictTable)
        {
            this._tabActionDictTable = tabActionDictTable;
            this.colorSetting = colorSetting;

            tabButtons = this.GetComponentsInChildren<PuffMsgTabBtn>();

            foreach (var t in tabButtons)
            {
                t.puffButton.onClick.RemoveAllListeners();
                t.puffButton.onClick.AddListener(() =>
                {
                    OnTabClick(t);
                });
            }
        }

        public void SetClickTab(int index) {
            if (index < tabButtons.Length)
                OnTabClick(tabButtons[index]);
        }

        private void OnTabClick(PuffMsgTabBtn btn)
        {
            foreach (var tab in tabButtons)
            {
                tab.puffButton.targetGraphic.color = colorSetting.TextUnSelectedColor;

                if (btn.tabType == tab.tabType)
                    tab.puffButton.targetGraphic.color = colorSetting.TextHighlightColor;
            }

            int btnIndex = btn.transform.GetSiblingIndex();

            if (_tabActionDictTable.TryGetValue(btnIndex, out System.Action p_action))
            {
                currentIndex = btnIndex; 
                p_action();
            }
        }

    }
}