using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] Slider hpSlider;
    public Animator animator;
    public NavMeshAgent agent;
    [SerializeField] Transform modelTransform;

    [Header("Stats")]
    [SerializeField] float maxHp = 100f;
    public float attackRange = 2f;
    public float damage = 20f;
    public float attackSpeed = 0.5f;

    Vector2 velocity;
    Vector2 smoothDeltaPos;
    Player player;
    float hp;
    float timer;
    [HideInInspector]
    public float attackCd;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        hp = maxHp;

        animator.applyRootMotion = true;

        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        SyncAnimatorAndAgent();

        if (player != null)
        {
            FollowAndAttack();
        }

        if (attackCd > 0f)
            attackCd -= Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        UpdateHealthBar();

        if (hp <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar()
    {
        hpSlider.value = hp / maxHp;
    }

    void FollowAndAttack()
    {
        if (attackRange < (player.transform.position - transform.position).magnitude)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                agent.SetDestination(player.transform.position);
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

            Quaternion targetRot = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 10 * Time.deltaTime);
        }
    }

    public virtual void Attack()
    {
        if (player != null)
        {
            player.TakeDamage((int)damage);
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
}
