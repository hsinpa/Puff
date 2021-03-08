using Hsinpa.Utility;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Hsinpa.Model {
    public class FriendModel
    {
        JsonTypes.FriendListType _cacheFriendList;

        public JsonTypes.FriendActionJson GetActionJSON(string accountID, string targetID, string auth_key) {
            var json = new JsonTypes.FriendActionJson();
            json.target_id = targetID;
            json.account_id = accountID;
            json.auth_key = auth_key;
            return json;
        }

        public async Task<JsonTypes.FriendListType> GetFriend(string account_id) {
            string url = string.Format(GeneralFlag.API.GetFriends, account_id);

            APIHttpRequest.HttpResult httpResult = await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Get);
            try
            {
                if (httpResult.isSuccess)
                    _cacheFriendList = JsonUtility.FromJson<JsonTypes.FriendListType>(httpResult.body);
            }
            catch {
                Debug.Log("Get Friend Error " + httpResult.body);
                _cacheFriendList = default(JsonTypes.FriendListType);
            }

            return _cacheFriendList;
        }

        public async Task AcceptFriend(JsonTypes.FriendActionJson friendJson)
        {
            string url = (GeneralFlag.API.AcceptFriend);
            await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(friendJson) );
        }

        public async Task RejectFriend(JsonTypes.FriendActionJson friendJson)
        {
            string url = (GeneralFlag.API.RejectFriend);
            await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(friendJson));
        }

        public void UpdateFriendList() { 
        
        }


    }
}

