using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomType type;
    public bool cleared;

    bool active;
    int enemyCount;

    [SerializeField] GameObject doors;

    EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner = GetComponentInChildren<EnemySpawner>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!cleared && other.gameObject.tag == "Player" && !active)
        {
            if (enemySpawner != null)
            {
                enemyCount = enemySpawner.SpawnEnemies();
                active = true;
                doors.SetActive(true);
            }
        }
    }

    void ReduceEnemyCount()
    {
        if (active)
        {
            if (enemyCount > 0)
            {
                enemyCount--;

                if (enemyCount <= 0)
                {
                    active = false;
                    doors.SetActive(false);
                }
            }
        }
    }

    private void OnEnable()
    {
        Enemy.onEnemyDeath += ReduceEnemyCount;
    }

    private void OnDisable()
    {
        Enemy.onEnemyDeath -= ReduceEnemyCount;
    }
}

public enum RoomType
{
    Normal,
    Spawn,
    Boss,
    Shop,
    Workshop
}
