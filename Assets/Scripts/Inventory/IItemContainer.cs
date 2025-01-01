using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContainer {
    ItemSlot AddItem(ItemSlot itemSlot);
    void RemoveItem(ItemSlot itemSlot);
    void RemoveAt(int slotIndex);
    void Swap(int slotIndex1, int slotIndex2);
    bool HasItem(InventoryItem item);
    int CountItems(InventoryItem item);
}
