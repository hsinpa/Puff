using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View {
    public class ProfileLibraryView : MonoBehaviour
    {
        [SerializeField]
        private GameObject ItemPrefab;

        public void Generate(List<JsonTypes.PuffMessageType> puffMsgList, System.Action<JsonTypes.PuffMessageType> OnClickCallback) {
            int listCount = puffMsgList.Count;

            this.gameObject.SetActive(true);
            UtilityMethod.ClearChildObject(this.transform);
            for (int i = 0; i < listCount; i++) {

                var spawnObject = UtilityMethod.CreateObjectToParent<Button>(this.transform, ItemPrefab);

                UtilityMethod.SetSimpleBtnEvent<JsonTypes.PuffMessageType>(spawnObject, OnClickCallback, puffMsgList[i]);
            }
        }
    }
}