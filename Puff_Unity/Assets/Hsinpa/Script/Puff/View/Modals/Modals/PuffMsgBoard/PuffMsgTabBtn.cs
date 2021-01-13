using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Puff.View
{
    public class PuffMsgTabBtn : MonoBehaviour
    {

        [SerializeField]
        private Button _puffButton;
        public Button puffButton => _puffButton;
    }
}