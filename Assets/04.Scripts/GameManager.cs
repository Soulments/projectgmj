using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int waveCount = 1;
    public bool portalTrigger;

    public GameObject[] portals;
    public GameObject[] enemies;
    public GameObject[] waves;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject w in waves)
        {
            w.SetActive(false);
        }
        portals[0].SetActive(false);
        WaveStart();
    }

    // Update is called once per frame
    void Update()
    {
        Portal();
        CheckEnemy();
    }

    void WaveStart()
    {
        waves[waveCount - 1].SetActive(true);
        return;
    }

    public void Portal()
    {
        if (portalTrigger)
        {
            waves[waveCount - 1].SetActive(false);
            waveCount++;
            WaveStart();
            portalTrigger = false;
        }
    }

    void CheckEnemy()
    {
        if (waves[waveCount - 1].transform.childCount == 0)
        {
            portals[0].SetActive(true);
        }
    }
}
