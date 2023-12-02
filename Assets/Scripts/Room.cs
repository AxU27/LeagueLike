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
    GameManager gameManager;

    private void Start()
    {
        enemySpawner = GetComponentInChildren<EnemySpawner>();
        gameManager = GameManager.i;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!cleared && other.gameObject.tag == "Player" && !active && !gameManager.roomActive)
        {
            if (enemySpawner != null)
            {
                enemyCount = enemySpawner.SpawnEnemies();
                active = true;
                gameManager.roomActive = true;
                doors.SetActive(true);
            }
            else
            {
                cleared = true;
                // Minimap show cleared
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
                    gameManager.roomActive = false;
                    doors.SetActive(false);
                    cleared = true;
                    gameManager.AddTokens(Random.Range(0, 4));
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
    Workshop
}
