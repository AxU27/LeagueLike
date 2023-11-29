using HighlightPlus;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    // Automatically assigned
    NavMeshAgent agent;
    Animator animator;
    Hud hud;
    GameManager gameManager;

    State playerState = State.Idle;
    Vector2 velocity;
    Vector2 smoothDeltaPos;
    Enemy targetEnemy;
    [HideInInspector]
    public float attackCd;
    float ability1CdRemaining;
    float timer;
    bool canAct = true;
    int hp;
    int maxHp;
    int damage;
    float attackSpeed;
    float attackRange;
    float movementSpeed;
    int defence;
    int crit;
    int cdr;

    int hpIncrease;
    float asMultiplier = 1f;
    int damageIncrease;
    int defenseIncrease;
    int critIncrease;
    int cdrIncrease;
    float msMultiplier = 1f;
    float attackRangeIncrease;

    [Header("Assignables")]
    [SerializeField] LayerMask clickabeLayers;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask interactLayer;
    [SerializeField] Transform modelTransform;
    [SerializeField] GameObject abilityProjectile;

    [Header("Stats")]
    public int baseMaxHp = 500;
    public int baseDamage = 50;
    public float baseAttackRange = 3f;
    public float baseMovementSpeed = 4f;
    public float baseAttackSpeed = 1f;
    public int baseDefence = 10;
    public int baseCrit = 0;
    public int baseCdr = 0;
    [SerializeField] float ability1Cd, ability2Cd, ability3Cd, ability4Cd;

    public delegate void OnHit(Enemy enemy, Player player);
    public static OnHit onHit;

    // Start is called before the first frame update
    void Start()
    {
        GetReferences();
        gameManager.player = this;
        animator.applyRootMotion = true;

        agent.updatePosition = false;
        agent.updateRotation = true;

        hp = baseMaxHp;
        UpdateStats();
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

        if (!canAct)
        {
            Freezing();
        }
    }

    void Attacking()
    {
        if (baseAttackRange < (targetEnemy.transform.position - transform.position).magnitude)
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
                attackCd = 1f / (baseAttackSpeed * asMultiplier);
            }

            Quaternion targetRot = Quaternion.LookRotation(targetEnemy.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 10 * Time.deltaTime);
        }
    }

    public void Attack()
    {
        if (targetEnemy != null)
        {
            float c = 1f;
            if (Random.Range(0, 100) < crit)
                c = 2f;

            targetEnemy.TakeDamage((baseDamage + damageIncrease) * c);
            onHit?.Invoke(targetEnemy, this);
        }
    }

    void Ability1Used(float freezeTime)
    {
        if (ability1CdRemaining > 0f)
            return;

        //agent.SetDestination(transform.position + transform.forward * 4f);
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        animator.SetTrigger("ability1");
        hud.SetCooldown(1, ability1Cd);
        ability1CdRemaining = ability1Cd;
        Freeze(freezeTime);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 50f;
        Vector3 raycastDir = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, raycastDir, out hit, 300f, groundLayer))
        {
            transform.forward = hit.point - transform.position;
        }
    }

    public void Ability1()
    {
        GameObject go = Instantiate(abilityProjectile, modelTransform.position + Vector3.up, modelTransform.rotation);
        go.GetComponent<Projectile>().Setup(damage * 2, 5, 40f, 10f, null, null);
    }

    void ModifyHealth(int amount)
    {
        hp += amount;

        hud.UpdateHealthBar(baseMaxHp, hp);

        //Check if dead
    }

    public void TakeDamage(int amount)
    {
        //OnDamageTaken
        ModifyHealth(-amount);
    }

    public void Heal(int amount)
    {
        //OnHealed
        ModifyHealth(amount);
    }

    void Freeze(float freezeTime)
    {
        Invoke("FreezeEnd", freezeTime);
        canAct = false;
        agent.updateRotation = false;
        targetEnemy = null;
    }

    void Freezing()
    {
        agent.nextPosition = modelTransform.position;
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
            if (Physics.Raycast(Camera.main.transform.position, raycastDir, out hit, 300f, clickabeLayers))
            {
                timer = 0f;
                if (hit.transform.tag == "Enemy")
                {
                    targetEnemy = hit.transform.GetComponent<Enemy>();
                    targetEnemy.GetComponent<HighlightEffect>().HitFX();
                }
                else
                {
                    targetEnemy = null;
                    agent.SetDestination(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 50f;
            Vector3 raycastDir = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, raycastDir, out hit, 300f, interactLayer))
            {
                hit.transform.GetComponent<IInteractable>().Interact(this);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ability1Used(0.8f);
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
        if (deltaMagnitude > agent.radius / 4)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, agent.nextPosition, smooth);
            modelTransform.position = transform.position;
        }
    }


    void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        hud = Hud.i;
        gameManager = GameManager.i;
    }

    public void UpdateStats()
    {
        hpIncrease = 0;
        asMultiplier = 1f;
        msMultiplier = 1f;
        damageIncrease = 0;
        defenseIncrease = 0;
        critIncrease = 0;
        cdrIncrease = 0;
        attackRangeIncrease = 0;

        gameManager.GetItemStats(this);

        maxHp = baseMaxHp + hpIncrease;
        attackSpeed = baseAttackSpeed * asMultiplier;
        movementSpeed = baseMovementSpeed * msMultiplier;
        damage = baseDamage + damageIncrease;
        defence = baseDefence + defenseIncrease;
        crit = baseCrit + critIncrease;
        cdr = baseCdr + cdrIncrease;
        attackRange = baseAttackRange + attackRangeIncrease;

        agent.speed = movementSpeed;

        animator.SetFloat("asMultiplier", asMultiplier);
        animator.SetFloat("msMultiplier", msMultiplier);
        hud.UpdateHudStats(damage, attackSpeed, crit, defence, movementSpeed, cdr);
        hud.UpdateHealthBar(maxHp, hp);
    }

    public void AddStats(int hpInc, float asMult, float msMult, int damageInc, int defenceInc, int critInc, int cdrInc, float arInc)
    {
        maxHp += hpInc;
        asMultiplier += asMult;
        msMultiplier += msMult;
        damageIncrease += damageInc;
        defenseIncrease += defenceInc;
        critIncrease += critInc;
        cdrIncrease += cdrInc;
        attackRangeIncrease += arInc;
    }
}

public enum State
{
    Idle,
    Moving,
    Attacking
}
