using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.View {
    public class PuffMessageModal : Modal
    {
        [SerializeField]
        private PuffMsgHeader PuffMsgHeader;

        [SerializeField]
        private PuffMsgFrontPage PuffMsgFrontPage;

        public PuffMsgFrontPage GetPuffFrontPage() {
            PuffMsgFrontPage.Show(true);
            PuffMsgHeader.HideBackBtn();

            return PuffMsgFrontPage;
        }


    }
}