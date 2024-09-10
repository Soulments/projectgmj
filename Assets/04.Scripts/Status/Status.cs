using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status : MonoBehaviour
{
    // ���� �ڵ�
    UnitCode unitCode { get; }
    // �̸�
    public string objectName { get; set; }
    // ���ݷ�
    public int attackDamage { get; set; }
    // ü��
    public int maxHP { get; set; }
    public int currentHP { get; set; }
    // ��ų ���
    public int[] skillPercent { get; set; }
    // �̵��ӵ�
    public float moveSpeed { get; set; }
    // ������
    public float jumpForce { get; set; }
    // ���ݼӵ�
    public float attackSpeed { get; set; }
    // �Ŀ�����
    public float powerLevel { get; set; }

    public Status()
    {

    }
    public Status(UnitCode unitCode)
    {
        switch (unitCode)
        {
            // �÷��̾��
            case UnitCode.Player:
                InitPlayerStatus();
                break;
            // ���ʹ̿�
            case UnitCode.Enemy:
                InitEnemyStatus();
                break;
            // �����ۿ�
            case UnitCode.Item:
                break;
            default:
                break;
        }
    }


    // �÷��̾��
    public Status InitPlayerStatus()
    {
        Status status = null;
        maxHP = 100;
        currentHP = maxHP;
        moveSpeed = 1.0f;
        jumpForce = 1.0f;
        attackSpeed = 1.5f;
        return status;
    }
    // ���ʹ̿�
    public Status InitEnemyStatus()
    {
        Status status = null;
        return status;
    }
    // �����ۿ�

    // �������ͽ� ���ÿ�
    public Status InitItemStatus()
    {
        Status status = null;
        return status;
    }
}
