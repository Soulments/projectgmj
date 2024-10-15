using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem; // 드래그 중인 아이템
    public static InventoryItem preCarriedItem;

    [SerializeField] InventorySlot[] inventorySlots; // 인벤토리 슬롯 배열

    [SerializeField] Transform draggablesTransform; // 드래그 가능한 아이템의 부모
    [SerializeField] InventoryItem itemPrefab; // 아이템 프리팹

    [SerializeField] TextMeshProUGUI addStat;   // 전투력표시
    public float powerLevel = 0;
    public float preItem;

    //[Header("Item List")]
    //[SerializeField] ItemData[] items; // 생성 가능한 아이템 리스트


    void Awake()
    {
        Singleton = this;
    }
    public void AddItem(ItemData item, Status status)
    {
        ItemData _item = item;
        Status _status = status;
        //아이템 랜덤 생성 코드
        /*if (_item == null)
        {
            int random = Random.Range(0, items.Length);
            _item = items[random];
        }*/

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // 슬롯 비었는지 체크
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

        // 드래그 중인 아이템을 마우스 위치로 이동
        carriedItem.transform.position = Input.mousePosition;
    }

    // 아이템 드래그
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
        item.transform.SetParent(draggablesTransform); // 드래그 가능하게 설정
    }

    // 장비 아이템 처리
    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        if(item != null)
        {
            powerLevel += item.myOne;
            preItem = item.myOne;
        }
        else
        {
            powerLevel -= preCarriedItem.myOne;
        }
        addStat.text = powerLevel.ToString();
    }
}
