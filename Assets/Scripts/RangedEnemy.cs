using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform shotPoint;

    public override void Attack()
    {
        GameObject go = Instantiate(projectile, shotPoint.position, transform.rotation);
        go.GetComponent<Projectile>().Setup(damage, 0, 20f, 500f, null, player, false);
    }
}
