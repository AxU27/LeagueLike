using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 100f;
    int damage;
    int piercing = 0;
    float lifeDistance = 100f;

    int pierced = 0;

    private void Start()
    {
        Destroy(gameObject, lifeDistance/speed);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            pierced++;

            if (piercing < pierced)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup(int damage, int piercing, float speed, float lifeDistance)
    {
        this.damage = damage;
        this.piercing = piercing;
        this.speed = speed;
        this.lifeDistance = lifeDistance;
    }
}
