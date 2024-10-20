using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    public Slider healthSlider;
    public Slider yellHealthSlider;
    
    public Enemy enemy;

    public float health;
    public float lerpSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        health = enemy.status.MaxHP;

        healthSlider.maxValue = enemy.status.MaxHP;
        yellHealthSlider.maxValue = enemy.status.MaxHP;

        Debug.Log("health: "+health);
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }
        if(healthSlider.value != yellHealthSlider.value)
        {
            yellHealthSlider.value = Mathf.Lerp(yellHealthSlider.value, health, lerpSpeed);
        }

        health = enemy.status.CurrentHP;
       // Debug.Log("currentHP"+ enemy.status.CurrentHP);
    }
}
