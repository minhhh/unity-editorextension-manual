using UnityEngine;
using System.Collections;
using UnityEditor;

public class ShowAuxWindow : EditorWindow
{
    [MenuItem ("Window/AuxWindow")]
    static void Open ()
    {
        var exampleWindow = CreateInstance<ShowAuxWindow> ();
        exampleWindow.ShowAuxWindow ();
    }
}
