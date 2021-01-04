using Hsinpa.View;
using Puff.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.Ctrl.Utility;
using Hsinpa.Utility;

namespace Puff.Ctrl
{
    public class PuffMsgBoardCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private string _fake_user_id = "hsinpa_fake";
        
        private PuffMessageModal puffMessageModal;
        private GeneralFlag.PuffMsgBoardState _puffMsgBoardState;
        private PuffMsgBoardHelper _puffMsgBoardHelper;

        private JsonTypes.PuffMessageType _currentPuffMsg;

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
                        _puffMsgBoardState = GeneralFlag.PuffMsgBoardState.Reviewer;
                        _currentPuffMsg = (JsonTypes.PuffMessageType)p_objects[0];
                        OnOpenPuffMsg(_currentPuffMsg);
                    }
                    break;

                case EventFlag.Event.OpenSendMsg:
                    {
                        _puffMsgBoardState = GeneralFlag.PuffMsgBoardState.Creator;
                        OnCreatePuffMsg();
                    }
                    break;
            }
        }

        private void SetUp()
        {
            this._puffMsgBoardHelper = new PuffMsgBoardHelper();
            this.puffMessageModal = Modals.instance.GetModal<PuffMessageModal>();

            puffMessageModal.puffActionSelectPage.SetUp(OpenTextMsgPage, () => { }, () => OpenFrontPage(_currentPuffMsg));
        }

        #region UI Event
        private void OnOpenPuffMsg(JsonTypes.PuffMessageType puffMessageType)
        {
            PuffMessageModal puffMessageModal = Modals.instance.OpenModal<PuffMessageModal>();
            OpenFrontPage(puffMessageType);
        }

        private void OnCreatePuffMsg() {
            PuffMessageModal puffMessageModal = Modals.instance.OpenModal<PuffMessageModal>();
            PuffTextMsgPage puffMsgPage = puffMessageModal.OpenPage<PuffTextMsgPage>();

            puffMsgPage.SetUp(OnCreatorMessageSubmitEvent, () =>
            {
                Modals.instance.Close();
            });

        }

        private void OpenFrontPage(JsonTypes.PuffMessageType puffMessageType) {
            PuffMsgFrontPage frontPage = puffMessageModal.OpenPage<PuffMsgFrontPage>();

            frontPage.SetContent(puffMessageType.author, "", puffMessageType.body, () =>
            {
                OpenSActionPage();
            });

            PuffTextMsgPage puffTextMsgPage = puffMessageModal.GetPage<PuffTextMsgPage>();
            puffTextMsgPage.SetUp(OnReviewerMessageSubmitEvent, OpenSActionPage);
        }

        private void OpenSActionPage() {
            var page = puffMessageModal.OpenPage<PuffActionSelectPage>();
        }

        private void OpenTextMsgPage() {
            var page = puffMessageModal.OpenPage<PuffTextMsgPage>();
        }

        private void OnCreatorMessageSubmitEvent(string p_message)
        {
            JsonTypes.PuffMessageType msgType = _puffMsgBoardHelper.GetCreateMessageType(_fake_user_id, p_message);
            string url = GeneralFlag.GetFullAPIUri(GeneralFlag.API.SendPuffMsg);

            _ = APIHttpRequest.Curl(url, BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(msgType));
        }

        private void OnReviewerMessageSubmitEvent(string p_message)
        {
            JsonTypes.PuffCommentType msgType = _puffMsgBoardHelper.GetCommentType(_fake_user_id, p_message);

            OpenFrontPage(this._currentPuffMsg);
        }

        #endregion

    }
}