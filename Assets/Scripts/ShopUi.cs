using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUi : MonoBehaviour
{
    [SerializeField] GameObject shopWindow;
    [SerializeField] Image[] itemImages;

    GameObject[] itemReferances;
    Item[] items;
    int[] itemPrices;

    public void OpenShop(GameObject[] itemRefs, int[] prices)
    {
        shopWindow.SetActive(true);

        itemReferances = itemRefs;
        items = new Item[itemRefs.Length];
        itemPrices = prices;

        for (int i = 0; i < itemRefs.Length; i++)
        {
            items[i] = itemRefs[i].GetComponent<Item>();
            itemImages[i].sprite = items[i].itemIcon;
        }
    }

    public void CloseShop()
    {
        shopWindow.SetActive(false);
    }

    public void BuyItem(int item)
    {
        if (itemPrices[item] <= GameManager.i.GetTokens())
            GameManager.i.AddItem(itemReferances[item]);
    }

    public static ShopUi i;

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
}
