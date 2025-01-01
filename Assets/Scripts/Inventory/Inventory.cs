using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : ScriptableObject, IItemContainer {
    private ItemSlot[] itemSlots = new ItemSlot[20];
    public Action OnItemsUpdated = delegate { };
    public ItemSlot GetSlotByIndex(int index) => itemSlots[index];
    public ItemSlot AddItem(ItemSlot itemSlot) {
        for (int i = 0; i < itemSlots.Length; i++) {
            if (itemSlots[i].item != null) {
                if (itemSlots[i].item == itemSlot.item) {
                    int slotRemainingSpace = itemSlot.item.MaxStackSize - itemSlots[i].amount;
                    if (itemSlot.amount <= slotRemainingSpace) {
                        itemSlots[i].amount += itemSlot.amount;
                        itemSlot.amount = 0;
                        OnItemsUpdated.Invoke();
                        return itemSlot;
                    } else if (slotRemainingSpace > 0) {
                        itemSlots[i].amount += slotRemainingSpace;
                        itemSlot.amount -= slotRemainingSpace;
                    }
                }
            }
        }
        for (int i = 0; i < itemSlots.Length; i++) {
            if (itemSlots[i].item == null) {
                if (itemSlot.amount <= itemSlot.item.MaxStackSize) {
                    itemSlots[i] = itemSlot;
                    itemSlot.amount = 0;
                    OnItemsUpdated.Invoke();
                    return itemSlot;
                }
            } else {
                itemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.MaxStackSize);
                itemSlot.amount -= itemSlot.item.MaxStackSize;
            }
        }
        OnItemsUpdated.Invoke();
        return itemSlot;
    }

    public int CountItems(InventoryItem item) {
        int total = 0;
        foreach (ItemSlot itemSlot in itemSlots) {
            if (itemSlot.item == null) { continue; }
            if (itemSlot.item != item) { continue; }
            total += itemSlot.amount;
        }
        return total;
    }

    public bool HasItem(InventoryItem item) {
        foreach (ItemSlot itemSlot in itemSlots) {
            if (itemSlot.item == null) { continue; }
            if (itemSlot.item != item) { continue; }
            return true;
        }
        return false;
    }

    public void RemoveAt(int slotIndex) {
        if (slotIndex < 0 || slotIndex > itemSlots.Length - 1) { return; }
        itemSlots[slotIndex] = new ItemSlot();
        OnItemsUpdated.Invoke();
    }

    public void RemoveItem(ItemSlot itemSlot) {
        for (int i = 0; i < itemSlots.Length; i++) {
            if (itemSlots[i].item != null) {
                if (itemSlots[i].item == itemSlot.item) {
                    if (itemSlots[i].amount < itemSlot.amount) {
                        itemSlot.amount -= itemSlots[i].amount;
                        itemSlots[i] = new ItemSlot();
                        OnItemsUpdated.Invoke();
                    } else {
                        itemSlots[i].amount -= itemSlot.amount;
                        if (itemSlots[i].amount == 0) {
                            itemSlots[i] = new ItemSlot();
                            OnItemsUpdated.Invoke();
                            return;
                        }
                    }
                }
            }
        }
    }

    public void Swap(int slotIndex1, int slotIndex2) {
        ItemSlot first = itemSlots[slotIndex1];
        ItemSlot second = itemSlots[slotIndex2];
        if (first == second) { return; }
        if (second.item != null) {
            if (first.item == second.item) {
                int secondSlotRemainingSpace = second.item.MaxStackSize - second.amount;
                if (first.amount <= secondSlotRemainingSpace) {
                    itemSlots[slotIndex2].amount += first.amount;
                    itemSlots[slotIndex1] = new ItemSlot();
                    OnItemsUpdated.Invoke();
                    return;
                }
            }
        }
        itemSlots[slotIndex1] = second;
        itemSlots[slotIndex2] = first;
        OnItemsUpdated.Invoke();
    }
}
