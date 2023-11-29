using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public GameObject[] lootDropPrefabs;
    public GameObject[] enemyPrefabs;

    public static GameAssets i;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(this);
        }
        else
        {
            i = this;
        }

        DontDestroyOnLoad(gameObject);
    }
}
