using UnityEngine;
using System.Collections;

public class GizmosDemo2 : MonoBehaviour
{

    void OnDrawGizmos ()
    {
        Gizmos.color = new Color32 (255, 0, 255, 255);
        Gizmos.DrawWireCube (transform.position, transform.lossyScale);
    }
}
