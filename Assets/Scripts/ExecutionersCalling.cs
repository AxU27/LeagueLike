using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionersCalling : Item
{
    private void Start()
    {
        asMultiplier = 0f;
        damageIncrease = 20;
        defenceIncrease = 0;
    }

    public override void OnHit(Enemy e)
    {
        if (e.hp <= e.maxHp / 10)
        {
            Collider[] cols = Physics.OverlapSphere(e.transform.position, 2f, LayerMask.GetMask("Clickable"));

            foreach (Collider col in cols)
            {
                if (col.tag == "Enemy")
                {
                    col.GetComponent<Enemy>().TakeDamage(e.maxHp / 10);
                }
            }
        }
    }
}
