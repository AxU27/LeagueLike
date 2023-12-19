using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUi : MonoBehaviour
{
    public static CraftingUi i;

    [SerializeField] GameObject craftingPanel;
    [SerializeField] Image craftingImage;
    [SerializeField] TextMeshProUGUI itemsNeededText;
    [SerializeField] Image[] itemImages;

    GameObject[] itemReferances;
    Item[] items;
    int selectedItem;

    GameManager gameManager;
    GameAssets gameAssets;

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

    private void Start()
    {
        gameManager = GameManager.i;
        gameAssets = GameAssets.i;

        itemReferances = gameAssets.t3ItemPrefabs;
        items = new Item[itemReferances.Length];

        for (int i = 0; i < itemReferances.Length; i++)
        {
            items[i] = itemReferances[i].GetComponent<Item>();
            itemImages[i].sprite = items[i].itemIcon;
        }

        SelectItem(0);

        ToggleCrafting();
    }

    public void SelectItem(int i)
    {
        selectedItem = i;

        craftingImage.sprite = items[selectedItem].itemIcon;

        string t = "Items needed to craft:";

        for (int j = 0; j < items[selectedItem].itemsNeededToCraft.Length; j++) 
        {
            t += "\n" + items[selectedItem].itemsNeededToCraft[j].ToString();
        }
        itemsNeededText.text = t;
    }

    public void CraftItem()
    {
        Item[] its = gameManager.GetCurrentItems();

        int itemsNeeded = items[selectedItem].itemsNeededToCraft.Length;

        for (int i = 0; i < items[selectedItem].itemsNeededToCraft.Length; i++)
        {
            for (int j = 0; j < its.Length; j++) 
            { 
                if (its[j].itemType == items[selectedItem].itemsNeededToCraft[i])
                {
                    itemsNeeded--;
                    break;
                }
            }
        }

        if (itemsNeeded > 0)
            return;

        for (int i = 0;i < items[selectedItem].itemsNeededToCraft.Length; i++)
        {
            gameManager.RemoveItem(items[selectedItem].itemsNeededToCraft[i]);
        }

        gameManager.AddItem(itemReferances[selectedItem]);
    }

    public void ToggleCrafting()
    {
        craftingPanel.SetActive(!craftingPanel.activeSelf);
        HideToolTip();
    }

    public void ShowTooltip(int i)
    {
        ItemTooltip.ShowItemToolTip(items[i].itemName, items[i].itemDescription, items[i].GetStatsString(), items[i].itemIcon);
    }

    public void HideToolTip()
    {
        ItemTooltip.HideItemToolTip();
    }

    public void ShowCurrentTooltip()
    {
        int i = selectedItem;
        ItemTooltip.ShowItemToolTip(items[i].itemName, items[i].itemDescription, items[i].GetStatsString(), items[i].itemIcon);
    }
}
