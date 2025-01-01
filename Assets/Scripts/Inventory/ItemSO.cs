using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject {
    [Header("Basic Info")]
    [SerializeField] private new string name = "New Item Name";
    [SerializeField] private Sprite icon = null;
    public string Name => name;
    public abstract string ColoredName { get; }
    public Sprite Icon => icon;
    public abstract string GetInfoDisplayText();
}
