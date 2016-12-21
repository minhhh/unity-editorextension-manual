using UnityEditor;
using UnityEngine;

public class IHasCustomMenuWindow : EditorWindow, IHasCustomMenu
{
    bool locked = true;
    public void AddItemsToMenu (GenericMenu menu)
    {
        menu.AddItem (new GUIContent ("example"), false, () => {

        });

        menu.AddItem (new GUIContent ("example2"), locked, () => {
            locked = !locked;
        });
    }

    [MenuItem ("Window/IHasCustomMenuWindow")]
    static void Open ()
    {
        GetWindow<IHasCustomMenuWindow> ();
    }
}