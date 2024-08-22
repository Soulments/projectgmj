using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public Slider hpBar;
    public float maxHp = 1000f;
    public float currentHp = 1000f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, currentHp / maxHp, Time.deltaTime * 5f);
    }
}
