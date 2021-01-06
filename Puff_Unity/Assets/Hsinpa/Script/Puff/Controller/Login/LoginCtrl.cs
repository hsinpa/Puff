using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.Ctrl.Utility;
using Hsinpa.View;
using Puff.View;

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
            loginModal.SetUp(OnLoginEvent, OnSignInEvent);
        }

        private void OnSignInEvent(string email, string username, string password) {
            Debug.Log("OnSignInEvent");
        }

        private void OnLoginEvent(string email, string password)
        {
            Debug.Log("OnLoginEvent");
        }

    }
}