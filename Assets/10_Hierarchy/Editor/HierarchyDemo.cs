using UnityEngine;
using System.Collections;
using UnityEditor;

public static class HierarchyDemo
{
    [InitializeOnLoadMethod]
    private static void DrawToHierarchy ()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
    }

    const int WIDTH = 40;
    const int LABEL_WIDTH = 40;
    static Color labelColor = Color.green;

    private static void OnGUI (int instanceID, Rect rect)
    {
        var go = EditorUtility.InstanceIDToObject (instanceID) as GameObject;

        if (go == null) {
            return;
        }

        var hierarchyItem = go.GetComponent<HierarchyItem> ();

        if (hierarchyItem == null) {
            return;
        }


        var posButton = rect;
        posButton.x = posButton.xMax - WIDTH;
        posButton.width = WIDTH;

        var pos = posButton;
        pos.x = posButton.x - WIDTH;
        pos.width = WIDTH;

        var posLabel = pos;
        posLabel.x = pos.x - LABEL_WIDTH;
        posLabel.width = LABEL_WIDTH;

        GUIStyle guiStyleLabel = new GUIStyle ();
        GUIStyleState styleState = new GUIStyleState ();
        styleState.textColor = labelColor;
        guiStyleLabel.normal = styleState;

        if (GUI.Button (posButton, "Play")) {
            
        }

        GUI.Label (posLabel, "myStr:", guiStyleLabel);
        hierarchyItem.myStr = EditorGUI.TextField (pos, hierarchyItem.myStr);

    }

}
