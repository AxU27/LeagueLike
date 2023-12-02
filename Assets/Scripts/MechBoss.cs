using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBoss : Enemy
{
    public override void Die()
    {
        animator.SetTrigger("die");
        //Play death animation
        //Spawn particles
        //Spawn loot
        Destroy(gameObject, 1.3f);
    }
}
