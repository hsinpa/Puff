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

        public const string EnterCameraMode = "event@enter_camera_mode";
        public const string ExitCameraMode = "event@exit_camera_mode";

        public const string OnProfileOpen = "event@profile_page_open";

        public const string OnProfileAccountIDSearch = "event@profile_accountID_search";

        //Generally, want to enable/disable according to wheter we need it or not;
        public const string GameWorldUpdateEnable = "event@game_world_enable";
        public const string GameWorldUpdateDisable = "event@game_world_disable";

        public const string LoginSuccessful = "event@login_success";
    }

    public class DatabaseStateType {
        public enum AccountState {Normal, Fail_Login_NoAccount, Fail_SignUp_DuplicateAccount, Fail_AuthLogin_NotValid };
    }

    public class PlayprefKey {
        public const string LoginAccount = "playpref@account_id";
        public const string AuthKey = "playpref@auth_key";
    }

    public class ModuleActionButton {
        public const string CameraBtn = "CameraBtn";
        public const string ReviewBtn = "ReviewBtn";
    }

}

