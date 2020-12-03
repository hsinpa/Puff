using Puff.View;
using Puff.WorldManager;
using System.Collections;
using System.Collections.Generic;
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

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        GenerateTestPuffObject(20);
                    }
                    break;

                case EventFlag.Event.OpenPuffMsg:
                    {

                    }
                    break;
            }
        }

        private void GenerateTestPuffObject(int testNumber) {

            Vector3 originPoint = _camera.transform.forward * 25;

            for (int i = 0; i < testNumber; i++) {

                float xPos = originPoint.x + Random.Range(-18.5f, 18.5f);
                float yPos = Random.Range(18, 35f);
                float zPos = originPoint.z +  Random.Range(-18.5f, 18.5f);

                JsonTypes.PuffMessageType msgType = new JsonTypes.PuffMessageType();
                msgType.author = "Helloworld";
                msgType.author_id = System.Guid.NewGuid().ToString();
                msgType.body = "Gkekoiosd fa sdf";
                msgType._id = System.Guid.NewGuid().ToString().Substring(0,8); 

                Vector3 randomPosition = new Vector3(xPos, yPos, zPos);
                puffItemManager.GeneratePuffObject(msgType, randomPosition);
            }    
        }

        //private Vector3 RandomPosition() { 
        
        //}


    }
}