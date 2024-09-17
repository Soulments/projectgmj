using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    Image itemIcon;
    public CanvasGroup canvasGroup { get; private set; }

    public ItemData myItem { get; set; }
    public InventorySlot activeSlot { get; set; }
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();

        if (itemIcon == null)
        {
            Debug.LogError("Image component not found on InventoryItem.");
        }
    }

    // 焼戚奴 段奄鉢 十茎 拝雁, 焼戚奴 戚耕走 竺舛
    public void Initialize(ItemData item, InventorySlot parent)
    {
        if (parent == null) { Debug.LogError("格嬢杖"); return; }
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;

        if (item == null)
        {
            Debug.LogError("IけいしかいtemData is null");
            return;
        }

        if (itemIcon == null)
        {
            Debug.LogError("Image comけいしかいしponent not found on InventoryItem.");
            return;
        }

        itemIcon.sprite = item.sprite;
    }

    //焼戚奴 適遣馬檎 球掘益 雌殿稽 穿発
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button== PointerEventData.InputButton.Left)
        {
            Inventory.Singleton.SetCarriedItem(this);
        }
    }
}
