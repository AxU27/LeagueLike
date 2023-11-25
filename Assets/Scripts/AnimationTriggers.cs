using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Enemy enemy;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        if (player != null)
        {
            player.Attack();
        }

        if (enemy != null)
        {
            enemy.Attack();
        }
    }

    void Ability1()
    {
        if (player != null)
        {
            player.Ability1();
        }
    }
}
