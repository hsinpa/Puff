using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.View
{
    public class PuffMsgInnerPage : MonoBehaviour
    {
        public void Show(bool isDisplay) {
            this.gameObject.SetActive(isDisplay);
        }
    }
}