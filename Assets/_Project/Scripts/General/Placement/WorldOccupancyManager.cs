using System.Collections.Generic;
using UnityEngine;

public class WorldOccupancyManager : MonoBehaviour
{
    public static WorldOccupancyManager Instance { get; private set; }

    private HashSet<Vector3Int> occupiedCells = new();

    private void Awake() => Instance = this;

    public bool IsOccupied(Vector3Int cellPos) => occupiedCells.Contains(cellPos);
    public void Occupy(Vector3Int cellPos) => occupiedCells.Add(cellPos);
    public void Free(Vector3Int cellPos) => occupiedCells.Remove(cellPos);
}