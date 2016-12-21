using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(EffectLayer))]
public class EffectLayerCustom : Editor
{
    EffectLayer layer;

    void OnEnable ()
    {
        layer = target as EffectLayer;
    }

    public void OnSceneGUI ()
    {
        
        Rect r = new Rect (Screen.width - 160, Screen.height - 120, 160, 80);

        Vector2 mouse = Event.current.mousePosition;

        Rect r2 = r;
        r2.yMin -= 30;
        r2.xMin -= 10;
        r2.xMax += 10;
        r2.yMax += 10;

        if (r2.Contains (mouse) && Event.current.type == EventType.Layout) {
            int controlID = GUIUtility.GetControlID (1024, FocusType.Passive);
            HandleUtility.AddControl (controlID, 0F);
        }

        Handles.BeginGUI ();
        GUILayout.BeginArea (r, layer.gameObject.name, "Window");

        layer.myBool = GUILayout.Toggle (layer.myBool, "Check Box?");

        EditorGUIUtility.labelWidth = 110f;
        layer.myFloat = EditorGUILayout.FloatField ("Some Float: ", layer.myFloat);
        EditorGUIUtility.labelWidth = 0f;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button ("Play")) {
        }

        if (GUILayout.Button ("Reset")) {
            layer.myFloat = 0;
            layer.myInt = 0;
            layer.myStr = "";
        }
        EditorGUILayout.EndHorizontal ();
       

        GUILayout.EndArea ();

        Handles.EndGUI ();

    }
}
