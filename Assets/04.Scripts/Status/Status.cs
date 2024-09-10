using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status : MonoBehaviour
{
    // 유닛 코드
    UnitCode unitCode { get; }
    // 이름
    public string objectName { get; set; }
    // 공격력
    public int attackDamage { get; set; }
    // 체력
    public int maxHP { get; set; }
    public int currentHP { get; set; }
    // 스킬 계수
    public int[] skillPercent { get; set; }
    // 이동속도
    public float moveSpeed { get; set; }
    // 점프력
    public float jumpForce { get; set; }
    // 공격속도
    public float attackSpeed { get; set; }
    // 파워레벨
    public float powerLevel { get; set; }

    public Status()
    {

    }
    public Status(UnitCode unitCode)
    {
        switch (unitCode)
        {
            // 플레이어용
            case UnitCode.Player:
                InitPlayerStatus();
                break;
            // 에너미용
            case UnitCode.Enemy:
                InitEnemyStatus();
                break;
            // 아이템용
            case UnitCode.Item:
                break;
            default:
                break;
        }
    }


    // 플레이어용
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
    // 에너미용
    public Status InitEnemyStatus()
    {
        Status status = null;
        return status;
    }
    // 아이템용

    // 스테이터스 세팅용
    public Status InitItemStatus()
    {
        Status status = null;
        return status;
    }
}
