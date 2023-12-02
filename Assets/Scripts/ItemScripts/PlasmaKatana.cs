using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaKatana : Item
{
    int hits;

    public override void OnHit(Enemy e, Player p)
    {
        e.TakeDamage(10);
    }
}
