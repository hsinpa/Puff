using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Utility
{
    public class GPSLocationService
    {
        public static LocationInfo cacheInfo;

        public static void GetGPS(MonoBehaviour mono, bool allowCache, System.Action<LocationInfo> callback)
        {

            if (allowCache && cacheInfo.isSuccess)
            {
                callback(cacheInfo);
                return;
            }

            mono.StartCoroutine(StartGPS((LocationInfo location) =>
            {

                if (location.isSuccess)
                {
                    cacheInfo = location;
                }

                if (callback != null)
                    callback(location);
            }));
        }

        private static IEnumerator StartGPS(System.Action<LocationInfo> callback)
        {

            LocationInfo location = new LocationInfo();
            location.isSuccess = false;

            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
            {
                callback(location);
                yield break;
            }
            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                Debug.LogError("Timed out");
                Input.location.Stop();

                callback(location);

                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped)
            {
                Debug.LogError("Unable to determine device location");
                Input.location.Stop();

                callback(location);
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude);

                location.latitude = Input.location.lastData.latitude;
                location.latitude = Input.location.lastData.longitude;
                location.isSuccess = true;

                Input.location.Stop();

                callback(location);
            }
        }

        public struct LocationInfo
        {
            public float latitude;
            public float longitude;

            public bool isSuccess;
        }

    }
}