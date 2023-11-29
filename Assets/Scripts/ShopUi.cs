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

    public void OpenShop(GameObject[] itemRefs)
    {
        shopWindow.SetActive(true);

        itemReferances = itemRefs;
        items = new Item[itemRefs.Length];

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
