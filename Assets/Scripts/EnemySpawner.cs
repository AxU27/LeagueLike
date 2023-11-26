using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemySpawn[] enemiesToSpawn;


    public int SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            Vector3 spawnPoint = new Vector3(transform.position.x + enemiesToSpawn[i].position.x, transform.position.y, transform.position.z + enemiesToSpawn[i].position.y);
            GameObject go = Instantiate(enemiesToSpawn[i].enemy, spawnPoint, Quaternion.identity);
        }

        return enemiesToSpawn.Length;
    }
}

[System.Serializable]
class EnemySpawn
{
    public GameObject enemy;
    public Vector2 position;
}
