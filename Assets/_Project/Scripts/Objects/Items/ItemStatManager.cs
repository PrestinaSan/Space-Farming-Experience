using System.Collections.Generic;
using UnityEngine;

public class ItemStatManager : MonoBehaviour
{
    public static ItemStatManager Instance { get; private set; }
    private class RuntimeItemData
    {
        public int healthiness;
        public int hunger;
        public int hydration;

        public RuntimeItemData(Item item)
        {
            healthiness = item.baseHealthiness;
            hunger = item.baseHunger;
            hydration = item.baseHydration;
        }
    }

    private Dictionary<Item, RuntimeItemData> runtimeStats = new();

    private RuntimeItemData ItemInfoBehavior(Item item)
    {
        if (!runtimeStats.ContainsKey(item))
            runtimeStats[item] = new RuntimeItemData(item);
        return runtimeStats[item];
    }

    public int GetHealthiness(Item item) => ItemInfoBehavior(item).healthiness;
    public int GetHunger(Item item) => ItemInfoBehavior(item).hunger;
    public int GetHydration(Item item) => ItemInfoBehavior(item).hydration;

    public void SetHealthiness(Item item, int value) => ItemInfoBehavior(item).healthiness = value;
    public void SetHunger(Item item, int value) => ItemInfoBehavior(item).hunger = value;
    public void SetHydration(Item item, int value) => ItemInfoBehavior(item).hydration = value;


    private void Awake()
    {
        Instance = this;
    }
}