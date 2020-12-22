using BestHTTP;
using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Puff.Model
{
    public class PuffModel
    {

        JsonTypes.PuffMessageType[] _puffMessageTypes = new JsonTypes.PuffMessageType[0];

        public async Task<JsonTypes.PuffMessageType[]> GetAllPuff() {

            APIHttpRequest.HttpResult rawPuffMsgData = await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(GeneralFlag.API.GetAll), BestHTTP.HTTPMethods.Get);

            if (rawPuffMsgData.isSuccess) {
                Debug.Log(rawPuffMsgData.body);
                _puffMessageTypes = JsonHelper.FromJson<JsonTypes.PuffMessageType>(rawPuffMsgData.body);

                foreach (var data in _puffMessageTypes) {
                    Debug.Log($"Body {data.body}, Author ID {data.author_id}, Date {data.parseDate}");
                }
                
            }

            return _puffMessageTypes;
        }

    }
}