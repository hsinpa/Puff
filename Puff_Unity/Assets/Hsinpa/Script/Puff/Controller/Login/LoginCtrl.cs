using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.Ctrl.Utility;
using Hsinpa.View;
using Puff.View;
using Hsinpa.Utility;

namespace Puff.Ctrl
{

    public class LoginCtrl : ObserverPattern.Observer
    {
        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        SetUp();
                    }
                    break;
            }
        }

        private void SetUp() {
            //Check if is already login

            var loginModal = Modals.instance.OpenModal<LoginModal>();
            loginModal.SetUp(OnLoginEvent, OnSignUpEvent);
        }

        private void OnSignUpEvent(string email, string username, string password) {
            Debug.Log("OnSignUpEvent");

            var accountStruct = GetAccountLoginStruct(email, username, password, (int)LoginModal.State.SignUp);

        }

        private void OnLoginEvent(string email, string password)
        {
            Debug.Log("OnLoginEvent");
            var accountStruct = GetAccountLoginStruct(email, "", password, (int)LoginModal.State.Login);

            Debug.Log($"Email {accountStruct.email}, Password {password}, Encode Password {accountStruct.password}");



        }

        private JsonTypes.PuffAccountLoginType GetAccountLoginStruct(string email, string username, string password, int type) {
            JsonTypes.PuffAccountLoginType accountType = new JsonTypes.PuffAccountLoginType();
            accountType.email = email;
            accountType.username = username;
            accountType.password = UtilityMethod.Base64Encode(password);
            accountType.type = type;

            return accountType;
        }

    }
}