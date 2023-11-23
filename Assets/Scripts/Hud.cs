using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public static Hud i;

    [SerializeField] Slider ability1Slider;
    [SerializeField] Slider ability2Slider;
    [SerializeField] Slider ability3Slider;
    [SerializeField] Slider ability4Slider;

    [SerializeField] TextMeshProUGUI ability1Cd;
    [SerializeField] TextMeshProUGUI ability2Cd;
    [SerializeField] TextMeshProUGUI ability3Cd;
    [SerializeField] TextMeshProUGUI ability4Cd;

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
    }


    public void SetCooldown(int abilityNumber, float cd, float cdLeft)
    {

    }
}
