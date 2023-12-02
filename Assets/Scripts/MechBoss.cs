using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBoss : Enemy
{
    float timer1;

    public override void Behavior()
    {
        if (dead || player == null)
            return;

        if (timer1 > 0f)
            timer1 -= Time.deltaTime;
        else
            timer1 = 20f;

        if (timer1 < 7f)
        {
            defence = 20;
            // special attack
        }
        else
        {
            defence = 400;
            base.Behavior();
        }
    }

    public override void Die()
    {
        animator.SetTrigger("die");
        //Play death animation
        //Spawn particles
        //Spawn loot
        Destroy(gameObject, 1.3f);
    }
}
