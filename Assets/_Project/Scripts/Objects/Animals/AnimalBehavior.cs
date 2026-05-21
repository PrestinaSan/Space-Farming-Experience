using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Rigidbody2D rb;

    [Tooltip("The animal's data")]
    private AnimalData animalData;

    private float movementSpeed = 3f;
    private Coroutine wanderCoroutine = null;
    private Coroutine panicking = null;
    private int health;
    private Item[] drops;
    private int[] dropCount;

    private Vector3 wanderTarget;
    private bool hasWanderTarget = false;
    

    /// <summary>
    /// Initializes the information for an animal using animalData
    /// </summary>
    /// <param name="_animal">the animalData, SciprtableObject</param>
    public void Initialize(AnimalData _animal)
    {
        spriteRenderer.sprite = _animal.sprite;
        gameObject.AddComponent<BoxCollider2D>();
        movementSpeed = _animal.movementSpeed;
        rb.mass = _animal.mass;
        rb.linearDamping = _animal.mass * 0.5f;
        drops = _animal.drops;
        dropCount = _animal.dropCount;
        health = _animal.health;
        animalData = _animal;
    }

    private void FixedUpdate()
    {
        if (hasWanderTarget)
        {
            Vector3 dir = (wanderTarget - transform.position).normalized;
            float currentSpeed;
            if (panicking != null) currentSpeed = movementSpeed * 3;
            else currentSpeed = movementSpeed;
                rb.MovePosition(transform.position + currentSpeed * Time.fixedDeltaTime * dir);

            if (Vector3.Distance(transform.position, wanderTarget) <= 0.3f)
                hasWanderTarget = false;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Item tool = InventoryManager.Instance.GetSelectedItem(false);
                    if (tool != null)
                    {
                        if (tool.itemID == 1)
                            OnHit(2);
                        else if (tool.itemID == 2 || tool.itemID == 28)
                            OnHit(1);
                    }
                }
            }
        }

        if (wanderCoroutine == null)
        {
            wanderCoroutine = StartCoroutine(WanderMovement());
        }
    }
    /// <summary>
    /// Animal picsk a certain spot and wanders around while nothing is happening.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WanderMovement()
    {
        wanderTarget = transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        hasWanderTarget = true;

        yield return new WaitUntil(() => !hasWanderTarget);

        float idleTime = Random.Range(3f, 15f);
        yield return new WaitForSeconds(idleTime);
        wanderCoroutine = null;

    }
    /// <summary>
    /// Handles damage taken and dropping of loot
    /// </summary>
    /// <param name="amount">damage</param>
    private void OnHit(int amount)
    {
        Infobook.Instance.UnlockAnimal(animalData);
        for (int hits = 0; hits < amount; hits++)
        {
            if (health > 0)
            {
                for (int i = 0; i < drops.Length; i++)
                {
                    ItemDroppingManager.Instance.OnItemDropped(null, dropCount[i], transform, drops[i]);
                    StartCoroutine(DamageVisual());
                    if (panicking == null && Random.Range(0f,1f) <= 0.75f)
                        panicking = StartCoroutine(Panic());
                    health--;
                }
            }
            if (health <= 0)
                Destroy(gameObject);
        }
    }
    /// <summary>
    /// gives the animal a little shake to indicate damage taken
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageVisual()
    {
        Vector3 offset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
        transform.position += offset;
        yield return new WaitForSeconds(0.05f);
        transform.position -= offset;
    }

    /// <summary>
    /// Makes thet animal run around frantically when hit
    /// </summary>
    /// <returns></returns>
    private IEnumerator Panic()
    {
        int waypoints = Random.Range(2, 4);
        for (int i = 0; i < waypoints; i++)
        {
            wanderTarget = transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            hasWanderTarget = true;
            yield return new WaitUntil(() => !hasWanderTarget);
        }
        panicking = null;
    }
}