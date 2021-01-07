using Hsinpa.Utility;
using Puff.View;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AccountModel : MonoBehaviour
{
    private JsonTypes.PuffAccountType _puffAccountType;
    public JsonTypes.PuffAccountType puffAccountType => _puffAccountType;

    public void SetAccount(JsonTypes.PuffAccountType p_account) {
        _puffAccountType = p_account;

        if (_puffAccountType.isValid) {
            PlayerPrefs.SetString(EventFlag.PlayprefKey.LoginAccount, _puffAccountType._id);
            PlayerPrefs.SetString(EventFlag.PlayprefKey.AuthKey, _puffAccountType.auth_key);
        }
    }

    public async Task<APIHttpRequest.HttpResult> PerformSignLogin(JsonTypes.PuffAccountLoginType loginJsonType) {
        string url = (loginJsonType.type == (int)LoginModal.State.Login) ? GeneralFlag.API.Login : GeneralFlag.API.SignUp;
        return await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(loginJsonType));
    }

    public async Task<APIHttpRequest.HttpResult> PerformAuthkeyCheck()
    {
        string account_id = PlayerPrefs.GetString(EventFlag.PlayprefKey.LoginAccount, "");
        string authKey = PlayerPrefs.GetString(EventFlag.PlayprefKey.AuthKey, "");

        if (string.IsNullOrEmpty(authKey) || string.IsNullOrEmpty(account_id))
            return default(APIHttpRequest.HttpResult);

        JsonTypes.AuthLoginType authType = new JsonTypes.AuthLoginType();
        authType.account_id = account_id;
        authType.auth_key = authKey;

        return await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(GeneralFlag.API.AuthLogin), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(authType));
    }

}
