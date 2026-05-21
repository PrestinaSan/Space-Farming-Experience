using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public class PlacementManager : MonoBehaviour
{
    [Header("References")]

    [Tooltip("Tilemap of valid placing areas")]
    [SerializeField] private Tilemap baseTilemap;

    [Tooltip("Sprite renderer of the ghost block showing where the player can deploy the buildings")]
    [SerializeField] private SpriteRenderer ghostRenderer;

    [Tooltip("The collision checker of the ghost block")]
    [SerializeField] private GhostTileCollision ghostTileCollision;

    [Tooltip("grid for alignment")]
    [SerializeField] private Grid grid;

    [Header("Buildings")]

    [Tooltip("List of buildings")]
    [SerializeField] private BuildingEntry[] buildingEntries;

    private int currentEntry;

    private Color validColor = new Color(1, 1, 1, 0.5f);
    private Color invalidColor = new Color(3, 0, 0, 0.4f);

    private void Start()
    {
        ghostRenderer.gameObject.SetActive(false);
    }
    void Update()
    {
        Item heldItem = InventoryManager.Instance.GetSelectedItem(false);

        for(int i = 0; i < buildingEntries.Length;i++)
        {
            if (heldItem == buildingEntries[i].buildItem)
            {
                currentEntry = i;
                HandleGhostPreview();
                break;
            }
            else
                ghostRenderer.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Toggles the translucent ghost tile you see when holding the tile block
    /// </summary>
    private void HandleGhostPreview()
    {
        ghostRenderer.sprite = buildingEntries[currentEntry].buildPrefab.GetComponent<SpriteRenderer>().sprite;
        ghostRenderer.gameObject.SetActive(true);
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector3Int cellPos = grid.WorldToCell(mouseWorld);
        Vector3 snappedPos = grid.GetCellCenterWorld(cellPos);
        snappedPos.z = 0;
        ghostRenderer.transform.position = snappedPos;

        bool isPlaceable = baseTilemap.HasTile(cellPos);
        bool isValid = AreCellsFree(cellPos) && isPlaceable && !ghostTileCollision.isOverlapping;

        ghostRenderer.enabled = isPlaceable;
        ghostRenderer.color = isValid ? validColor : invalidColor;

        if (Input.GetMouseButtonDown(0) && isValid && !EventSystem.current.IsPointerOverGameObject())
        {
            InventoryManager.Instance.GetSelectedItem(true);
            PlaceBuilding(cellPos);
        }
    }

    /// <summary>
    /// Places the building on the map and adds taken areas to a tilemap to mark it as occupied.
    /// </summary>
    /// <param name="cellPos">Position after snapping</param>
    private void PlaceBuilding(Vector3Int cellPos)
    {
        for (int x = 0; x < buildingEntries[currentEntry].size.x; x++)
        {
            for (int y = 0; y < buildingEntries[currentEntry].size.y; y++)
            {
                WorldOccupancyManager.Instance.Occupy(cellPos + new Vector3Int(x, y, 0));
            }
        }
        Vector3 worldPos = grid.GetCellCenterWorld(cellPos);
        worldPos.z = 0;
        GameObject placed = Instantiate(buildingEntries[currentEntry].buildPrefab, worldPos, Quaternion.identity);

        PlaceableBuilding building = placed.GetComponent<PlaceableBuilding>();
        building.entry = buildingEntries[currentEntry];
        building.anchorCell = cellPos;
    }

    private bool AreCellsFree(Vector3Int anchor)
    {
        for (int x = 0; x < buildingEntries[currentEntry].size.x; x++)
            for (int y = 0; y < buildingEntries[currentEntry].size.y; y++)
                if (WorldOccupancyManager.Instance.IsOccupied(anchor + new Vector3Int(x, y, 0)))
                    return false;
        return true;
    }
}
[System.Serializable]
[Tooltip("Stores information about each building")]
public class BuildingEntry
{
    [Tooltip("The item of the building")]
    public Item buildItem;
    [Tooltip("The building")]
    public GameObject buildPrefab;
    [Tooltip("The building's size")]
    public Vector2Int size = new(1,1);
}
