using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantHarvesting : MonoBehaviour
{
    [Tooltip("The list of items that would be dropped from harvesting the plant")]
    public Item[] drops;

    [Tooltip("The chance of each item dropping")]
    public float[] dropChance;

    [Tooltip("The amount of items that would drop from each item in the drops")]
    public int[] dropCount;

    [Tooltip("The item ID for the tool needed to harvest the plant, 0 being any tool")]
    public int toolNeeded;

    [Tooltip("stores the data of the plant being harvested")]
    public WildPlantData plantData;

    [Tooltip("how many hits the plant needs to break")]
    public int health = 1;
    private void Update()
    {
        if (toolNeeded == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HarvestBehavior();
            }
            return;
        }
        if (Input.GetMouseButton(0))
        {
            HarvestBehavior();
        }
    }
    /// <summary>
    /// Checks for when the plant is clicked and checks the item used to click it
    /// </summary>
    private void HarvestBehavior()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (toolNeeded != 0)
                {
                    Item tool = InventoryManager.Instance.GetSelectedItem(false);
                    if (tool == null) return;
                    if (tool.itemID == toolNeeded)
                    {
                        OnHit(1);
                    }
                }
                else
                {
                    OnHit(1);
                }
            }
        }
    }
    /// <summary>
    /// Handles subtracting the health of the plant and the item dropping
    /// </summary>
    /// <param name="amount">the amount of health to subtract</param>
    private void OnHit(int amount)
    {
        if (health > 0)
        {
            for (int i  = 0; i < drops.Length; i++)
            {
                if (Random.Range(0f,1f) <= dropChance[i])
                {
                    ItemDroppingManager.Instance.OnItemDropped(null, dropCount[i], transform, drops[i]);
                }
                StartCoroutine(DamageVisual());
                health -= amount;
            }

        }
        if (health <= 0)
        {
            Infobook.Instance.UnlockPlant(plantData);
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Does a little shake to show the plant took damage
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageVisual()
    {
        Vector3 offset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
        transform.position += offset;
        yield return new WaitForSeconds(0.05f);
        transform.position -= offset;

    }
}
