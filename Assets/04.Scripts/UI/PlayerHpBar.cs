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
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        health = player.status.MaxHP;

        healthSlider.maxValue = player.status.MaxHP;
        //yellHealthSlider.maxValue = player.status.MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, health, Time.deltaTime * 5f);
        }
        //healthSlider.value = Mathf.Lerp(healthSlider.value, health, Time.deltaTime * 5f);

        health = player.status.CurrentHP;
    }
}
