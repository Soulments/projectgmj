using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem; // �巡�� ���� ������

    [SerializeField] InventorySlot[] inventorySlots; // �κ��丮 ���� �迭

    [SerializeField] Transform draggablesTransform; // �巡�� ������ �������� �θ�
    [SerializeField] InventoryItem itemPrefab; // ������ ������

    //[Header("Item List")]
    //[SerializeField] ItemData[] items; // ���� ������ ������ ����Ʈ


    void Awake()
    {
        Singleton = this;
    }
    public void AddItem(ItemData item)
    {
        ItemData _item = item;
        //������ ���� ���� �ڵ�
        /*if (_item == null)
        {
            int random = Random.Range(0, items.Length);
            _item = items[random];
        }*/

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // ���� ������� üũ
            if (inventorySlots[i].myItem == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                break;
            }
        }
    }
    void Update()
    {
        if (carriedItem == null) return;

        // �巡�� ���� �������� ���콺 ��ġ�� �̵�
        carriedItem.transform.position = Input.mousePosition;
    }

    // ������ �巡��
    public void SetCarriedItem(InventoryItem item)
    {
        if(carriedItem != null)
        {
            if (item.activeSlot.myTag != SlotTag.None) return;
            item.activeSlot.SetItem(carriedItem);
        }
        if (item.activeSlot.myTag != SlotTag.None)
        { EquipEquipment(item.activeSlot.myTag, null); }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform); // �巡�� �����ϰ� ����
    }

    // ��� ������ ó��
    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {

    }
}
