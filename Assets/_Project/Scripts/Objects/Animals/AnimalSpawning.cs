using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalSpawning : MonoBehaviour
{
    [System.Serializable]
    public class Biome
    {
        public string biomeName;
        public Tilemap tilemap;
        public List<AnimalData> animalTypes;
    }

    [SerializeField] private List<Biome> biomes;
    [SerializeField] private GameObject animalPrefab;
    private GameObject parentObj;

    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

    void Start()
    {
        foreach (var biome in biomes)
        {
            SpawnBiome(biome);
        }
    }

    void SpawnBiome(Biome biome)
    {
        if (biome.tilemap == null) return;

        BoundsInt bounds = biome.tilemap.cellBounds;

        foreach (var animal in biome.animalTypes)
        {
            parentObj = new GameObject(animal.name + "s");
            SpawnAnimalType(animal, bounds, biome.tilemap);
        }
    }

    void SpawnAnimalType(AnimalData animal, BoundsInt bounds, Tilemap tilemap)
    {
        for (int x = bounds.xMin; x <= bounds.xMax - animal.width; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax - animal.height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                if (!CanPlace(pos, animal, tilemap))
                    continue;

                if (Random.value < animal.spawnRate)
                {
                    SpawnAt(pos, animal, tilemap);
                }
            }
        }
    }

    bool CanPlace(Vector3Int origin, AnimalData animal, Tilemap tilemap)
    {
        for (int x = 0; x < animal.width; x++)
        {
            for (int y = 0; y < animal.height; y++)
            {
                Vector3Int checkPos = new Vector3Int(origin.x + x, origin.y + y, 0);

                if (!tilemap.HasTile(checkPos))
                    return false;

                if (occupiedTiles.Contains(checkPos))
                    return false;
            }
        }

        return true;
    }

    void SpawnAt(Vector3Int pos, AnimalData animal, Tilemap tilemap)
    {
        Vector3 worldPos = tilemap.CellToWorld(pos);

        worldPos += new Vector3(animal.width * tilemap.cellSize.x, 0, 0);
        worldPos.z = 0;

        GameObject obj = Instantiate(animalPrefab, worldPos, Quaternion.identity);
        obj.GetComponent<AnimalBehavior>().Initialize(animal);
        obj.name = animal.name;
        obj.transform.parent = parentObj.transform;

        MarkOccupied(pos, animal);
    }

    void MarkOccupied(Vector3Int origin, AnimalData animal)
    {
        for (int x = 0; x < animal.width; x++)
        {
            for (int y = 0; y < animal.height; y++)
            {
                occupiedTiles.Add(new Vector3Int(origin.x + x, origin.y + y, 0));
            }
        }
    }
}