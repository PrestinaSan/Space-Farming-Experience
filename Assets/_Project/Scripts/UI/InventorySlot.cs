using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        InventoryItem draggedItem = dropped.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        if (draggedItem.IsDragging == false) return;

        // if slot empty just place
        if (transform.childCount == 0)
        {
            draggedItem.SetParentAfterDrag(transform);
        }
        else
        {
            // slot already has something
            InventoryItem slotItem = transform.GetChild(0).GetComponent<InventoryItem>();

            // check if same item type AND stackable
            if (slotItem.Item == draggedItem.Item &&
                slotItem.Item.stackable)
            {
                MergeStacks(slotItem, draggedItem);
            }
        }
    }

    public void MergeStacks(InventoryItem slotItem, InventoryItem draggedItem)
    {
        int maxStack = InventoryManager.Instance.ItemStackLimit;

        int total = slotItem.Count + draggedItem.Count;

        if (total <= maxStack)
        {
            // Everything fits
            slotItem.Count = total;
            slotItem.RefreshCount();

            Destroy(draggedItem.gameObject);
        }
        else
        {
            // Only part fits
            int spaceLeft = maxStack - slotItem.Count;

            slotItem.Count = maxStack;
            slotItem.RefreshCount();

            draggedItem.Count -= spaceLeft;
            draggedItem.RefreshCount();
        }
    }
}
