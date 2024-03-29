using HighlightPlus;
using System.Collections.Generic;
using System.Linq;
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
    Enemy targetEnemy;
    [HideInInspector]
    public float attackCd;
    float passiveCdRemaining;
    float ability1CdRemaining;
    float ability2CdRemaining;
    float ability3CdRemaining;
    float ability4CdRemaining;
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
    float vamp;

    Dictionary<string, Buff> buffs;

    int hpIncrease;
    float asMultiplier = 1f;
    int damageIncrease;
    int defenseIncrease;
    int critIncrease;
    int cdrIncrease;
    float msMultiplier = 1f;
    float attackRangeIncrease;
    float vampIncrease;

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
    public float baseVamp = 0f;
    [SerializeField] float passiveCd;
    [SerializeField] float ability1Cd, ability2Cd, ability3Cd, ability4Cd;

    public delegate void OnHit(Enemy enemy, Player player);
    public static OnHit onHit;

    // Start is called before the first frame update
    void Start()
    {
        buffs = new Dictionary<string, Buff>();
        GetReferences();
        gameManager.player = this;
        animator.applyRootMotion = false;

        agent.updatePosition = true;
        agent.updateRotation = false;

        hp = baseMaxHp;
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        bool shouldMove = agent.velocity.magnitude > 0.5f;
        animator.SetBool("move", shouldMove);
        animator.SetFloat("locomotion", agent.velocity.magnitude);

        if (agent.desiredVelocity.magnitude > 0f)
        {
            transform.forward = Vector3.Lerp(transform.forward, agent.desiredVelocity, 7 * Time.deltaTime);
        }

        if (passiveCdRemaining > 0f)
            passiveCdRemaining -= Time.deltaTime;

        if (attackCd > 0f)
            attackCd -= Time.deltaTime;

        if (ability1CdRemaining > 0f)
            ability1CdRemaining -= Time.deltaTime;

        if (ability2CdRemaining > 0f)
            ability2CdRemaining -= Time.deltaTime;

        if (ability3CdRemaining > 0f)
            ability3CdRemaining -= Time.deltaTime;

        if (ability4CdRemaining > 0f)
            ability4CdRemaining -= Time.deltaTime;


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

            int dmgDealt = targetEnemy.TakeDamage((int)(damage * c));
            Heal((int)(dmgDealt * vamp));
            onHit?.Invoke(targetEnemy, this);

            if (passiveCdRemaining <= 0f)
            {
                targetEnemy.TakeDamage((int)(targetEnemy.hp * 0.1f));
                Heal((int)((float)(maxHp - hp) * 0.2f));
                passiveCdRemaining = GetCooldown(passiveCd);
                hud.SetCooldown(0, passiveCdRemaining);
            }

            if (targetEnemy.dead)
                targetEnemy = null;
        }
    }

    void Ability1Used(float freezeTime)
    {
        if (ability1CdRemaining > 0f)
            return;

        agent.ResetPath();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        animator.applyRootMotion = true;
        animator.SetTrigger("ability1");
        ability1CdRemaining = GetCooldown(ability1Cd);
        hud.SetCooldown(1, ability1CdRemaining);
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

    void Ability2Used()
    {
        if (ability2CdRemaining > 0f)
            return;

        ability2CdRemaining = GetCooldown(ability2Cd);
        hud.SetCooldown(2, ability2CdRemaining);
        AddBuff(GameAssets.i.buffPrefabs[0]);
    }

    public void Ability1()
    {
        GameObject go = Instantiate(abilityProjectile, modelTransform.position + Vector3.up, modelTransform.rotation);
        go.GetComponent<Projectile>().Setup(damage * 2, 5, 40f, 10f, null, null, false);
    }

    float GetCooldown(float cd)
    {
        return cd * (1f - ((float)cdr / 100f));
    }

    void ModifyHealth(int amount)
    {
        hp += amount;

        hud.UpdateHealthBar(baseMaxHp, hp);

        //Check if dead
    }

    /// <summary>
    /// Calculates damage taken from pure damage and defence and applies it to the Player.
    /// </summary>
    /// <param name="amount">Amount of pure damage</param>
    /// <returns>The amount of damage dealt after the defence calculation</returns>
    public int TakeDamage(int amount)
    {
        amount = (int)Mathf.Clamp((amount * (100f / (100f + defence))), 1f, 99999f);
        ModifyHealth(-amount);
        //OnDamageTaken
        return amount;
    }

    public void Heal(int amount)
    {
        if (amount > maxHp - hp)
            amount = maxHp - hp;
        ModifyHealth(amount);
        //if amount > 0:OnHealed
    }

    void Freeze(float freezeTime)
    {
        Invoke("FreezeEnd", freezeTime);
        canAct = false;
        targetEnemy = null;
    }

    void Freezing()
    {
        transform.position = modelTransform.position;
        modelTransform.position = transform.position;
        agent.nextPosition = modelTransform.position;
    }

    void FreezeEnd()
    {
        canAct = true;
        animator.applyRootMotion = false;
        agent.ResetPath();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
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
            Ability1Used(0.8f * (1f/msMultiplier));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Ability2Used();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CraftingUi.i.ToggleCrafting();
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
        vampIncrease = 0f;

        gameManager.GetItemStats(this);

        foreach (var buffPair in buffs)
        {
            buffPair.Value.AddStats(this);
        }

        maxHp = baseMaxHp + hpIncrease;
        attackSpeed = baseAttackSpeed * asMultiplier;
        movementSpeed = baseMovementSpeed * msMultiplier;
        damage = baseDamage + damageIncrease;
        defence = baseDefence + defenseIncrease;
        crit = baseCrit + critIncrease;
        cdr = Mathf.RoundToInt(Mathf.Clamp(baseCdr + cdrIncrease, 0f, 75f));
        attackRange = baseAttackRange + attackRangeIncrease;
        vamp = baseVamp + vampIncrease;

        agent.speed = movementSpeed;

        animator.SetFloat("asMultiplier", asMultiplier);
        animator.SetFloat("msMultiplier", msMultiplier);
        hud.UpdateHudStats(damage, attackSpeed, crit, defence, movementSpeed, cdr);
        hud.UpdateHealthBar(maxHp, hp);
    }

    public void AddStats(int hpInc, float asMult, float msMult, int damageInc, int defenceInc, int critInc, int cdrInc, float arInc, float vampInc)
    {
        maxHp += hpInc;
        asMultiplier += asMult;
        msMultiplier += msMult;
        damageIncrease += damageInc;
        defenseIncrease += defenceInc;
        critIncrease += critInc;
        cdrIncrease += cdrInc;
        attackRangeIncrease += arInc;
        vampIncrease += vampInc;
    }

    public void AddBuff(GameObject buffPrefab)
    {
        GameObject go = Instantiate(buffPrefab, hud.buffPanel);
        Buff buff = go.GetComponent<Buff>();

        if (buffs.ContainsKey(buff.buffName))
        {
            buffs.TryGetValue(buff.buffName, out buff);
            Debug.Log(buff.buffName);
            buff.AddStack();
            Destroy(go);
            UpdateStats();
            return;
        }

        buffs.Add(buff.buffName, buff);

        UpdateStats();
    }

    public void RemoveBuff(string buffName)
    {
        if (buffs.ContainsKey(buffName))
        {
            buffs.Remove(buffName);
        }

        UpdateStats();
    }
}

public enum State
{
    Idle,
    Moving,
    Attacking
}
