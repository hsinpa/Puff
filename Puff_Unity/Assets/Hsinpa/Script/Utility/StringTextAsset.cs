using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTextAsset
{

    public class GeneralText {
        public const string Success = "Operation successful";
        public const string Failure = "Operation fail";
        public const string FailureWithVariable = "Operation fail : {0}";
    }

    public class Login
    {
        public const string SignUpBtn = "Sign Up";
        public const string LoginBtn = "Login";

        public const string SignUpTip = "Don’t have an account yet? <color=#3b9d90>Sign Up</color>";
        public const string LoginTip = "Already have an account? <color=#3b9d90>Log In</color>";

        public const string EmailWrongFormat = "Wrong Email Format";
        public const string PasswordWrongFormat = "Wrong password foramt; Need to be between 6-20 characters count";
        public const string PasswordRepeatFormat = "Wrong password foramt; Password and confirm password field is different";
        public const string UserWrongFormat = "Wrong Username Format; Cannot be empty";
        public const string AgreementToggleFormat = "Please check the agreement box by reading through \"Terms and Condition\"";

        public const string DatabaseFail_SignUp = "Sign up fail; Duplicate email existed";
        public const string DatabaseFail_Login = "Server fail; No Account is find";
        public const string InternetError = "Fail to establish connection to server; Please check internet visibility";
    }

    public class Friend {
        public const string FriendTitleCount = "Friends {0}";

        public const string FriendBtnConfirm = "Accept";
        public const string FriendBtnInvite = "Invite";
        public const string FriendBtnReject = "Reject";
        public const string FriendBtnPending = "Pending";

        public const string FriendDialogueBoxTitle = "Friend Management";
        public const string FriendAcceptMessage = "Accept friend request from {0}";
        public const string FriendRejectMessage = "Do you wish to reject friend offer from {0}";
    }

    public class DialogueBox {
        public const string Confirm = "<color=#16a2ff>Proceed</color>";
        public const string Cancel = "<color=#ff4823>Cancel</color>";
    }

    public class Messaging {
        public const string DurationText = "Until {0}";
        public const string DurationCues = "{0} \nDay";

        public const string CameraIndicator = "{0}/{1}";

        public const string DistanceNear = "Near({0}M)";
        public const string DistanceMedium = "City"; // Medium({0}KM)
        public const string DistanceFar = "Country"; // Far({0}KM)
        public const string DistanceWorld = "World";

        public const string WarningNoMessageOrTitle = "Please fulfill title and message box to proceed";

        public const string SubmitBtnShare = "Share";
        public const string SubmitBtnNext = "Next step";

        public const string SubmitSuccess = "Message is made!";

        //Save Puff to library
        public const string PuffLibraryError_IsAccountOwner = "Cannot save your own message";
        public const string PuffLibraryError_IsAlreadySave = "This message is already saved";
    }
}
