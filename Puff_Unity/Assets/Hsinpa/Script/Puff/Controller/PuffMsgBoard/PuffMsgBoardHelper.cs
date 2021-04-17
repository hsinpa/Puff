using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.Ctrl.Utility {
    public class PuffMsgBoardHelper
    {

        public static JsonTypes.PuffCommentType GetCommentType(string puff_id, string author_id, string author_name, string body) {
            JsonTypes.PuffCommentType puffCommentType = new JsonTypes.PuffCommentType();

            puffCommentType.message_id = puff_id;
            puffCommentType.author_id = author_id;
            puffCommentType.body = body;
            puffCommentType.author = author_name;

            return puffCommentType;
        }

        public static JsonTypes.PuffMessageType GetCreateMessageType(int type, string author_id, string author_name, string body, string title,
            int privacy, int duration, float distance, GPSLocationService.LocationInfo locationInfo)
        {
            JsonTypes.PuffMessageType puffMessageType = new JsonTypes.PuffMessageType();

            puffMessageType.author = author_name;
            puffMessageType.author_id = author_id;
            puffMessageType.body = body;
            puffMessageType.title = title;

            puffMessageType.type = type;
            puffMessageType.privacy = privacy;
            puffMessageType.duration = duration;
            puffMessageType.distance = distance;

#if UNITY_EDITOR
            //Geographic info of Taipei, only use during editor mode
            locationInfo.longitude = 121.564315f;
            locationInfo.latitude = 24.906367f;
#endif

            puffMessageType.geo_location = new JsonTypes.GeographicType(locationInfo.longitude, locationInfo.latitude); // Longitude, Latitude

            return puffMessageType;
        }
    }
}