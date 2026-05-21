using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/WildPlantData")]
public class WildPlantData : ScriptableObject
{
    public Sprite sprite;
    public Item[] drops;
    public int[] dropCount;
    public float[] dropChance;

    public int width = 1;
    public int height = 1;
    public float spawnRate = 0.1f;
    public int health;

    public bool hasCollision;
    public int toolNeeded;

    public string description;

    public string[] biomes;

    public Sprite farmableVariant;
}