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

        private System.Action<JsonTypes.FriendType> FriendAcceptCallback;
        private System.Action<JsonTypes.FriendType> FriendRejectCallback;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
        }

        public void SetUp(AccountModel accountModel, FriendModel friendModel, System.Action<JsonTypes.FriendType> FriendAcceptCallback, System.Action<JsonTypes.FriendType> FriendRejectCallback) {
            this.accountModel = accountModel;
            this.friendModel = friendModel;
            this.FriendAcceptCallback = FriendAcceptCallback;
            this.FriendRejectCallback = FriendRejectCallback;

            if (this.accountModel.puffAccountType.isValid)
            {
                DisplayFriendList(this.accountModel.puffAccountType);
            }
        }


        #region Friend Panel
        private async void DisplayFriendList(JsonTypes.PuffAccountType account)
        {
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

        private void GenerateFriendItem(JsonTypes.FriendType friend)
        {
            FriendItemView friendItemView = UtilityMethod.CreateObjectToParent<FriendItemView>(FriendItemHHolder, FriendItemPrefab.gameObject);
            friendItemView.SetInfo(friend);

            friendItemView.SetActionEvent(OnFriendAccept, OnFriendReject);
        }

        private void OnFriendAccept(FriendItemView friendItem)
        {

            var dialogueModal = Modals.instance.OpenModal<DialogueModal>();

            string[] btnStringArray = new string[] { StringTextAsset.DialogueBox.Confirm, StringTextAsset.DialogueBox.Cancel };
            dialogueModal.SetDialogue(StringTextAsset.Friend.FriendDialogueBoxTitle,
                string.Format(StringTextAsset.Friend.FriendAcceptMessage, friendItem.friendType.username), btnStringArray, (int index) => {
                    Debug.Log("Friend Accept : " + friendItem.friendType.username);

                    if (index == 0 && FriendAcceptCallback != null)
                    {
                        FriendAcceptCallback(friendItem.friendType);
                        friendItem.ModifyStatusUI(JsonTypes.FriendStatus.Friends);
                    }
                });
        }

        private void OnFriendReject(FriendItemView friendItem)
        {

            var dialogueModal = Modals.instance.OpenModal<DialogueModal>();

            string[] btnStringArray = new string[] { StringTextAsset.DialogueBox.Confirm, StringTextAsset.DialogueBox.Cancel };
            dialogueModal.SetDialogue(StringTextAsset.Friend.FriendDialogueBoxTitle,
                string.Format(StringTextAsset.Friend.FriendRejectMessage, friendItem.friendType.username), btnStringArray, (int index) => {
                    Debug.Log("Friend Reject : " + friendItem.friendType.username);

                    if (index == 0 && FriendAcceptCallback != null)
                    {
                        FriendRejectCallback(friendItem.friendType);
                        friendItem.ModifyStatusUI(JsonTypes.FriendStatus.Block);
                    }
                });
        }
        #endregion
    }
}