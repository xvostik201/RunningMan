using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct FigureEntry
{
    public GeometryFigures Type;
    public GeometryCollectable Prefab;
}

[Serializable]
public struct SpawnCountEntry
{
    public GeometryFigures Type;
    public int Count;
}

public class FiguresSpawnManager : MonoBehaviour
{
    public static FiguresSpawnManager Instance { get; private set; }

    [SerializeField] private FigureEntry[] _figureEntries;

    [SerializeField] private int _spawnCount = 10;
    [SerializeField] private Vector3 _areaMin = new Vector3(-5, 0, -5);
    [SerializeField] private Vector3 _areaMax = new Vector3(5, 0, 5);

    [SerializeField] private List<SpawnCountEntry> _spawnCountsList;

    private Dictionary<GeometryFigures, GeometryCollectable> _lookup;
    private Dictionary<GeometryFigures, int> _spawnedCounts;
    public IReadOnlyList<SpawnCountEntry> SpawnCountsList => _spawnCountsList;
    public GeometryFigures TargetType { get; private set; }
    public int TotalToSpawn => _spawnCount;
    public int AveragePerType => Mathf.CeilToInt((float)_spawnCount / _lookup.Count);
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _lookup = _figureEntries
            .ToDictionary(e => e.Type, e => e.Prefab);

        _spawnedCounts = _figureEntries
            .Select(e => e.Type)
            .Distinct()
            .ToDictionary(t => t, t => 0);

        var types = _lookup.Keys.ToArray();
        TargetType = types[UnityEngine.Random.Range(0, types.Length)];
    }

    private void Start()
    {
        SpawnFigures();
        UpdateSpawnCountList();
    }

    public void SpawnFigures()
    {
        var types = _lookup.Keys.ToArray();

        for (int i = 0; i < _spawnCount; i++)
        {
            var randomType = types[UnityEngine.Random.Range(0, types.Length)];
            var prefab = _lookup[randomType];

            Vector3 pos = new Vector3(
                UnityEngine.Random.Range(_areaMin.x, _areaMax.x),
                UnityEngine.Random.Range(_areaMin.y, _areaMax.y),
                UnityEngine.Random.Range(_areaMin.z, _areaMax.z)
            );

            Instantiate(prefab, pos, Quaternion.identity, transform);

            _spawnedCounts[randomType]++;
        }

        UpdateSpawnCountList();
    }
    private void UpdateSpawnCountList()
    {
        _spawnCountsList = new List<SpawnCountEntry>
        {
            new SpawnCountEntry
            {
                Type  = TargetType,
                Count = _spawnedCounts[TargetType]
            }
        };
    }
    public int GetSpawnedCount(GeometryFigures type)
        => _spawnedCounts.TryGetValue(type, out var c) ? c : 0;
}
