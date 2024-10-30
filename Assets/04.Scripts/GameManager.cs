using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int waveCount = 1;
    public bool portalTrigger = false;
    public bool stageEnd = false;
    public bool townStage = false;
    public bool bossStage = false;
    public bool bossAwake = false;

    public GameObject portal;
    public GameObject boss;
    public GameObject[] enemies;
    public GameObject[] waves;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        InitScene();
        if (!bossStage && !townStage)
        {
            WaveStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stageEnd)
        {
            if (!townStage)
            {
                if (bossStage)
                {
                    BossCheck();
                }
                else
                {
                    WaveChange();
                }
            }
            
            ActivatePortal();
        }
    }
    
    // 씬 초기화
    void InitScene()
    {
        if (!bossStage && !townStage) FindWaves();
        GetPortal();
    }
    
    // 웨이브 찾기
    void FindWaves()
    {
        GameObject enemyControl = GameObject.Find("EnemyControl");
        List<GameObject> childrenWaves = new List<GameObject>();

        foreach (Transform wave in enemyControl.transform)
        {
            childrenWaves.Add(wave.gameObject);
        }

        waves = childrenWaves.ToArray();

        foreach (GameObject wave in waves)
        {
            wave.SetActive(false);
        }
    }

    // 포탈 세팅
    void GetPortal()
    {
        portal = GameObject.FindWithTag("Portal");
        portal.SetActive(false);
    }

    // 웨이브 내 적 탐지용
    void GetEnemies(GameObject wave)
    {
        List<GameObject> enemiesInWave = new List<GameObject>();

        foreach (Transform enemy in wave.transform)
        {
           enemiesInWave.Add(enemy.gameObject);
        }

        enemies = enemiesInWave.ToArray();
    }

    // 웨이브 시작
    void WaveStart()
    {
        GetEnemies(waves[0]);
        waves[0].SetActive(true);
    }

    // 웨이브 변경
    void WaveChange()
    {
        if (CheckEnemy())
        {
            waves[waveCount - 1].SetActive(false);

            if (waveCount == 3)
            {
                portalTrigger = true;
                stageEnd = true;
            }
            else
            {
                GetEnemies(waves[waveCount]);
                waves[waveCount].SetActive(true);
                waveCount++;
            }

        }
    }

    // 포탈 활성화
    public void ActivatePortal()
    {
        if (portalTrigger)
        {
            portal.SetActive(true);
        }
    }

    // 웨이브 내 남은 적 체크
    bool CheckEnemy()
    {
        foreach (GameObject e in enemies)
        {
            if (e != null)
                return false;
        }

        return true;
    }

    // 보스 파괴 체크
    void BossCheck()
    {
        if (boss == null)
        {
            portalTrigger = true;
            stageEnd = true;
        }
    }
}
