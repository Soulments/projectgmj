using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem; // �巡�� ���� ������
    public static InventoryItem preCarriedItem;

    [SerializeField] InventorySlot[] inventorySlots; // �κ��丮 ���� �迭

    [SerializeField] Transform draggablesTransform; // �巡�� ������ �������� �θ�
    [SerializeField] InventoryItem itemPrefab; // ������ ������

    [SerializeField] TextMeshProUGUI addStat;   // ������ǥ��
    public int powerLevel = 0;
    public float preItem;

    //[Header("Item List")]
    //[SerializeField] ItemData[] items; // ���� ������ ������ ����Ʈ


    void Awake()
    {
        Singleton = this;
    }
    public void AddItem(ItemData item, Status status)
    {
        ItemData _item = item;
        Status _status = status;
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
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, _status, inventorySlots[i]);
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
        {
            preCarriedItem = item;
            EquipEquipment(item.activeSlot.myTag, null); 
        }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform); // �巡�� �����ϰ� ����
    }

    // ��� ������ ó��
    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        if(item != null)
        {
            powerLevel += W_cal(item.myStatus);
            preItem = item.myOne;
        }
        else
        {
            powerLevel -= W_cal(preCarriedItem.myStatus);
        }
        addStat.text = powerLevel.ToString();
    }
    public int W_cal(Status status)
    {
        double A, B, C, D, E;
        if (status.MaxHP > 0)
        {
            A = status.MaxHP * 1000.0;
            return (int)A;
        }
        else if (status.AttackDamage > 0)
        {
            B = status.AttackDamage * 2000.0;
            return (int)B;
        }
        else if (status.Defense > 0)
        {
            C = status.Defense * 1500.0;
            return (int)C;
        }
        else if (status.AttackSpeed > 0)
        {
            D = status.AttackSpeed * 1200.0;
            return (int)D;
        }
        else if (status.SkillPercent[0] > 0)
        {
            E = status.SkillPercent[0] * 1800.0;
            return (int)E;
        }
        return 0;
    }
}
