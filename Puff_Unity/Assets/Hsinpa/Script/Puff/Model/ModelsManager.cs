using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Utility;

namespace Puff.Model
{
    public class ModelsManager : MonoBehaviour
    {
        [SerializeField]
        private ColorItemSObj _colorSetting;
        public ColorItemSObj colorSetting => _colorSetting;

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