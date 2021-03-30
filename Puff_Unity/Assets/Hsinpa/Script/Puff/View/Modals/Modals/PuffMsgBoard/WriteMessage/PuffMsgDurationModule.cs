using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffMsgDurationModule : PuffMsgTabModule
    {
        [SerializeField]
        private Text durationText;

        [SerializeField]
        private Vector2Int dateConstraint;

        [SerializeField]
        private Button MinusBtn;

        [SerializeField]
        private Button PlusBtn;

        [SerializeField]
        private Text dateText;

        private int _date;
        public int date => _date;

        void Start()
        {
            Hsinpa.Utility.UtilityMethod.SetSimpleBtnEvent(MinusBtn, OnMinusBtnClick);
            Hsinpa.Utility.UtilityMethod.SetSimpleBtnEvent(PlusBtn, OnPlusBtnClick);
        }

        public void SetValue(int append_date) {
            _date = append_date;

            System.DateTime dateTime = System.DateTime.UtcNow;
            dateTime = dateTime.AddDays(_date);

            string dateTimeStr = dateTime.ToString("MM/dd/yyyy");
            durationText.text = string.Format(StringTextAsset.Messaging.DurationText, dateTimeStr);

            dateText.text = string.Format(StringTextAsset.Messaging.DurationCues, _date.ToString());
        }

        private void OnPlusBtnClick() {

            if (date + 1 <= dateConstraint.y)
                SetValue(date + 1);

        }

        private void OnMinusBtnClick()
        {
            if (date - 1 >= dateConstraint.x)
                SetValue(date - 1);

        }



    }
}