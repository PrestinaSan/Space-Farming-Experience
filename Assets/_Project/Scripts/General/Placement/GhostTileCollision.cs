using UnityEngine;

public class GhostTileCollision : MonoBehaviour
{
    public bool isOverlapping = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        isOverlapping = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOverlapping = false;
    }
}