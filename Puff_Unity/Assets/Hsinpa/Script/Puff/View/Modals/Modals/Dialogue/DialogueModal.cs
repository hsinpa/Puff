using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;

namespace Hsinpa.View
{
    public class DialogueModal : Modal
    {

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Text contentText;

        [SerializeField]
        private Transform buttonContainer;

        [SerializeField]
        private GameObject buttonPrefab;


        public void SetDialogue(string title, string content, string[] allowBtns, System.Action<int> btnEvent) {
            ResetContent();

            titleText.text = title;
            contentText.text = content;

            RegisterButtons(allowBtns, btnEvent);
        }


        private void RegisterButtons(string[] allowBtns, System.Action<int> btnEvent) {
            int btnlength = allowBtns.Length;

            for (int i = 0; i < btnlength; i++) {
                int index = i;
                Button buttonObj = UtilityMethod.CreateObjectToParent<Button>(buttonContainer, buttonPrefab);
                Text textObj = buttonObj.GetComponentInChildren<Text>();

                textObj.text = allowBtns[i];

                buttonObj.onClick.AddListener(() =>
                {
                    Modals.instance.Close();

                    if (btnEvent != null)
                        btnEvent(index);
                });
            }
        }

        private void ResetContent() {
            UtilityMethod.ClearChildObject(buttonContainer);
        }


    }
}