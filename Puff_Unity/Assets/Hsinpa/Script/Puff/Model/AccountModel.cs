using Hsinpa.Utility;
using Puff.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountModel : MonoBehaviour
{
    private JsonTypes.PuffAccountType _puffAccountType;
    public JsonTypes.PuffAccountType puffAccountType => _puffAccountType;

    public async void PerformSignLogin(JsonTypes.PuffAccountLoginType loginJsonType) {
        string url = (loginJsonType.type == (int)LoginModal.State.Login) ? GeneralFlag.API.Login : GeneralFlag.API.SignUp;
        APIHttpRequest.HttpResult rawPuffMsgData = await APIHttpRequest.Curl(GeneralFlag.GetFullAPIUri(url), BestHTTP.HTTPMethods.Post, JsonUtility.ToJson(loginJsonType));

        if (rawPuffMsgData.isSuccess)
        {
            
        }
    }


    }
