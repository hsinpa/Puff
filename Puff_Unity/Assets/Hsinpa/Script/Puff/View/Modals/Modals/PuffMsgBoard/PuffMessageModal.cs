using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.View {
    public class PuffMessageModal : Modal
    {
        [SerializeField]
        private PuffMsgHeader puffMsgHeader;

        [SerializeField]
        private PuffMsgFrontPage _puffMsgFrontPage;
        public PuffMsgFrontPage puffMsgFrontPage => _puffMsgFrontPage;

        [SerializeField]
        private PuffActionSelectPage _puffActionSelectPage;
        public PuffActionSelectPage puffActionSelectPage => _puffActionSelectPage;

        [SerializeField]
        private PuffTextMsgPage _puffTextMsgPage;
        public PuffTextMsgPage puffTextMsgPage => _puffTextMsgPage;

        PuffMsgInnerPage[] pages;

        protected override void Start()
        {
            base.Start();
            this.pages = GetComponentsInChildren<PuffMsgInnerPage>(includeInactive: true);
        }

        public T OpenPage<T>() where T : PuffMsgInnerPage {

            T innerPage = null;

            if (this.pages != null && this.pages.Length > 0) {
                for (int i = 0; i < pages.Length; i++) {

                    pages[i].Show(false);
                   
                    if (pages[i].GetType() == typeof(T)) {
                        innerPage = (T)pages[i];
                        pages[i].Show(true);

                        puffMsgHeader.gameObject.SetActive( typeof(T) == typeof(PuffMsgFrontPage));
                    }
                }
            }

            return innerPage;
        }
    }
}