
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Level/ItemData")]
public class ItemData : SpawnableObjectData
{
    [SerializeField] private float spawnOffsetY = 0.15f; // Y offset above platform

    public float SpawnOffsetY => spawnOffsetY;
}


//item data for Coins and gems