using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inventoryItemsContainer;
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private void Start()
    {
        inventoryUI.SetActive(false);
        ReloadItems();
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
        foreach (var itemPrefab in inventoryItems)
        {
            var addedItem = Instantiate(itemPrefab, inventoryItemsContainer.transform);
            addedItem.OnClicked += AddedItem_OnClicked;
        }
    }

    private void AddedItem_OnClicked(object sender, InventoryItem item)
    {
        RemoveItem(item);
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
}
