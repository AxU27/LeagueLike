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
    Enemy targetEnemy;
    float attackCd;
    float ability1CdRemaining;
    float timer;
    bool canAct = true;

    [Header("Assignables")]
    [SerializeField] LayerMask walkLayers;
    [SerializeField] Transform modelTransform;

    [Header("Stats")]
    public float damage = 50f;
    public float attackRange = 3f;
    public float movementSpeed = 4f;
    public float attackSpeed = 1f;
    [SerializeField] float ability1Cd, ability2Cd, ability3Cd, ability4Cd;

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

        if (attackCd > 0f)
            attackCd -= Time.deltaTime;

        if (ability1CdRemaining > 0f)
            ability1CdRemaining -= Time.deltaTime;


        if (targetEnemy != null)
        {
            Attacking();
        }
        if (agent.remainingDistance == 0f)
        {
            playerState = State.Idle;
        }
        else
        {
            playerState = State.Moving;
        }
    }

    void Attacking()
    {
        if (attackRange < (targetEnemy.transform.position - transform.position).magnitude)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                agent.SetDestination(targetEnemy.transform.position);
                timer = 0.5f;
            }
        }
        else
        {
            if (agent.hasPath) { agent.ResetPath(); }

            if (attackCd <= 0f)
            {
                animator.SetTrigger("attack");
                attackCd = 1f / attackSpeed;
            }

            Quaternion targetRot = Quaternion.LookRotation(targetEnemy.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 10 * Time.deltaTime);
        }
    }

    public void Attack()
    {
        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(damage);
        }
    }

    void Ability1Used(float freezeTime)
    {
        if (ability1CdRemaining > 0f)
            return;

        agent.SetDestination(transform.position + transform.forward * 4f);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        animator.SetTrigger("ability1");
        Hud.i.SetCooldown(1, ability1Cd);
        ability1CdRemaining = ability1Cd;
        Freeze(freezeTime);
    }

    public void Ability1()
    {
        
    }

    void Freeze(float freezeTime)
    {
        Invoke("FreezeEnd", freezeTime);
        canAct = false;
        agent.updateRotation = false;
        targetEnemy = null;
    }

    void FreezeEnd()
    {
        canAct = true;
        agent.updateRotation = true;
        agent.ResetPath();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
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
        if (!canAct)
            return;

        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 50f;
            Vector3 raycastDir = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, raycastDir, out hit, 300f, walkLayers))
            {
                timer = 0f;
                if (hit.transform.tag == "Enemy")
                {
                    targetEnemy = hit.transform.GetComponent<Enemy>();
                    Debug.Log("Hit enemy");
                }
                else
                {
                    targetEnemy = null;
                    agent.SetDestination(hit.point);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ability1Used(0.9f);
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
