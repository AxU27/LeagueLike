using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionersCalling : Item
{
    [SerializeField] GameObject effect;

    public override void OnHit(Enemy e, Player p)
    {
        if (e.hp <= e.maxHp / 10)
        {
            Collider[] cols = Physics.OverlapSphere(e.transform.position, 3f, LayerMask.GetMask("Clickable"));

            foreach (Collider col in cols)
            {
                if (col.tag == "Enemy")
                {
                    col.GetComponent<Enemy>().TakeDamage(e.maxHp / 10);
                }
            }

            GameObject go = Instantiate(effect, e.transform.position, Quaternion.identity);
            Destroy(go, 5f);
        }
    }
}
