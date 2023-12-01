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


    public virtual void AddStats(Player player)
    {
        player.AddStats(maxHpIncrease, asMultiplier, msMultiplier, damageIncrease, defenceIncrease, critIncrease, cdrIncrease, attackRangeIncrease);
    }

    public virtual void OnHit(Enemy e, Player p)
    {
        
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
