using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Potion
}
[CreateAssetMenu(menuName = "Game Data/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemId;
    public string ItemId => itemId;
    [SerializeField] private ItemType itemType;
    public ItemType ItemType => itemType;
    [SerializeField] private string itemName;
    public string ItemName => itemName;
    [SerializeField] private float itemMaxHp;
    public float ItemMaxHp => itemMaxHp;
    [SerializeField] private float itemHp;
    public float ItemHp => itemHp;
    [SerializeField] private float itemAtk;
    public float ItemAtk => itemAtk;
    [SerializeField] private float itemDef;
    public float ItemDef => itemDef;
    [SerializeField] private string detail;
    public string Detail => detail;
}