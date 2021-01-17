using System.Collections;
using System.Collections.Generic;
using Hsinpa.Utility;
using UnityEngine;

namespace Puff.View
{
    public class PuffMsgTabModule : MonoBehaviour
    {
        [SerializeField]
        private RectTransform tabHolder;

        private PuffMsgTabBtn[] tabButtons;

        private int _currentIndex;
        public int CurrentIndex => _currentIndex;

        private ColorItemSObj colorSetting;

        private Dictionary<int, System.Action> _tabActionDictTable = new Dictionary<int, System.Action>();

        public System.Action<int> OnButtonIndexClick;

        public void SetUp(ColorItemSObj colorSetting, Dictionary<int, System.Action> tabActionDictTable)
        {
            this._tabActionDictTable = tabActionDictTable;
            this.colorSetting = colorSetting;

            tabButtons = tabHolder.GetComponentsInChildren<PuffMsgTabBtn>(includeInactive:true);

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

                if (btn.name == tab.name)
                    tab.puffButton.targetGraphic.color = colorSetting.TextHighlightColor;
            }

            int btnIndex = btn.transform.GetSiblingIndex();
            this._currentIndex = btnIndex;

            if (_tabActionDictTable != null && _tabActionDictTable.TryGetValue(btnIndex, out System.Action p_action))
            {
                p_action();

            }

            if (OnButtonIndexClick != null)
                OnButtonIndexClick(btnIndex);
        }

    }
}