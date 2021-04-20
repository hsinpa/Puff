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
using LitJson;
using System.IO.IsolatedStorage;

namespace Puff.Ctrl
{
    public class ProfileCtrl : ObserverPattern.Observer
    {

        private AccountModel _accountModel;
        private FriendModel _friendModel;
        private PuffModel _puffModel;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event) {

                case EventFlag.Event.GameStart:
                    {
                        _accountModel = PuffApp.Instance.models.accountModel;
                        _friendModel = PuffApp.Instance.models.friendModel;
                        _puffModel = PuffApp.Instance.models.puffModel;
                    }
                    break;
                case EventFlag.Event.OnProfileOpen:
                    OnProfileOpenEvent();
                    break;

                case EventFlag.Event.OnProfileAccountIDSearch:
                    if (p_objects == null || p_objects.Length <= 1) return;

                    string account_id = (string)p_objects[0];
                    string account_name = (string)p_objects[1];

                    OnFriendIDSearchtEvent(account_id, account_name);
                    break;


                case EventFlag.Event.OnProfileSaveToLibrary:
                    if (p_objects == null || p_objects.Length <= 0) return;

                    SaveToSelfLibrary((JsonTypes.PuffMessageType)p_objects[0]);
                    break;
            }
        }

        private void OnProfileOpenEvent() {
            ProfileModal profileModal = Modals.instance.OpenModal<ProfileModal>();
            profileModal.SetUp(this._accountModel, this._friendModel, this._puffModel.puffSaveMsgUtility,
                                OnAddNewFriendtEvent, OnFriendAcceptEvent, OnFriendRejectEvent, OnLibraryPuffClick);
        }

        #region Friend Callback
        private async void OnAddNewFriendtEvent()
        {
            FindFriendModal findFriendModal = Modals.instance.OpenModal<FindFriendModal>();

            findFriendModal.SetSearchPanel(OnFriendEmailSearchtEvent);
        }

        private async void OnFriendEmailSearchtEvent(string email)
        {
            if (!AccountModel.CheckEmail(email)) {
                Debug.Log("Wrong Email Format " + email);
                HUDToastView.instance.Toast(StringTextAsset.Login.EmailWrongFormat, 3, GeneralFlag.Colors.ToastColorNormal);
                return;
            }

            var r = await _accountModel.FindAccountByEmail(email);

            ProcessUserAccountHttpCallback(r);
        }

        private async void OnFriendIDSearchtEvent(string id, string account_name)
        {
            //No Relation and not self
            bool isFriendInvitationAllow = !this._friendModel.HasRelationWithAccount(id) &&
                                            id != this._accountModel.puffAccountType._id;

            if (!isFriendInvitationAllow) {
                FindFriendModal findFriendModal = Modals.instance.OpenModal<FindFriendModal>();

                findFriendModal.SetFriendInvitePanel(id, account_name, isFriendInvitationAllow, OnFriendInviteEvent);

                return;    
            }

            var r = await _accountModel.FindAccountByID(id);

            ProcessUserAccountHttpCallback(r);
        }

        private void ProcessUserAccountHttpCallback(Hsinpa.Utility.APIHttpRequest.HttpResult r) {
            if (r.isSuccess)
            {
                JsonTypes.DatabaseResultType databaseResultType = JsonUtility.FromJson<JsonTypes.DatabaseResultType>(r.body);

                if (databaseResultType.status == (int)EventFlag.DatabaseStateType.AccountState.Fail_Login_NoAccount)
                {
                    Debug.Log("No Account");
                    HUDToastView.instance.Toast(StringTextAsset.Login.DatabaseFail_Login, 3, GeneralFlag.Colors.ToastColorNormal);

                    return;
                }

                JsonTypes.FriendType friendJSON = JsonUtility.FromJson<JsonTypes.FriendType>(databaseResultType.result);

                //No Relation and not self
                bool isFriendInvitationAllow = !this._friendModel.HasRelationWithAccount(friendJSON._id) &&
                                                friendJSON._id != this._accountModel.puffAccountType._id;

                FindFriendModal findFriendModal = Modals.instance.OpenModal<FindFriendModal>();

                findFriendModal.SetFriendInvitePanel(friendJSON._id, friendJSON.username, isFriendInvitationAllow, OnFriendInviteEvent);
            }
        }

        private async void OnFriendInviteEvent(string friend_id)
        {
            Modals.instance.Close();

            JsonTypes.FriendActionJson friendActionJson = new JsonTypes.FriendActionJson();
            friendActionJson.account_id = _accountModel.puffAccountType._id;
            friendActionJson.auth_key = _accountModel.puffAccountType.auth_key;
            friendActionJson.target_id = friend_id;

            await _friendModel.RequestFriend(friendActionJson);
        }

        private async void OnFriendAcceptEvent(JsonTypes.FriendType friendType) {
            await _friendModel.AcceptFriend( _friendModel.GetActionJSON(_accountModel.puffAccountType._id, friendType._id, _accountModel.puffAccountType.auth_key) );
        }

        private async void OnFriendRejectEvent(JsonTypes.FriendType friendType)
        {
            await _friendModel.RejectFriend(_friendModel.GetActionJSON(_accountModel.puffAccountType._id, friendType._id, _accountModel.puffAccountType.auth_key));
        }
        #endregion


        #region Puff Library Callback

        private async void SaveToSelfLibrary(JsonTypes.PuffMessageType puffMessageType)
        {
            //Check if message is belong to account user
            if (puffMessageType.author_id == _accountModel.puffAccountType._id)
            {

                HUDToastView.instance.Toast(StringTextAsset.Messaging.PuffLibraryError_IsAccountOwner, 4, GeneralFlag.Colors.ToastColorError);

                return;
            }

            var actionType = PuffSaveMsgUtility.GetPuffSaveActionType(_accountModel.puffAccountType._id, puffMessageType._id);
            bool isSucess = await this._puffModel.puffSaveMsgUtility.AddNewSaveMsg(puffMessageType, actionType);

            if (!isSucess)
                HUDToastView.instance.Toast(StringTextAsset.Messaging.PuffLibraryError_IsAlreadySave, 4, GeneralFlag.Colors.ToastColorError);
            else
                HUDToastView.instance.Toast(StringTextAsset.GeneralText.Success, 4, GeneralFlag.Colors.ToastColorNormal);
        }

        private void OnLibraryPuffClick(JsonTypes.PuffMessageType puffMessageType)
        {
            Debug.Log("puffMessageType id " + puffMessageType._id);

            PuffApp.Instance.Notify(EventFlag.Event.OpenPuffMsg, puffMessageType);
        }

        #endregion
    }
}
