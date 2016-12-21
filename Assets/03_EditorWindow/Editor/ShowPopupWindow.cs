using UnityEditor;
using UnityEngine;

public class ShowPopupWindow : EditorWindow
{
    static ShowPopupWindow exampleWindow;

    [MenuItem ("Window/ShowPopupWindow")]
    static void Open ()
    {
        if (exampleWindow == null) {
            exampleWindow = CreateInstance<ShowPopupWindow> ();
        }
        exampleWindow.ShowPopup ();
    }


    void OnGUI ()
    {
        if (Event.current.keyCode == KeyCode.Escape) {
            exampleWindow.Close ();
        }
    }
}
