using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToSpawn;
    [SerializeField] Vector2[] spawnPositions;

    public void SpawnEnemy()
    {
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            Vector3 spawnPoint = new Vector3(transform.position.x + spawnPositions[i].x, transform.position.y, transform.position.z + spawnPositions[i].y);
            GameObject go = Instantiate(enemiesToSpawn[i], spawnPoint, Quaternion.identity);
        }
    }
}
