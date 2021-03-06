﻿using Puff.Model;
using Puff.View;
using Puff.WorldManager;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Hsinpa.Utility;
using LitJson;

namespace Puff.Ctrl
{
    public class PuffARCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private ARSession arSession;

        [SerializeField]
        private PuffItemManager puffItemManager;

        [SerializeField, Range(5, 20)]
        private int APIRequestCycleTime = 5;
        private float APIRequestTimeRecord;
        private bool enableAPIRequest = false;

        private PuffModel _puffModel;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        Init();
                    }
                    break;

                case EventFlag.Event.LoginSuccessful:
                    {
                        enableAPIRequest = true;

                        InitiateAppDataset();
                    }
                    break;
            }
        }

        private void Init() {
            _puffModel = PuffApp.Instance.models.puffModel;
            _puffModel.OnReceiveNewPuffMsgEvent += RenderPuffObjectFromDatabase;

#if UNITY_ANDROID
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
#endif
        }

        private void InitiateAppDataset()
        {
            var friendModel = PuffApp.Instance.models.friendModel;
            var accountModel = PuffApp.Instance.models.accountModel;

            _ = this._puffModel.puffSaveMsgUtility.GetSaveMsgFromServer(PuffSaveMsgUtility.GetPuffSaveActionType(accountModel.puffAccountType._id));
            _ = friendModel.GetFriend(accountModel.puffAccountType._id);
        }

        private void Update()
        {
            if (Time.time > APIRequestTimeRecord && enableAPIRequest) {
                RefreshGPSInfo();
                APIRequestTimeRecord = Time.time + APIRequestCycleTime;
            }
        }

        //Sky Floating Object
        private PuffItemView GeneratePuffObjectToWorld(JsonTypes.PuffMessageType puffMsg) {
            Vector3 originPoint = _camera.transform.forward * 1.5f;

            float xPos = originPoint.x + Random.Range(-3.5f, 3.5f);
            float yPos = Random.Range(1.2f, 2.5f);
            float zPos = originPoint.z + Random.Range(-3.5f, 3.5f);

            GeneralFlag.SharedVectorUnit.Set(xPos, yPos, zPos);

            Vector3 randomPosition = GeneralFlag.SharedVectorUnit;
            return puffItemManager.GeneratePuffObject(puffMsg, randomPosition);
        }

        //Ground Plant Object
        public PuffItemView GeneratePlantObjectToWorld(JsonTypes.PuffMessageType puffMsg) {
            Vector3 originPoint = _camera.transform.forward * 1.5f;

            float xPos = originPoint.x + Random.Range(-2.5f, 2.5f);
            float yPos = Random.Range(-1f, -1.5f);
            float zPos = originPoint.z + Random.Range(-2.5f, 2.5f);

            return puffItemManager.GeneratePuffObject(puffMsg, new Vector3(xPos, yPos, zPos));
        }

        private void RefreshGPSInfo()
        {
            GPSLocationService.GetGPS(this, false, (GPSLocationService.LocationInfo locationInfo) =>
            {
                RefreshPuffMsg(locationInfo);
            });
        }


        private async void RefreshPuffMsg(GPSLocationService.LocationInfo locationInfo) {
            await _puffModel.GetAllPuff(locationInfo);
        }

        private void RenderPuffObjectFromDatabase(List<JsonTypes.PuffMessageType> newPuffMsgArray)
        {
            foreach (var data in newPuffMsgArray)
            {
                //Debug.Log($"Body {data.body}, Author ID {data.author_id}, Date {data.parseDate}, Type {data.type}");

                JsonTypes.PuffTypes puffTypes = (JsonTypes.PuffTypes) data.type;

                switch (puffTypes) {
                    case JsonTypes.PuffTypes.FloatSeed:
                        GeneratePuffObjectToWorld(data);
                        break;

                    case JsonTypes.PuffTypes.NewsAvatar:
                        GeneratePuffObjectToWorld(data);
                        break;

                    case JsonTypes.PuffTypes.Plant:
                        GeneratePlantObjectToWorld(data);
                        break;
                }
            }
        }

    }
}