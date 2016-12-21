using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class MeshVisualizeWindow : EditorWindow
{
    [MenuItem ("Window/Mesh visualizer")]
    public static void OpenWindow ()
    {
        GetWindow<MeshVisualizeWindow> ().Focus ();
    }

    private Mesh selectedMesh;
    private Vector3[] selectedVertices, selectedNormals;
    private Color[] selectedColors, randomColors;
    private int[] selectedTriangles;

    private int startVertIndex, endVertIndex, startTriIndex, endTriIndex;

    private bool shouldVisualizeVertices = true, shouldVisualizeTriangles = false;
    private bool useVertexColors = true;

    private bool hasVertexColors { get { return selectedMesh.colors != null && selectedMesh.colors.Length > 0; } }

    void OnEnable ()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        init ();
        randomColors = generateColors (255);
    }

    void OnDisable ()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

    }

    void OnSelectionChange ()
    {
        init ();
        //Force to repaint UI, otherwise the new values arent visible until you interact with the window
        Repaint ();
    }

    //Initialises the variables for the current selection.
    private void init ()
    {
        //Reset to default
        selectedMesh = null;

        if (Selection.activeGameObject == null)
            return;

        MeshFilter mFilter;
        SkinnedMeshRenderer mSkinRender;

        if ((mFilter = Selection.activeGameObject.GetComponent<MeshFilter> ()) != null)
            selectedMesh = mFilter.sharedMesh;

        if ((mSkinRender = Selection.activeGameObject.GetComponent<SkinnedMeshRenderer> ()) != null)
            selectedMesh = mSkinRender.sharedMesh;

        if (selectedMesh == null || (selectedVertices != null && selectedMesh.vertices.Length == selectedVertices.Length))
            return;

        selectedVertices = selectedMesh.vertices;
        selectedNormals = selectedMesh.normals;
        selectedTriangles = selectedMesh.triangles;
        selectedColors = validateColors (selectedMesh.colors);
        if (!hasVertexColors || !useVertexColors)
            selectedColors = generateColors (selectedVertices.Length);

        startVertIndex = 0;
        endVertIndex = selectedVertices.Length;
        endTriIndex = selectedTriangles.Length / 3;
    }

    private Color[] validateColors (Color[] colors)
    {
        for (int i = 0; i < colors.Length; i++) {
            if (colors [i].a < .5f)
                colors [i].a = 1f;
        }

        return colors;
    }

    private Color[] generateColors (int size)
    {
        Color[] cols = new Color[size];
        for (int i = 0; i < cols.Length; i++)
            cols [i] = new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        return cols;
    }


    #region GUI

    void OnGUI ()
    {
        if (selectedMesh == null) {
            EditorGUILayout.HelpBox ("Please select a meshfilter or skinnedmeshrenderer!", MessageType.Error);
            return;
        }

        drawGeneralGUI ();
        EditorGUILayout.Space ();
        drawVertexGUI ();
        EditorGUILayout.Space ();
        drawTriangleGUI ();

        if (GUI.changed)
            SceneView.RepaintAll ();
    }

    private void drawGeneralGUI ()
    {
        EditorGUILayout.LabelField ("General settings", EditorStyles.boldLabel);

        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField (string.Format ("Vertex count: {0}, triangle count: {1}", selectedVertices.Length, selectedMesh.triangles.Length / 3));

        if (hasVertexColors) {
            useVertexColors = EditorGUILayout.Toggle ("Use vertex colors", useVertexColors);

            if (GUI.changed && useVertexColors)
                selectedColors = validateColors (selectedMesh.colors);
            else if (GUI.changed && !useVertexColors)
                selectedColors = generateColors (selectedVertices.Length);
        }

        EditorGUI.indentLevel--;
    }

    private void drawVertexGUI ()
    {
        shouldVisualizeVertices = EditorGUILayout.BeginToggleGroup ("Show vertices", shouldVisualizeVertices);

        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField ("Visualise from index: " + startVertIndex + " to " + endVertIndex);

        //Lame, MinMaxSlider only has a float version!
        float tmpStart = startVertIndex, tmpEnd = endVertIndex;
        EditorGUILayout.MinMaxSlider (ref tmpStart, ref tmpEnd, 0, selectedVertices.Length);
        startVertIndex = (int)tmpStart;
        endVertIndex = (int)tmpEnd;

        EditorGUILayout.BeginHorizontal ();
        EditorGUILayout.LabelField ("Min");

        if (GUILayout.Button ("-"))
            startVertIndex = Mathf.Clamp (startVertIndex - 1, 0, endVertIndex);
        if (GUILayout.Button ("+"))
            startVertIndex = Mathf.Clamp (startVertIndex + 1, 0, endVertIndex);

        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.BeginHorizontal ();
        EditorGUILayout.LabelField ("Max");

        if (GUILayout.Button ("-"))
            endVertIndex = Mathf.Clamp (endVertIndex - 1, startVertIndex, selectedVertices.Length);
        if (GUILayout.Button ("+"))
            endVertIndex = Mathf.Clamp (endVertIndex + 1, startVertIndex, selectedVertices.Length);

        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.EndToggleGroup ();

        EditorGUI.indentLevel--;
    }

    private void drawTriangleGUI ()
    {
        shouldVisualizeTriangles = EditorGUILayout.BeginToggleGroup ("Show triangles", shouldVisualizeTriangles);

        EditorGUILayout.LabelField ("Visualise from index: " + startTriIndex + " to " + endTriIndex);
        EditorGUI.indentLevel++;

        //Lame, MinMaxSlider only has a float version!
        float tmpStart = startTriIndex, tmpEnd = endTriIndex;
        EditorGUILayout.MinMaxSlider (ref tmpStart, ref tmpEnd, 0, selectedTriangles.Length / 3f);
        startTriIndex = (int)tmpStart;
        endTriIndex = (int)tmpEnd;

        EditorGUILayout.BeginHorizontal ();
        EditorGUILayout.LabelField ("Min");

        if (GUILayout.Button ("-"))
            startTriIndex = Mathf.Clamp (startTriIndex - 1, 0, endTriIndex);
        if (GUILayout.Button ("+"))
            startTriIndex = Mathf.Clamp (startTriIndex + 1, 0, endTriIndex);

        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.BeginHorizontal ();
        EditorGUILayout.LabelField ("Max");

        if (GUILayout.Button ("-"))
            endTriIndex = Mathf.Clamp (endTriIndex - 1, startTriIndex, selectedTriangles.Length / 3);
        if (GUILayout.Button ("+"))
            endTriIndex = Mathf.Clamp (endTriIndex + 1, startTriIndex, selectedTriangles.Length / 3);

        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.EndToggleGroup ();
        EditorGUI.indentLevel--;
    }

    #endregion

    #region Scene

    private void OnSceneGUI (SceneView sceneView)
    {
        if (selectedMesh == null || Selection.activeGameObject == null)
            return;
        //Everything we do is in local space
        Handles.matrix = Selection.activeGameObject.transform.localToWorldMatrix;

        if (shouldVisualizeVertices)
            visualizeVertices ();

        if (shouldVisualizeTriangles)
            visualizeTriangles ();

    }

    private void visualizeVertices ()
    {
        for (int i = startVertIndex; i < endVertIndex; i++) {
            Handles.color = selectedColors [i];

            DrawRay (selectedVertices [i], selectedNormals [i]);
            Handles.Label (selectedVertices [i] + selectedNormals [i] * 1.1f, i.ToString ());
        }
    }

    private void visualizeTriangles ()
    {
        for (int i = startTriIndex * 3; i < endTriIndex * 3; i += 3) {
            Handles.color = randomColors [i % randomColors.Length];
            Vector3 p1 = selectedVertices [selectedTriangles [i + 0]],
            p2 = selectedVertices [selectedTriangles [i + 1]],
            p3 = selectedVertices [selectedTriangles [i + 2]];

            DrawArrow (p1, p2 - p1);
            DrawArrow (p2, p3 - p2);
            DrawArrow (p3, p1 - p3);

            Handles.Label ((p1 + p2 + p3) / 3f, (i / 3).ToString ());
        }

    }

    //Draws an arrow in the handles. `arrowatpercent` determines where the arrow cap is, 1 means at the end 0 at the beginning
    static void DrawArrow (Vector3 origin, Vector3 direction, float arrowAtPercent = .8f)
    {
        DrawRay (origin, direction);
        DrawRay (origin + direction * arrowAtPercent, Quaternion.LookRotation (direction) * Quaternion.Euler (0, 200f, 0) * Vector3.forward * 0.1f);
        DrawRay (origin + direction * arrowAtPercent, Quaternion.LookRotation (direction) * Quaternion.Euler (0, 160f, 0) * Vector3.forward * 0.1f);
    }
    //Draws a line that starts at origin and ends at origin+direction
    static void DrawRay (Vector3 origin, Vector3 direction)
    {
        Handles.DrawLine (origin, origin + direction);
    }

    #endregion
}