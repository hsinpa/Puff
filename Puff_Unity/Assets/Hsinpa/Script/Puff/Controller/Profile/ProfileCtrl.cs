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


        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event) {

                case EventFlag.Event.GameStart:
                    {
                        _accountModel = PuffApp.Instance.models.accountModel;
                        _friendModel = PuffApp.Instance.models.friendModel;
                    }
                    break;
                case EventFlag.Event.OnProfileOpen:
                    OnProfileOpenEvent();
                    break;
            }
        }

        private void OnProfileOpenEvent() {
            ProfileModal profileModal = Modals.instance.OpenModal<ProfileModal>();
            profileModal.SetUp(this._accountModel, this._friendModel, OnAddNewFriendtEvent, OnFriendAcceptEvent, OnFriendRejectEvent);
        }

        private async void OnAddNewFriendtEvent()
        {
            FindFriendModal findFriendModal = Modals.instance.OpenModal<FindFriendModal>();

            findFriendModal.SetSearchPanel(OnFriendSearchtEvent);
        }

        private async void OnFriendSearchtEvent(string email)
        {
            if (!AccountModel.CheckEmail(email)) {
                Debug.Log("Wrong Email Format " + email);
                HUDToastView.instance.ShowMessage(StringTextAsset.Login.EmailWrongFormat, 3);
                return;
            }

            var r = await _accountModel.FindAccountByEmail(email);

            if (r.isSuccess) {

                JsonTypes.DatabaseResultType databaseResultType = JsonUtility.FromJson<JsonTypes.DatabaseResultType>(r.body);

                if (databaseResultType.status == (int)EventFlag.DatabaseStateType.AccountState.Fail_Login_NoAccount) {
                    Debug.Log("No Account");
                    HUDToastView.instance.ShowMessage(StringTextAsset.Login.DatabaseFail_Login, 3);

                    return;
                }

                JsonTypes.FriendType friendJSON = JsonUtility.FromJson<JsonTypes.FriendType>(databaseResultType.result);

                FindFriendModal findFriendModal = Modals.instance.GetModal<FindFriendModal>();

                findFriendModal.SetFriendInvitePanel(friendJSON, OnFriendInviteEvent);
            }
        }

        private async void OnFriendInviteEvent(string user_id)
        {
            Debug.Log("OnFriendInviteEvent : " + user_id);
            Modals.instance.Close();
        }

        private async void OnFriendAcceptEvent(JsonTypes.FriendType friendType) {
            await _friendModel.AcceptFriend( _friendModel.GetActionJSON(_accountModel.puffAccountType._id, friendType._id, _accountModel.puffAccountType.auth_key) );
        }

        private async void OnFriendRejectEvent(JsonTypes.FriendType friendType)
        {
            await _friendModel.RejectFriend(_friendModel.GetActionJSON(_accountModel.puffAccountType._id, friendType._id, _accountModel.puffAccountType.auth_key));
        }
    }
}
