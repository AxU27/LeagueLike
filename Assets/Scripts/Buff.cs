using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stackText;
    [SerializeField] Image timeImage;

    [Header("Buff Config")]
    public string buffName;
    public Sprite buffIcon;
    public string buffDescription;
    public float buffDuration;
    public int maxBuffStacks;
    public float fallOffTime;

    [Header("Stats")]
    public int maxHpIncrease;
    public float asMultiplier;
    public float msMultiplier;
    public int damageIncrease;
    public int defenceIncrease;
    public int critIncrease;
    public int cdrIncrease;
    public float attackRangeIncrease;
    public float vampIncrease;

    [HideInInspector] public bool ending;
    float lifeTime;
    int stacks = 1;
    bool hovered;

    public virtual void AddStats(Player player)
    {
        player.AddStats(maxHpIncrease * stacks, asMultiplier * stacks, msMultiplier * stacks, damageIncrease * stacks, defenceIncrease * stacks, critIncrease * stacks, cdrIncrease * stacks, attackRangeIncrease * stacks, vampIncrease * stacks);
    }

    private void Start()
    {
        lifeTime = buffDuration;
        stacks = 1;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            if (stacks <= 1)
                EndBuff();

            stacks--;
            lifeTime = fallOffTime;
            GameManager.i.player.UpdateStats();
        }

        stackText.text = stacks.ToString();
        timeImage.fillAmount = lifeTime / buffDuration;
    }

    public virtual void EndBuff()
    {
        if (hovered)
            HideToolTip();
        GameManager.i.player.RemoveBuff(buffName);
        Destroy(gameObject);
    }

    public void AddStack()
    {
        lifeTime = buffDuration;
        if (stacks < maxBuffStacks)
            stacks++;
    }

    public void ShowToolTip()
    {
        ToolTip.ShowGeneralToolTip(buffDescription);
        hovered = true;
    }

    public void HideToolTip()
    {
        ToolTip.HideGeneralToolTip();
        hovered = false;
    }
}
