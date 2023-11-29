using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] GameObject projectile;

    public override void Attack()
    {
        GameObject go = Instantiate(projectile, transform.position + Vector3.up, transform.rotation);
        go.GetComponent<Projectile>().Setup(damage, 0, 20f, 500f, null, player);
    }
}
