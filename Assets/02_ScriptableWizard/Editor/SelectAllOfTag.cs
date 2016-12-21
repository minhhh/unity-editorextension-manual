using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class SelectAllOfTag : ScriptableWizard
{
    public string searchTag = "";

    [MenuItem ("Tools/Select All Of Tag")]
    public static void SelectAllOfTagWizard ()
    {
        ScriptableWizard.DisplayWizard <SelectAllOfTag> ("Select All of Tag...", "Reset", "Make Selection");
    }

    void OnWizardCreate ()
    {
        searchTag = "";
        SelectGameObjectsByTag (searchTag);
    }

    void OnWizardOtherButton ()
    {
        SelectGameObjectsByTag (searchTag);
    }

    void SelectGameObjectsByTag (string tag)
    {
        try {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);
            Selection.objects = gameObjects;
        } catch (Exception e) {
            Selection.objects = new GameObject[0];
        }

    }

}
