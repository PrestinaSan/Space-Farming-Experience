using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDroppingManager : MonoBehaviour
{
    public static ItemDroppingManager Instance { get; private set; }

    [Tooltip("Universal prefab for all dropped items")]
    [SerializeField] private GameObject droppedItemPrefab;

    [Tooltip("Reference to the player")]
    [SerializeField] private GameObject player;

    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Drops an item on the ground. If the player is the one dropping the item, only the first field is needed to be filled.
    /// </summary>
    /// <param name="inventoryItem">Gives the information about the item the player is dropping</param>
    /// <param name="amount">how many items</param>
    /// <param name="worldPos">where</param>
    /// <param name="item">what item</param>
    public void OnItemDropped(InventoryItem inventoryItem, int amount = 0, Transform worldPos = null, Item item = null)
    {
        if (worldPos == null)
        {
            worldPos = player.transform;
        }
        if (inventoryItem != null)
        {
            int count = inventoryItem.Count;
            GameObject droppedItemObj = Instantiate(droppedItemPrefab, player.transform.position, player.transform.rotation);
            DroppedItem droppedItemScript = droppedItemObj.GetComponent<DroppedItem>();
            droppedItemScript.InitializeItem(inventoryItem.Item, player, count);

            return;
        }
        else
        {
            GameObject droppedItemObj = Instantiate(droppedItemPrefab, worldPos.position, worldPos.rotation);
            DroppedItem droppedItemScript = droppedItemObj.GetComponent<DroppedItem>();
            droppedItemScript.PickupTimer = 0.5f;
            droppedItemScript.InitializeItem(item, player, amount);

            return;
        }
    }
    /// <summary>
    /// Drops an item on the ground, but uses vector3 position instead of transform.
    /// </summary>
    /// <param name="position">where</param>
    /// <param name="amount">how many items</param>
    /// <param name="item">what item</param>
    public void OnItemDropped(Vector3 position, int amount, Item item)
    {
        GameObject droppedItemObj = Instantiate(droppedItemPrefab, position, Quaternion.identity);
        DroppedItem droppedItemScript = droppedItemObj.GetComponent<DroppedItem>();
        droppedItemScript.PickupTimer = 0.5f;
        droppedItemScript.InitializeItem(item, player, amount);
    }
}
