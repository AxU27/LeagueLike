using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] LayerMask hitLayers;
    [SerializeField] float delay;
    [SerializeField] float radius;
    int damage;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", delay);
    }

    void Explode()
    {
        Collider[] cols;

        cols = Physics.OverlapSphere(transform.position, radius, hitLayers);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == "Player")
            {
                cols[i].gameObject.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }

    public void Init(int dmg)
    {
        damage = dmg;
    }
}
