
using UnityEngine;
using System.Collections;

public class DecoratorDrawerDemo : MonoBehaviour
{
    public int a = 1;
    [Space(10)]
    public int b = 2;
    public int c = 3;

    // this shows our custom Decorator Drawer between the groups of properties
    [ColorSpacer (30, 3, 100, 1, 0, 0)]

    public string d = "d";

    [Header ("New group")]
    public string e = "e";
    public string f = "f";

}