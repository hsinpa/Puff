using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{
    public static Vector3 SharedVectorUnit = Vector3.zero;

    public class Colors { 
        public static readonly Color HideColor = new Color(1,1,1,0);
        public static readonly Color DisplayColor = new Color(1,1,1,1);
        public static readonly Color HighLightColor = new Color(0.976f, 0.913f, 0.8196f, 1);

        public static readonly Color ToastColorNormal = new Color(0.08235294f, 0.4313726f, 0.345098f, 1);
        public static readonly Color ToastColorError = new Color(0.735849f, 0.227451f, 0.1843137f, 1);
        public static readonly Color ToastColorWarning = new Color(0.7169812f, 0.4941176f, 0.1254902f, 1);
    }

    public class RegularExpression {
        public const string Email = @"(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)";
        public const string UniversalSyntaxRex = @"^.{6,30}$";
    }

    public class Puff {
        public const int Layer = 1 << 8;
    }

    public enum PuffMsgBoardState {
        Creator,
        Reviewer
    }

    public class String {
        public const string SaveToMailbox = "Save to mailbox";
        public const string ReleaseBackToSky = "Back to sky";
    }

    public class Domain
    {
        public const string LocalHost = "http://localhost:8020/";
        public const string GoogleServer = "http://35.222.38.95:81/";
    }

    public class API
    {
        //Messages
        public const string GetAll = "puff/get_all/{0}/{1}/{2}";
        public const string SendPuffComment = "puff/send_puff_comment";
        public const string SendPuffMsg = "puff/send_puff_msg";

        //Puff Save / Safe Msg
        public const string GetAllSavePuff = "puff/get_all_self_msg"; //account_id
        public const string SavePuffToLibrary = "puff/save_puff_to_library"; //puff_id, account_id
        public const string RemovePuffFromLibrary = "puff/remove_puff_from_library"; //puff_id, account_id

        //Account
        public const string Login = "account/login";
        public const string SignUp = "account/sign_up";
        public const string AuthLogin = "account/auth_login";
        public const string PublicInfoByEmail = "account/publicInfo/email/{0}";
        public const string PublicInfoByID = "account/publicInfo/id/{0}";

        //Friend
        public const string GetFriends = "friends/{0}";
        //Post
        public const string AddFriends = "friends/request_friend";
        public const string AcceptFriend = "friends/accept_friend";
        public const string RejectFriend = "friends/reject_friend";
    }

    public class IMGBB {
        public const string API_KEY = "6c02340a0965edd308b94dc05669f294";
    }

    public static string GetFullAPIUri(string apiUrl)
    {
        return Domain.GoogleServer + apiUrl;
    }
}
