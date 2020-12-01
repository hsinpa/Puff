using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffActionSelectPage : PuffMsgInnerPage
    {
        [SerializeField]
        private Button TextBtn;

        [SerializeField]
        private Button ImageBtn;

        [SerializeField]
        private Button BackBtn;
    
        public void SetUp(System.Action textBtnEvent, System.Action imageBtnEvent, System.Action backBtnEvent)
        {
            TextBtn.onClick.AddListener(() => textBtnEvent());
            ImageBtn.onClick.AddListener(() => imageBtnEvent());
            BackBtn.onClick.AddListener(() => backBtnEvent());
        }
    }
}