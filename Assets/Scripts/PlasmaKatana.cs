using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaKatana : Item
{

    int hits;

    public override void OnHit(Enemy e, Player p)
    {
        hits++;

        if (hits > 3)
        {
            p.attackCd = 1 / (p.baseAttackSpeed * 10);
            hits = 0;
        }
    }
}
