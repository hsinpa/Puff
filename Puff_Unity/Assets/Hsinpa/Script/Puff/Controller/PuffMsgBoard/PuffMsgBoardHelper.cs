using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.Ctrl.Utility {
    public class PuffMsgBoardHelper
    {

        public JsonTypes.PuffCommentType GetCommentType(string author_id, string body) {
            JsonTypes.PuffCommentType puffCommentType = new JsonTypes.PuffCommentType();

            puffCommentType.author_id = author_id;
            puffCommentType.body = body;

            return puffCommentType;
        }

        public JsonTypes.PuffMessageType GetCreateMessageType(string author_id, string body)
        {
            JsonTypes.PuffMessageType puffMessageType = new JsonTypes.PuffMessageType();

            puffMessageType.author = author_id;
            puffMessageType.author_id = author_id;
            puffMessageType.body = body;

            return puffMessageType;
        }
    }
}