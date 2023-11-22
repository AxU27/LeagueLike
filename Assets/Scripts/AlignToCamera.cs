using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToCamera : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
