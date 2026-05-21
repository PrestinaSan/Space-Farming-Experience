using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; private set; }
    [SerializeField] private GameObject recipeBook;
    [SerializeField] private CraftingRecipe[] recipes;
    [SerializeField] private InventorySlot[] craftingSlots;
    [SerializeField] private Image outputResult;
    [SerializeField] int currentOutput;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        outputResult.gameObject.SetActive(false);
        CheckRecipe();
    }
    public void ToggleRecipeBook()
    {
        recipeBook.SetActive(!recipeBook.activeSelf);
    }
    /// <summary>
    /// Clicking on the crafting result to collect it
    /// </summary>
    public void OnResultCollect()
    {
        bool itemAdded = InventoryManager.Instance.AddItem(recipes[currentOutput].output.item, recipes[currentOutput].output.count);
        if (!itemAdded)
        {
            ItemDroppingManager.Instance.OnItemDropped(null, recipes[currentOutput].output.count, null, recipes[currentOutput].output.item);
        }

        foreach (InventorySlot slot in craftingSlots)
        {
            if (slot.transform.childCount == 0) continue;
            InventoryItem slotItem = slot.transform.GetChild(0).GetComponent<InventoryItem>();
            if (slotItem == null) continue;

            foreach (var material in recipes[currentOutput].materials)
            {
                if (slotItem.Item == material.item)
                {
                    for (int i = 0; i < material.count; i++)
                        InventoryManager.Instance.GetSelectedItem(true, slot);
                    break;
                }
            }
        }
        outputResult.gameObject.SetActive(false);
        CheckRecipe();
    }
    /// <summary>
    /// Check in the Recipe List to find matches
    /// </summary>
    public void CheckRecipe()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            bool recipeMatches = true;
            foreach (var material in recipes[i].materials)
            {
                bool materialFound = false;
                foreach (var slot in craftingSlots)
                {
                    if (slot.transform.childCount == 0) continue;
                    InventoryItem item = slot.transform.GetChild(0).GetComponent<InventoryItem>();
                    if (InventoryManager.Instance.GetSelectedItem(false, slot) == material.item && item.Count >= material.count)
                    {
                        materialFound = true;
                        break;
                    }
                }
                if (materialFound == false) recipeMatches = false;
            }
            if (!recipeMatches)
            {
                outputResult.gameObject.SetActive(false);
                continue;
            }
            foreach (var slot in craftingSlots)
            {
                if (slot.transform.childCount == 0) continue;
                InventoryItem slotItem = slot.transform.GetChild(0).GetComponent<InventoryItem>();

                bool itemBelongsToRecipe = false;
                foreach (var material in recipes[i].materials)
                {
                    if (slotItem.Item == material.item)
                    {
                        itemBelongsToRecipe = true;
                        break;
                    }
                }
                if (!itemBelongsToRecipe)
                {
                    recipeMatches = false;
                    break;
                }
            }
            if (recipeMatches)
            {
                currentOutput = i;
                outputResult.gameObject.SetActive(true);
                outputResult.sprite = recipes[i].output.item.image;
                break;
            }
            else
            {
                outputResult.gameObject.SetActive(false);
            }
        }
    }
}