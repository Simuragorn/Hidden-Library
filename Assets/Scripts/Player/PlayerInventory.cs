using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int inventoryItemsCellsCount = 12;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private InventoryItem defaultInventoryItemPrefab;
    [SerializeField] private GameObject inventoryItemsContainer;
    [SerializeField] private Image inventoryItemLargeImage;
    [SerializeField] private TextMeshProUGUI inventoryItemNameText;
    [SerializeField] private TextMeshProUGUI inventoryItemDescriptionText;
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private void Start()
    {
        inventoryUI.SetActive(false);
        ReloadItems();
        DisplayItemInfo(defaultInventoryItemPrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.activeSelf);
        }
    }

    private void ReloadItems()
    {
        foreach (Transform child in inventoryItemsContainer.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < inventoryItemsCellsCount; i++)
        {
            InventoryItem itemPrefab = defaultInventoryItemPrefab;
            if (inventoryItems.Count > i)
            {
                itemPrefab = inventoryItems[i];
            }

            var addedItem = Instantiate(itemPrefab, inventoryItemsContainer.transform);
            addedItem.OnClicked += AddedItem_OnClicked;
        }
    }

    private void AddedItem_OnClicked(object sender, InventoryItem item)
    {
        DisplayItemInfo(item);
    }

    public void AddItem(InventoryItem newItem)
    {
        inventoryItems.Add(newItem);
        ReloadItems();
    }

    public void RemoveItem(InventoryItem item)
    {
        InventoryItem itemForRemoving = inventoryItems.First(i => i.ItemIndex == item.ItemIndex);
        if (itemForRemoving != null)
        {
            inventoryItems.Remove(itemForRemoving);
        }
        ReloadItems();
    }

    public void DisplayItemInfo(InventoryItem item)
    {
        inventoryItemLargeImage.sprite = item.ItemLargeSprite;
        inventoryItemNameText.text = item.ItemName;
        inventoryItemDescriptionText.text = item.ItemDescription;
    }
}
