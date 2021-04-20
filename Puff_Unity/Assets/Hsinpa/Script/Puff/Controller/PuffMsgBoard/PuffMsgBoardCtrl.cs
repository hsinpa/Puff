using Hsinpa.View;
using Puff.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.Ctrl.Utility;
using Hsinpa.Utility;
using Puff.Model;
using System.Threading.Tasks;
using Hsinpa.Model;

namespace Puff.Ctrl
{
    public class PuffMsgBoardCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private ARCameraController _arCameraCtrl;

        [SerializeField]
        private PuffInspectView _puffInspectView;

        private PuffMessageModal puffMessageModal;
        private GeneralFlag.PuffMsgBoardState _puffMsgBoardState;
        private PuffMsgBoardHelper _puffMsgBoardHelper;
        private AccountModel _accountModel;
        private PuffModel _puffModel;
        private FriendModel _friendModel;

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

                case EventFlag.Event.LoginSuccessful:
                    {
                        RefreshSaveLibraryPuff();
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
            this._friendModel = PuffApp.Instance.models.friendModel;
            this._arCameraCtrl.OnScreenShotIsDone += OnCameraScreenShot;
        }

        private void RefreshSaveLibraryPuff() {
            _ = this._puffModel.puffSaveMsgUtility.GetSaveMsgFromServer(PuffSaveMsgUtility.GetPuffSaveActionType(this._accountModel.puffAccountType._id));
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

                if (puffMsgPage.cameraModule.isTakePhotoAvailable)
                    PuffApp.Instance.Notify(EventFlag.Event.EnterCameraMode);
            });
        }

        private void OpenFrontPage(JsonTypes.PuffMessageType puffMessageType) {
            PuffMsgFrontPage frontPage = puffMessageModal.OpenPage<PuffMsgFrontPage>();

            bool isSaveToLibrary = this._puffModel.puffSaveMsgUtility.IsDuplicate(puffMessageType._id);

            frontPage.SetContent(puffMessageType, isSaveToLibrary, OnIrrigateButtonClick, OnSaveToLiraryEvent, OnSearchProfileInfoEvent,(string replayMsg) =>
            {
                PushCommentToServer(frontPage, puffMessageType._id, replayMsg);
            });
        }

        private void OpenTextMsgPage() {
            var page = puffMessageModal.OpenPage<PuffTextMsgPage>();
        }

        private void OnSearchProfileInfoEvent(string account_id, string account_name) {
            PuffApp.Instance.Notify(EventFlag.Event.OnProfileAccountIDSearch, account_id, account_name);
        }

        private void OnSaveToLiraryEvent(JsonTypes.PuffMessageType puffMsg) {
            PuffApp.Instance.Notify(EventFlag.Event.OnProfileSaveToLibrary, puffMsg);
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

            _puffModel.SendNewMsg(puffMessage);

            HUDToastView.instance.Toast(StringTextAsset.Messaging.SubmitSuccess, 3, GeneralFlag.Colors.ToastColorNormal);
        }

        private void OnCameraScreenShot(Texture renderTexture) {
            PuffApp.Instance.Notify(EventFlag.Event.ExitCameraMode);

            PuffMessageModal puffMessageModal = Modals.instance.OpenModal<PuffMessageModal>();
            PuffTextMsgPage puffMsgPage = puffMessageModal.GetPage<PuffTextMsgPage>();

            puffMsgPage.cameraModule.AssignRawImage(renderTexture);
        }

        private void OnIrrigateButtonClick() {
            puffMessageModal.Show(false);
            Modals.instance.EnableBackgroundImg(false);

            _puffInspectView.SetFunctionCanvasAlpha(0);

            _puffInspectView.irrigatePanelView.SetContent("Fake exp +5%", "Total 50% exp reached", () =>
            {
                _puffInspectView.SetFunctionCanvasAlpha(1);
                Modals.instance.EnableBackgroundImg(true);
                puffMessageModal.Show(true);
            });
        }
        #endregion

    }
}