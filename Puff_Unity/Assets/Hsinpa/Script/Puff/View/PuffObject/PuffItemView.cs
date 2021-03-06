﻿using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.View {
    public class PuffItemView : MonoBehaviour
    {
        [SerializeField]
        private float _gravity;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Transform holderSpace;

        private string _puffID;
        public string puffID => _puffID;

        public bool isLanded = false;
        public bool isBackToPosPeriod = false;

        private Vector3 moveVector = new Vector3();
        private float offsetTime;

        private Vector3 _catchPosition;

        private JsonTypes.PuffMessageType _puffMessageType;
        public JsonTypes.PuffMessageType puffMessageType => _puffMessageType;

        public void SetUp(JsonTypes.PuffMessageType p_puffMessageType) {
            this._puffMessageType = p_puffMessageType;
            offsetTime = Random.Range(0.1f, 2f);
            _puffID = p_puffMessageType._id;
            gameObject.name = p_puffMessageType.author;
        }

        public void OnUpdate() {

            if (isBackToPosPeriod) {
                isBackToPosPeriod = RunOriginalPositionAnim(_catchPosition);
                if (isBackToPosPeriod) return;
            }

            if (isLanded) return;

            float offset = (Time.time * offsetTime);
            float randomTime = offset - Mathf.Floor(offset) * Mathf.PI;

            moveVector.Set(Mathf.Sin(randomTime * 1f) * Time.deltaTime * 0.1f,
                0 //- Time.deltaTime * _gravity
                , Mathf.Cos(randomTime * 1f) * Time.deltaTime * 0.1f);

            transform.Translate(moveVector);

            if (transform.position.y < -1) {
                isLanded = true;
            }
        }

        private bool RunOriginalPositionAnim(Vector3 targetPosition) {

            float dist = Vector3.Distance(targetPosition, transform.position);

            if (dist < 0.1f) {
                transform.position = _catchPosition;
                return false;
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
            return true;
        }

        public void SetAnimatorBoolEvent(string p_eventName, bool p_enable)
        {
            if (animator == null) return;
            animator.SetBool(p_eventName, p_enable);
        }

        public void InsertToHolderSpace(GameObject prefab) {
            var avatarDemeItem = UtilityMethod.CreateObjectToParent<PuffAvatarDemoView>(holderSpace, prefab);
            avatarDemeItem.SetAvatarMessage(puffMessageType.body);
            SetAnimatorBoolEvent("Open", true);
        }

        public void Dismiss() {
            isBackToPosPeriod = true;
            isLanded = false;

            if (this.holderSpace != null)
                UtilityMethod.ClearChildObject(this.holderSpace);

            SetAnimatorBoolEvent("Open", false);
        }

        public void CatchToFront() {
            _catchPosition = transform.position;
            isLanded = true;
        }

    }
}