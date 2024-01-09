using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionersCalling : Item
{
    [SerializeField] GameObject effect;

    public override void OnHit(Enemy e, Player p)
    {
        if (e.hp <= e.maxHp * 0.15f)
        {
            Collider[] cols = Physics.OverlapSphere(e.transform.position, 3f, LayerMask.GetMask("Clickable"));

            foreach (Collider col in cols)
            {
                if (col.tag == "Enemy")
                {
                    col.GetComponent<Enemy>().TakeDamage((int)(e.maxHp * 0.15f));
                }
            }

            GameObject go = Instantiate(effect, e.transform.position, Quaternion.identity);
            Destroy(go, 5f);
        }
    }
}
