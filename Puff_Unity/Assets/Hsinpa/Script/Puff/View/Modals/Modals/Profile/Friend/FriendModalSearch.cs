using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.View.Friend
{
    public class FriendModalSearch : MonoBehaviour
    {
        [SerializeField]
        private InputField SearchInputField;

        [SerializeField]
        private Button ActionBtn;

        public void SetUp(System.Action<string> Callback) {
            this.gameObject.SetActive(true);

            this.SearchInputField.text = "";

            UtilityMethod.SetSimpleBtnEvent<string>(ActionBtn, Callback, SearchInputField.text);
        }
    }
}