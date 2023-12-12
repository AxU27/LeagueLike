using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;

    public int maxHpIncrease;
    public float asMultiplier;
    public float msMultiplier;
    public int damageIncrease;
    public int defenceIncrease;
    public int critIncrease;
    public int cdrIncrease;
    public float attackRangeIncrease;
    public float vampIncrease;

    public virtual void AddStats(Player player)
    {
        player.AddStats(maxHpIncrease, asMultiplier, msMultiplier, damageIncrease, defenceIncrease, critIncrease, cdrIncrease, attackRangeIncrease, vampIncrease);
    }

    public virtual void OnHit(Enemy e, Player p)
    {
        
    }

    public string GetStatsString()
    {
        string statsString = "";
        if (maxHpIncrease != 0)
            statsString += $"+{maxHpIncrease} Health\n";
        if (damageIncrease != 0)
            statsString += $"+{damageIncrease} Damage\n";
        if (asMultiplier != 0)
            statsString += $"+{(int)(asMultiplier * 100)}% Attack Speed\n";
        if (defenceIncrease != 0)
            statsString += $"+{defenceIncrease} Defence\n";
        if (vampIncrease != 0)
            statsString += $"+{(int)(vampIncrease * 100)}% Life Steal\n";
        if (critIncrease != 0)
            statsString += $"+{critIncrease}% Critical Strike Chance\n";
        if (cdrIncrease != 0)
            statsString += $"+{cdrIncrease}% Cooldown Reduction\n";
        if (msMultiplier != 0)
            statsString += $"+{(int)(msMultiplier * 100)}% Movement Speed\n";
        if (attackRangeIncrease != 0)
            statsString += $"+{attackRangeIncrease} Attack Range\n";

        return statsString;
    }

    private void OnEnable()
    {
        Player.onHit += OnHit;
    }

    private void OnDisable()
    {
        Player.onHit -= OnHit;
    }
}
