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
        private LoginModal _loginModal;
        private AccountModel _accountModel;

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
            _accountModel = PuffApp.Instance.models.accountModel;

            //Check if is already login
            _loginModal = Modals.instance.GetModal<LoginModal>();
            _loginModal.SetUp(OnSignLoginEvent);

            CheckAuthkeyValid();
        }

        private async void CheckAuthkeyValid() {

            var r = await _accountModel.PerformAuthkeyCheck();
            bool isAuthValid = ProcessReturnAccountInfo(r);

            if (!isAuthValid) {
                Modals.instance.OpenModal<LoginModal>();
                _loginModal.Openpage(LoginModal.State.Login);
            }
        }

        private async void OnSignLoginEvent(string email, string username, string password) {
            Debug.Log("OnSignLoginEvent");

            _loginModal.signBtn.interactable = false;

            //Prepare and curl request
            LoginModal.State actionType = (username == "") ? LoginModal.State.Login : LoginModal.State.SignUp;
            var accountStruct = GetAccountLoginStruct(email, username, password, actionType);

            var rawResult = await _accountModel.PerformSignLogin(accountStruct);
            _loginModal.ShowErrorMsg("");
            _loginModal.signBtn.interactable = true;

            ProcessReturnAccountInfo(rawResult);
        }

        private bool ProcessReturnAccountInfo(APIHttpRequest.HttpResult rawResult) {
            //Internet Error
            if (!rawResult.isSuccess)
            {
                _loginModal.ShowErrorMsg(StringTextAsset.Login.InternetError);

                return false;
            }

            //Check Server Error
            JsonTypes.DatabaseResultType databaseResultType = JsonUtility.FromJson<JsonTypes.DatabaseResultType>(rawResult.body);
            switch (databaseResultType.status)
            {
                case (int)EventFlag.DatabaseStateType.AccountState.Fail_Login_NoAccount:
                    _loginModal.ShowErrorMsg(StringTextAsset.Login.DatabaseFail_Login);
                    return false;

                case (int)EventFlag.DatabaseStateType.AccountState.Fail_SignUp_DuplicateAccount:
                    _loginModal.ShowErrorMsg(StringTextAsset.Login.DatabaseFail_SignUp);
                    return false;

                //No error feedback needed
                case (int)EventFlag.DatabaseStateType.AccountState.Fail_AuthLogin_NotValid:
                    return false;
            }

            Debug.Log(databaseResultType.result);
            JsonTypes.PuffAccountType account = JsonUtility.FromJson<JsonTypes.PuffAccountType>(databaseResultType.result);
            _accountModel.SetAccount(account);

            Modals.instance.CloseAll();
            PuffApp.Instance.Notify(EventFlag.Event.LoginSuccessful);
            return true;
        }

        private JsonTypes.PuffAccountLoginType GetAccountLoginStruct(string email, string username, string password, LoginModal.State type) {
            JsonTypes.PuffAccountLoginType accountType = new JsonTypes.PuffAccountLoginType();
            accountType.email = email;
            accountType.username = username;
            accountType.password = UtilityMethod.Base64Encode(password);
            accountType.type = (int)type;

            return accountType;
        }

    }
}