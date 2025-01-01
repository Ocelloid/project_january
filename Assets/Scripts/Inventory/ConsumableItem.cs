using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ConsumableItem : InventoryItem {
    [Header("Consumable Data")]
    [SerializeField] string useText = "Use";
    public override string GetInfoDisplayText() {
        StringBuilder sb = new StringBuilder();
        sb.Append(Name).AppendLine();
        sb.Append("<color=#00FF00>").Append(useText).Append("</color>").AppendLine();
        sb.Append("Sell Price: ").Append(SellPrice).AppendLine();
        sb.Append("Max Stack: ").Append(MaxStackSize).AppendLine();
        return sb.ToString();
    }
}
