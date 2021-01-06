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
    }

    public async Task<APIHttpRequest.HttpResult> PerformSignLogin(JsonTypes.PuffAccountLoginType loginJsonType) {
        string url = (loginJsonType.type == (int)LoginModal.State.Login) ? GeneralFlag.API.Login : GeneralFlag.API.SignUp;
        return await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(loginJsonType));
    }


}
