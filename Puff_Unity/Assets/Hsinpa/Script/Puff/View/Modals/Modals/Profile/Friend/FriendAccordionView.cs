using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class FriendAccordionView : MonoBehaviour
    {
        [SerializeField]
        private Button AddFriendBtn;

        [SerializeField]
        private Text friendCountText;

        public void SetFriendBtnEvent(System.Action AddFriendEvent)
        {
            UtilityMethod.SetSimpleBtnEvent(AddFriendBtn, AddFriendEvent);
        }

        public void SetFriendTitle(int count) {
            string friendString = string.Format(StringTextAsset.Friend.FriendTitleCount, count);
            friendCountText.text = friendString;
        }
    }
}