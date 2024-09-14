using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status : MonoBehaviour
{
    // 유닛 코드
    UnitCode unitCode { get; }
    // 이름
    public string ObjectName { get; set; }
    // 체력
    public int MaxHP { get; set; }
    public int CurrentHP { get; set; }
    // 공격력
    public float AttackDamage { get; set; }
    // 방어력
    public float Defense {  get; set; }
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
        Status status = new Status();
    }
    public Status(UnitCode unitCode, string objectName, int stageCount)
    {
        this.unitCode = unitCode;
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
                InitEnemyStatus(objectName);
                break;
            // 아이템용
            case UnitCode.Item:
                InitItemStatus(objectName);
                break;
            default:
                break;
        }
    }


    // 플레이어용
    public void InitPlayerStatus(string objectName)
    {
        Status status = null;
        SkillPercent = new float[Enum.GetValues(typeof(SkillCode)).Length];

        ObjectName = objectName;
        MaxHP = 100;
        CurrentHP = MaxHP;
        AttackDamage = 60.0f;
        Defense = 5;
        SkillPercent[(int)SkillCode.Jump] = 90;
        SkillPercent[(int)SkillCode.Upper] = 80;
        SkillPercent[(int)SkillCode.Windmill] = 45;
        MoveSpeed = 1.0f;
        JumpForce = 1.0f;
        AttackSpeed = 1.5f;
        PowerLevel = 1;
    }
    // 에너미용
    public void InitEnemyStatus(string objectName)
    {
        Status status = null;

        ObjectName = objectName;
        if (ObjectName == "근거리")
        {
            MaxHP = 100;
            CurrentHP = MaxHP;
            AttackDamage = 5;
            Defense = 5;
        }
        else if(ObjectName == "원거리")
        {
            MaxHP = 60;
            CurrentHP = MaxHP;
            AttackDamage = 6;
            Defense = 7;
        }
    }
    // 보스용
    public void InitBossStatus(string objectName)
    {
        ObjectName = objectName;

        MaxHP = 100;
        CurrentHP = MaxHP;
        AttackDamage = 5;
        Defense = 5;
    }
    // 아이템용
    public void InitItemStatus(string objectName)
    {
        Status status = null;
        SkillPercent = new float[Enum.GetValues(typeof(SkillCode)).Length];
        ObjectName = objectName;

        MaxHP = 3;
        CurrentHP = MaxHP;
        AttackDamage = 10;
        Defense = 5;
        AttackSpeed = 1.0f;
        for (int i = 0; i < SkillPercent.Length; i++)
            SkillPercent[i] = 2;
    }

    public void EditItemStatus(int stageCount)
    {

        MaxHP = 3;
        CurrentHP = MaxHP;
        AttackDamage = 10;
        Defense = 5;
        AttackSpeed = 1.0f;
        AttackSpeed = 1.0f;
        for (int i = 0; i < SkillPercent.Length; i++)
            SkillPercent[i] = 2;
    }
}
