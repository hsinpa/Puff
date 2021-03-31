using BestHTTP;
using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using SimpleJSON;

namespace Puff.Model
{
    public class PuffModel
    {

        Dictionary<string, JsonTypes.PuffMessageType> _puffMessageTypes = new Dictionary<string, JsonTypes.PuffMessageType>();

        public System.Action<List<JsonTypes.PuffMessageType>> OnReceiveNewPuffMsgEvent;

        public JsonTypes.PuffMessageType GetMessageTypeByID(string id)
        {
            if (_puffMessageTypes.TryGetValue(id, out JsonTypes.PuffMessageType messageType))
            {
                return messageType;
            }
            return default(JsonTypes.PuffMessageType);
        }

       public async Task<List<JsonTypes.PuffMessageType>> GetAllPuff(GPSLocationService.LocationInfo locationInfo) {
            int defaultRadius = 10000;
            string url = string.Format(GeneralFlag.API.GetAll, locationInfo.latitude, locationInfo.longitude, defaultRadius);

            APIHttpRequest.HttpResult rawPuffMsgData = await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Get);

            List<JsonTypes.PuffMessageType> puffArray = new List<JsonTypes.PuffMessageType>();

            if (rawPuffMsgData.isSuccess) {

                puffArray = JsonHelper.FromJson<JsonTypes.PuffMessageType>(rawPuffMsgData.body).ToList();

                RegisterNewPuffMsg(puffArray);
            }

            return puffArray;
        }

        public async Task<APIHttpRequest.HttpResult> PushCommentToServer(JsonTypes.PuffCommentType commentType) {
            APIHttpRequest.HttpResult rawPuffMsgData = await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(GeneralFlag.API.SendPuffComment), 
                                                                                BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(commentType));
            return rawPuffMsgData;
        }

        public async Task<List<string>> UploadTextureToIMGBB(List<byte[]> texBytes) {
            int taskCount = texBytes.Count;
            Task<string>[] waitingTask = new Task<string>[taskCount];

            for (int i = 0; i < taskCount; i++) {
                waitingTask[i] = APIHttpRequest.CurlIMGBB(texBytes[i]);
            }

            var imgbbResults =  await Task.WhenAll(waitingTask);

            return ParseIMGBBData(imgbbResults);
        }

        private List<string> ParseIMGBBData(string[] imgbbResults) {

            List<string> results = new List<string>();

            try
            {
                for (int i = 0; i < imgbbResults.Length; i++)
                {

                    Debug.Log(imgbbResults[i]);

                    JSONNode json = JSONNode.Parse(imgbbResults[i]);

                    bool success = json["success"].AsBool;

                    if (success)
                        results.Add(json["data"]["url"]);
                }
            }
            catch {
                Debug.LogError("ParseIMGBBData Error");
            }


            return results;
        }

        public bool UpdateMessageComments(string msg_id, JsonTypes.PuffCommentType commentType) {
            if (_puffMessageTypes.TryGetValue(msg_id, out JsonTypes.PuffMessageType messageType)) {

                if (messageType.comments == null)
                    messageType.comments = new List<JsonTypes.PuffCommentType>();

                messageType.comments.Add(commentType);

                _puffMessageTypes[msg_id] = messageType;
                return true;
            }
            return false;
        }
       

        private async void RegisterNewPuffMsg(List<JsonTypes.PuffMessageType> puffArray) {
            var filterArray = await FilterExistPuffMsg(puffArray);
            
            if (OnReceiveNewPuffMsgEvent != null)
            {
                OnReceiveNewPuffMsgEvent(filterArray);
            }
        }

        private async Task<List<JsonTypes.PuffMessageType>> FilterExistPuffMsg(List<JsonTypes.PuffMessageType> puffArray) {

                var filterPuff = await Task.Run(() =>
                {
                    lock (_puffMessageTypes)
                    {
                        int puffLength = puffArray.Count;

                        for (int i = puffLength - 1; i >= 0; i--) {

                            if (_puffMessageTypes.ContainsKey(puffArray[i]._id)) {
                                puffArray.RemoveAt(i);
                                continue;
                            }

                            _puffMessageTypes.Add(puffArray[i]._id, puffArray[i]);
                        }

                    }
                    return puffArray;
                });

            return filterPuff;
        }

    }
}