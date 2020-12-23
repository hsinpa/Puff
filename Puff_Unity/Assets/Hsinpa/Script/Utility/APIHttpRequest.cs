using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp;
using UnityEngine.Networking;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hsinpa.Utility {
    public class APIHttpRequest
    {

        static HttpResult _httpResult = new HttpResult();

        public static async Task<HttpResult> Curl(string url, HTTPMethods httpMethods, string rawJsonObject = null)
        {
            var request = new HTTPRequest(new System.Uri(url), httpMethods);
            if (rawJsonObject != null) {
                request.AddHeader("Content-Type", "application/json");
                request.RawData = Encoding.UTF8.GetBytes(rawJsonObject);
            }
            request.DisableCache = true;

            try
            {
                var resultString = await request.GetAsStringAsync();
                _httpResult.isSuccess = true;
                _httpResult.body = resultString;
            }
            catch (System.Exception ex)
            {
                _httpResult.isSuccess = false;
                Debug.LogException(ex);
            }

            request.Dispose();

            return _httpResult;
        }

        //public static IEnumerator NativeCurl(string url, string httpMethods, string rawJsonObject, System.Action<string> success_callback, System.Action fail_callback)
        //{
        //    using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        //    {
        //        webRequest.timeout = 40;
        //        webRequest.method = httpMethods;

        //        if (rawJsonObject != null) {
        //            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(rawJsonObject));
        //            webRequest.uploadHandler.contentType = "application/json";
        //        }

        //        // Request and wait for the desired page.
        //        yield return webRequest.SendWebRequest();

        //        if (webRequest.isNetworkError) {
        //            if (fail_callback != null) fail_callback();
        //            Debug.Log("Web  Error " + webRequest.error);

        //            yield break;
        //        }

        //        try
        //        {
        //            string rawJSON = webRequest.downloadHandler.text;
        //            var DatabaseResult = JsonUtility.FromJson<TypeFlag.SocketDataType.GeneralDatabaseType>(rawJSON);
        //            Debug.Log(rawJSON);
        //            Debug.Log(DatabaseResult.result);

        //            if (DatabaseResult.status && !string.IsNullOrEmpty(DatabaseResult.result))
        //            {
        //                if (success_callback != null) success_callback(DatabaseResult.result);
        //            }
        //            else {
        //                if (fail_callback != null) fail_callback();
        //            }
        //        }
        //        catch {
        //            if (fail_callback != null) fail_callback();
        //            Debug.Log("Web  Error " + webRequest.error);
        //        }
        //    }
        //}

        public struct HttpResult {
            public bool isSuccess;
            public string body;
        }
    }

}
