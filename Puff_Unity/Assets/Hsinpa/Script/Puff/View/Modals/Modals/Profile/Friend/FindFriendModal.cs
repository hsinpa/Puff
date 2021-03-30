using Hsinpa.View;
using Hsinpa.View.Friend;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.View
{
    public class FindFriendModal : Modal
    {
        [SerializeField]
        private FriendModalInvite FriendModalInvite;

        [SerializeField]
        private FriendModalSearch FriendModalSearch;

        public void SetSearchPanel(System.Action<string> OnSearchBtnCallback) {
            FriendModalSearch.SetUp(OnSearchBtnCallback);
            FriendModalInvite.gameObject.SetActive(false);
        }

        public void SetFriendInvitePanel(JsonTypes.FriendType friend, System.Action<string> OnInviteBtnCallback) {
            FriendModalSearch.gameObject.SetActive(false);
            FriendModalInvite.SetUp(friend.username, friend._id, null, OnInviteBtnCallback);
        }
    }
}