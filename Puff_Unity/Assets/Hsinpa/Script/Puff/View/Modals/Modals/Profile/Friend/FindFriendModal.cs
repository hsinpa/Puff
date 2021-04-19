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

        public void SetFriendInvitePanel(string id, string username, bool isFriendInvitationAllow, System.Action<string> OnInviteBtnCallback) {
            FriendModalSearch.gameObject.SetActive(false);

            FriendModalInvite.SetUp(id, username, isFriendInvitationAllow, OnInviteBtnCallback);
        }
    }
}