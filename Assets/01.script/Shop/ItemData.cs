using UnityEngine;

public enum ItemType { HP_Potion, Mana_Potion }

[CreateAssetMenu(menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int price;
    public int recoveryAmount; // 회복량 (HP 또는 Mana)
    public ItemType itemType;
    [TextArea] public string description;
}
