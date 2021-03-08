using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Utility;
using Hsinpa.Model;

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

        private FriendModel _friendModel;
        public FriendModel friendModel => _friendModel;

        public void SetUp() {
            _puffModel = new PuffModel();
            _accountModel = new AccountModel();

            _friendModel = new FriendModel();
        }

    }
}