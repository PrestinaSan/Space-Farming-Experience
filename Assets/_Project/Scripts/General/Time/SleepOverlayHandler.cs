using UnityEngine;

public class SleepOverlayHandler : MonoBehaviour
{
    public static SleepOverlayHandler Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        DisableOverlay();
    }
    public void DisableOverlay()
    {
        Instance.gameObject.SetActive(false);
    }
    public void EnableOverlay()
    {
        Instance.gameObject.SetActive(true);
    }
}
