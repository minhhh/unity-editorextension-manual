using UnityEngine;
using System.Collections;
using UnityEditor;

public class MultipleWindow : EditorWindow
{
    [MenuItem ("Window/MultipleWindow")]
    static void Open ()
    {
        var exampleWindow = CreateInstance<MultipleWindow> ();
        exampleWindow.Show ();
    }
}
