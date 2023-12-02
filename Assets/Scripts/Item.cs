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
            statsString += $"HP: {maxHpIncrease}\n";
        if (damageIncrease != 0)
            statsString += $"DMG: {damageIncrease}\n";
        if (asMultiplier != 0)
            statsString += $"AS: {(int)(asMultiplier * 100)}%\n";
        if (defenceIncrease != 0)
            statsString += $"DEF: {defenceIncrease}\n";
        if (vampIncrease != 0)
            statsString += $"VAMP: {(int)(vampIncrease * 100)}%\n";
        if (critIncrease != 0)
            statsString += $"CRIT: {critIncrease}%\n";
        if (cdrIncrease != 0)
            statsString += $"CDR: {cdrIncrease}%\n";
        if (msMultiplier != 0)
            statsString += $"MS: {(int)(msMultiplier * 100)}%\n";
        if (attackRangeIncrease != 0)
            statsString += $"AR: {attackRangeIncrease}\n";

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
