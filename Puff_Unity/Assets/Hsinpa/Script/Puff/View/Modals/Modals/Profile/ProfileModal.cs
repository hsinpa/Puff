using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.View;
using UnityEngine.UI;
using Hsinpa.Model;
using Hsinpa.Utility;
using System.Threading;
using System.Threading.Tasks;

namespace Puff.View
{
    public class ProfileModal : Modal
    {
        [SerializeField]
        private FriendAccordionView FriendAccordionView;

        [SerializeField]
        private FriendItemView FriendItemPrefab;

        [SerializeField]
        private Transform FriendItemHHolder;

        private AccountModel accountModel;
        private FriendModel friendModel;

        Task<JsonTypes.FriendListType> friendsTask;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
        }

        public void SetUp(AccountModel accountModel, FriendModel friendModel) {
            this.accountModel = accountModel;
            this.friendModel = friendModel;

            if (this.accountModel.puffAccountType.isValid)
            {
                DisplayFriendList(this.accountModel.puffAccountType);
            }
        }

        private async void DisplayFriendList(JsonTypes.PuffAccountType account) {
            //Its is still running
            if (friendsTask != null) return;

            UtilityMethod.ClearChildObject(FriendItemHHolder, FriendAccordionView.name);
            friendsTask = this.friendModel.GetFriend(account._id);

            var fList = await friendsTask;
            if (fList.friend_info != null)
            {
                foreach (JsonTypes.FriendType friend in fList.friend_info)
                    GenerateFriendItem(friend);
            }
            friendsTask = null;
        }

        private void GenerateFriendItem(JsonTypes.FriendType friend) {
            FriendItemView friendItemView = UtilityMethod.CreateObjectToParent<FriendItemView>(FriendItemHHolder, FriendItemPrefab.gameObject);
            friendItemView.SetInfo(friend);

            friendItemView.SetActionEvent(OnFriendAccept, OnFriendReject);
        }

        private void OnFriendAccept(FriendItemView friendItem) {
            Debug.Log("Friend Accept : " + friendItem.friendType.username);
        }

        private void OnFriendReject(FriendItemView friendItem)
        {
            Debug.Log("Friend Reject : " + friendItem.friendType.username);
        }



    }
}