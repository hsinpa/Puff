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

        void Start()
        {
            this.OnButtonIndexClick += OnTabClickEvent;
        }

        public void OnTabClickEvent(int p_index) {

            System.DateTime dateTime = System.DateTime.UtcNow;

            switch (p_index) {

                case (int) PuffTextMsgPage.Duration.Date :
                    dateTime = dateTime.AddDays(1);

                    break;

                case (int)PuffTextMsgPage.Duration.Week:
                    dateTime = dateTime.AddDays(7);

                    break;

                case (int)PuffTextMsgPage.Duration.Month:
                    dateTime = dateTime.AddMonths(1);
                    break;

            }

            string dateTimeStr = dateTime.ToString("MM/dd/yyyy");
            durationText.text = string.Format(StringTextAsset.Messaging.DurationText, dateTimeStr);
        }


    }
}