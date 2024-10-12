using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotTag { None, Neck, Ring }
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public SlotTag itemTag;

    /*
    [Header("If the item can be equipped")]
    public GameObject equipmentPrefab; // �������� ������ �� ����� 3d ��.....*/
}
