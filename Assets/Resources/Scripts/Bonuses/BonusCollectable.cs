using UnityEngine;
public enum BonusType
{
    SpeedBoost,
}

[RequireComponent(typeof(Collider))]
public class BonusCollectable : Collectable
{
    [SerializeField] private BonusType _bonusType;

    [SerializeField] private float _rotationSpeed = 80;
    public BonusType BonusType => _bonusType;

    public override void Collect()
    {
        base.Collect();
        CollectManager.Instance.RaiseBonusCollected(_bonusType);
    }

    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
}
