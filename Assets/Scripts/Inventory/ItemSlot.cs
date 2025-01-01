public struct ItemSlot {
    public InventoryItem item;
    public int amount;
    public ItemSlot(InventoryItem item, int amount) {
        this.item = item;
        this.amount = amount;
    }
    public static bool operator ==(ItemSlot a, ItemSlot b) {
        return a.Equals(b);
    }
    public static bool operator !=(ItemSlot a, ItemSlot b) {
        return !a.Equals(b);
    }
}
