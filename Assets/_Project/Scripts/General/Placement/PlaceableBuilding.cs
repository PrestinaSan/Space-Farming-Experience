using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableBuilding : MonoBehaviour
{
    [Tooltip("The building's contained data")]
    public BuildingEntry entry;
    [Tooltip("Where the building is deployed")]
    public Vector3Int anchorCell;

    [Tooltip("Shovel Item for picking up things")]
    [SerializeField] private Item shovel;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        CheckForPickup();
    }
    /// <summary>
    /// Checks to see if the player clicked on this object with a shovel, which drops the object as an item.
    /// </summary>
    private void CheckForPickup()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider.gameObject == gameObject)
            {
                Item heldItem = InventoryManager.Instance.GetSelectedItem(false);
                if (heldItem != shovel) return;

                for (int x = 0; x < entry.size.x; x++)
                    for (int y = 0; y < entry.size.y; y++)
                        WorldOccupancyManager.Instance.Free(anchorCell + new Vector3Int(x, y, 0));

                ItemDroppingManager.Instance.OnItemDropped(transform.position, 1, entry.buildItem);
                Destroy(gameObject);
            }
        }
    }
}