using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    // Automatically assigned
    NavMeshAgent agent;

    State playerState = State.Idle;

    [Header("Assignables")]
    [SerializeField] LayerMask walkLayers;

    [Header("Stats")]
    public float damage = 50f;
    public float attackRange = 3f;
    public float movementSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        GetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        if (agent.remainingDistance == 0f)
        {
            playerState = State.Idle;
        }
        else
        {
            playerState = State.Moving;
        }
    }

    void GetInput()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 50f;
            Vector3 raycastDir = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, raycastDir, out hit, 300f, walkLayers))
            {
                agent.SetDestination(hit.point);
            }
        }
    }


    void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}

public enum State
{
    Idle,
    Moving,
    Attacking
}
