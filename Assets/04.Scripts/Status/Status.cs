using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status : MonoBehaviour
{
    // ���� �ڵ�
    UnitCode unitCode { get; }
    // �̸�
    public string ObjectName { get; set; }
    // ü��
    public int MaxHP { get; set; }
    public int CurrentHP { get; set; }
    // ���ݷ�
    public float AttackDamage { get; set; }
    // ����
    public float Defense {  get; set; }
    // ��ų ���
    public float[] SkillPercent { get; set; }
    // �̵��ӵ�
    public float MoveSpeed { get; set; }
    // ������
    public float JumpForce { get; set; }
    // ���ݼӵ�
    public float AttackSpeed { get; set; }
    // �Ŀ�����
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
            // �÷��̾��
            case UnitCode.Player:
                InitPlayerStatus(objectName);
                break;
            // ���ʹ̿�
            case UnitCode.Enemy:
                InitEnemyStatus(objectName);
                break;
            // ������
            case UnitCode.Boss:
                InitEnemyStatus(objectName);
                break;
            // �����ۿ�
            case UnitCode.Item:
                InitItemStatus(objectName);
                break;
            default:
                break;
        }
    }


    // �÷��̾��
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
    // ���ʹ̿�
    public void InitEnemyStatus(string objectName)
    {
        Status status = null;

        ObjectName = objectName;
        if (ObjectName == "�ٰŸ�")
        {
            MaxHP = 100;
            CurrentHP = MaxHP;
            AttackDamage = 5;
            Defense = 5;
        }
        else if(ObjectName == "���Ÿ�")
        {
            MaxHP = 60;
            CurrentHP = MaxHP;
            AttackDamage = 6;
            Defense = 7;
        }
    }
    // ������
    public void InitBossStatus(string objectName)
    {
        ObjectName = objectName;

        MaxHP = 100;
        CurrentHP = MaxHP;
        AttackDamage = 5;
        Defense = 5;
    }
    // �����ۿ�
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