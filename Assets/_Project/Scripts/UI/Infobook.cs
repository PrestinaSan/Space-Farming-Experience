using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Infobook : MonoBehaviour
{
    public static Infobook Instance { get; private set; }
    [Tooltip("Inventory UI Gameobject")]
    [SerializeField] private GameObject inventoryObj;

    [Tooltip("Infobook UI GameObject")]
    [SerializeField] private GameObject infobookObj;

    [Tooltip("List of Items")]
    [SerializeField] private Item[] items;

    [Tooltip("List of Plants")]
    [SerializeField] private WildPlantData[] plants;

    [Tooltip("List of Animals")]
    [SerializeField] private AnimalData[] animals;

    [Tooltip("List of Item Sprites in the infobook")]
    [SerializeField] private Image[] itemImageList;

    [Tooltip("List of Plant Sprites in the infobook")]
    [SerializeField] private Image[] plantImageList;

    [Tooltip("List of Animal Sprites in the infobook")]
    [SerializeField] private Image[] animalImageList;

    private Item currentlyOpenItem;
    private Dictionary<Item, string> customItemNames = new();

    private WildPlantData currentlyOpenPlant;
    private Dictionary<WildPlantData, string> customPlantNames = new();


    private AnimalData currentlyOpenAnimal;
    private Dictionary<AnimalData, string> customAnimalNames = new();

    public HashSet<Item> unlockedItems = new();
    public HashSet<WildPlantData> unlockedPlants = new();
    public HashSet<AnimalData> unlockedAnimals = new();

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            Image _sprite = itemImageList[i];
            _sprite.sprite = items[i].image;
            if (unlockedItems.Contains(items[i]) == false)
            {
                _sprite.color = Color.black;
            }
            BookInfoHandler bookInfo = _sprite.GetComponent<BookInfoHandler>();
            bookInfo.InitializeData(items[i]);
        }
        for (int i = 0; i < plants.Length; i++)
        {
            Image _sprite = plantImageList[i];
            _sprite.sprite = plants[i].sprite;
            if (unlockedPlants.Contains(plants[i]) == false)
            {
                _sprite.color = Color.black;
            }
            BookInfoHandler bookInfo = _sprite.GetComponent<BookInfoHandler>();
            bookInfo.InitializeData(null,plants[i]);
        }
        for (int i = 0; i < animals.Length; i++)
        {
            Image _sprite = animalImageList[i];
            _sprite.sprite = animals[i].sprite;
            if (unlockedAnimals.Contains(animals[i]) == false)
            {
                _sprite.color = Color.black;
            }
            BookInfoHandler bookInfo = _sprite.GetComponent<BookInfoHandler>();
            bookInfo.InitializeData(null,null,animals[i]);
        }

    }

    /// <summary>
    /// Mark item as unlocked in infobook
    /// </summary>
    /// <param name="item"></param>
    public void UnlockItem(Item item)
    {
        if (unlockedItems.Add(item))
        {
            RefreshUnlockedStatus();
        }
    }

    /// <summary>
    /// Mark plant as unlocked in infobook
    /// </summary>
    /// <param name="plant"></param>
    public void UnlockPlant(WildPlantData plant)
    {
        if (unlockedPlants.Add(plant))
        {
            RefreshUnlockedStatus();
        }
    }

    /// <summary>
    /// Mark animal as unlocked in infobook
    /// </summary>
    /// <param name="animal"></param>
    public void UnlockAnimal(AnimalData animal)
    {
        if (unlockedAnimals.Add(animal))
        {
            RefreshUnlockedStatus();
        }
    }


    /// <summary>
    /// Refreshes the infobook sprites to show things that are unlocked
    /// </summary>
    private void RefreshUnlockedStatus()
    {
        for (int i = 0; i < itemImageList.Length; i++)
        {
            Image _sprite = itemImageList[i];
            if (unlockedItems.Contains(items[i]))
            {
                _sprite.color = Color.white;
            }
        }
        for (int i = 0; i < plantImageList.Length; i++)
        {
            Image _sprite = plantImageList[i];
            if (unlockedPlants.Contains(plants[i]))
            {
                _sprite.color = Color.white;
            }
        }
        for (int i = 0; i < animalImageList.Length; i++)
        {
            Image _sprite = animalImageList[i];
            if (unlockedAnimals.Contains(animals[i]))
            {
                _sprite.color = Color.white;
            }
        }
    }

    /// <summary>
    /// Turns the infobook UI on and off
    /// </summary>
    public void ToggleInfoBook()
    {
        if (inventoryObj.activeSelf == true) return;
        bool isOpen = infobookObj.activeSelf;
        infobookObj.SetActive(!isOpen);
    }

    public string GetItemName(Item item)
    {
        if (customItemNames.ContainsKey(item))
            return customItemNames[item];

        return "Click to name Item";
    }

    public void SetItemName(Item item, string name)
    {
        customItemNames[item] = name;
    }

    public void SetCurrentItem(Item item)
    {
        currentlyOpenItem = item;
    }


    public string GetPlantName(WildPlantData plant)
    {
        if (customPlantNames.ContainsKey(plant))
            return customPlantNames[plant];

        return "Click to name Item";
    }

    public void SetPlantName(WildPlantData plant, string name)
    {
        customPlantNames[plant] = name;
    }

    public void SetCurrentPlant(WildPlantData plant)
    {
        currentlyOpenPlant = plant;
    }


    public string GetAnimalName(AnimalData animal)
    {
        if (customAnimalNames.ContainsKey(animal))
            return customAnimalNames[animal];

        return "Click to name Animal";
    }

    public void SetAnimalName(AnimalData animal, string name)
    {
        customAnimalNames[animal] = name;
    }

    public void SetCurrentAnimal(AnimalData animal)
    {
        currentlyOpenAnimal = animal;
    }

    public void OnNameChanged(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return;

        if (currentlyOpenItem != null)
        {
            customItemNames[currentlyOpenItem] = newName;
            return;
        }

        if (currentlyOpenPlant != null)
        {
            customPlantNames[currentlyOpenPlant] = newName;
            return;
        }

        if (currentlyOpenAnimal != null)
        {
            customAnimalNames[currentlyOpenAnimal] = newName;
            return;
        }
    }

    public void ClearCurrentItem()
    {
        currentlyOpenItem = null;
    }

    public void ClearCurrentPlant()
    {
        currentlyOpenPlant = null;
    }

    public void ClearCurrentAnimal()
    {
        currentlyOpenAnimal = null;
    }
}
