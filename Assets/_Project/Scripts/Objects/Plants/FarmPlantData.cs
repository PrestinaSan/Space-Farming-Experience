using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FarmPlantData")]
public class FarmPlantData : ScriptableObject
{
    public Sprite saplingSprite;
    public Sprite grownSprite;
    public float timeToGrow;

    public Item[] drops;
    public int[] dropCount;
    public float[] dropChance;

    public int width = 1;
    public int height = 1;

    public bool hasCollision;
    public int toolNeeded;
}