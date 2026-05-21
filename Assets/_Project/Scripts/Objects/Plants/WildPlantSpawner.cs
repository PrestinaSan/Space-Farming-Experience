using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WildPlantSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Biome
    {
        public string biomeName;
        public Tilemap tilemap;
        public List<WildPlantData> plantTypes;
    }

    [SerializeField] private List<Biome> biomes;
    [SerializeField] private GameObject wildPlantPrefab;
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

        foreach (var plant in biome.plantTypes)
        {
            parentObj = new GameObject(plant.name + "s");
            SpawnPlantType(plant, bounds, biome.tilemap);
        }
    }

    void SpawnPlantType(WildPlantData plant, BoundsInt bounds, Tilemap tilemap)
    {
        for (int x = bounds.xMin; x <= bounds.xMax - plant.width; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax - plant.height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                if (!CanPlace(pos, plant, tilemap))
                    continue;

                if (Random.value < plant.spawnRate)
                {
                    SpawnAt(pos, plant, tilemap);
                }
            }
        }
    }

    bool CanPlace(Vector3Int origin, WildPlantData plant, Tilemap tilemap)
    {
        for (int x = 0; x < plant.width; x++)
        {
            for (int y = 0; y < plant.height; y++)
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

    void SpawnAt(Vector3Int pos, WildPlantData plant, Tilemap tilemap)
    {
        Vector3 worldPos = tilemap.CellToWorld(pos);

        worldPos += new Vector3(plant.width * tilemap.cellSize.x, 0, 0);
        worldPos.z = 0;

        GameObject obj = Instantiate(wildPlantPrefab, worldPos, Quaternion.identity);
        obj.GetComponent<WildPlant>().Initialize(plant);
        obj.name = plant.name;
        obj.transform.parent = parentObj.transform;

        MarkOccupied(pos, plant);
    }

    void MarkOccupied(Vector3Int origin, WildPlantData plant)
    {
        for (int x = 0; x < plant.width; x++)
        {
            for (int y = 0; y < plant.height; y++)
            {
                occupiedTiles.Add(new Vector3Int(origin.x + x, origin.y + y, 0));
            }
        }
    }
}