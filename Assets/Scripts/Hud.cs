using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public static Hud i;

    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Slider[] abilitySliders;
    [SerializeField] TextMeshProUGUI[] abilityCds;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI attackspeedText;

    float[] cooldowns;
    float[] cooldownsRemaining;

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

    public void UpdateHudStats(int damage, float attackspeed)
    {
        damageText.text = "DMG: " + damage;
        attackspeedText.text = "AS: " + attackspeed.ToString("0.00");
    }
}
