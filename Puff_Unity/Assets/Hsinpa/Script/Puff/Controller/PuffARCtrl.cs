using Puff.Model;
using Puff.View;
using Puff.WorldManager;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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

        private PuffModel _puffModel;

        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        Init();

                        //GenerateTestPuffObject(20);
                        RefreshPuffMsg();
                    }
                    break;

                case EventFlag.Event.OpenPuffMsg:
                    {

                    }
                    break;
            }
        }

        private void Init() {
            _puffModel = PuffApp.Instance.models.puffModel;
            _puffModel.OnReceiveNewPuffMsgEvent += RenderPuffObjectFromDatabase;

        }

        private void GenerateTestPuffObject(int testNumber) {
            for (int i = 0; i < testNumber; i++) {
                JsonTypes.PuffMessageType msgType = new JsonTypes.PuffMessageType();
                msgType.author = "Helloworld";
                msgType.author_id = System.Guid.NewGuid().ToString();
                msgType.body = "Gkekoiosd fa sdf";
                msgType._id = System.Guid.NewGuid().ToString().Substring(0,8); 

                GeneratePuffObjectToWorld(msgType);
            }
        }

        private PuffItemView GeneratePuffObjectToWorld(JsonTypes.PuffMessageType puffMsg) {
            Vector3 originPoint = _camera.transform.forward * 25;

            float xPos = originPoint.x + Random.Range(-18.5f, 18.5f);
            float yPos = Random.Range(18, 35f);
            float zPos = originPoint.z + Random.Range(-18.5f, 18.5f);

            Vector3 randomPosition = new Vector3(xPos, yPos, zPos);
            return puffItemManager.GeneratePuffObject(puffMsg, randomPosition);
        }

        private async void RefreshPuffMsg() {
            await _puffModel.GetAllPuff();

            RepeatRefreshPuffMsg(tokenSource);
        }

        private void RenderPuffObjectFromDatabase(List<JsonTypes.PuffMessageType> newPuffMsgArray)
        {
            foreach (var data in newPuffMsgArray)
            {
                Debug.Log($"Body {data.body}, Author ID {data.author_id}, Date {data.parseDate}");
                GeneratePuffObjectToWorld(data);
            }
        }

        private async void RepeatRefreshPuffMsg(CancellationTokenSource tokenSource) {
            await Task.Delay(5000, tokenSource.Token);

            if (!tokenSource.IsCancellationRequested)
                RefreshPuffMsg();
        }

        private void OnApplicationQuit()
        {
            tokenSource.Cancel();
        }
    }
}