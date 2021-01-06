
using System;

public class JsonTypes {
    [System.Serializable]
    public struct PuffMessageType
    {
        public string _id;
        public string author_id;
        public string author;
        public string body;

        public string date;
        public string expire;

        public DateTime parseDate => DateTime.Parse(date);
        public DateTime parseExpire => DateTime.Parse(expire);

        public PuffCommentType[] comments;
    }

    [System.Serializable]
    public struct PuffCommentType
    {
        public string _id;
        public string author_id;
        public string author;
        public string body;

        public string date;
        public DateTime parseDate => DateTime.Parse(date);
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
        public bool isValid => !string.IsNullOrEmpty(_id);
    }

    public struct DatabaseResultType
    {
        public int status;
        public string result;
    }
}