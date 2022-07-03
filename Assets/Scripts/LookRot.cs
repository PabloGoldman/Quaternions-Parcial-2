using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRot : MonoBehaviour
{
    public Transform arrow;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(arrow.forward, Vector3.up);
    }
}
