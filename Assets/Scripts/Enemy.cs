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
    public float maxHp = 100f;
    public float attackRange = 2f;
    public int damage = 20;
    public float attackSpeed = 0.5f;
    public int defence = 0;

    [HideInInspector]
    public Player player;
    [HideInInspector]
    public float hp;
    float timer;
    [HideInInspector]
    public float attackCd;
    public bool dead;

    public delegate void OnEnemyDeath();
    public static OnEnemyDeath onEnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        hp = maxHp;

        animator.applyRootMotion = false;

        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("locomotion", agent.velocity.magnitude / agent.speed);

        Behavior();

        if (attackCd > 0f)
            attackCd -= Time.deltaTime;
    }

    public virtual void Behavior()
    {
        if (player != null && !dead)
        {
            FollowAndAttack();
        }
    }

    public int TakeDamage(float amount)
    {
        amount = (int)(amount * (100f / (100f + defence)));
        hp -= amount;
        UpdateHealthBar();

        if (hp <= 0f && !dead)
        {
            dead = true;
            onEnemyDeath?.Invoke();
            Die();
        }

        return (int)amount;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
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
            player.TakeDamage(damage);
        }
    }
}
