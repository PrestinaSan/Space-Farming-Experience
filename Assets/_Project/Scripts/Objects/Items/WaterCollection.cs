
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaterCollection : MonoBehaviour
{
    private void Update()
    {
        // checks for mouse inputs and runs the collection if the mouse input is on the water
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider.gameObject == gameObject)
                {
                    InventoryManager.Instance.CollectWater();
                }
            }
        }
    }

}
