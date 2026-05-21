using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TS
{
    public class Sleeping : MonoBehaviour
    {
        private TimeSystem timeSystem;

        private void Awake()
        {
            if (timeSystem == null)
            {
                timeSystem = FindFirstObjectByType<TimeSystem>();
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
                        timeSystem.Sleep(transform);
                        SleepOverlayHandler.Instance.EnableOverlay();
                    }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (timeSystem.isSleeping)
                {
                    timeSystem.SleepCancel();
                }
                SleepOverlayHandler.Instance.DisableOverlay();
            }
            
        }

    }
}

