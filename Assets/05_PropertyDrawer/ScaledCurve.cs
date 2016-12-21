using UnityEngine;
using System.Collections;

// Custom serializable class
[System.Serializable]
public class ScaledCurve
{
    public float scale = 1;
    public AnimationCurve curve = AnimationCurve.Linear (0, 0, 1, 1);
}