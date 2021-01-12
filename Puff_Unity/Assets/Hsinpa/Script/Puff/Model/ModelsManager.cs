using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puff.Model
{
    public class ModelsManager : MonoBehaviour
    {
        private PuffModel _puffModel;
        public PuffModel puffModel => _puffModel;

        private AccountModel _accountModel;
        public AccountModel accountModel => _accountModel;

        public void SetUp() {
            _puffModel = new PuffModel();
            _accountModel = new AccountModel();
        }

    }
}