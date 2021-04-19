using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.View;
using UnityEngine.UI;
using Hsinpa.Model;
using Hsinpa.Utility;
using System.Threading;
using System.Threading.Tasks;
using Puff.Model;
using LitJson;

namespace Puff.View
{
    public class ProfileModal : Modal
    {
        [SerializeField]
        ColorItemSObj colorSetting;

        [Header("Tab")]
        [SerializeField]
        private PuffMsgTabModule puffMsgTabModule; 

        [Header("SelfSaveMsg Panel")]
        [SerializeField]
        private Transform SelfSaveMsgHolder;

        [SerializeField]
        private Button SelfMsgBtn;

        [SerializeField]
        private Button SaveMsgBtn;

        [Header("Friend Panel")]
        [SerializeField]
        private FriendAccordionView FriendAccordionView;

        [SerializeField]
        private Button SearchFriendBtn;

        [SerializeField]
        private FriendItemView FriendItemPrefab;

        [SerializeField]
        private Transform FriendItemHHolder;

        [Header("Library")]
        [SerializeField]
        private ProfileLibraryView libraryView;
            
        private AccountModel accountModel;
        private FriendModel friendModel;
        private PuffSaveMsgUtility puffSaveUtility;

        Task<JsonTypes.FriendListType> friendsTask;

        private System.Action<JsonTypes.FriendType> FriendAcceptCallback;
        private System.Action<JsonTypes.FriendType> FriendRejectCallback;
        private System.Action<JsonTypes.PuffMessageType> OnLibraryPuffCallback;

        public override void Show(bool isShow)
        {
            base.Show(isShow);
        }

        public void SetUp(AccountModel accountModel, FriendModel friendModel, PuffSaveMsgUtility puffSaveUtility,
            System.Action AddNewFriendCallback,
            System.Action<JsonTypes.FriendType> FriendAcceptCallback, System.Action<JsonTypes.FriendType> FriendRejectCallback, System.Action<JsonTypes.PuffMessageType> OnLibraryPuffCallback) {
            this.accountModel = accountModel;
            this.friendModel = friendModel;
            this.puffSaveUtility = puffSaveUtility;

            this.FriendAcceptCallback = FriendAcceptCallback;
            this.FriendRejectCallback = FriendRejectCallback;
            this.OnLibraryPuffCallback = OnLibraryPuffCallback;

            UtilityMethod.SetSimpleBtnEvent(SearchFriendBtn, AddNewFriendCallback);

            if (this.accountModel.puffAccountType.isValid)
            {
                DisplayFriendList(this.accountModel.puffAccountType);
            }

            UtilityMethod.SetSimpleBtnEvent(SelfMsgBtn, OnSelfPuffClick);
            UtilityMethod.SetSimpleBtnEvent(SaveMsgBtn, OnSavePuffClick);

            puffMsgTabModule.SetUp(colorSetting, new Dictionary<int, System.Action>()
            {
                {(int)Tab.Library, OnSelfSaveTabClick},
                {(int)Tab.Friend, OnFriendTabClick}
            });

            puffMsgTabModule.SetClickTab((int)Tab.Friend);
        }

        #region Button Action
        private void OnSelfSaveTabClick() {
            FriendItemHHolder.gameObject.SetActive(false);
            SelfSaveMsgHolder.gameObject.SetActive(true);
            libraryView.gameObject.SetActive(false);
        }

        private void OnFriendTabClick()
        {
            FriendItemHHolder.gameObject.SetActive(true);
            SelfSaveMsgHolder.gameObject.SetActive(false);
            libraryView.gameObject.SetActive(false);
        }

        private void OnSelfPuffClick() {
            libraryView.Generate(puffSaveUtility.GetFilterCacheMsg(PuffSaveMsgUtility.State.Self, this.accountModel.puffAccountType._id), this.OnLibraryPuffCallback);
        }

        private void OnSavePuffClick() {
            libraryView.Generate(puffSaveUtility.GetFilterCacheMsg(PuffSaveMsgUtility.State.Other, this.accountModel.puffAccountType._id), this.OnLibraryPuffCallback);
        }
        #endregion

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

                    if (index == 0 && FriendRejectCallback != null)
                    {
                        FriendRejectCallback(friendItem.friendType);
                        friendItem.ModifyStatusUI(JsonTypes.FriendStatus.Block);
                    }
                });
        }
        #endregion

        private enum Tab { 
            Library, Friend
        }
    }
}