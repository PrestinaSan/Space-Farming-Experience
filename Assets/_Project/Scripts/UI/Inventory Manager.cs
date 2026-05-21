using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Tooltip("The inventory UI")]
    [SerializeField] private GameObject inventoryObj;

    [Tooltip("The Infobook UI")]
    [SerializeField] private GameObject infobookObj;

    [Tooltip("The list of all the slots in the inventory")]
    [SerializeField] private InventorySlot[] inventorySlots;

    [Tooltip("The slot that is considered to be what the player is holding")]
    [SerializeField] private InventorySlot mainHand;

    [Tooltip("Prefab for all the items that will be added to the inventory UI")]
    [SerializeField] private GameObject inventoryItemPrefab;

    [Tooltip("The limit of stackable items in 1 stack")]
    [SerializeField] private int itemStackLimit;

    [Tooltip("States of the bucket used for interactions with water")]
    [SerializeField] private Item bucket, filledBucket;

    [Tooltip("List of items that the player spawns with")]
    [SerializeField] private Item[] starterItems;

    public int ItemStackLimit
    {
        get { return itemStackLimit; }
    }
    public GameObject InventoryItemPrefab
    {
        get { return inventoryItemPrefab; }
    }

    private void Awake()
    {
        Instance = this;
        inventoryObj.SetActive(false);
    }

    /// <summary>
    /// Turns the inventory UI on and off
    /// </summary>
    public void ToggleInventory()
    {
        if (infobookObj.activeSelf == true) return;
        bool isOpen = inventoryObj.activeSelf;
        inventoryObj.SetActive(!isOpen);
    }

    private void Start()
    {
        InitializeInventory();
    }

    /// <summary>
    /// Attempts to add an item to the inventory. Returns true if the item was successfully added, false if not.
    /// </summary>
    /// <param name="item">The item</param>
    /// <param name="count">How many of that item</param>
    /// <returns></returns>
    public bool AddItem(Item item, int count)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null 
                && itemInSlot.Item == item 
                && itemInSlot.Count < itemStackLimit 
                && itemInSlot.Item.stackable == true)
            {
                itemInSlot.Count += count;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot, count);
                Infobook.Instance.UnlockItem(item);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Spawns a new item in the inventory
    /// </summary>
    /// <param name="item">what item</param>
    /// <param name="slot">in which slot</param>
    /// <param name="count">how many</param>
    private void SpawnNewItem(Item item, InventorySlot slot, int count)
    {
        GameObject newItemObj = Instantiate(inventoryItemPrefab, slot.transform); 
        InventoryItem inventoryItem = newItemObj.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
        if (inventoryItem.Item.stackable == true)
        {
            inventoryItem.Count = count;
        }
        inventoryItem.RefreshCount();
    }
    /// <summary>
    /// Checks the mainhand and returns the item the player is holding.
    /// Returns null if mainhand is empty.
    /// </summary>
    /// <param name="use">if set to true, consumes the item on use (if item is stackable)</param>
    /// <param name="slot">to check another slot apart from the mainhand, but is set to mainhand by default</param>
    /// <returns></returns>
    public Item GetSelectedItem(bool use, InventorySlot slot = null)
    {
        if (slot == null) slot = mainHand;
        if (slot.transform.childCount == 0) return null;
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.Item;
            if (use == true)
            {
                if (item == filledBucket)
                {
                    itemInSlot.InitializeItem(bucket);
                    return item;
                }
                if (item.stackable == false) return item;
                itemInSlot.Count--;
                if (itemInSlot.Count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        } 
        return null;
    }


    /// <summary>
    /// Allows the player to pick up water from the waterbodies nearby
    /// </summary>
    public void CollectWater()
    {
        if (mainHand.transform.childCount == 0) return;
        Transform itemObj = mainHand.transform.GetChild(0);
        InventoryItem item = itemObj.GetComponent<InventoryItem>();
        if (item.Item == bucket)
        {
            item.InitializeItem(filledBucket);
        }
    }

    /// <summary>
    /// Adds the starter items to the inventory
    /// </summary>
    public void InitializeInventory()
    {
        foreach (var item in starterItems)
        {
            AddItem(item,1);
        }
    }
}
