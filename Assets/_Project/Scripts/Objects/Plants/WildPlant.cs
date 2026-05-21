using UnityEngine;

public class WildPlant : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlantHarvesting harvest;
    public void Initialize(WildPlantData data)
    {
        spriteRenderer.sprite = data.sprite;
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        if (data.hasCollision)
        {
            boxCollider.isTrigger = false;
        }
        harvest.drops = data.drops;
        harvest.health = data.health;
        harvest.toolNeeded = data.toolNeeded;
        harvest.plantData = data;
        harvest.dropChance = data.dropChance;
        harvest.dropCount = data.dropCount;
    }
}