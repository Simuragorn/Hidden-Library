using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Sprite itemImageSprite;
    [SerializeField] private Image itemImage;
    [SerializeField] private int itemIndex;

    public event EventHandler<InventoryItem> OnClicked;
    public int ItemIndex => itemIndex;

    private void Start()
    {
        itemImage.sprite = itemImageSprite;
    }

    public void Click()
    {
        OnClicked?.Invoke(this, this);
    }
}
