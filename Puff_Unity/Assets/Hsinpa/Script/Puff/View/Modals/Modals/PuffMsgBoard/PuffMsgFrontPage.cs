using Hsinpa.Utility;
using Hsinpa.View;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View {
    public class PuffMsgFrontPage : PuffMsgInnerPage
    {
        [SerializeField]
        private Text title;

        [SerializeField]
        private Text author;

        [SerializeField]
        private Text create_date;

        [SerializeField]
        private Text Description;

        [SerializeField]
        private InputField _ReplyInputfield;
        private InputField ReplyInputfield => _ReplyInputfield;

        [SerializeField]
        private Button _ReplyBtn;
        private Button ReplyBtn => _ReplyBtn;

        [SerializeField]
        private RectTransform commentsHolder;

        [SerializeField]
        private PuffMsgCommentItem commentItemPrefab;

        public void SetContent(JsonTypes.PuffMessageType puffMsgType, System.Action<string> ReplyBtnEvent) {
            title.text = puffMsgType.author;
            author.text = puffMsgType.author;
            create_date.text = puffMsgType.parseDate.ToString("MM/dd/yyyy hh:mm tt");
            Description.text = puffMsgType.body;

            _ReplyInputfield.text = "";

            _ReplyBtn.onClick.RemoveAllListeners();
            _ReplyBtn.onClick.AddListener(() => {
                if (!string.IsNullOrEmpty(_ReplyInputfield.text))
                    ReplyBtnEvent(_ReplyInputfield.text);
            });

            if (puffMsgType.comments != null)
                GenerateComments(puffMsgType.comments);
        }

        private void GenerateComments(List<JsonTypes.PuffCommentType> commentList) {
            int maxLength = 3;
            int commentCount = commentList.Count;
            int commentStopIndex = (int)Mathf.Clamp(commentCount - maxLength, 0, commentCount);

            UtilityMethod.ClearChildObject(commentsHolder);

            for (int i = commentCount - 1; i >= commentStopIndex; i--)
            {
                InsertNewComments(commentList[i]);
            }
        }

        public void InsertNewComments(JsonTypes.PuffCommentType comment) {
            GameObject commentObject = UtilityMethod.CreateObjectToParent(commentsHolder, commentItemPrefab.gameObject);
            PuffMsgCommentItem commentItem = commentObject.GetComponent<PuffMsgCommentItem>();

            commentItem.name = comment._id;
            commentItem.SetComment(comment);
        }

        public void EnableReplyInput(bool enable) {
            ReplyBtn.interactable = enable;

            if (enable) {
                _ReplyInputfield.text = "";
            }
        }
    }
}