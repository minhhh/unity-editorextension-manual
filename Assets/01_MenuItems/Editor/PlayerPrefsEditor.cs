using UnityEngine;
using UnityEditor;

public class MenuItems
{
    [MenuItem ("Tools/Clear PlayerPrefs %g")]
    private static void ClearPlayerPrefs ()
    {
        PlayerPrefs.DeleteAll ();
    }

    [MenuItem ("Tools/Option1", false, 1)]
    private static void NewMenuOption ()
    {
    }

    [MenuItem ("Tools/Option2", false, 2)]
    private static void NewMenuOption2 ()
    {
    }

    [MenuItem ("Tools/Option3", false, 3)]
    private static void NewMenuOption3 ()
    {
    }

    [MenuItem ("Tools/Option4", false, 51)]
    private static void NewMenuOption4 ()
    {
    }

    [MenuItem ("Tools/Option5", false, 52)]
    private static void NewMenuOption5 ()
    {
    }
}