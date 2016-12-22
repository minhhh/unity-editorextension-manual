# Custom Extension Manual

The Unity Editor offers many APIs for extending its functionality in various ways. It helps bring extreme flexibility to your workflow as a developer, designer or artist.

In this manual, I will explore the options for creating Unity Editor extensions. The examples are drawn from the code of plenty of tutorials on the Internet, open-source projects and Unity plugins.

## Menu Items

Unity Editor Extensions allow adding custom menus for commonly used functionality that is frequently accessed.

### Adding Menu Items

1. Create `Editor` script
1. Create static methods that are marked with the `MenuItem` attribute.

**Example**

```
using UnityEngine;
using UnityEditor;

public class MenuItems
{
    [MenuItem ("Tools/Clear PlayerPrefs")]
    private static void ClearPlayerPrefs ()
    {
        PlayerPrefs.DeleteAll ();
    }
}
```

It is sometimes preferable to collect all custom menu items in a single class, such as shown in [https://github.com/EsotericSoftware/spine-runtimes/blob/36000e3c550a80c712afcf65a73103c7d29c9f5f/spine-unity/Assets/spine-unity/Editor/Menus.cs](https://github.com/EsotericSoftware/spine-runtimes/blob/36000e3c550a80c712afcf65a73103c7d29c9f5f/spine-unity/Assets/spine-unity/Editor/Menus.cs).

### Hotkeys

Hotkey can be added to the menus as follows:

```
[MenuItem("Tools/New Option %#a")]
private static void NewMenuOption()
{
}

// Add a new menu item with hotkey CTRL-G

[MenuItem("Tools/Item %g")]
private static void NewNestedOption()
{
}

// Add a new menu item with hotkey G
[MenuItem("Tools/Item2 _g")]
private static void NewOptionWithHotkey()
{
}
```

[These are the supported keys](https://docs.unity3d.com/ScriptReference/MenuItem.html):

    % – CTRL on Windows / CMD on OSX
    # – Shift
    & – Alt
    LEFT/RIGHT/UP/DOWN – Arrow keys
    F1...F2 – F keys
    HOME, END, PGUP, PGDN

### Component Menu Items
Unity does not allow you to add items to `Component` menu with `MenuItem` attribute. Instead, you have to use a different attribute, `AddComponentMenu`, on the MonoBehaviour class itself.

**Examples**

```
using UnityEngine;
using System.Collections;

[AddComponentMenu ("My/SuperMonoBehaviour")]
public class MyMonoBehaviour : MonoBehaviour
{
}
```

A real-world example is [https://github.com/EsotericSoftware/spine-runtimes/blob/master/spine-unity/Assets/spine-unity/SkeletonUtility/SkeletonUtilityBone.cs](https://github.com/EsotericSoftware/spine-runtimes/blob/master/spine-unity/Assets/spine-unity/SkeletonUtility/SkeletonUtilityBone.cs)

### Special Paths

Unity has a few “special” paths that act as context menus (menus that are accessible using right-click):

    Assets – items will be available under the “Assets” menu, as well using right-click inside the project view.
    Assets/Create – items will be listed when clicking on the “Create” button in the project view (useful when adding new types that can be added to the project)
    CONTEXT/ComponentName – items will be available by right-clicking inside the inspector of the given component.

**MenuCommand**

When adding a new menu item to an inspector (using `CONTEXT/Component`, as described above), sometimes it is necessary to get a reference to the actual component (e.g: to modify its data).

This can be done by adding a MenuCommand argument to the static method that defines the new menu item:

```
[MenuItem ("CONTEXT/Rigidbody/New Option")]
    private static void NewOpenForRigidBody (MenuCommand menuCommand)
    {
        var rigid = menuCommand.context as Rigidbody;
        Debug.Log ("Change rigidbody " + rigid);
    }
```

**ContextMenu**

This is similar to defining `MenuItem` with a path that starts with `CONTEXT/...`

Examples

```
public class MyMonoBehaviour : MonoBehaviour
{
    public string name = "";

    [ContextMenu ("Reset Name")]
    private void ResetName ()
    {
        name = string.Empty;
    }
}
```

A real world example is [this](https://github.com/EsotericSoftware/spine-runtimes/blob/9c21942482701ccf28778a210fef03f3cae151e6/spine-unity/Assets/Examples/Getting%20Started/Scripts/SpineboyBeginnerView.cs).


**ContextMenuItem**

This attribute is added to fields of a component (MonoBehaviour) class, to allow adding context menus at a finer resolution

Example

```
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
    [ContextMenuItem("Reset", "ResetBiography")]
    [Multiline(8)]
    public string playerBiography = "";
    void ResetBiography() {
        playerBiography = "";
    }
}
```

### Validation

Some menu items should only be available if certain conditions are met, and should not be available otherwise. Enabling/disabling menu items according to their usage context is done by adding validation methods.

**Example**

```
[MenuItem("Assets/ProcessTexture")]
private static void DoSomethingWithTexture()
{
}

// Note that we pass the same path, and also pass "true" to the second argument.
[MenuItem("Assets/ProcessTexture", true)]
private static bool NewMenuOptionValidation()
{
    // This returns true when the selected object is a Texture2D (the menu item will be disabled otherwise).
    return Selection.activeObject.GetType() == typeof(Texture2D);
}
```

A real world example is [this](https://github.com/EsotericSoftware/spine-runtimes/blob/a6d9915c95b3f0c0db050b603fa3d5fd868f1346/spine-unity/Assets/spine-unity/Editor/BoneFollowerInspector.cs).

### Controlling Order with Priority
Priority is a number that can be assigned to a MenuItem attribute that controls the ordering of menu items under the root menu. Menu items are also automatically grouped according to their assigned priority in increments of 50.

**Example**

```
[MenuItem("NewMenu/Option1", false, 1)]
private static void NewMenuOption()
{
}

[MenuItem("NewMenu/Option2", false, 2)]
private static void NewMenuOption2()
{
}

[MenuItem("NewMenu/Option3", false, 3)]
private static void NewMenuOption3()
{
}

[MenuItem("NewMenu/Option4", false, 51)]
private static void NewMenuOption4()
{
}

[MenuItem("NewMenu/Option5", false, 52)]
private static void NewMenuOption5()
{
}
```


**References**
* [Unity Editor Extensions – Menu Items](http://www.tallior.com/unity-editor-extensions-menu-items/)
* [Guide to Extending Unity Editor’s Menus](https://blog.redbluegames.com/guide-to-extending-unity-editors-menus-b2de47a746db#.5lwjprjv8)


## ScriptableWizard
One of the simplest forms of custom editor windows. It opens like a dialog box, you set some values in it, then hit a button to execute some action.

Editor wizards are typically opened using a menu item.

```
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
```

**References**
* [Creating Basic Editor Tools](https://www.youtube.com/watch?v=L24GKk1qQD4)

## Custom EditorWindow

Create your own custom editor window that can float free or be docked as a tab, just like the native windows: Scene Window, Game Window, Inspector Window in the Unity interface.

EditorWindow are typically opened using a menu item.

**Example**

```
public class MultipleWindow : EditorWindow
{
    [MenuItem ("Window/MultipleWindow")]
    static void Open ()
    {
        var exampleWindow = CreateInstance<MultipleWindow> ();
        exampleWindow.Show ();
    }
}
```

Using `CreateInstance`, we can create multiple instances of the EditorWindow. However, most of the time, we would only want 1 instance of the EditorWindow to do specific tasks. In that case, we will use `EditorWindow.GetWindow`

```
public class SingleWindow : EditorWindow
{
    [MenuItem("Window/SingleWindow")]
    static void Open ()
    {
        GetWindow<SingleWindow> ();
    }
}
```

### Showing Windows

There are several ways to show the EditorWindow

**Show**

This is the default way, using `EditorWindow.Show`. The default call to `EditorWindow.GetWindow` also use this method internally. Windows shown in this way can be docked as a tab to a DockArea

**ShowUtility**

If you want the window to be always on top of other windows, and not dockable, you can use `EditorWindow.ShowUtility` or `EditorWindow.GetWindow (true)`.

**ShowPopup**

This is a Window with no window title and no Close button. You have to implement the Close function yourself.

The `Slice` menu in the `SpriteEditor` window is implemented using this approach.

**PopupWindow**

This is similar to `ShowPopup`

**ShowAuxWindow**

Create a window that can not be set as Tab Window. It is similar to `ShowUtility`, but if you focus on another window, this window will be deleted.

**ShowAsDropDown**

Similar to `ShowPopup` except that it will always be displayed within the visible area of the screen


### Preference Item

`PreferenceItem` is a way to add menus to Unity Preferences

```
public class PreferenceItemExample : MonoBehaviour
{
    [PreferenceItem ("Example")]
    static void OnPreferenceGUI ()
    {
    }
}
```

Real world example: [BitStrapPreferences](https://bitbucket.org/Bitcake-Studio/bitstrap/src/4999a15ed7af5a2f6786c38d57733e1a5871579f/Assets/BitStrap/Plugins/Preferences/Editor/BitStrapPreferences.cs?at=default&fileviewer=file-view-default)

### IHasCustomMenu

You can implement this interface to add more item to the Context Menu of the EditorWindow

```
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
```

## Saving Data
When using Editor Extension, there are 4 options

### EditorPrefs

EditorPrefs is a simple way to store key-value tuple in Unity Editor environment.

Example

```
public class EditorPrefsWindow : EditorWindow
{
    int someValue = 1;
    const string SOME_KEY = "KEY";


    [MenuItem ("Window/EditorPrefsWindow")]
    static void Open ()
    {
        GetWindow <EditorPrefsWindow> ();
    }

    void OnEnable ()
    {
        someValue = EditorPrefs.GetInt (SOME_KEY, 1);
    }

    void OnGUI ()
    {
        EditorGUI.BeginChangeCheck ();

        someValue = EditorGUILayout.IntSlider ("My Value", someValue, 1, 100);

        if (EditorGUI.EndChangeCheck ()) {
            EditorPrefs.SetInt (SOME_KEY, someValue);
        }

    }
}
```

### EditorUserSettings

It's a way to save data only for the current project. The data will be stored in `Library/EditorUserSettings.asset`.

It can be used to save sensitive information such as password or oauth accesss token. The values will be encrypted.

### ScriptableObject

This is the main form of saving shared data in Unity. You can save various kinds of data, and you can read back these data easily from outside Editor Script.


### JSON
Finally, one can use `JSON` format to save settings.

There are many ways to serialize Unity Objects to JSON, including Unity's `JsonUtility`, `EditorJsonUtility` and other third-party libraries such as `MiniJSON` or `SimpleJSON`.

## Property Drawer

`PropertyDrawer` is a great way to create custom drawer for your Serializable class. Another use is to customize the GUI of script members with custom `PropertyAttribute`

### CustomPropertyDrawer with PropertyAttribute

Unity already provides CustomPropertyDrawer for several `PropertyAttribute` including: `Range, Multiline, TextArea, ColorUsage, HideInInspector, Tooltip`

Example

```
[CustomPropertyDrawer (typeof (MyAttribute))]
public class MyAttributeDrawer : PropertyDrawer {

	// Draw the property inside the given rect
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
    ...
```

Real world examples: [property-drawer-collection](https://github.com/anchan828/property-drawer-collection)

### CustomPropertyDrawer for a class

You can override the default property drawer for a class, similarly to a `PropertyAttribute`

```
[CustomPropertyDrawer (typeof (MyClass))]
public class MyClassDrawer : PropertyDrawer {

	// Draw the property inside the given rect
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
    ...
```

**References**

* [Property Drawers & Custom Inspectors](https://unity3d.com/learn/tutorials/topics/interface-essentials/property-drawers-custom-inspectors)

## DecoratorDrawer
A DecoratorDrawer is similar to a PropertyDrawer, except that it doesn't draw a property but rather draws decorative elements based purely on the data it gets from its corresponding PropertyAttribute.

Some built-in DecoratorDrawer includes: Header, Space

Example

```
[CustomPropertyDrawer(typeof(MyAttribute))]
public class MyAttributeDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
    ...
```

[Unity example](https://docs.unity3d.com/ScriptReference/DecoratorDrawer.html)

## Custom Editor

Unity allows creating a custom inspector for your custom defined classes.

To create a new inspector, create a new `EditorClass` and decorate it with `CustomEditor` attribute.

```
[CustomEditor(typeof(MySettingsClass))]
public class MySettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
    }

    public override void OnSceneGUI()
    {
    }

    public override void OnPreviewGUI (Rect r, GUIStyle background)
    {
    }
}
```

Real world example of Custom Inspector: (https://github.com/EsotericSoftware/spine-runtimes/blob/master/spine-unity/Assets/spine-unity/Editor/SkeletonRendererInspector.cs)

Example of OnPreviewGUI: [Unity: ScriptableObject and AssetDatabase](https://www.youtube.com/watch?v=74Ph6y0rR-g)

### SceneView

You can draw GUI stuff on the Scene View with `SceneView.onSceneGUIDelegate += OnSceneGUI;`. Inside a CustomEditor, you can use the function `OnSceneGUI`.

Example

```
public void OnSceneGUI ()
{
    Handles.BeginGUI ();
    ...
    Handles.EndGUI ();
}
```

Real world example of using Scene View: [Editor Scripting from the real world](https://www.youtube.com/watch?v=s1o0gZwJS-4), SplineEditor of [Xffect Editor](https://www.assetstore.unity3d.com/en/#!/content/3810), and  [MeshVisualizeWindow.cs](https://gist.github.com/TJHeuvel/c4f8d218a0d774682560a8f348a90dff)



**References**

* [Property Drawers & Custom Inspectors](https://unity3d.com/learn/tutorials/topics/interface-essentials/property-drawers-custom-inspectors)
* [Unity Editor Extensions](http://www.tallior.com/unity-editor-extensions/)
* [Expose property in inspector](http://wiki.unity3d.com/index.php?title=Expose_properties_in_inspector)
* [ScriptableObject and AssetDatabase](https://www.youtube.com/watch?v=74Ph6y0rR-g)


## Gizmos
Gizmos are used to give visual debugging or setup aids in the scene view.

### Gizmos setup in Inspector

You can setup the Gizmos added to GameObject in the Inspector.


### Drawing Gizmos

All gizmo drawing has to be done in either OnDrawGizmos or OnDrawGizmosSelected functions of the script.

```
void OnDrawGizmos ()
{
    Gizmos.color = new Color32 (255, 0, 255, 255);
    Gizmos.DrawWireCube (transform.position, transform.lossyScale);
}
```

**References**

* [How to Add Your Own Tools to Unity’s Editor](https://code.tutsplus.com/tutorials/how-to-add-your-own-tools-to-unitys-editor--active-10047)
* [Gizmos](https://docs.unity3d.com/ScriptReference/Gizmos.html)


## EditorApplication.hierarchyWindowItemOnGUI
Delegate for OnGUI events for every visible list item in the HierarchyWindow.

Example

```
public static class HierarchyDemo
{
    [InitializeOnLoadMethod]
    private static void DrawToHierarchy ()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI (int instanceID, Rect rect)
    {

    }
```

## AssetPostProcessor

* [Property Drawers & Custom Inspectors](https://unity3d.com/learn/tutorials/topics/interface-essentials/property-drawers-custom-inspectors)

## Miscs
* [Editor Default Resources](http://anchan828.github.io/editor-manual/web/part2-beginner.html)

## Tips and Tricks


## References
* [Unity Editor Extensions](http://www.tallior.com/unity-editor-extensions/)
* [Editor Scripting from the real world](https://www.youtube.com/watch?v=s1o0gZwJS-4)
* [Property Drawers & Custom Inspectors](https://unity3d.com/learn/tutorials/topics/interface-essentials/property-drawers-custom-inspectors)
* [Editor Manual](http://anchan828.github.io/editor-manual/web/index.html)
