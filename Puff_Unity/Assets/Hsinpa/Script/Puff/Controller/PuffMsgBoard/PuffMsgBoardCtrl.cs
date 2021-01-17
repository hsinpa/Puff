using Hsinpa.View;
using Puff.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.Ctrl.Utility;
using Hsinpa.Utility;
using Puff.Model;
using System.Threading.Tasks;

namespace Puff.Ctrl
{
    public class PuffMsgBoardCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private ARCameraController _arCameraCtrl;

        private PuffMessageModal puffMessageModal;
        private GeneralFlag.PuffMsgBoardState _puffMsgBoardState;
        private PuffMsgBoardHelper _puffMsgBoardHelper;
        private AccountModel _accountModel;
        private PuffModel _puffModel;

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
            this._accountModel = PuffApp.Instance.models.accountModel;
            this._puffModel = PuffApp.Instance.models.puffModel;
            this._arCameraCtrl.OnScreenShotIsDone += OnCameraScreenShot;

            //puffMessageModal.puffActionSelectPage.SetUp(OpenTextMsgPage, () => { }, () => OpenFrontPage(_currentPuffMsg));
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

            puffMsgPage.SetContent(_accountModel, OnCreatorMessageSubmitEvent, () => {

                //var screenshot = _arCameraCtrl.TakeScreenShot();

                //if (screenshot != null)
                //    puffMsgPage.cameraModule.AssignRawImage(screenshot);

                if (puffMsgPage.cameraModule.isTakePhotoAvailable)
                    PuffApp.Instance.Notify(EventFlag.Event.EnterCameraMode);
            });
        }

        private void OpenFrontPage(JsonTypes.PuffMessageType puffMessageType) {
            PuffMsgFrontPage frontPage = puffMessageModal.OpenPage<PuffMsgFrontPage>();

            frontPage.SetContent(puffMessageType, (string replayMsg) =>
            {
                PushCommentToServer(frontPage, puffMessageType._id, replayMsg);
            });
        }

        private void OpenTextMsgPage() {
            var page = puffMessageModal.OpenPage<PuffTextMsgPage>();
        }

        private async void PushCommentToServer(PuffMsgFrontPage frontPage, string msg_id, string comment) {
            frontPage.EnableReplyInput(false);

            //Update on UI
            JsonTypes.PuffCommentType commentType = PuffMsgBoardHelper.GetCommentType(msg_id, this._accountModel.puffAccountType._id, 
                                                                                    this._accountModel.puffAccountType.username, comment);
            frontPage.InsertNewComments(commentType);

            this._puffModel.UpdateMessageComments(msg_id, commentType);

            //Upload to Server
           _ = await this._puffModel.PushCommentToServer(commentType);

            frontPage.EnableReplyInput(true);
        }

        private async void OnCreatorMessageSubmitEvent(JsonTypes.PuffMessageType puffMessage, List<byte[]> textureBytes)
        {
            Modals.instance.Close();

            List<string> rawIMGBBResult = await _puffModel.UploadTextureToIMGBB(textureBytes);
            puffMessage.images = rawIMGBBResult;

            string url = GeneralFlag.GetFullAPIUri(GeneralFlag.API.SendPuffMsg);

            await APIHttpRequest.Curl(url, BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(puffMessage));
        }

        private void OnCameraScreenShot(Texture renderTexture) {
            PuffApp.Instance.Notify(EventFlag.Event.ExitCameraMode);

            PuffMessageModal puffMessageModal = Modals.instance.OpenModal<PuffMessageModal>();
            PuffTextMsgPage puffMsgPage = puffMessageModal.GetPage<PuffTextMsgPage>();

            puffMsgPage.cameraModule.AssignRawImage(renderTexture);
        }
        #endregion

    }
}