using UnityEngine;
using System.Collections;
using UnityEditor;

public class PreferenceItemExample : MonoBehaviour
{
    [PreferenceItem ("Example")]
    static void OnPreferenceGUI ()
    {

    }
}
