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

    // �������ͽ� �ʱ�ȭ
    void StatusInit()
    {
        // ���� ����
        string itemName = "������ �̸�";
        unitCode = UnitCode.Item;
        status = new Status(unitCode, itemName, gameManager.stageCount);
    }

    // ��� ������ �Լ�
    void RandomStatus()
    {
        // �������� �����
        
    }
}
