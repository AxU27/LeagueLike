using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public static Hud i;

    [Header("Health")]
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;

    [Header("Abilities")]
    [SerializeField] Slider[] abilitySliders;
    [SerializeField] TextMeshProUGUI[] abilityCds;

    [Header("Stats")]
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI attackspeedText;
    [SerializeField] TextMeshProUGUI defenceText;
    [SerializeField] TextMeshProUGUI critText;
    [SerializeField] TextMeshProUGUI movespeedText;
    [SerializeField] TextMeshProUGUI cdrText;

    [SerializeField] TextMeshProUGUI tokenText;

    [Header("Items")]
    [SerializeField] Image[] itemImages;


    float[] cooldowns;
    float[] cooldownsRemaining;
    List<Item> items;

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

        cooldowns = new float[abilitySliders.Length];
        cooldownsRemaining = new float[abilitySliders.Length];

        for (int i = 0; i < abilityCds.Length; i++)
        {
            if (abilityCds[i] != null)
            {
                abilityCds[i].enabled = false;
            }
        }

        items = new List<Item>();
    }


    private void Update()
    {
        for (int i = 0; i < cooldowns.Length; i++)
        {
            if (cooldownsRemaining[i] > 0f)
            {
                UpdateCooldown(i);
            }
        }
    }


    public void SetCooldown(int abilityNumber, float cd)
    {
        cooldowns[abilityNumber] = cd;
        cooldownsRemaining[abilityNumber] = cd;
        abilityCds[abilityNumber].enabled = true;
    }

    void UpdateCooldown(int abilityNumber)
    {
        cooldownsRemaining[abilityNumber] -= Time.deltaTime;
        abilitySliders[abilityNumber].value = cooldownsRemaining[abilityNumber] / cooldowns[abilityNumber];
        abilityCds[abilityNumber].text = Mathf.RoundToInt(cooldownsRemaining[abilityNumber]) + "s";

        if (cooldownsRemaining[abilityNumber] <= 0f)
        {
            abilityCds[abilityNumber].enabled = false;
        }
    }

    public void UpdateHealthBar(int maxHp, int currentHp)
    {
        hpSlider.value = (float)currentHp / maxHp;
        hpText.text = currentHp + "/" + maxHp;
    }

    public void UpdateHudStats(int damage, float attackspeed, int crit, int defence, float movespeed, int cdr)
    {
        damageText.text = damage.ToString();
        attackspeedText.text = attackspeed.ToString("0.00");
        critText.text = crit + "%";
        defenceText.text = defence.ToString();
        movespeedText.text = ((int)(movespeed * 100)).ToString();
        cdrText.text = cdr + "%";
    }

    public void UpdateInventory(List<Item> it)
    {
        items = it;

        for (int i = 0; i < items.Count; i++)
        {
            itemImages[i].sprite = items[i].itemIcon;
        }

        for (int i = items.Count; i < 10; i++) 
        {
            itemImages[i].sprite = null;
        }
    }

    public void ShowTooltip(int i)
    {
        if (items.Count <= i)
            return;
        if (items[i] != null)
            ItemTooltip.ShowItemToolTip(items[i].itemName, items[i].itemDescription, items[i].GetStatsString(), items[i].itemIcon);
    }

    public void HideToolTip()
    {
        ItemTooltip.HideItemToolTip();
    }

    public void UpdateTokens(int tokens)
    {
        tokenText.text = tokens.ToString();
    }
}
