using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.View.Friend {
    public class FriendModalInvite : MonoBehaviour
    {
        [SerializeField]
        private Text FriendInfoText;

        [SerializeField]
        private RawImage FriendAvatar;

        [SerializeField]
        private Button ActionBtn;

        [SerializeField]
        private Texture defaultFriendSprite;

        public void SetUp(string friend_id, string friendInfo, bool allowInvitation, System.Action<string> action)
        {
            this.gameObject.SetActive(true);

            FriendInfoText.text = friendInfo;

            FriendAvatar.texture = defaultFriendSprite;

            ActionBtn.interactable = allowInvitation;

            UtilityMethod.SetSimpleBtnEvent<string>(ActionBtn, action, friend_id);
        }

        public void SetAvatarTexture(Texture texture) {
            if (texture != null)
                FriendAvatar.texture = texture;
        }

    }
}