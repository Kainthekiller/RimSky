using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawSphere : MonoBehaviour
{

    [Header ("Color")]
    public Color color = Color.red;

    [Space()]
    [Header("Get size from object")]
    public bool getSize = true;

    [Space ()]
    [Header("Size")]
    [Range(0.1f,100f)]
    public float size = 1f;

    [Space()]
    [Header("On or off")]
    public bool isOn = true;

    void OnDrawGizmos()
    {
        if (isOn)
        {
            if (getSize)
            {
                size = transform.localScale.x/2;
            }
            Gizmos.color = color;
            Gizmos.DrawWireSphere(this.transform.position, size);

        }
    }
}
