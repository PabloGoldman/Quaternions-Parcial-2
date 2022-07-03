using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpRot : MonoBehaviour
{
    public Transform redArrow;
    public Transform blueArrow;

    [Range(0.0f, 1.0f)]
    public float t;

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(redArrow.rotation, blueArrow.rotation, t);
    }
}
