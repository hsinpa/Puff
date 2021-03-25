﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{
    public static Vector3 SharedVectorUnit = Vector3.zero;

    public class Colors { 
        public static readonly Color HideColor = new Color(1,1,1,0);
        public static readonly Color DisplayColor = new Color(1,1,1,1);

    }

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
        //Messages
        public const string GetAll = "get_all/{0}/{1}/{2}";
        public const string SendPuffComment = "send_puff_comment";
        public const string SendPuffMsg = "send_puff_msg";

        //Account
        public const string Login = "account/login";
        public const string SignUp = "account/sign_up";
        public const string AuthLogin = "account/auth_login";
        
        public const string SavePuffMsg = "account/save_puff_msg";
        public const string RemovePuffMsg = "account/release_puff_msg";

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
        return Domain.LocalHost + apiUrl;
    }
}
