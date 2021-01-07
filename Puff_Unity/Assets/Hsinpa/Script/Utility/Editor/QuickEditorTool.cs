using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuickEditorTool : UnityEngine.Object
{
    [MenuItem("Tools/Erase PlayerPref")]
    static void ErasePlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }

}
