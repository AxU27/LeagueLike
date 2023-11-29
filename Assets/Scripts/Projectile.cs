using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 100f;
    int damage;
    int piercing = 0;
    float lifeDistance = 100f;
    Enemy targetEnemy;
    Player targetPlayer;

    int pierced = 0;

    private void Start()
    {
        Destroy(gameObject, lifeDistance/speed);
    }

    private void Update()
    {
        if (targetEnemy != null)
        {
            Vector3 lookVector = targetEnemy.transform.position - transform.position;
            lookVector = new Vector3(lookVector.x, 0, lookVector.z);
            if (lookVector.magnitude < 0.1f)
            {
                targetEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }

            lookVector.Normalize();
            transform.forward = lookVector;
        }
        if (targetPlayer != null)
        {
            Vector3 lookVector = targetPlayer.transform.position - transform.position;
            lookVector = new Vector3(lookVector.x, 0, lookVector.z);
            if (lookVector.magnitude < 0.1f)
            {
                targetPlayer.TakeDamage(damage);
                Destroy(gameObject);
            }
            lookVector.Normalize();
            transform.forward = lookVector;
        }
        transform.position += transform.forward * speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetEnemy != null || targetPlayer != null)
            return;

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

    public void Setup(int damage, int piercing, float speed, float lifeDistance, Enemy enemy, Player player)
    {
        this.damage = damage;
        this.piercing = piercing;
        this.speed = speed;
        this.lifeDistance = lifeDistance;
        targetEnemy = enemy;
        targetPlayer = player;
    }
}
