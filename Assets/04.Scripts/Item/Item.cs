using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    UnitCode unitCode;
    Status status;

    public GameObject[] randomObjects;

    void Awake()
    {
        StatusInit();

    }

    // 스테이터스 초기화
    void StatusInit()
    {
        // 수정 예정
        unitCode = UnitCode.Item;
        status = new Status(unitCode);
    }

    // 장비 랜덤식 함수
    void RandomStatus()
    {
        // 뭐넣을지 고민중
        
    }
}
