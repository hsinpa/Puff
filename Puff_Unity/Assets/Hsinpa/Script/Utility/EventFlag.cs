using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFlag 
{
    public class Event
    {
        public const string GameStart = "event@game_start";

        public const string OpenPuffMsg = "event@open_puff_msg";
        public const string OpenSendMsg = "event@open_send_msg";

        public const string LoginSuccessful = "event@login_success";
    }

    public class DatabaseStateType {
        public enum AccountState {Normal, Fail_Login_NoAccount, Fail_SignUp_DuplicateAccount, Fail_AuthLogin_NotValid };
    }

    public class PlayprefKey {
        public const string LoginAccount = "playpref@account_id";
        public const string AuthKey = "playpref@auth_key";
    }

}

