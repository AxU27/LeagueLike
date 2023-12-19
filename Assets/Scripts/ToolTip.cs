using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public static ToolTip i;

    TextMeshProUGUI descriptionText;
    RectTransform backgroundRectTransform;
    RectTransform canvasRectTransform;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
        }
        else
        {
            i = this;
        }


        descriptionText = transform.Find("DescText").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

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

    void ShowToolTip(string text)
    {
        gameObject.SetActive(true);
        descriptionText.text = text;

        Vector2 bgSize = new Vector2(descriptionText.preferredWidth, descriptionText.preferredHeight);
        backgroundRectTransform.sizeDelta = bgSize;

        backgroundRectTransform.localPosition = backgroundRectTransform.sizeDelta / 2;
    }

    void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowGeneralToolTip(string text)
    {
        i.ShowToolTip(text);
    }

    public static void HideGeneralToolTip()
    {
        i.HideToolTip();
    }
}
