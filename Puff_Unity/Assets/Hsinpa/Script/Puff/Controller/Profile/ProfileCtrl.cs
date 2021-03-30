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
            if (string.IsNullOrEmpty(email)) return;

            FindFriendModal findFriendModal = Modals.instance.GetModal<FindFriendModal>();

            var fakeJSON = new JsonTypes.FriendType();
            fakeJSON.username = "Fake";
            fakeJSON._id = "dfasdfh";

            findFriendModal.SetFriendInvitePanel(fakeJSON, OnFriendInviteEvent);
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
