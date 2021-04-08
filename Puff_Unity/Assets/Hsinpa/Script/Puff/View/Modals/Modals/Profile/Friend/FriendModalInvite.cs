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

        public void SetUp(string friendInfo, JsonTypes.FriendType friendJSON, Texture texture, System.Action<JsonTypes.FriendType> action)
        {
            this.gameObject.SetActive(true);

            FriendInfoText.text = friendInfo;

            if (texture != null)
                FriendAvatar.texture = texture;

            UtilityMethod.SetSimpleBtnEvent<JsonTypes.FriendType>(ActionBtn, action, friendJSON);
        }
    }
}