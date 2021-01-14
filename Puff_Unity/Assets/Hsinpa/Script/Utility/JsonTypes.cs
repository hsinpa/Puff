
using System;
using System.Collections.Generic;

public class JsonTypes {
    [System.Serializable]
    public struct PuffMessageType
    {
        public string _id;
        public string author_id;
        public string author;
        public string body;
        public string title;

        public int type;
        public int privacy;
        public int duration;

        public float distance;
        public float latitude;
        public float longitude;

        public string date;
        public string expire;

        public DateTime parseDate => DateTime.Parse(date);
        public DateTime parseExpire => DateTime.Parse(expire);

        public List<PuffCommentType> comments;
        public List<string> images;
    }

    [System.Serializable]
    public struct PuffCommentType
    {
        public string _id;
        public string message_id;
        public string author_id;
        public string author;
        public string body;

        public string date;
        public DateTime parseDate => DateTime.Parse(date);

        //Not upload to server yet
        public bool isFake => string.IsNullOrEmpty(_id);
    }

    [System.Serializable]
    public struct PuffAccountLoginType {
        public string password;
        public string username;
        public string email;
        public int type;
    }

    public struct PuffAccountType
    {
        public string _id;
        public string username;
        public string email;
        public string auth_key;
        public bool isValid => !string.IsNullOrEmpty(_id);
    }

    public struct AuthLoginType
    {
        public string account_id;
        public string auth_key;
    }

    public struct DatabaseResultType
    {
        public int status;
        public string result;
    }
}