using UnityEngine;
using System.Collections;

[AddComponentMenu ("My/SuperMonoBehaviour")]
public class MyMonoBehaviour : MonoBehaviour
{
    [ContextMenuItem ("Reset", "ResetBiography")]
    public string playerBiography = "";

    public string name = "";

    [ContextMenu ("Reset Name")]
    private void ResetName ()
    {
        name = string.Empty;
    }

    void ResetBiography ()
    {
        playerBiography = "";
    }
}
