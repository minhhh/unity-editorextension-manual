using UnityEditor;
using UnityEngine;

public class SamplePopupWindow : EditorWindow
{
    [MenuItem ("Window/PopupWindow")]
    static void Open ()
    {
        GetWindow<SamplePopupWindow> ();
    }

    ExamplePupupContent popupContent = new ExamplePupupContent ();

    void OnGUI ()
    {
        if (GUILayout.Button ("PopupContent", GUILayout.Width (128))) {
            var activatorRect = GUILayoutUtility.GetLastRect ();

            PopupWindow.Show (new Rect (activatorRect.x, activatorRect.y + 10, activatorRect.width, activatorRect.height), popupContent);
        }
    }
}

public class ExamplePupupContent : PopupWindowContent
{
    public override void OnGUI (Rect rect)
    {
        EditorGUILayout.LabelField ("Label");
    }

    public override void OnOpen ()
    {
        Debug.Log ("OnOpen");
    }

    public override void OnClose ()
    {
        Debug.Log ("OnClose");
    }

    public override Vector2 GetWindowSize ()
    {
        return new Vector2 (300, 100);
    }
}