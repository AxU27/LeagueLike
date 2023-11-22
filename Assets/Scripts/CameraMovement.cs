using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float maxDistance = 30f;
    [SerializeField] float minDistance = 5f;
    [SerializeField] float cameraSmooth = 0.1f;

    Transform followTransform;

    void Start()
    {
        followTransform = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        Camera.main.transform.position += Camera.main.transform.forward * Input.mouseScrollDelta.y;

        if ((Camera.main.transform.position - transform.position).magnitude < minDistance)
        {
            Camera.main.transform.position += Camera.main.transform.forward * -1f;
        }
        else if ((Camera.main.transform.position - transform.position).magnitude > maxDistance)
        {
            Camera.main.transform.position += Camera.main.transform.forward;
        }

        if (followTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, cameraSmooth);
        }
    }
}
