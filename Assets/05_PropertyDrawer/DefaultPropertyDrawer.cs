using UnityEngine;
using System.Collections;

public class DefaultPropertyDrawer : MonoBehaviour
{
    [Range (1, 10)]
    [Tooltip("Value between 1 and 10.")]
    public int num1;

    [Range (1, 10)]
    public float num2;

    [Multiline(5)]
    public string multiline;

    [TextArea(3, 5)]
    public string textArea;

    [ColorUsage (false)]
    public Color color2;

    [ColorUsage (true, true, 0, 8, 0.125f, 3)]
    public Color color3;

    [HideInInspector]
    public string str1;
}
