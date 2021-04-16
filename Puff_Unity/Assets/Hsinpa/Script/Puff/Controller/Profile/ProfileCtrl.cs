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

            findFriendModal.SetSearchPanel(OnFriendSearchtEvent);
        }

        private async void OnFriendSearchtEvent(string email)
        {
            if (!AccountModel.CheckEmail(email)) {
                Debug.Log("Wrong Email Format " + email);
                HUDToastView.instance.Toast(StringTextAsset.Login.EmailWrongFormat, 3, GeneralFlag.Colors.ToastColorNormal);
                return;
            }

            var r = await _accountModel.FindAccountByEmail(email);

            if (r.isSuccess) {

                JsonTypes.DatabaseResultType databaseResultType = JsonUtility.FromJson<JsonTypes.DatabaseResultType>(r.body);

                if (databaseResultType.status == (int)EventFlag.DatabaseStateType.AccountState.Fail_Login_NoAccount) {
                    Debug.Log("No Account");
                    HUDToastView.instance.Toast(StringTextAsset.Login.DatabaseFail_Login, 3, GeneralFlag.Colors.ToastColorNormal);

                    return;
                }

                JsonTypes.FriendType friendJSON = JsonUtility.FromJson<JsonTypes.FriendType>(databaseResultType.result);

                FindFriendModal findFriendModal = Modals.instance.GetModal<FindFriendModal>();

                findFriendModal.SetFriendInvitePanel(friendJSON, OnFriendInviteEvent);
            }
        }

        private async void OnFriendInviteEvent(JsonTypes.FriendType friendJSON)
        {
            Debug.Log("OnFriendInviteEvent : " + friendJSON._id);

            Modals.instance.Close();

            JsonTypes.FriendActionJson friendActionJson = new JsonTypes.FriendActionJson();
            friendActionJson.account_id = _accountModel.puffAccountType._id;
            friendActionJson.auth_key = _accountModel.puffAccountType.auth_key;
            friendActionJson.target_id = friendJSON._id;

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
        private void OnLibraryPuffClick(JsonTypes.PuffMessageType puffMessageType)
        {
            Debug.Log("puffMessageType id " + puffMessageType._id);

            PuffApp.Instance.Notify(EventFlag.Event.OpenPuffMsg, puffMessageType);
        }

        #endregion
    }
}
