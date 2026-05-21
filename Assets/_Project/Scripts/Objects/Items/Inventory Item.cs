using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDrag;
    private Item item;
    private int count = 1;
    private bool isDragging = false;
    private PointerEventData.InputButton draggingButton;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;

    public int Count
    {
        get { return count; }
        set { count = value; }
    }
    public Item Item
    {
        get { return item; }
    }
    public bool IsDragging { get { return isDragging; }}
    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }
    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.transform.parent.gameObject.SetActive(textActive);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDragging) return;

        isDragging = true;
        draggingButton = eventData.button;

        parentAfterDrag = transform.parent;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            StartDragging();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (count > 1)
            {
            TakeHalfStack();
            }
            StartDragging();
            return;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != draggingButton)
        {
            return;
        }
        CraftingSystem.Instance.CheckRecipe();
        transform.position = Input.mousePosition + new Vector3(0,5,0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != draggingButton)
        {
            return;
        }
        transform.SetParent(parentAfterDrag);
        TryMergeWithSibling();
        image.raycastTarget = true;
        isDragging = false;
        CraftingSystem.Instance.CheckRecipe();
    }
    public void SetParentAfterDrag(Transform parent)
    {
        parentAfterDrag = parent;
    }
    private void StartDragging()
    {
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    private void TakeHalfStack()
    {
        int takenAmount = count / 2;
        Count -= takenAmount;
        RefreshCount();

        // Spawn clean prefab
        GameObject newObj = Instantiate(InventoryManager.Instance.InventoryItemPrefab, parentAfterDrag);
        InventoryItem newItem = newObj.GetComponent<InventoryItem>();

        // Initialize properly
        newItem.InitializeItem(item);
        newItem.Count = takenAmount;
        newItem.RefreshCount();
    }
    private void TryMergeWithSibling()
    {
        if (transform.parent.childCount <= 1)
            return;

        InventoryItem sibling = transform.parent
            .GetChild(0)
            .GetComponent<InventoryItem>();

        if (sibling == this)
            sibling = transform.parent
                .GetChild(1)
                .GetComponent<InventoryItem>();

        if (sibling.Item == this.Item && sibling.Item.stackable)
        {
            sibling.Count += this.Count;
            sibling.RefreshCount();
            Destroy(gameObject);
        }
    }
}