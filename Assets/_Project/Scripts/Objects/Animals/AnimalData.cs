using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AnimalData")]
public class AnimalData : ScriptableObject
{
    public Sprite sprite;
    public Item[] drops;
    public int[] dropCount;

    public int width = 1;
    public int height = 1;

    public int mass;

    public float spawnRate = 0.1f;
    public float movementSpeed = 5f;
    public int health;

    public string description;

    public string[] biomes;
}
