using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectItem
{
    void OnPickup();
    ItemData ClickItem();
}
public class ObjectItem : MonoBehaviour, IObjectItem
{
    [Header("������")]
    public ItemData item;
    /*[Header("������ �̹���")]
    public SpriteRenderer itemImage;
    void Start()
    {
        itemImage.sprite = itemData.sprite;
    }*/

/*    public string Name
    {
        get { return itemData.itemName; }
    }
    public Sprite Image
    {
        get { return itemData.sprite; }
    }*/
    public void OnPickup()
    {
        // TODO: Add logic what happens when item is picked up by player
        gameObject.SetActive(false);
    }
    public  ItemData ClickItem()
    {
        return this.item;
    }
}
