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
    public InventorySlot activeSlot { get; set; }

    public Item itemState { get; set; }

    UIController uiController;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();

        if (itemIcon == null)
        {
            Debug.LogError("Image component not found on InventoryItem.");
        }

        uiController = FindObjectOfType<UIController>(); // UIController를 찾아 할당
        if (uiController == null)
        {
            Debug.LogError("UIController not found in the scene.");
        }
    }

    // 아이템 초기화 슬롯 할당, 아이템 이미지 설정
    public void Initialize(ItemData item, InventorySlot parent)
    {
        if (parent == null) { Debug.LogError("너어얼"); return; }
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;

        if (item == null)
        {
            Debug.LogError("IㅁㄴㅇㄻㄴtemData is null");
            return;
        }

        if (itemIcon == null)
        {
            Debug.LogError("Image comㅁㄴㅇㄻㄴㅇponent not found on InventoryItem.");
            return;
        }

        itemIcon.sprite = item.sprite;
    }

    //아이템 클릭하면 드래그 상태로 전환
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canvasGroup.alpha == 0) return;

        if (eventData.button== PointerEventData.InputButton.Left)
        {
            Inventory.Singleton.SetCarriedItem(this);
        }
    }

    // 아이템 위에 마우스 올리면 정보 팝업
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canvasGroup.alpha == 0) return;

        // 임시 -------uiController.OpenMessagePanel(myItem.itemName);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (canvasGroup.alpha == 0) return;

        uiController.CloseMessagePanel();
    }
}
