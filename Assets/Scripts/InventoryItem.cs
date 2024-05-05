using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Sprite itemImageSprite;
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite itemLargeSprite;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private int itemIndex;

    public event EventHandler<InventoryItem> OnClicked;
    public int ItemIndex => itemIndex;
    public Sprite ItemLargeSprite=>itemLargeSprite;
    public string ItemName => itemName;
    public string ItemDescription => itemDescription;

    private void Start()
    {
        itemImage.sprite = itemImageSprite;
    }

    public void Click()
    {
        OnClicked?.Invoke(this, this);
    }
}
