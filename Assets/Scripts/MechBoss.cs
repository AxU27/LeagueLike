using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBoss : Enemy
{
    [SerializeField] GameObject meteorStrikePrefab;
    [SerializeField] float meteorCd;

    float timer1;
    float timer2 = 0.8f;

    bool healersSpawned;

    protected override void Behavior()
    {
        if (dead || player == null)
            return;

        HandleTimers();

        if (timer1 < 10f)
        {
            if (agent.hasPath)
                agent.ResetPath();
            defence = 20;
            if (timer2 <= 0f)
            {
                GameObject go = Instantiate(meteorStrikePrefab, player.transform.position, Quaternion.identity);
                go.GetComponent<Explosion>().Init(damage * 2);
                timer2 = meteorCd;
            }
            
        }
        else
        {
            defence = 400;
            base.Behavior();
        }

        if (!healersSpawned)
        {
            if (hp <= maxHp / 2)
            {
                healersSpawned = true;
                for (int i = 0; i < 4; i++)
                {
                    Vector3 point = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                    GameObject go = Instantiate(GameAssets.i.enemyPrefabs[2], transform.position + point, Quaternion.identity);
                    go.GetComponent<HealerRobot>().SetHealingTarget(this);
                }
            }
        }
    }

    protected override void Die()
    {
        animator.SetTrigger("die");
        //Spawn particles
        //Spawn loot
        Destroy(gameObject, 1.3f);
    }

    void HandleTimers()
    {
        if (timer1 > 0f)
            timer1 -= Time.deltaTime;
        else
            timer1 = 20f;

        if (timer2 > 0f)
            timer2 -= Time.deltaTime;
    }
}
