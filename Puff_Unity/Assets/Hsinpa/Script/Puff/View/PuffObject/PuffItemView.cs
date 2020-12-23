using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.View {
    public class PuffItemView : MonoBehaviour
    {
        [SerializeField]
        private float _gravity;

        private JsonTypes.PuffMessageType _puffMessageType;
        public JsonTypes.PuffMessageType puffMessageType => _puffMessageType;

        public bool isLanded = false;
        public bool isBackToPosPeriod = false;

        private Vector3 moveVector = new Vector3();
        private float offsetTime;

        private Vector3 _catchPosition;

        public void SetUp(JsonTypes.PuffMessageType p_puffMessageType) {
            offsetTime = Random.Range(0.1f, 2f);
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

            moveVector.Set(Mathf.Sin(randomTime * 1f) * Time.deltaTime * 0.2f, - Time.deltaTime * _gravity, Mathf.Cos(randomTime * 1f) * Time.deltaTime * 0.2f);

            transform.Translate(moveVector);

            if (transform.position.y < -5) {
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

        public void Dismiss() {
            isBackToPosPeriod = true;
            isLanded = false;
        }

        public void CatchToFront() {
            _catchPosition = transform.position;
            isLanded = true;
        }

    }
}