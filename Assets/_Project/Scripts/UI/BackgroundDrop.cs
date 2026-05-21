using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundDrop: MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item == null) return;
        ItemDroppingManager.Instance.OnItemDropped(item);

        Debug.Log("Dropped " + item.Count + " " + item.Item.name);
        Destroy(item.gameObject);
    }
}
