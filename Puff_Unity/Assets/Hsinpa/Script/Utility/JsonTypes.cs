
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
}