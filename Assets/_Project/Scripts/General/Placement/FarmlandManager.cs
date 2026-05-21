using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static UnityEngine.EventSystems.EventTrigger;
public class FarmlandManager : MonoBehaviour
{
    [Header("References")]
    
    [Tooltip("The tilemap displaying all the farmland tiles")]
    [SerializeField] private Tilemap farmlandTilemap;

    [Tooltip("Tilemap of valid placing areas")]
    [SerializeField] private Tilemap baseTilemap;

    [Tooltip("The tile representing the farmland")]
    [SerializeField] private RuleTile farmlandRuleTile;

    [Tooltip("Sprite renderer of the ghost block showing where the player can deploy the buildings")]
    [SerializeField] private SpriteRenderer ghostRenderer;

    [Tooltip("The collision checker of the ghost block")]
    [SerializeField] private GhostTileCollision ghostTileCollision;

    [Tooltip("Grid for alignment")]
    [SerializeField] private Grid grid;

    [Header ("Farmland")]

    [Tooltip("The item to deploy farmland")]
    [SerializeField] private Item farmlandItem;

    [Tooltip("The shovel to pick up farmland")]
    [SerializeField] private Item shovel;

    [Header("Plant")]

    [Tooltip("list of seeds and their plants")]
    [SerializeField] private SeedEntry[] seeds;

    [Tooltip("The default plant prefab for all farmplants")]
    [SerializeField] private GameObject plantPrefab;

    // Runtime state — keyed by tile position
    private HashSet<Vector3Int> farmlandState = new();
    private Dictionary<Vector3Int, GameObject> occupiedFarmlands = new();

    // Ghost colors
    private Color validColor = new Color(1, 1, 1, 0.5f);
    private Color invalidColor = new Color(3, 0, 0, 0.4f);

    private void Start()
    {
        ghostRenderer.sprite = farmlandRuleTile.m_DefaultSprite;
    }
    void Update()
    {
        Item heldItem = InventoryManager.Instance.GetSelectedItem(false);


        if (heldItem == farmlandItem)
            HandleGhostPreview();
        else
            ghostRenderer.gameObject.SetActive(false);

        HandleTileInteraction();
    }
    /// <summary>
    /// Toggles the translucent farmland tile you see when holding the tile block
    /// </summary>
    private void HandleGhostPreview()
    {
        ghostRenderer.gameObject.SetActive(true);
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector3Int cellPos = grid.WorldToCell(mouseWorld);
        Vector3 snappedPos = grid.GetCellCenterWorld(cellPos);
        snappedPos.z = 0;
        ghostRenderer.transform.position = snappedPos;

        bool tileExists = farmlandState.Contains(cellPos);
        bool isPlaceable = baseTilemap.HasTile(cellPos);
        bool isValid = !tileExists && isPlaceable && !ghostTileCollision.isOverlapping && !WorldOccupancyManager.Instance.IsOccupied(cellPos);

        ghostRenderer.enabled = isPlaceable;
        ghostRenderer.color = isValid ? validColor : invalidColor;

        if (Input.GetMouseButtonDown(0) && isValid && !EventSystem.current.IsPointerOverGameObject())
        {
            InventoryManager.Instance.GetSelectedItem(true);
            PlaceFarmland(cellPos);
        }
    }

    /// <summary>
    /// Places the farmland rule tile on the tilemap (visual)
    /// </summary>
    /// <param name="cellPos">Position after snapping</param>
    private void PlaceFarmland(Vector3Int cellPos)
    {
        farmlandTilemap.SetTile(cellPos, farmlandRuleTile);
        farmlandState.Add(cellPos);
        WorldOccupancyManager.Instance.Occupy(cellPos);
    }
    /// <summary>
    /// Handles the interaction when the player clicks on the farmland, whether they have a seed or not
    /// </summary>
    private void HandleTileInteraction()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector3Int cellPos = grid.WorldToCell(mouseWorld);

        Item heldItem = InventoryManager.Instance.GetSelectedItem(false);

        foreach (SeedEntry entry in seeds)
        {
            if (heldItem == entry.seedItem && farmlandState.Contains(cellPos) && !occupiedFarmlands.ContainsKey(cellPos))
            {
                PlantSeed(cellPos, entry.plantData);
                break;
            }
        }
        if (heldItem == shovel && farmlandState.Contains(cellPos))
        {
            farmlandTilemap.SetTile(cellPos, null);
            farmlandState.Remove(cellPos);
            WorldOccupancyManager.Instance.Free(cellPos);
            Vector3 worldPos = grid.GetCellCenterWorld(cellPos);
            worldPos.z = 0;
            ItemDroppingManager.Instance.OnItemDropped(worldPos, 1, farmlandItem);
            if (occupiedFarmlands.TryGetValue(cellPos, out GameObject plant))
            {
                Destroy(plant);
                occupiedFarmlands.Remove(cellPos);
            }
        }
    }
    /// <summary>
    /// Plants the seed at the clicked farmland, also marks that spot as occupied so other plants cannot be placed there.
    /// </summary>
    /// <param name="cellPos">The position on the grid</param>
    /// <param name="data">the plant's data</param>
    private void PlantSeed(Vector3Int cellPos, FarmPlantData data)
    {
        Vector3 worldPos = grid.GetCellCenterWorld(cellPos);
        worldPos.z = 0;
        GameObject plant = Instantiate(plantPrefab, worldPos, Quaternion.identity);
        plant.GetComponent<FarmPlantBehavior>().InitializePlant(data, this, grid);
        InventoryManager.Instance.GetSelectedItem(true);
        occupiedFarmlands[cellPos] = plant;
    }
    /// <summary>
    /// Clears up that position in the hashset, allowing other plants to be placed there
    /// </summary>
    /// <param name="cellPos">the position to be cleared up</param>
    public void ClearFarmPlant(Vector3Int cellPos)
    {
        occupiedFarmlands.Remove(cellPos);
    }

}
[System.Serializable]
public class SeedEntry
{
    [Tooltip("the item that deploys the plant")]
    public Item seedItem;

    [Tooltip("the scriptable object that contains the plant's data")]
    public FarmPlantData plantData;
}