using Hsinpa.View;
using Puff.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.Ctrl
{
    public class PuffMsgBoardCtrl : ObserverPattern.Observer
    {

        private PuffMessageModal puffMessageModal;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        SetUp();
                    }
                    break;

                case EventFlag.Event.OpenPuffMsg:
                    {
                        OnOpenPuffMsg();
                    }
                    break;
            }
        }

        private void SetUp()
        {
            this.puffMessageModal = Modals.instance.GetModal<PuffMessageModal>();
            RegisterPageEvent(this.puffMessageModal);
        }

        #region UI Event
        private void OnOpenPuffMsg()
        {
            PuffMessageModal puffMessageModal = Modals.instance.OpenModal<PuffMessageModal>();
            OpenFrontPage();
        }

        private void RegisterPageEvent(PuffMessageModal puffMessageModal) {
            puffMessageModal.puffActionSelectPage.SetUp(OpenTextMsgPage, () => { }, OpenFrontPage);

            puffMessageModal.puffTextMsgPage.SetUp(OnMessageSubmitEvent, OpenSActionPage);
        }

        private void OpenFrontPage() {
            PuffMsgFrontPage frontPage = puffMessageModal.OpenPage<PuffMsgFrontPage>();

            frontPage.SetContent("Fake Title", "Fake semi title", "Fake Description", () =>
            {
                OpenSActionPage();
            });
        }

        private void OpenSActionPage() {
            var page = puffMessageModal.OpenPage<PuffActionSelectPage>();
        }

        private void OpenTextMsgPage() {
            var page = puffMessageModal.OpenPage<PuffTextMsgPage>();
        }

        private void OnMessageSubmitEvent(string p_message)
        {
            OpenFrontPage();
        }
        #endregion

    }
}