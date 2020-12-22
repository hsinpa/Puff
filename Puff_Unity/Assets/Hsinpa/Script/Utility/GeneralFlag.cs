using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{

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
    }

    public class API
    {
        public const string GetAll = "get_all";
        public const string SendPuffComment = "send_puff_comment";
        public const string SendPuffMsg = "send_puff_msg";
    }

    public static string GetFullAPIUri(string apiUrl)
    {
        return Domain.LocalHost + apiUrl;
    }
}
