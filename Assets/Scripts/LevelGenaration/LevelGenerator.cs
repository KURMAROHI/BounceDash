using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private PlatformData _regularPlatformData; // Regular platforms
    [SerializeField] private PlatformData _specialPlatformData; // Special platforms
    [SerializeField] private ItemData _coinData; // Coins
    [SerializeField] private ItemData _gemData; // Gems
    [SerializeField] private ObstacleData _staticObstacleData; // Static obstacles
    [SerializeField] private ObstacleData _horizontalFlyData; // Horizontal flies
    [SerializeField] private float _lastMaxYheight = 2f;
    [SerializeField] private float _xMinDistance = 1f;
    [SerializeField] private float _xMaxDistance = 2f;
    [SerializeField] private float _yMinDistance = 0.5f;
    [SerializeField] private float _yMaxDistance = 1.5f;
    [SerializeField] private float _thresoldValaue = 5f;
    [SerializeField] private List<GameObject> _activeObjects = new List<GameObject>();
    [SerializeField] private Transform _parentObject;
    private Camera _mainCamera;


    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        UIManager.Instance.ResetUIBackGround += ResetBackGround;
        GameOverUIPopUp.OnGenarateLevel += OnGenarateLevel;
    }

    private void Start()
    {
        Debug.Log($"_mainCamera|Aspect: {_mainCamera.aspect}|OrthoSize: {_mainCamera.orthographicSize}|Width: {_mainCamera.orthographicSize * _mainCamera.aspect}");
        SpwnObjects(-_mainCamera.orthographicSize);
    }

    private void Update()
    {
        float cameraTop = _mainCamera.orthographicSize + _mainCamera.transform.position.y;
        if (_lastMaxYheight < cameraTop + _thresoldValaue)
        {
            SpwnObjects(_lastMaxYheight);
        }

        DespawnOldObjects();
    }

    private void OnDisable()
    {
        UIManager.Instance.ResetUIBackGround -= ResetBackGround;
        GameOverUIPopUp.OnGenarateLevel -= OnGenarateLevel;
    }

    private void ResetBackGround(object sender, EventArgs e)
    {
        Debug.Log("Set up level Calling");
        foreach (Transform item in _parentObject.transform)
        {
            Destroy(item.gameObject);
        }
        _activeObjects.Clear();
    }

    private void OnGenarateLevel(object sender, EventArgs e)
    {
        SpwnObjects(-_mainCamera.orthographicSize);
    }

    private void SpwnObjects(float lastplatFromYValue)
    {
        float xMax = _mainCamera.orthographicSize * _mainCamera.aspect;
        float yMax = _mainCamera.orthographicSize;
        float currentY = lastplatFromYValue;

        _lastMaxYheight = currentY;

        while (_lastMaxYheight < yMax + _thresoldValaue + _mainCamera.transform.position.y)
        {
            // Spawn platform
            float newY = GetNextYPosition(currentY);
            float randomX = GetRandomXPosition(xMax);
            bool isRegularPlatform;
            Transform platform = SpawnPlatform(randomX, newY, xMax, yMax, out isRegularPlatform);
            _activeObjects.Add(platform.gameObject);

            // Spawn coin or gem on regular platforms
            if (isRegularPlatform)
            {
                SpawnCoinOrGem(platform, randomX);
            }

            // Spawn obstacle
            SpawnObstacle(xMax, newY);

            // Update Y values
            currentY = newY;
            if (_lastMaxYheight < newY)
            {
                _lastMaxYheight = newY;
            }
        }
    }

    private float GetRandomXPosition(float xMax)
    {
        return UnityEngine.Random.Range(-xMax + _xMinDistance, xMax - _xMinDistance);
    }

    private float GetNextYPosition(float currentY)
    {
        float noise = Mathf.PerlinNoise(currentY * 0.1f, Time.time * 0.1f);
        float mappedYDistance = Mathf.Lerp(_yMinDistance, _yMaxDistance, noise);
        return currentY + mappedYDistance;
    }

    private Transform SpawnPlatform(float x, float y, float xMax, float yMax, out bool isRegularPlatform)
    {
        PlatformData platformData = _regularPlatformData;
        isRegularPlatform = true;

        if (_specialPlatformData != null && UnityEngine.Random.value * 100f < _specialPlatformData.SpawnChance
            && y > yMax - _specialPlatformData.MinHeightThreshold)
        {
            platformData = _specialPlatformData;
            isRegularPlatform = false;
        }

        if (platformData.Prefabs.Length == 0) return null;

        Transform prefabToSpawn = platformData.Prefabs[UnityEngine.Random.Range(0, platformData.Prefabs.Length)];
        float platformHalfWidth = prefabToSpawn.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
        x = Mathf.Clamp(x, -xMax + platformHalfWidth, xMax - platformHalfWidth);

        Transform platform = Instantiate(prefabToSpawn, _parentObject);
        platform.position = new Vector3(x, y, 0);
        return platform;
    }

    private void SpawnCoinOrGem(Transform platform, float platformX)
    {
        if (_coinData == null && _gemData == null) return;

        SpriteRenderer platformRenderer = platform.GetComponentInChildren<SpriteRenderer>();
        float platformTopY = platform.position.y + platformRenderer.bounds.extents.y;
        float platformHalfWidth = platformRenderer.bounds.extents.x;
        float randomOffsetX = UnityEngine.Random.Range(-platformHalfWidth * 0.8f, platformHalfWidth * 0.8f);

        ItemData itemData = null;
        if (_coinData != null && UnityEngine.Random.value * 100f < _coinData.SpawnChance)
        {
            itemData = _coinData;
        }
        else if (_gemData != null && UnityEngine.Random.value * 100f < _gemData.SpawnChance)
        {
            itemData = _gemData;
        }

        if (itemData != null && itemData.Prefabs.Length > 0)
        {
            Transform prefabToSpawn = itemData.Prefabs[UnityEngine.Random.Range(0, itemData.Prefabs.Length)];
            Vector3 itemPosition = new Vector3(platformX + randomOffsetX, platformTopY + itemData.SpawnOffsetY, 0);
            Transform item = Instantiate(prefabToSpawn, itemPosition, Quaternion.identity, _parentObject);
            item.localPosition = new Vector3(item.localPosition.x, item.localPosition.y, 9f);
            _activeObjects.Add(item.gameObject);
        }
    }

    private void SpawnObstacle(float xMax, float baseY)
    {
        ObstacleData obstacleData = null;
        Vector3 obstaclePosition = Vector3.zero;

        if (_staticObstacleData != null && UnityEngine.Random.value * 100f < _staticObstacleData.SpawnChance)
        {
            obstacleData = _staticObstacleData;
            obstaclePosition = new Vector3(
                GetRandomXPosition(xMax),
                baseY + _staticObstacleData.SpawnOffsetY + UnityEngine.Random.Range(0f, _yMaxDistance),
                0
            );
        }
        else if (_horizontalFlyData != null && UnityEngine.Random.value * 100f < _horizontalFlyData.SpawnChance)
        {
            obstacleData = _horizontalFlyData;
            bool fromLeft = UnityEngine.Random.value < 0.5f;
            obstaclePosition = new Vector3(
                fromLeft ? -xMax - 0.5f : xMax + 0.5f,
                baseY + _horizontalFlyData.SpawnOffsetY + UnityEngine.Random.Range(0f, _yMaxDistance),
                0
            );
        }

        if (obstacleData != null && obstacleData.Prefabs.Length > 0 && !IsOverlappingWithPlatform(obstaclePosition, obstacleData))
        {
            Transform prefabToSpawn = obstacleData.Prefabs[UnityEngine.Random.Range(0, obstacleData.Prefabs.Length)];
            Transform obstacle = Instantiate(prefabToSpawn, obstaclePosition, Quaternion.identity, _parentObject);
            _activeObjects.Add(obstacle.gameObject);
        }
    }

    private bool IsOverlappingWithPlatform(Vector3 position, ObstacleData obstacleData)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, obstacleData.SpawnCheckRadius, obstacleData.PlatformLayerMask);
        return colliders.Length > 0;
    }

    private void DespawnOldObjects()
    {
        float cameraBottom = _mainCamera.transform.position.y - _mainCamera.orthographicSize;

        for (int i = _activeObjects.Count - 1; i >= 0; i--)
        {
            if (_activeObjects[i] != null && _activeObjects[i].transform.position.y < cameraBottom)
            {
                Destroy(_activeObjects[i]);
                _activeObjects.RemoveAt(i);
            }
        }
    }



}
