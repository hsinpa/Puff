using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.View {
    public class PuffMsgTypePanel : BaseView
    {
        [SerializeField]
        private Button FloatSeedBtn;

        [SerializeField]
        private Button PlantBtn;

        private JsonTypes.PuffTypes currentType = JsonTypes.PuffTypes.FloatSeed;

        public JsonTypes.PuffTypes SelectedType => currentType;

        private void Start()
        {
            Utility.UtilityMethod.SetSimpleBtnEvent<Button>(FloatSeedBtn, OnTypeBtnClick, FloatSeedBtn);
            Utility.UtilityMethod.SetSimpleBtnEvent<Button>(PlantBtn, OnTypeBtnClick, PlantBtn);
        }

        public void SetPuffType(JsonTypes.PuffTypes puffTypes)
        {
            currentType = puffTypes;

            FloatSeedBtn.image.color = (puffTypes == JsonTypes.PuffTypes.FloatSeed) ? GeneralFlag.Colors.HighLightColor : GeneralFlag.Colors.HideColor;
            PlantBtn.image.color = (puffTypes == JsonTypes.PuffTypes.Plant) ? GeneralFlag.Colors.HighLightColor : GeneralFlag.Colors.HideColor;
        }

        private void OnTypeBtnClick(Button clickBtn) {
            if (clickBtn == FloatSeedBtn)
                SetPuffType(JsonTypes.PuffTypes.FloatSeed);
            else if (clickBtn == PlantBtn)
                SetPuffType(JsonTypes.PuffTypes.Plant);
        }

    }
}