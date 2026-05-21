using UnityEngine;
using UnityEngine.EventSystems;

public class CookingSystem : MonoBehaviour
{
    [SerializeField] private ItemEntry[] entries;
    private Item currentItem;

    private void Update()
    {
        currentItem = InventoryManager.Instance.GetSelectedItem(false);
        if (currentItem == null) return;
        if (!currentItem.itemType.HasFlag(Item.ItemType.Food)) return;
        foreach (var entry in entries)
        {
            if (currentItem == entry.uncookedFood)
            {
                HandleCooking(entry);
            }
        }
    }

    private void HandleCooking(ItemEntry entry)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider.gameObject == gameObject)
                {
                    InventoryManager.Instance.GetSelectedItem(true);
                    if (!InventoryManager.Instance.AddItem(entry.cookedFood, 1))
                    {
                        ItemDroppingManager.Instance.OnItemDropped(null, 1, transform, entry.cookedFood);
                    }

                }
            }
        }

    }
}
[System.Serializable]
public class ItemEntry
{
    public Item uncookedFood;
    public Item cookedFood;
}