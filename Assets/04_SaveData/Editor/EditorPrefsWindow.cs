using UnityEngine;
using UnityEditor;

public class EditorPrefsWindow : EditorWindow
{
    int someValue = 1;
    const string SOME_KEY = "KEY";


    [MenuItem ("Window/EditorPrefsWindow")]
    static void Open ()
    {
        GetWindow <EditorPrefsWindow> ();
    }

    void OnEnable ()
    {
        someValue = EditorPrefs.GetInt (SOME_KEY, 1);
    }

    void OnGUI ()
    {
        EditorGUI.BeginChangeCheck ();

        someValue = EditorGUILayout.IntSlider ("My Value", someValue, 1, 100);

        if (EditorGUI.EndChangeCheck ()) {
            EditorPrefs.SetInt (SOME_KEY, someValue);
        }

    }
}