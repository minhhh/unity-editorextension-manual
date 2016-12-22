using UnityEditor;

[CustomEditor (typeof(MyType), true)]
public class MyTypeEditor : PropertyInspectorEditor<MyType>
{
}
