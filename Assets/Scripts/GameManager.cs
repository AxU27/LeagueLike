using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool roomActive;

    public static GameManager i;

    [SerializeField] List<Item> items;

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

        items = new List<Item>();
    }

    public void GetItemStats(Player player)
    {
        foreach (Item item in items)
        {
            item.AddStats(player);
        }
    }
}
