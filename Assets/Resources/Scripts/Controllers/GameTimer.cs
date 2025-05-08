using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    public float ElapsedTime { get; private set; }

    public event Action<float> OnTimeChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ElapsedTime = GameTimeManager.LoadTime();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;
        OnTimeChanged?.Invoke(ElapsedTime);
    }

    private void OnDisable()
    {
        GameTimeManager.SaveTime(ElapsedTime);
    }
}
