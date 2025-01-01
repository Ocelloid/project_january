using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : ItemSO {
    [Header("Item Data")]
    [Min(0)] private int sellPrice = 1;
    [Min(1)] private int maxStackSize = 1;
    public override string ColoredName {
        get {
            return Name;
        }
    }
    public int SellPrice {
        get {
            return sellPrice;
        }
    }
    public int MaxStackSize {
        get {
            return maxStackSize;
        }
    }

    public override string GetInfoDisplayText() {
        throw new System.NotImplementedException();
    }
}
