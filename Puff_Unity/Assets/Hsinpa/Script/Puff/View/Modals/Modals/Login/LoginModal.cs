using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.View;
using UnityEngine.UI;

namespace Puff.View
{
    public class LoginModal : Modal
    {
        [SerializeField]
        private InputField emailField;

        [SerializeField]
        private InputField passwordField;

        [SerializeField]
        private Toggle agreementToggle;

        [SerializeField]
        private Button signBtn;
        private Text signBtnText => signBtn.GetComponentInChildren<Text>();

        [SerializeField]
        private Button signSwitch;
        private Text signSwitchText => signSwitch.GetComponent<Text>();

        private System.Action<string, string> OnLoginEvent;
        private System.Action<string, string> OnSignInEvent;
        public enum State { Login, SignUp }
        private State _state = State.Login;

        protected override void Start()
        {
            base.Start();

            signSwitch.onClick.AddListener(() =>
            {
                Openpage((_state == State.Login) ? State.SignUp : State.Login);
            });
            signBtn.onClick.AddListener(OnSubmitEvent);

            Openpage(State.Login);
        }

        public void SetUp(System.Action<string, string> loginEvent, System.Action<string, string> signInEvent) {
            OnLoginEvent = loginEvent;
            OnSignInEvent = signInEvent;
        }

        public void Openpage(State p_state) {
            EmptyPageContent();
            this._state = p_state;

            if (this._state == State.Login)
                OpenLoginPage();
            if (this._state == State.SignUp)
                OpenSignInPage();
        }

        private void OpenLoginPage() {
            emailField.text = "";
            signBtnText.text = StringTextAsset.Login.SignUpBtn;
            signSwitchText.text = StringTextAsset.Login.SignUpTip;
        }

        private void OpenSignInPage() {
            agreementToggle.gameObject.SetActive(true);
            signBtnText.text = StringTextAsset.Login.LoginBtn;
            signSwitchText.text = StringTextAsset.Login.LoginTip;
        }

        private void EmptyPageContent() {
            emailField.text = "";
            passwordField.text = "";
            agreementToggle.gameObject.SetActive(false);
        }

        private void OnSubmitEvent() {
            if (OnLoginEvent != null && _state == State.Login)
                OnLoginEvent(emailField.text, passwordField.text);

            if (OnSignInEvent != null && _state == State.SignUp)
                OnSignInEvent(emailField.text, passwordField.text);
        }


    }
}