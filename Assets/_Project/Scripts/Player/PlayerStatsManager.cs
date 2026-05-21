using System;
using TS;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Attributes")]
    private int currentHealthiness; // General healthiness
    private int currentHunger; // Food
    private int currentHydration; // Drinks
    private readonly int maxHealthiness = 100;
    private readonly int maxHunger = 100;
    private readonly int maxHydration = 100;

    [Header("Stat Intervals (in-game minutes)")]
    [SerializeField] private int hungerDecreaseInterval = 60;
    [SerializeField] private int hydrationDecreaseInterval = 30;
    [SerializeField] private int starvationInterval = 20;
    [SerializeField] private int dehydrationInterval = 10;
    [SerializeField] private int healthinessGain = 30;


    [Header("References")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider hydrationSlider;
    public enum Stat
    {
        healthiness,
        hunger,
        hydration
    }
    void Start()
    {
        InitializeSliders();
        UpdateStat(Stat.healthiness,80);
        UpdateStat(Stat.hunger,70);
        UpdateStat(Stat.hydration,100);
        TimeSystem.TimeSystemChanged += PassiveTimer;
    }

    private void OnDestroy()
    {
        TimeSystem.TimeSystemChanged -= PassiveTimer;
    }


    void Update()
    {
        if (currentHealthiness <= 40)
            movement.ChangeMovementSpeed(movement.BaseMovementSpeed * 0.5f);
        else if (currentHealthiness >= 70)
        {
            movement.ChangeMovementSpeed(movement.BaseMovementSpeed * 1.5f);
        }
        else
            movement.ChangeMovementSpeed(movement.BaseMovementSpeed);
    }


    public void UpdateStat(Stat stat, int amount)
    {
        switch (stat)
        {
            case Stat.healthiness:
                currentHealthiness = Mathf.Clamp(currentHealthiness + amount, 0, maxHealthiness);
                healthSlider.value = currentHealthiness;
                break;

            case Stat.hunger:
                currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
                hungerSlider.value = currentHunger;
                break;

            case Stat.hydration:
                currentHydration = Mathf.Clamp(currentHydration + amount, 0, maxHydration);
                hydrationSlider.value = currentHydration;
                break;
        }
    }
    private void InitializeSliders()
    {
        healthSlider.maxValue = maxHealthiness;
        healthSlider.value = currentHealthiness;
        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = currentHunger;
        hydrationSlider.maxValue = maxHydration;
        hydrationSlider.value = currentHydration;
    }

    private void PassiveTimer(object sender, TimeSpan time)
    {
        int totalMinutes = (time.Hours * 60) + time.Minutes;

        if (totalMinutes % hungerDecreaseInterval == 0)
            UpdateStat(Stat.hunger, -5);

        if (totalMinutes % hydrationDecreaseInterval == 0)
            UpdateStat(Stat.hydration, -5);

        if (currentHunger <= 30 && totalMinutes % starvationInterval == 0)
            UpdateStat(Stat.healthiness, -2);

        if (currentHydration <= 30 && totalMinutes % dehydrationInterval == 0)
            UpdateStat(Stat.healthiness, -3);

        if (totalMinutes % healthinessGain == 0 && currentHunger >= 70 && currentHydration >= 70)
        {
            if (currentHunger >= 90 && currentHydration >= 90)
            {
                UpdateStat(Stat.healthiness, 5);
            }
            else
            {
                UpdateStat(Stat.healthiness, 2);
            }
        }
    }
}
