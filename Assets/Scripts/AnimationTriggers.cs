using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    [SerializeField] Player player;
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
    }

    void Ability1()
    {
        if (player != null)
        {
            player.Ability1();
        }
    }
}
