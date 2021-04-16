
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
        public float longitidue;

        public GeographicType geo_location;

        public string date;
        public string expire;

        public DateTime parseDate => DateTime.Parse(date);
        public DateTime parseExpire => DateTime.Parse(expire);

        public List<PuffCommentType> comments;
        public List<string> images;
    }

    [System.Serializable]
    public struct GeographicType {
        public float[] coordinates;
        public string type;

        public GeographicType(float longitude, float latitude) {
            coordinates = new float[] { longitude, latitude };
            type = "Point";
        }
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

    [System.Serializable]
    public struct PuffAccountType
    {
        public string _id;
        public string username;
        public string email;
        public string auth_key;

        public bool isValid => !string.IsNullOrEmpty(_id);
    }

    [System.Serializable]
    public struct PuffSaveLibAction
    {
        public string account_id;
        public string puff_id;
        public string auth_key;
    }

    [System.Serializable]
    public struct FriendListType {
        public string _id;
        public string email;

        public FriendType[] friend_info;
    }

    [System.Serializable]
    public struct FriendType {
        public string _id;
        public string email;
        public string username;
        public FriendStatus status;
    }

    [System.Serializable]
    public struct FriendActionJson
    {
        public string account_id;
        public string target_id;
        public string auth_key;
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

    public struct IMGBBType {
        public IMGBBDataType data;
        public bool success;
    }

    public struct IMGBBDataType {
        public string url;
    }

    #region Enum 
    public enum PuffTypes { FloatSeed = 0, Plant }
    
    public enum FriendStatus { Friends = 0, RequestFriend, ReceiveRequest, Block }

    #endregion
}