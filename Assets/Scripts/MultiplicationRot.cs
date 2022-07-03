using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplicationRot : MonoBehaviour
{
    public float degrees;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.rotation *= Quaternion.Euler(0, 0, degrees); 
        }   
    }
}
