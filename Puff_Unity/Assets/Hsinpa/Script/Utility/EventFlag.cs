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
    }

    public class Domain
    {
        public const string Local = "http://localhost:8020/";
    }

    public class API
    {
        public const string Login = "login";
        public const string Register = "register";

        public const string GetPuffMsg = "get_all";
        public const string SendPuffMsg = "send_puff_msg";
        public const string ReplyPuffMsg = "reply_puff_msg";
    }

    public static string GetFullAPIUri(string apiUrl)
    {
        return Domain.Local + apiUrl;
    }

}

