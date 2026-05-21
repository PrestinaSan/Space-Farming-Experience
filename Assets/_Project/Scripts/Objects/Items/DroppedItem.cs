using System.Collections;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Item item;
    private int count = 1;
    private GameObject player;
    private float timer;
    private float movementSpeed = 10;
    private float pickupTimer = 3;
    private readonly float despawnTimer = 300;

    public float PickupTimer {  get { return pickupTimer; } set { pickupTimer = value; } }

    private void Start()
    {
        transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > pickupTimer)
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= 3)
            {
                float step = movementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position,step);
                if (Vector2.Distance(transform.position, player.transform.position) <= 1)
                {
                    if (InventoryManager.Instance.AddItem(item, count))
                    {
                        Destroy(gameObject);
                    }
                    
                }
            }
        }
        if (timer > despawnTimer)
        {
            Destroy(gameObject);
        }
    }
    public void InitializeItem(Item newItem, GameObject _player, int _count)
    {
        item = newItem;
        spriteRenderer.sprite = newItem.image;
        player = _player;
        count = _count;
    }
}
