using System;
using System.Collections.Generic;
using UnityEngine;
public class CollectManager : MonoBehaviour
{
    public static CollectManager Instance { get; private set; }

    public event Action<GeometryFigures, int> OnCountChanged;

    public event Action<BonusType, int> OnBonusCollected;

    private Dictionary<GeometryFigures, int> _geometryCounts = new();
    private Dictionary<BonusType, int> _bonusCounts = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            foreach (GeometryFigures g in Enum.GetValues(typeof(GeometryFigures)))
                _geometryCounts[g] = 0;
            foreach (BonusType b in Enum.GetValues(typeof(BonusType)))
                _bonusCounts[b] = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RaiseGeometryCollected(GeometryFigures type)
    {
        _geometryCounts[type]++;
        OnCountChanged?.Invoke(type, _geometryCounts[type]);
    }

    public void RaiseBonusCollected(BonusType type)
    {
        _bonusCounts[type]++;
        OnBonusCollected?.Invoke(type, _bonusCounts[type]);
    }

    public int GetGeometryCount(GeometryFigures type)
        => _geometryCounts.TryGetValue(type, out var c) ? c : 0;

    public int GetBonusCount(BonusType type)
        => _bonusCounts.TryGetValue(type, out var c) ? c : 0;
}
