using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    GameObject[] items;
    float interactDistance = 3f;

    public void Interact(Player player)
    {
        if ((player.transform.position - transform.position).magnitude < interactDistance)
        {
            ShopUi.i.OpenShop(items);
        }
    }

    void Start()
    {
        items = new GameObject[4];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = GameAssets.i.itemPrefabs[Random.Range(0, GameAssets.i.itemPrefabs.Length)];
        }
    }
}
