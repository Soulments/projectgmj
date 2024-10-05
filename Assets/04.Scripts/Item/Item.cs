using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour
{
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
    public UnitCode unitCode;

    public Sprite sprite;
    public SlotTag itemTag;


    void Awake()
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
    }

    // �������ͽ� �ʱ�ȭ
    void StatusInit()
    {
        // ���� ����
        string itemName = "������ �̸�";
        unitCode = UnitCode.Item;
        // status = new Status(unitCode, itemName, gameManager.stageCount);
        status = new Status(unitCode, itemName, 1);
    }

    // ��� ������ �Լ�
    void RandomStatus()
    {
        int randomEnhance = Random.Range(0, 5);

        Debug.Log(randomEnhance);
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

    // ������ �ı�
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
