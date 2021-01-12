using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffMsgCommentItem : MonoBehaviour
    {
        [SerializeField]
        private Text commentAuthor;

        [SerializeField]
        private Text commentContent;

        [SerializeField]
        private Text createTime;

        public void SetComment(JsonTypes.PuffCommentType commentType) {
            this.commentAuthor.text = commentType.author;
            this.commentContent.text = commentType.body;
            this.createTime.text = commentType.date;
        }

    }
}