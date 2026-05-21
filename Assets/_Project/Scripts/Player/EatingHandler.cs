using UnityEngine;
using UnityEngine.UI;

public class EatingHandler : MonoBehaviour
{
    [SerializeField] private PlayerStatsManager playerStats;
    [SerializeField] private Button eatButton;
    private void Update()
    {
        Item item = InventoryManager.Instance.GetSelectedItem(false);
        if (item != null && item.itemType.HasFlag(Item.ItemType.Food))
        {
            eatButton.gameObject.SetActive(true);
        }
        else
        {
            eatButton.gameObject.SetActive(false);
        }
    }

    public void OnEat()
    {
        Item foodItem =  InventoryManager.Instance.GetSelectedItem(true);
        playerStats.UpdateStat(PlayerStatsManager.Stat.healthiness, ItemStatManager.Instance.GetHealthiness(foodItem));
        playerStats.UpdateStat(PlayerStatsManager.Stat.hunger, ItemStatManager.Instance.GetHunger(foodItem));
        playerStats.UpdateStat(PlayerStatsManager.Stat.hydration, ItemStatManager.Instance.GetHydration(foodItem));
    }

}
