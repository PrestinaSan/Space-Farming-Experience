using System;
using TS;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmPlantBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Tooltip("The yellow flower")]
    [SerializeField] private Item boostFlower;
    private BoxCollider2D boxCollider;
    private Sprite grownUpSprite;
    private Item[] drops;
    private float[] dropChance;
    private int[] dropCount;
    private int toolNeeded;
    private float timeToGrow;
    private float timePlanted = 0;
    private FarmlandManager farmlandManager;
    private Grid grid;


    private void Start()
    {
        TimeSystem.TimeSystemChanged += OnTimeChanged;
    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (timePlanted >= timeToGrow)
                {
                    spriteRenderer.sprite = grownUpSprite;
                    if (Input.GetMouseButton(0))
                    {
                        HarvestBehavior();
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        BoostBehavior();
                    }
                    
                }
            }
        }
    }

    private void OnDestroy()
    {
        TimeSystem.TimeSystemChanged -= OnTimeChanged;
    }

    public void InitializePlant(FarmPlantData data, FarmlandManager manager, Grid _grid)
    {
        drops = data.drops;
        dropChance = data.dropChance;
        dropCount = data.dropCount;
        toolNeeded = data.toolNeeded;
        spriteRenderer.sprite = data.saplingSprite;
        grownUpSprite = data.grownSprite;
        timeToGrow = data.timeToGrow;
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        farmlandManager = manager;
        grid = _grid;
        if (!data.hasCollision)
        {
            boxCollider.isTrigger = true;
        }
    }
    private void OnTimeChanged(object sender, TimeSpan newTime)
    {
        if (newTime.Minutes == 0)
            IncrementTimePlanted(0.1f);
    }



    private void HarvestBehavior()
    {
        if (toolNeeded != 0)
        {
            Item tool = InventoryManager.Instance.GetSelectedItem(false);
            if (tool == null) return;
            if (tool.itemID == toolNeeded)
            {
                HarvestPlant();
            }
        }
        else
        {
            HarvestPlant();
        }
    }
    private void BoostBehavior()
    {
        Item flower = InventoryManager.Instance.GetSelectedItem(false);
        if (flower == null) return;
        if (flower.itemID == boostFlower.itemID)
        {
            IncrementTimePlanted(0.5f);
            InventoryManager.Instance.GetSelectedItem(true);
        }
    }
    private void HarvestPlant()
    {
        for (int i = 0; i < drops.Length; i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= dropChance[i])
            {
                ItemDroppingManager.Instance.OnItemDropped(null, dropCount[i], transform, drops[i]);
            }
        }
        Vector3Int pos = grid.WorldToCell(transform.position);
        farmlandManager.ClearFarmPlant(pos);
        Destroy(gameObject);
    }

    public void IncrementTimePlanted(float time)
    {
        timePlanted += time;
    }
}


