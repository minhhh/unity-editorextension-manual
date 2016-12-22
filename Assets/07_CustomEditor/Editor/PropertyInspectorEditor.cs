using UnityEditor;
using UnityEngine;
using System.Collections;

public class PropertyInspectorEditor<T> : Editor where T : MonoBehaviour
{
    T m_Instance;
    PropertyField[] m_fields;

    public void OnEnable ()
    {
        m_Instance = target as T;
        m_fields = ExposeProperties.GetProperties (m_Instance);
    }

    public override void OnInspectorGUI ()
    {
        if (m_Instance == null)
            return;

        this.DrawDefaultInspector ();

        ExposeProperties.Expose (m_fields);

        EditorUtility.SetDirty (m_Instance);
    }
}
