using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerRobot : RangedEnemy
{
    Enemy healingTarget;

    [SerializeField] int healAmount;

    protected override void Behavior()
    {
        if (healingTarget != null)
            FollowAndHeal();
        else
            base.Behavior();
    }

    void FollowAndHeal()
    {
        if (attackRange < (healingTarget.transform.position - transform.position).magnitude)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                agent.SetDestination(healingTarget.transform.position);
                timer = 0.5f;
            }
        }
        else
        {
            if (agent.hasPath) { agent.ResetPath(); }

            if (attackCd <= 0f)
            {
                if (animator != null)
                    animator.SetTrigger("attack");
                else
                    ShootHealingProjectile();

                attackCd = 1f / attackSpeed;
            }

            Quaternion targetRot = Quaternion.LookRotation(healingTarget.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 10 * Time.deltaTime);
        }
    }

    void ShootHealingProjectile()
    {
        if (healingTarget == null)
            return;

        GameObject go = Instantiate(projectile, shotPoint.position, transform.rotation);
        go.GetComponent<Projectile>().Setup(healAmount, 0, 20f, 500f, healingTarget, null, true);
    }

    public void SetHealingTarget(Enemy enemy)
    {
        healingTarget = enemy;
    }
}
