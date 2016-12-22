using UnityEngine;
using System.Collections;

[System.Serializable]
public class ExplosionShape : ScriptableObject
{
    public int m_width;
    public int m_height;
    public int[] m_data;

    public ExplosionShape ()
    {
        m_width = 8;
        m_height = 8;
        m_data = new int[m_width * m_height];
        for (int i = 0; i < m_data.Length; i++)
            m_data [i] = 0;
    }
};