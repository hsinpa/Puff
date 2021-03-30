using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.View {
    public class CustomButton : Button
    {
        public void SetTitle(string p_string) {
            Text titleComp = this.transform.GetComponentInChildren<Text>();

            if (titleComp != null)
                titleComp.text = p_string;
        }
    }
}