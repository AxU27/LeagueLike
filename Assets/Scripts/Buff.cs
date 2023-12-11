using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public string buffName;
    public Sprite buffIcon;
    public string buffDescription;
    public float buffDuration;
    public int maxBuffStacks;

    public int maxHpIncrease;
    public float asMultiplier;
    public float msMultiplier;
    public int damageIncrease;
    public int defenceIncrease;
    public int critIncrease;
    public int cdrIncrease;
    public float attackRangeIncrease;
    public float vampIncrease;

    [HideInInspector] public bool ending;
    float lifeTime;
    int stacks = 1;

    public virtual void AddStats(Player player)
    {
        player.AddStats(maxHpIncrease * stacks, asMultiplier * stacks, msMultiplier * stacks, damageIncrease * stacks, defenceIncrease * stacks, critIncrease * stacks, cdrIncrease * stacks, attackRangeIncrease * stacks, vampIncrease * stacks);
    }

    private void Start()
    {
        lifeTime = buffDuration;
        stacks = 1;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            if (stacks <= 1)
                EndBuff();

            stacks--;
            lifeTime = buffDuration;
            GameManager.i.player.UpdateStats();
        }
    }

    public virtual void EndBuff()
    {
        GameManager.i.player.RemoveBuff(buffName);
        Destroy(gameObject);
    }

    public void AddStack()
    {
        if (stacks < maxBuffStacks)
            stacks++;
    }
}
