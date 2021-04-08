using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.View
{

    public class Modals : MonoBehaviour
    {
        [SerializeField]
        Image background;

        [SerializeField]
        private bool _hasBackground;
        public bool hasBackground => _hasBackground;

        BaseView[] modals;

        private static Modals _instance;

        public static Modals instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Modals>();
                    _instance.SetUp();
                }
                return _instance;
            }
        }

        private List<Modal> openModals = new List<Modal>();
        private Modal currentModals;

        public void SetUp()
        {
            modals = GetComponentsInChildren<Modal>();
        }

        public T GetModal<T>() where T : Modal
        {
            return modals.First(x=> typeof(T) == x.GetType()) as T;
        }

        public T OpenModal<T>(bool hideOthers = false) where T : Modal
        {
            if (modals == null) return null;

            Modal targetModal = null;

            foreach (Modal modal in modals)
            {
                if (typeof(T) == modal.GetType())
                {
                    targetModal = modal;
                    targetModal.Show(true);
                }
                else
                {
                    if (hideOthers)
                        modal.Show(false);
                }            
            }

            bool isModalDuplicate = openModals.FindIndex(x => x.GetType() == typeof(T)) < 0;
            if (isModalDuplicate) {
                openModals.Add(targetModal as T);
            }

            //Put the current modal to last sibling
            int siblingCount = transform.childCount;
            currentModals = targetModal as T;
            currentModals.transform.SetSiblingIndex(siblingCount - 1);

            background.enabled = (_hasBackground);

            return targetModal as T;
        }

        public void Close() {
            if (currentModals != null)
                currentModals.Show(false);

            if (openModals.Count > 0) {
                openModals.RemoveAt(openModals.Count - 1);
            }

            Debug.Log("Modal Close " + currentModals.name);

            currentModals = (openModals.Count > 0) ? openModals[openModals.Count - 1] : null;
            background.enabled = (currentModals != null && _hasBackground);
        }

        public void CloseAll()
        {
            if (modals == null) return;

            foreach (var modal in modals)
            {
                modal.Show(false);
            }

            background.enabled = false;
            openModals.Clear();
        }

        public void EnableBackgroundImg(bool p_enable) {
            background.enabled = p_enable;
        }
    }
}