using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using Random = UnityEngine.Random;

public enum SlotTag { None, equip }
[CreateAssetMenu]
public class Item : MonoBehaviour
{

    public ItemData itemData;   // ScriptableObject에서 데이터 가져오기

    UnitCode unitCode;
    Status status;

    enum Stats
    {
        HP,
        AttackDamage,
        Defense,
        AttackSpeed,
        SkillPercent
    }


    GameManager gameManager;

    long tick;

    float addStats;

    public int randomSeed;

    public Status status;
    public GameObject[] randomObjects;
    public UnitCode unitCode;

    public Sprite sprite;
    public SlotTag itemTag;

    [Header("If the item can be equipped")]
    public GameObject equipmentPrefab;

    private void Start()
    {
        RandomSeed();
        StatusInit();
        RandomStatus();
    }

    void RandomSeed()
    {
        tick = DateTime.Now.Ticks;
        randomSeed = (int)(tick % int.MaxValue);
        UnityEngine.Random.InitState((int)randomSeed);
        Debug.Log(randomSeed);
    }

    // 스테이터스 초기화
    void StatusInit()
    {
        // 수정 예정
        string itemName = "아이템 이름";
        unitCode = UnitCode.Item;
        status = new Status(unitCode, itemName, 1);
    }

    // 장비 랜덤식 함수
    void RandomStatus()
    {
        int randomEnhance = Random.Range(0, 5);

        addStats = (float)(1 + (0.05 * 1));
        //addStats = (float)(1 + (0.05 * gameManager.stageCount));

        switch (randomEnhance)
        {
            case (int)Stats.HP:
                status.MaxHP = (int)(3 * addStats);
                status.CurrentHP = status.MaxHP;
                break;
            case (int)Stats.AttackDamage:
                status.AttackDamage = 10 * addStats;
                break;
            case (int)Stats.Defense:
                status.Defense = 5 * addStats;
                break;
            case (int)Stats.AttackSpeed:
                status.AttackSpeed = 1.0f * addStats;
                break;
            case (int)Stats.SkillPercent:
                for (int i = 0; i < status.SkillPercent.Length; i++)
                    status.SkillPercent[i] = 2 * addStats;
                break;
            default:
                break;
        }
    }

    // 아이템 파괴
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
