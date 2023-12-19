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

    int tokens;

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

        AddTokens(100);
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
        Hud.i.UpdateInventory(items);

        return true;
    }

    public void RemoveItem(ItemType type)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].GetComponent<Item>().itemType == type)
            {
                Item item = items[i];
                items.RemoveAt(i);
                Destroy(item.gameObject);
                break;
            }
        }
    }

    public Item[] GetCurrentItems()
    {
        Item[] itms = new Item[items.Count];
        for (int i = 0;i < items.Count;i++)
        {
            itms[i] = items[i];
        }
        return itms;
    }

    public void GetItemStats(Player player)
    {
        foreach (Item item in items)
        {
            item.AddStats(player);
        }
    }

    public void AddTokens(int amount)
    {
        tokens += amount;
        Hud.i.UpdateTokens(tokens);
    }

    public int GetTokens()
    {
        return tokens;
    }
}
