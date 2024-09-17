using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class Item : MonoBehaviour
{
    public ItemData itemData;   // ScriptableObject에서 데이터 가져오기

    UnitCode unitCode;
    Status status;
    GameManager gameManager;

    long tick;

    public int randomSeed;

    public GameObject[] randomObjects;

    void Awake()
    {
        StatusInit();
        RandomSeed();
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

    void RandomSeed()
    {
        tick = DateTime.Now.Ticks;
        randomSeed = (int)(tick % int.MaxValue);
        UnityEngine.Random.InitState((int)randomSeed);
    }
}
