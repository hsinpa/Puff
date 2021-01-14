using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{
    public static Vector3 SharedVectorUnit = Vector3.zero;

    public class RegularExpression {
        public const string Email = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";
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
        public const string GetAll = "get_all/{0}/{1}/{2}";
        public const string SendPuffComment = "send_puff_comment";
        public const string SendPuffMsg = "send_puff_msg";

        public const string Login = "login";
        public const string SignUp = "sign_up";
        public const string AuthLogin = "auth_login";
    }

    public static string GetFullAPIUri(string apiUrl)
    {
        return Domain.GoogleServer + apiUrl;
    }
}
