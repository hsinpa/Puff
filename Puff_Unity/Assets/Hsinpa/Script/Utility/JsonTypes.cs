
using System;

public class JsonTypes {
    [System.Serializable]
    public struct PuffMessageType
    {
        public string _id;
        public string author_id;
        public string author;
        public string body;
        public DateTime date;
        public DateTime expire;

        public PuffCommentType[] comments;
    }

    [System.Serializable]
    public struct PuffCommentType
    {
        public string _id;
        public string author_id;
        public string author;
        public string body;
        public DateTime date;
    }
}