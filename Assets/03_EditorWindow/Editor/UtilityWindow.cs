using UnityEngine;
using System.Collections;
using UnityEditor;

public class UtilityWindow : EditorWindow
{
    [MenuItem ("Window/UtilityWindow")]
    static void Open ()
    {
        GetWindow<UtilityWindow> (true);
    }
}
