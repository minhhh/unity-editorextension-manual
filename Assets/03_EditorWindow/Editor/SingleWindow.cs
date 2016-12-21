using UnityEngine;
using System.Collections;
using UnityEditor;

public class SingleWindow : EditorWindow
{
    [MenuItem("Window/SingleWindow")]
    static void Open ()
    {
        GetWindow<SingleWindow> ();
    }
}
