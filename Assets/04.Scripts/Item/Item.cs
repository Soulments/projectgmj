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

    // �������ͽ� �ʱ�ȭ
    void StatusInit()
    {
        // ���� ����
        unitCode = UnitCode.Item;
        status = new Status(unitCode);
    }

    // ��� ������ �Լ�
    void RandomStatus()
    {
        // �������� �����
        
    }
}
