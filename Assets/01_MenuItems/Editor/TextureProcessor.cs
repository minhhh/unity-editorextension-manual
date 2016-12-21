using UnityEngine;
using System.Collections;
using UnityEditor;

public class TextureProcessor
{
    [MenuItem ("Assets/ProcessTexture")]
    private static void DoSomethingWithTexture ()
    {
    }

    // Note that we pass the same path, and also pass "true" to the second argument.
    [MenuItem ("Assets/ProcessTexture", true)]
    private static bool NewMenuOptionValidation ()
    {
        // This returns true when the selected object is a Texture2D (the menu item will be disabled otherwise).
        return Selection.activeObject.GetType () == typeof(Texture2D);
    }
}
