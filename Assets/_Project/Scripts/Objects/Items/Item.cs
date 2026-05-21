using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class Item: ScriptableObject
{
    [System.Flags] public enum ItemType
    {
        None = 0,

        Food = 1 << 0,
        Plantable = 1 << 1,
        Material = 1 << 2,
        Tool = 1 << 3
    }
    public Sprite image;
    public bool stackable = true;
    public int itemID;
    /// <summary>
    /// the catagory/catagories in which the item belongs
    /// </summary>
    public ItemType itemType;
    /// <summary>
    /// the item's description in the infobook
    /// </summary>
    public string description;
    /// <summary>
    /// unique noteworthy interactions in the infobook
    /// </summary>
    public string specialText;
    /// <summary>
    /// how much of this stat the item will give before any effects
    /// </summary>
    public int baseHealthiness,baseHunger, baseHydration;
    /// <summary>
    /// how much of this stat the item will currently give
    /// </summary>
    [HideInInspector] public int healthiness,hunger, hydration;
    
}
