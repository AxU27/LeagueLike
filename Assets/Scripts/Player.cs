using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    // Automatically assigned
    NavMeshAgent agent;
    Animator animator;

    State playerState = State.Idle;
    Vector2 velocity;
    Vector2 smoothDeltaPos;

    [Header("Assignables")]
    [SerializeField] LayerMask walkLayers;
    [SerializeField] Transform modelTransform;

    [Header("Stats")]
    public float damage = 50f;
    public float attackRange = 3f;
    public float movementSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        GetReferences();
        animator.applyRootMotion = true;

        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        SyncAnimatorAndAgent();

        if (agent.remainingDistance == 0f)
        {
            playerState = State.Idle;
        }
        else
        {
            playerState = State.Moving;
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPos = animator.rootPosition;
        rootPos.y = agent.nextPosition.y;
        transform.position = rootPos;
        agent.nextPosition = rootPos;
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

    void SyncAnimatorAndAgent()
    {
        Vector3 worldDeltaPos = agent.nextPosition - transform.position;
        worldDeltaPos.y = 0f;

        float dx = Vector3.Dot(transform.right, worldDeltaPos);
        float dy = Vector3.Dot(transform.forward, worldDeltaPos);
        Vector2 deltaPos = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        smoothDeltaPos = Vector2.Lerp(smoothDeltaPos, deltaPos, smooth);

        velocity = smoothDeltaPos / Time.deltaTime;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            velocity = Vector2.Lerp(Vector2.zero, velocity, agent.remainingDistance / agent.stoppingDistance);
        }

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.stoppingDistance;

        animator.SetBool("move", shouldMove);
        animator.SetFloat("locomotion", velocity.magnitude);

        float deltaMagnitude = worldDeltaPos.magnitude;
        if (deltaMagnitude > agent.radius / 2)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, agent.nextPosition, smooth);
            modelTransform.position = transform.position;
        }
    }


    void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
}

public enum State
{
    Idle,
    Moving,
    Attacking
}
