using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Puff.Model {
    public class PuffSaveMsgUtility
    {
        private List<JsonTypes.PuffMessageType> _puffMessageTypes;
        public List<JsonTypes.PuffMessageType> cachePuffMsgTypes => _puffMessageTypes;

        public bool IsDuplicate(string puff_id) {
            return _puffMessageTypes.FindIndex(x => x._id == puff_id) >= 0;
        }

        public async Task<List<JsonTypes.PuffMessageType>> GetSaveMsgFromServer(JsonTypes.PuffSaveLibAction puffSaveLibAction)
        {
            string url = GeneralFlag.API.GetAllSavePuff;

            APIHttpRequest.HttpResult rawServerResult = await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(puffSaveLibAction));

            _puffMessageTypes = JsonHelper.FromJson<JsonTypes.PuffMessageType>(rawServerResult.body).ToList();

            Debug.Log("_puffMessageTypes length " + _puffMessageTypes.Count);

            return _puffMessageTypes;
        }

        public List<JsonTypes.PuffMessageType> GetFilterCacheMsg(State state, string account_id) {
            return cachePuffMsgTypes.FindAll(x => (state == State.Self) ? x.author_id == account_id : x.author_id != account_id);
        }

        public async Task<bool> AddNewSaveMsg(JsonTypes.PuffMessageType savePuffType, JsonTypes.PuffSaveLibAction puffSaveLibAction) {

            _puffMessageTypes.Add(savePuffType);

            string url = GeneralFlag.API.SavePuffToLibrary;
            APIHttpRequest.HttpResult rawServerResult = await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(puffSaveLibAction));

            JsonTypes.DatabaseResultType databaseResultType = JsonUtility.FromJson<JsonTypes.DatabaseResultType>(rawServerResult.body);

            return databaseResultType.status == (int)EventFlag.DatabaseStateType.AccountState.Normal;
        }

        public void RemoveNewMsg(JsonTypes.PuffSaveLibAction puffSaveLibAction) {
            string url = GeneralFlag.API.RemovePuffFromLibrary;
            _ = APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(puffSaveLibAction));

            int cacheIndex = _puffMessageTypes.FindIndex(x => x._id == puffSaveLibAction.puff_id);
            if (cacheIndex >= 0)
                _puffMessageTypes.RemoveAt(cacheIndex);
        }

        public static JsonTypes.PuffSaveLibAction GetPuffSaveActionType(string account_id, string puff_id = "") {
            JsonTypes.PuffSaveLibAction puffSaveLibAction = new JsonTypes.PuffSaveLibAction();
            puffSaveLibAction.account_id = account_id;
            puffSaveLibAction.puff_id = puff_id;
            puffSaveLibAction.auth_key = "";

            return puffSaveLibAction;
        }

        public enum State { 
            Self, Other
        }
    }
}
