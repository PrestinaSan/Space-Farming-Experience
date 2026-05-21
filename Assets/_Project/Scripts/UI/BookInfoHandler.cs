using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BookInfoHandler : MonoBehaviour
{
    private Item item;
    private WildPlantData plant;
    private AnimalData animal;
    [Header("Item specific references")]
    [SerializeField] private TMP_InputField itemName;
    [SerializeField] private TMP_Text specialText;
    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text statsOnConsumption;

    [Header("Plant specific references")]
    [SerializeField] private TMP_InputField plantName;
    [SerializeField] private TMP_Text biome;
    [SerializeField] private Transform plantDrops;
    [SerializeField] private Image farmableVariant;

    [Header("Animal specific references")]
    [SerializeField] private TMP_InputField animalName;
    [SerializeField] private Transform animalDrops;
    [SerializeField] private TMP_Text animalInfo;


    [Header("General references")]

    [Tooltip("Sprite for the object")]
    [SerializeField] private Image image;

    [Tooltip("Object description")]
    [SerializeField] private TMP_Text description;

    [Tooltip("The GameObject that contains the info of the main page")]
    [SerializeField] private GameObject mainPageObj;

    [Tooltip("The GameObject that contains the info of the object")]
    [SerializeField] private GameObject infoPageObj;

    [Tooltip("The button to go back to the main page")]
    [SerializeField] private GameObject backButton;


    public void InitializeData(Item _item = null, WildPlantData _plant = null, AnimalData _animal = null)
    {
        item = _item;
        plant = _plant;
        animal = _animal;
    }
    public void OnClick()
    {
        if (item != null)
        {
            BehaviorIfItem();
        }
        else if (plant != null)
        {
            BehaviorIfPlant();
        }
        else if (animal != null)
        {
            BehaviorIfAnimal();
        }
    }
    /// <summary>
    /// Shows the item UI Page
    /// </summary>
    private void BehaviorIfItem()
    {
        if (!Infobook.Instance.unlockedItems.Contains(item)) return;

        Infobook.Instance.SetCurrentItem(item);
        Infobook.Instance.ClearCurrentPlant();
        Infobook.Instance.ClearCurrentAnimal();

        infoPageObj.SetActive(true);
        backButton.SetActive(true);
        mainPageObj.SetActive(false);

        string storedName = Infobook.Instance.GetItemName(item);
        itemName.text = storedName;

        image.sprite = item.image;
        description.text = item.description;
        specialText.text = item.specialText;
        string types = "";
        bool isFood = false;
        foreach (Item.ItemType type in System.Enum.GetValues(typeof(Item.ItemType)))
        {
            if (type == Item.ItemType.None) continue;
            if (item.itemType.HasFlag(type))
            {
                types += type.ToString() + "\n";
                if (type == Item.ItemType.Food) isFood = true;
            }

        }
        type.text = types;
        if (isFood)
        {
            statsOnConsumption.text = "Properties: " + "\n";
            statsOnConsumption.text += "Healthiness: " + item.healthiness.ToString() + "\n";
            statsOnConsumption.text += "Hunger: " + item.hunger.ToString() + "\n";
            statsOnConsumption.text += "Hydration: " + item.hydration.ToString() + "\n";
        }
        else
        {
            statsOnConsumption.text = "Not Edible.";
        }
    }
    /// <summary>
    /// Shows the plant UI page
    /// </summary>
    private void BehaviorIfPlant()
    {
        if (!Infobook.Instance.unlockedPlants.Contains(plant)) return;

        Infobook.Instance.SetCurrentPlant(plant);
        Infobook.Instance.ClearCurrentItem();
        Infobook.Instance.ClearCurrentAnimal();

        infoPageObj.SetActive(true);
        backButton.SetActive(true);
        mainPageObj.SetActive(false);

        string storedName = Infobook.Instance.GetPlantName(plant);
        plantName.text = storedName;

        image.sprite = plant.sprite;
        description.text = plant.description;
        for (int i = 0; i < plantDrops.childCount; i++)
        {
            Image dropImage = plantDrops.GetChild(i).GetComponent<Image>();
            if (i < plant.drops.Length)
            {
                dropImage.sprite = plant.drops[i].image;
                dropImage.gameObject.SetActive(true);
            }
            else
            {
                dropImage.gameObject.SetActive(false);
                continue;
            }
        }
        farmableVariant.gameObject.SetActive(true);
        if (plant.farmableVariant != null)
        {
            farmableVariant.sprite = plant.farmableVariant; // placeholder
        }
        else
        {
            farmableVariant.gameObject.SetActive(false);
        }
        biome.text = "";
        foreach (var _biome in plant.biomes)
        {
            biome.text += _biome + "\n";
        }
    }


    /// <summary>
    /// Shows the Animal UI page
    /// </summary>
    private void BehaviorIfAnimal()
    {
        if (!Infobook.Instance.unlockedAnimals.Contains(animal)) return;

        Infobook.Instance.SetCurrentAnimal(animal);
        Infobook.Instance.ClearCurrentItem();
        Infobook.Instance.ClearCurrentPlant();

        infoPageObj.SetActive(true);
        backButton.SetActive(true);
        mainPageObj.SetActive(false);

        string storedName = Infobook.Instance.GetAnimalName(animal);
        animalName.text = storedName;
            
        image.sprite = animal.sprite;
        description.text = animal.description;
        for (int i = 0; i < animalDrops.childCount; i++)
        {
            Image dropImage = animalDrops.GetChild(i).GetComponent<Image>();
            if (i < animal.drops.Length)
            {
                dropImage.sprite = animal.drops[i].image;
                dropImage.gameObject.SetActive(true);
            }
            else
            {
                dropImage.gameObject.SetActive(false);
                continue;
            }
        }
        animalInfo.text = "Health: ";
        animalInfo.text += animal.health;
    }


    /// <summary>
    /// Used to change the name of items ingame
    /// </summary>
    /// <param name="newName"></param>
    public void OnNameChanged(string newName)
    {
        if (item == null) return;

        if (string.IsNullOrWhiteSpace(newName))
        {
            Infobook.Instance.SetItemName(item, "Click to name Item");
            itemName.text = "Click to name Item";
            return;
        }

        Infobook.Instance.SetItemName(item, newName);
    }

}
