using BestHTTP;
using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Puff.Model
{
    public class PuffModel
    {

        Dictionary<string, JsonTypes.PuffMessageType> _puffMessageTypes = new Dictionary<string, JsonTypes.PuffMessageType>();

        public System.Action<List<JsonTypes.PuffMessageType>> OnReceiveNewPuffMsgEvent;

        public async Task<List<JsonTypes.PuffMessageType>> GetAllPuff() {

            APIHttpRequest.HttpResult rawPuffMsgData = await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(GeneralFlag.API.GetAll), BestHTTP.HTTPMethods.Get);

            List<JsonTypes.PuffMessageType> puffArray = new List<JsonTypes.PuffMessageType>();

            if (rawPuffMsgData.isSuccess) {
                Debug.Log(rawPuffMsgData.body);

                puffArray = JsonHelper.FromJson<JsonTypes.PuffMessageType>(rawPuffMsgData.body).ToList();

                RegisterNewPuffMsg(puffArray);
            }

            return puffArray;
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