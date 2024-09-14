using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class Item : MonoBehaviour
{
    UnitCode unitCode;
    Status status;
    GameManager gameManager;

    public GameObject[] randomObjects;

    void Awake()
    {
        StatusInit();

    }

    // 스테이터스 초기화
    void StatusInit()
    {
        // 수정 예정
        string itemName = "아이템 이름";
        unitCode = UnitCode.Item;
        status = new Status(unitCode, itemName, gameManager.stageCount);
    }

    // 장비 랜덤식 함수
    void RandomStatus()
    {
        // 뭐넣을지 고민중
        
    }
}
