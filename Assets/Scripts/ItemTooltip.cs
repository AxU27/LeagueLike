using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    TextMeshProUGUI nameText;
    TextMeshProUGUI descriptionText;
    TextMeshProUGUI statsText;
    Image image;
    RectTransform backgroundRectTransform;
    RectTransform canvasRectTransform;

    public static ItemTooltip i;

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

        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        descriptionText = transform.Find("DescText").GetComponent<TextMeshProUGUI>();
        statsText = transform.Find("StatsText").GetComponent<TextMeshProUGUI>();
        image = transform.Find("ItemImage").GetComponent<Image>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        transform.localPosition = localPoint;

        Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }
        transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }

    void ShowToolTip(string itemName, string itemDesc, string itemStats, Sprite itemImage)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        nameText.text = itemName;
        descriptionText.text = itemDesc;
        statsText.text = itemStats;
        image.sprite = itemImage;
    }

    void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowItemToolTip(string itemName, string itemDesc, string itemStats, Sprite itemImage)
    {
        i.ShowToolTip(itemName, itemDesc, itemStats, itemImage);
    }

    public static void HideItemToolTip()
    {
        i.HideToolTip();
    }

}
