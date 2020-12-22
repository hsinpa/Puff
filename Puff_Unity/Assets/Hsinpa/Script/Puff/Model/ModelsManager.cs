using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.Model
{
    public class ModelsManager : MonoBehaviour
    {
        private PuffModel _puffModel;
        public PuffModel puffModel => _puffModel;

        public void SetUp() {
            _puffModel = new PuffModel();
        }

    }
}