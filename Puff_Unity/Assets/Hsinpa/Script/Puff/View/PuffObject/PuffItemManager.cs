using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.View;

namespace Puff.WorldManager {
    public class PuffItemManager : MonoBehaviour
    {
        [SerializeField]
        private PuffItemView puffPuffPrefab; // Sky

        [SerializeField] 
        private PuffItemView plantPuffPrefab; // Ground

        private List<PuffItemView> puffItemViews = new List<PuffItemView>();

        public PuffItemView GeneratePuffObject(JsonTypes.PuffMessageType puffMsgType, Vector3 spawnPositon)
        {
            JsonTypes.PuffTypes puffTypes = (JsonTypes.PuffTypes)puffMsgType.type;

            PuffItemView spawnPrefab = (puffTypes == JsonTypes.PuffTypes.FloatSeed) ? puffPuffPrefab : plantPuffPrefab;

            GameObject generateObj = Instantiate(spawnPrefab.gameObject, this.transform);
            PuffItemView itemView = generateObj.GetComponent<PuffItemView>();
            itemView.SetUp(puffMsgType);
            itemView.transform.position = spawnPositon;
            puffItemViews.Add(itemView);

            return itemView;
        }

        private void Update()
        {
            int puffCount = puffItemViews.Count;

            for (int i = 0; i < puffCount; i++) {
                puffItemViews[i].OnUpdate();
            }
        }



    }
}
