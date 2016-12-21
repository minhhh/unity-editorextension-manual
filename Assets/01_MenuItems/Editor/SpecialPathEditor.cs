using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpecialPathEditor : MonoBehaviour
{

    [MenuItem ("Assets/Load Additive Scene")]
    private static void LoadAdditiveScene ()
    {
        var selected = Selection.activeObject;
        EditorApplication.OpenSceneAdditive (AssetDatabase.GetAssetPath (selected));
    }

    // Adding a new menu item under Assets/Create

    [MenuItem ("Assets/Create/Add Configuration")]
    private static void AddConfig ()
    {
        // Create and add a new ScriptableObject for storing configuration
    }

    // Add a new menu item that is accessed by right-clicking inside the RigidBody component

    [MenuItem ("CONTEXT/Rigidbody/New Option")]
    private static void NewOpenForRigidBody (MenuCommand menuCommand)
    {
        var rigid = menuCommand.context as Rigidbody;
        Debug.Log ("Change rigidbody " + rigid);
    }
}
