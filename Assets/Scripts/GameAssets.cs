using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public GameObject[] t1ItemPrefabs;
    public GameObject[] t2ItemPrefabs;
    public GameObject[] t3ItemPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] buffPrefabs;

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
