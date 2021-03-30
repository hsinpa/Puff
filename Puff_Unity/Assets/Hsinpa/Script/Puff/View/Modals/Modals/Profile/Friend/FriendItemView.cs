using Hsinpa.Utility;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class FriendItemView : MonoBehaviour
    {
        [SerializeField]
        private RawImage avatar;

        [SerializeField]
        private Text friendName;

        [SerializeField]
        private Button actionBtn;

        [SerializeField]
        private Button rejectBtn;

        public JsonTypes.FriendType friendType {get; private set; }

        public void SetAvatar(RawImage p_avatar) {
            this.avatar = p_avatar;
        }

        public void SetInfo(JsonTypes.FriendType friendType) {
            this.friendType = friendType;
            this.friendName.text = friendType.username;

            //No need action, if its already friend
            actionBtn.gameObject.SetActive(ShowActionBtnByStatus(friendType.status));
            string friendActionBtnName = GetActionBtnNameByStatus(friendType.status);
            SetActionBtnName(actionBtn, friendActionBtnName);

            //Reject
            rejectBtn.gameObject.SetActive(friendType.status == JsonTypes.FriendStatus.ReceiveRequest);
            SetActionBtnName(rejectBtn, StringTextAsset.Friend.FriendBtnReject);
        }

        public void SetActionEvent(System.Action<FriendItemView> actionEvent, System.Action<FriendItemView> rejectEvent) {
            UtilityMethod.SetSimpleBtnEvent<FriendItemView>(actionBtn, actionEvent, this);
            UtilityMethod.SetSimpleBtnEvent<FriendItemView>(rejectBtn, rejectEvent, this);
        }

        private void SetActionBtnName(Button p_btn, string nameString) {
            p_btn.interactable = nameString != StringTextAsset.Friend.FriendBtnPending;

            Text btnName = p_btn.GetComponentInChildren<Text>();
            btnName.text = nameString;
        }

        public void ModifyStatusUI(JsonTypes.FriendStatus status) {
            switch (status)
            {
                case JsonTypes.FriendStatus.Friends:
                    actionBtn.gameObject.SetActive(false);
                    rejectBtn.gameObject.SetActive(false);
                    break;


                case JsonTypes.FriendStatus.Block:
                    this.gameObject.SetActive(false);
                    break;
            }
        }

        public static string GetActionBtnNameByStatus(JsonTypes.FriendStatus status) {
            switch (status) {
                case JsonTypes.FriendStatus.RequestFriend:
                    return StringTextAsset.Friend.FriendBtnPending;

                case JsonTypes.FriendStatus.ReceiveRequest:
                    return StringTextAsset.Friend.FriendBtnConfirm;
            }

            return "";
        }

        public static bool ShowActionBtnByStatus(JsonTypes.FriendStatus status)
        {
            return (status != JsonTypes.FriendStatus.Friends);
        }


    }
}