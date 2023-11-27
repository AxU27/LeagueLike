using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float asMultiplier;
    public int damageIncrease;
    public int defenceIncrease;


    public virtual void AddStats(Player player)
    {
        player.AddStats(asMultiplier, damageIncrease, defenceIncrease);
    }

    public virtual void OnHit(Enemy e)
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
