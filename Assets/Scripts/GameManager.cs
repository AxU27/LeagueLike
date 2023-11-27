using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool roomActive;

    public static GameManager i;

    [SerializeField] Item[] items;

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

    public void GetItemStats(Player player)
    {
        foreach (Item item in items)
        {
            item.AddStats(player);
        }
    }
}
