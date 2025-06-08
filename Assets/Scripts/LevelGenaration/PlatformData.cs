using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "Level/PlatformData")]
public class PlatformData : SpawnableObjectData
{
    [SerializeField] private bool isRegularPlatform = true; // True for regular, false for special platforms
    [SerializeField] private float minHeightThreshold; // Min Y for special platforms

    public bool IsRegularPlatform => isRegularPlatform;
    public float MinHeightThreshold => minHeightThreshold;
}
