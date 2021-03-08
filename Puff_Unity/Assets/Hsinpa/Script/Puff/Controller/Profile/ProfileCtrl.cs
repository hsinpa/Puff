using Hsinpa.View;
using Puff.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puff.Ctrl.Utility;
using Hsinpa.Utility;
using Puff.Model;
using System.Threading.Tasks;
using Hsinpa.Model;

namespace Puff.Ctrl
{
    public class ProfileCtrl : ObserverPattern.Observer
    {

        private AccountModel _accountModel;
        private FriendModel _friendModel;


        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event) {

                case EventFlag.Event.GameStart:
                    {
                        _accountModel = PuffApp.Instance.models.accountModel;
                        _friendModel = PuffApp.Instance.models.friendModel;
                    }
                    break;
                case EventFlag.Event.OnProfileOpen:
                    OnProfileOpenEvent();
                    break;
            }
        }

        private void OnProfileOpenEvent() {
            ProfileModal profileModal = Modals.instance.OpenModal<ProfileModal>();
            profileModal.SetUp(this._accountModel, this._friendModel);
        }



    }
}
