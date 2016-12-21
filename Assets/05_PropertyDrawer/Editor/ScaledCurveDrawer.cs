using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof(ScaledCurve))]
public class ScaledCurveDrawer : PropertyDrawer
{
    const int curveWidth = 50;
    const float min = 0;
    const float max = 1;

    public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty (pos, label, prop);

        SerializedProperty scale = prop.FindPropertyRelative ("scale");
        SerializedProperty curve = prop.FindPropertyRelative ("curve");

        // Draw scale
        EditorGUI.Slider (
            new Rect (pos.x, pos.y, pos.width - curveWidth, pos.height),
            scale, min, max, label);

        // Draw curve
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField (
            new Rect (pos.width - curveWidth, pos.y, curveWidth, pos.height),
            curve, GUIContent.none);
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty ();
    }

}