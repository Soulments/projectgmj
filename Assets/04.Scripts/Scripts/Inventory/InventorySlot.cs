using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// ipointerclickhandler -> 아이템 장착 기능에 사용
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; } // 이 슬롯에 있는 아이템
    public SlotTag myTag;

    // 슬롯이 클릭될 때 호출되어, 드래그 중인 아이템을 슬롯에 배치
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inventory.carriedItem == null) return; // 드래그 중인 아이템
            if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag) return; // 장비 슬롯이 아니면 리턴
            SetItem(Inventory.carriedItem); // 슬롯에 아이템 설정
        }
    }

    // 슬롯에 아이템을 설정하고 , 장비 아이템 처리
    public void SetItem(InventoryItem item)
    {
        Inventory.carriedItem = null;

        // 기존 슬롯에서 아이템 초기화
        item.activeSlot.myItem = null;

        // 현재 슬롯에 아이템 설정
        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;

        // 장비 슬롯이면 장착
        if(myTag != SlotTag.None)
        {
            Inventory.Singleton.EquipEquipment(myTag, myItem);
        }
    }
}
