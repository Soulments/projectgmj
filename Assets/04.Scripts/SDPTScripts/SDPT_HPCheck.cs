using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDPT_HPCheck : MonoBehaviour
{

    public PlayerController player;

    public GameObject OnOffObject;

    public float waitSecond = 0;

        
    void Start()
    {
        if (player == null) player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if(player.status.CurrentHP <= 0 ) 
        {
            StartCoroutine(WaitAndGo(waitSecond));
        }
    }

    IEnumerator WaitAndGo(float second)
    {
        yield return new WaitForSeconds(second);
        OnOffObject.SetActive(true);
    }
}
