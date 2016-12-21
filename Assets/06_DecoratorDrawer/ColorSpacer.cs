using UnityEngine;
using System.Collections;
using UnityEditor;

// This class defines the ColorSpacer attribute, so that
// it can be used in your regular MonoBehaviour scripts:

public class ColorSpacer : PropertyAttribute
{
    public float spaceHeight;
    public float lineHeight;
    public float lineWidth;
    public Color lineColor = Color.red;

    public ColorSpacer (float spaceHeight, float lineHeight, float lineWidth, float r, float g, float b)
    {
        this.spaceHeight = spaceHeight;
        this.lineHeight = lineHeight;
        this.lineWidth = lineWidth;

        // unfortunately we can't pass a color through as a Color object
        // so we pass as 3 floats and make the object here
        this.lineColor = new Color (r, g, b);
    }
}