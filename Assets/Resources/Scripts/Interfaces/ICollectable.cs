using System;
using UnityEngine;

public interface ICollectable
{
    void Collect();
}

public abstract class Collectable : MonoBehaviour, ICollectable
{
    public event Action<ICollectable> OnCollected;

    public virtual void Collect()
    {
        OnCollected?.Invoke(this);
        AudioManager.Instance.PlayCollect();    
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out _))
            Collect();
    }
}
