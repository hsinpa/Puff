﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.View;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace Puff.View
{
    public class LoginModal : Modal
    {
        [SerializeField]
        private Text errorTips;

        [SerializeField]
        private InputField usernameField;

        [SerializeField]
        private InputField emailField;

        [SerializeField]
        private InputField passwordField;

        [SerializeField]
        private InputField confirmPasswordField;

        [SerializeField]
        private Toggle agreementToggle;

        [SerializeField]
        private Button _signBtn;
        public Button signBtn => _signBtn;

        private Text signBtnText => _signBtn.GetComponentInChildren<Text>();

        [SerializeField]
        private Button signSwitch;
        private Text signSwitchText => signSwitch.GetComponent<Text>();

        private System.Action<string, string, string> OnSignSubmitEvent;
        public enum State { Login, SignUp }
        private State _state = State.Login;

        protected override void Start()
        {
            base.Start();

            signSwitch.onClick.AddListener(() =>
            {
                Openpage((_state == State.Login) ? State.SignUp : State.Login);
            });
            _signBtn.onClick.AddListener(OnSubmitEvent);

            Openpage(State.Login);
        }

        public void SetUp(System.Action<string, string, string> signInEvent) {
            OnSignSubmitEvent = signInEvent;
        }

        public void ShowErrorMsg(string error_msg) {
            errorTips.text = error_msg;
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
            signBtnText.text = StringTextAsset.Login.LoginBtn;
            signSwitchText.text = StringTextAsset.Login.SignUpTip;
        }

        private void OpenSignInPage() {
            agreementToggle.gameObject.SetActive(true);
            usernameField.gameObject.SetActive(true);
            confirmPasswordField.gameObject.SetActive(true);
            signBtnText.text = StringTextAsset.Login.SignUpBtn;
            signSwitchText.text = StringTextAsset.Login.LoginTip;
        }

        private void EmptyPageContent() {
            emailField.text = "";
            passwordField.text = "";
            usernameField.text = "";
            confirmPasswordField.text = "";
            errorTips.text = "";
            agreementToggle.isOn = false;

            confirmPasswordField.gameObject.SetActive(false);
            usernameField.gameObject.SetActive(false);
            agreementToggle.gameObject.SetActive(false);
        }

        private void OnSubmitEvent() {
            if (_state == State.Login)
                ProcessLoginValidation();

            if ( _state == State.SignUp)
                ProcessSignupValidation();
        }

        private void ProcessLoginValidation() {

            string errorMessage = "";

            if (! AccountModel.CheckEmail(emailField.text) && errorMessage == "")
                errorMessage = StringTextAsset.Login.EmailWrongFormat;

            if (!AccountModel.CheckPassword(passwordField.text) && errorMessage == "")
                errorMessage = StringTextAsset.Login.PasswordWrongFormat; 

            errorTips.text = errorMessage;

            if (errorMessage == "")
                OnSignSubmitEvent(emailField.text, "",passwordField.text);
        }

        private void ProcessSignupValidation()
        {
            string errorMessage = "";

            //Check Username validation
            if (string.IsNullOrEmpty(usernameField.text) && errorMessage == "")
                errorMessage = StringTextAsset.Login.UserWrongFormat;

            if (!AccountModel.CheckEmail(emailField.text) && errorMessage == "")
                errorMessage = StringTextAsset.Login.EmailWrongFormat;

            if (!AccountModel.CheckPassword(passwordField.text) && errorMessage == "")
                errorMessage = StringTextAsset.Login.PasswordWrongFormat;

            if (passwordField.text != confirmPasswordField.text && errorMessage == "")
                errorMessage = StringTextAsset.Login.PasswordRepeatFormat;

            if (!agreementToggle.isOn && errorMessage == "")
                errorMessage = StringTextAsset.Login.AgreementToggleFormat;

            errorTips.text = errorMessage;

            if (errorMessage == "")
                OnSignSubmitEvent(emailField.text, usernameField.text, passwordField.text);
        }

    }
}