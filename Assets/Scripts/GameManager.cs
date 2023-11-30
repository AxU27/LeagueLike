using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public bool roomActive;

    public static GameManager i;

    [SerializeField] List<Item> items;
    [SerializeField] int itemCap = 10;

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
    }

    public bool AddItem(GameObject itemPrefab)
    {
        if (items.Count >= itemCap)
        {
            return false;
        }

        GameObject go = Instantiate(itemPrefab, transform);
        items.Add(go.GetComponent<Item>());
        player.UpdateStats();

        return true;
    }

    public void GetItemStats(Player player)
    {
        foreach (Item item in items)
        {
            item.AddStats(player);
        }
    }
}