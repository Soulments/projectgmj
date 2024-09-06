using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public string name;
    // 공격력
    public int attackDamage { get; set; }
    // 체력
    public int maxHP { get; set; }
    public int currentHP { get; set; }
    // 스킬 계수
    public int skillPercent { get; set; }
    // 이동속도
    public float moveSpeed { get; set; }
    // 점프력
    public float jumpForce { get; set; }
    // 공격속도
    public float attackSpeed { get; set; }

    public Status ()
    {

    }
}
