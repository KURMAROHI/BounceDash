using UnityEngine;

public abstract class SpawnableObjectData : ScriptableObject
{
    [SerializeField] protected Transform[] prefabs; // Array of prefabs to spawn
    [SerializeField, Range(0f, 100f)] protected float spawnChance; // Chance to spawn (0-100%)

    public Transform[] Prefabs => prefabs;
    public float SpawnChance => spawnChance;
}
