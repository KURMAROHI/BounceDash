using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Level/ObstacleData")]
public class ObstacleData : SpawnableObjectData
{
    [SerializeField] private bool isStatic = true; // True for static, false for Dynamic objects
    [SerializeField] private float spawnOffsetY = 1f; // Min Y offset above platforms
    [SerializeField] private float spawnCheckRadius = 1f; // Radius for overlap check
    [SerializeField] private LayerMask platformLayerMask; // Layer for platforms

    public bool IsStatic => isStatic;
    public float SpawnOffsetY => spawnOffsetY;
    public float SpawnCheckRadius => spawnCheckRadius;
    public LayerMask PlatformLayerMask => platformLayerMask;
}


//For ObStacles 