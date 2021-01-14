using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Puff.View
{
    public class PuffMsgButtonModule : MonoBehaviour
    {
        private Button[] buttons;

        public void SetUp(params System.Action[] actions) {
            buttons = this.GetComponentsInChildren<Button>();

            int callbackCount = actions.Length;
            int buttonCount = buttons.Length;

            for (int i = 0; i < buttonCount; i++) {

                //Available Buttons
                if (i < callbackCount) {
                    buttons[i].onClick.RemoveAllListeners();

                    buttons[i].onClick.AddListener(() => actions[i]());
                    return;
                }

                buttons[i].gameObject.SetActive(false);
            }
        }

    }
}