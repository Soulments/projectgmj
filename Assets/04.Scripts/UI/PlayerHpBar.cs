using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public Slider healthSlider;

    public PlayerController player;

    public float health;

    void Start()
    {
        health = player.status.MaxHP;

        healthSlider.maxValue = player.status.MaxHP;
        //yellHealthSlider.maxValue = enemy.status.MaxHP;

        Debug.Log("health: " + health);
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, health, Time.deltaTime * 5f);
        }
        //healthSlider.value = Mathf.Lerp(healthSlider.value, health, Time.deltaTime * 5f);
    }
}
