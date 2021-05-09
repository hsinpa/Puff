using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuffAvatarDemoView : MonoBehaviour
{
    [SerializeField]
    private Text dialogueMessage;

    public void SetAvatarMessage(string message) {
        this.dialogueMessage.text = message;
    }

    public void SetAvatarVisibility(bool visible) {
        this.gameObject.SetActive(visible);
    }
}
