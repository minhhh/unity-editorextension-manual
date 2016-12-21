using UnityEditor;
using UnityEngine;

public class ShowAsDropDownWindow : EditorWindow
{
    static ShowAsDropDownWindow exampleWindow;

    [MenuItem("Window/ShowAsDropDownWindow")]
    static void Open ()
    {
        if (exampleWindow == null) {
            exampleWindow = CreateInstance<ShowAsDropDownWindow> ();
        }

        var buttonRect = new Rect (1800, 1300, 300, 100);
        var windowSize = new Vector2 (300, 100);
        exampleWindow.ShowAsDropDown (buttonRect, windowSize);
    }
}