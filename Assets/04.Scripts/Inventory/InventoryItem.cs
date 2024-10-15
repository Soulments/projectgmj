using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Image itemIcon;
    public CanvasGroup canvasGroup { get; private set; }

    public ItemData myItem { get; set; }
    public Status myStatus { get; set; }
    public float myOne { get; set; }
    public InventorySlot activeSlot { get; set; }

    //public Item itemState { get; set; }

    UIController uiController;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();

        if (itemIcon == null)
        {
            Debug.LogError("Image component not found on InventoryItem.");
        }

        uiController = FindObjectOfType<UIController>(); // UIController�� ã�� �Ҵ�
        if (uiController == null)
        {
            Debug.LogError("UIController not found in the scene.");
        }
    }

    // ������ �ʱ�ȭ ���� �Ҵ�, ������ �̹��� ����
    public void Initialize(ItemData item, Status status, InventorySlot parent)
    {
        if (parent == null) { Debug.LogError("�ʾ��"); return; }
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        myStatus = status;

        if (status.MaxHP > 0)
        {
            myOne = status.MaxHP;
        }
        else if (status.AttackDamage > 0)
        {
            myOne = status.AttackDamage;
        }
        else if (status.Defense > 0)
        {
            myOne = status.Defense;
        }
        else if (status.AttackSpeed > 0)
        {
            myOne = status.AttackSpeed;
        }
        else if (status.SkillPercent[0] > 0)
        {
            myOne = status.SkillPercent[0];
        }

        if (item == null)
        {
            Debug.LogError("I����������temData is null");
            return;
        }

        if (itemIcon == null)
        {
            Debug.LogError("Image com������������ponent not found on InventoryItem.");
            return;
        }

        itemIcon.sprite = item.sprite;
    }

    //������ Ŭ���ϸ� �巡�� ���·� ��ȯ
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canvasGroup.alpha == 0) return;

        if (eventData.button== PointerEventData.InputButton.Left)
        {
            Inventory.Singleton.SetCarriedItem(this);
        }
    }

    // ������ ���� ���콺 �ø��� ���� �˾�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canvasGroup.alpha == 0) return;

        uiController.OpenMessagePanel(myItem.itemName, myStatus);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (canvasGroup.alpha == 0) return;

        uiController.CloseMessagePanel();
    }
}
