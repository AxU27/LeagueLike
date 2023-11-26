using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomType type;
    public bool cleared;

    EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner = GetComponentInChildren<EnemySpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cleared && other.gameObject.tag == "Player")
        {
            // Close doors

            if (enemySpawner != null)
            {
                enemySpawner.SpawnEnemies();
            }
        }
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
