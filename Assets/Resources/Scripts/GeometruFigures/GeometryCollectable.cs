using UnityEngine;

public enum GeometryFigures { Cube, Sphere, Cylinder }

public class GeometryCollectable : Collectable
{
    [SerializeField] private GeometryFigures _figureType;
    public GeometryFigures FigureType => _figureType;

    public override void Collect()
    {
        base.Collect();
        CollectManager.Instance.RaiseGeometryCollected(_figureType);
    }
}
