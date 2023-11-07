using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float heigth = 10f;

    Transform followTransform;

    void Start()
    {
        followTransform = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
    }
}
