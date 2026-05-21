using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Scriptable Objects/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    [Serializable] public class ItemAndCount
    {
        public Item item;
        public int count;
    }
    public ItemAndCount[] materials;
    public ItemAndCount output;
}
