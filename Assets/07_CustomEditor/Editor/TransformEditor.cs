using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(Transform))]
public class TransformInspector : Editor
{

    public bool showTools;
    public bool copyPosition;
    public bool copyRotation;
    public bool copyScale;
    public bool pastePosition;
    public bool pasteRotation;
    public bool pasteScale;
    public bool selectionNullError;

    public override void OnInspectorGUI ()
    {

        Transform t = (Transform)target;

        // Replicate the standard transform inspector gui
        EditorGUI.indentLevel = 0;
        Vector3 position = EditorGUILayout.Vector3Field ("Position", t.localPosition);
        Vector3 eulerAngles = EditorGUILayout.Vector3Field ("Rotation", t.localEulerAngles);
        Vector3 scale = EditorGUILayout.Vector3Field ("Scale", t.localScale);

        //
        if (GUILayout.Button ((showTools) ? "Hide Transform Tools" : "Show Transform Tools")) {
            showTools = !showTools;
            EditorPrefs.SetBool ("ShowTools", showTools);
        }
        //  START TRANSFORM TOOLS FOLD DOWN //
        if (showTools) {
            if (!copyPosition && !copyRotation && !copyScale) {
                selectionNullError = true;
            } else {
                selectionNullError = false;
            }
            EditorGUILayout.BeginHorizontal ();
            if (GUILayout.Button (selectionNullError ? "Nothing Selected" : "Copy Transform")) {
                if (copyPosition) {
                    EditorPrefs.SetFloat ("LocalPosX", t.localPosition.x);
                    EditorPrefs.SetFloat ("LocalPosY", t.localPosition.y);
                    EditorPrefs.SetFloat ("LocalPosZ", t.localPosition.z);
                }
                if (copyRotation) {
                    EditorPrefs.SetFloat ("LocalRotX", t.localEulerAngles.x);
                    EditorPrefs.SetFloat ("LocalRotY", t.localEulerAngles.y);
                    EditorPrefs.SetFloat ("LocalRotZ", t.localEulerAngles.z);
                }
                if (copyScale) {
                    EditorPrefs.SetFloat ("LocalScaleX", t.localScale.x);
                    EditorPrefs.SetFloat ("LocalScaleY", t.localScale.y);
                    EditorPrefs.SetFloat ("LocalScaleZ", t.localScale.z);
                }

                Debug.Log ("LP: " + t.localPosition + " LT: (" + t.localEulerAngles.x + ", " + t.localEulerAngles.y + ", " + t.localEulerAngles.z + ") LS: " + t.localScale);
            }
            if (GUILayout.Button ("Paste Transform")) {
                Vector3 tV3 = new Vector3 ();
                if (pastePosition) {
                    tV3.x = EditorPrefs.GetFloat ("LocalPosX", 0.0f);
                    tV3.y = EditorPrefs.GetFloat ("LocalPosY", 0.0f);
                    tV3.z = EditorPrefs.GetFloat ("LocalPosZ", 0.0f);
                    t.localPosition = tV3;
                }
                if (pasteRotation) {
                    tV3.x = EditorPrefs.GetFloat ("LocalRotX", 0.0f);
                    tV3.y = EditorPrefs.GetFloat ("LocalRotY", 0.0f);
                    tV3.z = EditorPrefs.GetFloat ("LocalRotZ", 0.0f);
                    t.localEulerAngles = tV3;
                }
                if (pasteScale) {
                    tV3.x = EditorPrefs.GetFloat ("LocalScaleX", 1.0f);
                    tV3.y = EditorPrefs.GetFloat ("LocalScaleY", 1.0f);
                    tV3.z = EditorPrefs.GetFloat ("LocalScaleZ", 1.0f);
                    t.localScale = tV3;
                }

                Debug.Log ("LP: " + t.localPosition + " LT: " + t.localEulerAngles + " LS: " + t.localScale);
            }
            EditorGUILayout.EndHorizontal ();

            EditorGUIUtility.LookLikeControls ();
            EditorGUILayout.BeginHorizontal ();
            GUILayout.Label ("Position", GUILayout.Width (75));
            GUILayout.Label ("Rotation", GUILayout.Width (75));
            GUILayout.Label ("Scale", GUILayout.Width (50));
            if (GUILayout.Button ("All", GUILayout.MaxWidth (40)))
                TransformCopyAll ();
            EditorGUILayout.EndHorizontal ();
            EditorGUILayout.BeginHorizontal ();
            GUILayout.Space (20);
            copyPosition = EditorGUILayout.Toggle (copyPosition, GUILayout.Width (75));
            copyRotation = EditorGUILayout.Toggle (copyRotation, GUILayout.Width (65));
            copyScale = EditorGUILayout.Toggle (copyScale, GUILayout.Width (45));
            if (GUILayout.Button ("None", GUILayout.MaxWidth (40)))
                TransformCopyNone ();
            EditorGUILayout.EndHorizontal ();
            EditorGUIUtility.LookLikeInspector ();
        }
        //  END TRANSFORM TOOLS FOLD DOWN   //

        if (GUI.changed) {
            SetCopyPasteBools ();
            Undo.RegisterUndo (t, "Transform Change");

            t.localPosition = FixIfNaN (position);
            t.localEulerAngles = FixIfNaN (eulerAngles);
            t.localScale = FixIfNaN (scale);
        }
    }

    private Vector3 FixIfNaN (Vector3 v)
    {
        if (float.IsNaN (v.x)) {
            v.x = 0;
        }
        if (float.IsNaN (v.y)) {
            v.y = 0;
        }
        if (float.IsNaN (v.z)) {
            v.z = 0;
        }
        return v;
    }

    void OnEnable ()
    {
        showTools = EditorPrefs.GetBool ("ShowTools", false);
        copyPosition = EditorPrefs.GetBool ("Copy Position", true);
        copyRotation = EditorPrefs.GetBool ("Copy Rotation", true);
        copyScale = EditorPrefs.GetBool ("Copy Scale", true);
        pastePosition = EditorPrefs.GetBool ("Paste Position", true);
        pasteRotation = EditorPrefs.GetBool ("Paste Rotation", true);
        pasteScale = EditorPrefs.GetBool ("Paste Scale", true);
    }

    void TransformCopyAll ()
    {
        copyPosition = true;
        copyRotation = true;
        copyScale = true;
        GUI.changed = true;
    }

    void TransformCopyNone ()
    {
        copyPosition = false;
        copyRotation = false;
        copyScale = false;
        GUI.changed = true;
    }

    void SetCopyPasteBools ()
    {
        pastePosition = copyPosition;
        pasteRotation = copyRotation;
        pasteScale = copyScale;

        EditorPrefs.SetBool ("Copy Position", copyPosition);
        EditorPrefs.SetBool ("Copy Rotation", copyRotation);
        EditorPrefs.SetBool ("Copy Scale", copyScale);
        EditorPrefs.SetBool ("Paste Position", pastePosition);
        EditorPrefs.SetBool ("Paste Rotation", pasteRotation);
        EditorPrefs.SetBool ("Paste Scale", pasteScale);
    }
}