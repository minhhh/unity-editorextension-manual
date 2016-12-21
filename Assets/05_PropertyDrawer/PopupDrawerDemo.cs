using UnityEngine;
using System.Collections;

public class PopupDrawerDemo : MonoBehaviour
{
    public string str1;

    [Popup ("value1", "value2", "value3")]
    public string str2;
}
