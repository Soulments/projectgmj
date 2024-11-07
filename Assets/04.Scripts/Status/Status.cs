using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status
{
    // 유닛 코드
    private UnitCode UnitCode { get; }
    // 이름
    public string ObjectName { get; set; }
    // 체력
    public int MaxHP { get; set; }
    public int CurrentHP { get; set; }
    // 공격력
    public float AttackDamage { get; set; }
    // 방어력
    public float Defense { get; set; }
    // 스킬 계수
    public float[] SkillPercent { get; set; }
    // 이동속도
    public float MoveSpeed { get; set; }
    // 점프력
    public float JumpForce { get; set; }
    // 공격속도
    public float AttackSpeed { get; set; }
    // 파워레벨
    public int PowerLevel { get; set; }

    public Status()
    {

    }
    public Status(UnitCode unitCode, string objectName, int stageCount)
    {
        this.UnitCode = unitCode;
        PowerLevel = stageCount;
        switch (unitCode)
        {
            // 플레이어용
            case UnitCode.Player:
                InitPlayerStatus(objectName);
                break;
            // 에너미용
            case UnitCode.Enemy:
                InitEnemyStatus(objectName);
                break;
            // 보스용
            case UnitCode.Boss:
                InitBossStatus(objectName);
                break;
            // 아이템용
            case UnitCode.Item:
                InitItemStatus(objectName, stageCount);
                break;
            default:
                break;
        }
    }

    ~Status()
    {

    }


    // 플레이어용
    public void InitPlayerStatus(string objectName)
    {
        SkillPercent = new float[Enum.GetValues(typeof(SkillCode)).Length];

        ObjectName = objectName;
        MaxHP = 100;
        CurrentHP = MaxHP;
        AttackDamage = 60.0f;
        Defense = 5;
        SkillPercent[(int)SkillCode.Jump] = 60;
        SkillPercent[(int)SkillCode.SwordGust] = 80;
        SkillPercent[(int)SkillCode.Upper] = 60;
        SkillPercent[(int)SkillCode.Windmill] = 45;
        MoveSpeed = 1.0f;
        JumpForce = 1.0f;
        AttackSpeed = 1.5f;
        PowerLevel = 1;
    }
    // 에너미용
    public void InitEnemyStatus(string objectName)
    {
        ObjectName = objectName;
        if (ObjectName == "근거리")
        {
            MaxHP = 400;
            CurrentHP = MaxHP;
            AttackDamage = 5;
            Defense = 5;
        }
        else if (ObjectName == "원거리")
        {
            MaxHP = 300;
            CurrentHP = MaxHP;
            AttackDamage = 6;
            Defense = 7;
        }
    }
    // 보스용
    public void InitBossStatus(string objectName)
    {
        ObjectName = objectName;

        MaxHP = 3000;
        CurrentHP = MaxHP;
        AttackDamage = 7;
        Defense = 5;
    }
    // 아이템용
    public void InitItemStatus(string objectName, int stageCount)
    {
        SkillPercent = new float[Enum.GetValues(typeof(SkillCode)).Length];
        ObjectName = objectName;

        MaxHP = 0;
        CurrentHP = MaxHP;
        AttackDamage = 0;
        Defense = 0;
        AttackSpeed = 0;
        for (int i = 0; i < SkillPercent.Length; i++)
            SkillPercent[i] = 0;
    }
}