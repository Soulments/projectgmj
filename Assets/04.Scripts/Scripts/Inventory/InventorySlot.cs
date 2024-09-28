using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// ipointerclickhandler -> ������ ���� ��ɿ� ���
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; } // �� ���Կ� �ִ� ������
    public SlotTag myTag;

    // ������ Ŭ���� �� ȣ��Ǿ�, �巡�� ���� �������� ���Կ� ��ġ
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inventory.carriedItem == null) return; // �巡�� ���� ������
            if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag) return; // ��� ������ �ƴϸ� ����
            SetItem(Inventory.carriedItem); // ���Կ� ������ ����
        }
    }

    // ���Կ� �������� �����ϰ� , ��� ������ ó��
    public void SetItem(InventoryItem item)
    {
        Inventory.carriedItem = null;

        // ���� ���Կ��� ������ �ʱ�ȭ
        item.activeSlot.myItem = null;

        // ���� ���Կ� ������ ����
        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;

        // ��� �����̸� ����
        if(myTag != SlotTag.None)
        {
            Inventory.Singleton.EquipEquipment(myTag, myItem);
        }
    }
}
