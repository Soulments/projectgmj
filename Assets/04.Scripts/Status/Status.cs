using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public string name;
    // ���ݷ�
    public int attackDamage { get; set; }
    // ü��
    public int maxHP { get; set; }
    public int currentHP { get; set; }
    // ��ų ���
    public int skillPercent { get; set; }
    // �̵��ӵ�
    public float moveSpeed { get; set; }
    // ������
    public float jumpForce { get; set; }
    // ���ݼӵ�
    public float attackSpeed { get; set; }

    public Status ()
    {

    }
}
