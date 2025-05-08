using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _targetSlider;
    [SerializeField] private TMP_Text _targetText;

    [SerializeField] private Slider _totalSlider;
    [SerializeField] private TMP_Text _totalText;

    [SerializeField] private Slider _timerSlider;
    [SerializeField] private TMP_Text _timerText;

    [SerializeField] private TMP_Text _restartText;

    private FiguresSpawnManager _spawnMgr;
    private int _totalCollected;
    private const float TimeLimit = 120f;
    private int _targetTotal;

    private void Awake()
    {
        _spawnMgr = FiguresSpawnManager.Instance;
    }

    private void Start()
    {
        var entry = _spawnMgr.SpawnCountsList
            .First(e => e.Type == _spawnMgr.TargetType);
        _targetTotal = entry.Count;

        _targetSlider.minValue = 0;
        _targetSlider.maxValue = _targetTotal;
        _targetSlider.value = 0;
        _targetText.text = $"{GetRussianName(_spawnMgr.TargetType)}: 0 из {_targetTotal}";

        int total = _spawnMgr.TotalToSpawn;
        _totalSlider.minValue = 0;
        _totalSlider.maxValue = total;
        _totalSlider.value = 0;
        _totalText.text = $"Всего: 0 из {total}";

        _timerSlider.minValue = 0;
        _timerSlider.maxValue = TimeLimit;
        float elapsed = Mathf.Min(GameTimer.Instance.ElapsedTime, TimeLimit);
        _timerSlider.value = elapsed;
        UpdateTimerText(elapsed);
    }

    private void OnEnable()
    {
        CollectManager.Instance.OnCountChanged += CountChanged;
        GameTimer.Instance.OnTimeChanged += UpdateTimerText;
    }

    private void OnDisable()
    {
        CollectManager.Instance.OnCountChanged -= CountChanged;
        GameTimer.Instance.OnTimeChanged -= UpdateTimerText;
    }

    private void CountChanged(GeometryFigures type, int newCount)
    {
        if (type == _spawnMgr.TargetType)
        {
            _targetSlider.value = newCount;
            _targetText.text = $"{GetRussianName(type)}: {newCount} из {_targetTotal}";
            if (newCount >= _targetTotal)
            {
                _targetText.fontStyle = FontStyles.Italic;
                _targetText.color = Color.gray;
                _targetText.text = "Выполнено!";
                _targetSlider.fillRect.GetComponent<Image>().color = Color.green;
            }
        }

        _totalCollected++;
        _totalSlider.value = _totalCollected;
        _totalText.text = $"Всего: {_totalCollected} из {_spawnMgr.TotalToSpawn}";
        if (_totalCollected >= _spawnMgr.TotalToSpawn)
        {
            _totalText.fontStyle = FontStyles.Italic;
            _totalText.color = Color.gray;
            _totalText.text = "Выполнено!";
            _totalSlider.fillRect.GetComponent<Image>().color = Color.green;
        }
    }

    private void UpdateTimerText(float elapsed)
    {
        float t = Mathf.Min(elapsed, TimeLimit);
        _timerSlider.value = t;
        int minutes = (int)(t / 60);
        int seconds = (int)(t % 60);
        _timerText.text = $"Проведите 2 минуты в игре! Текущее: {minutes:00}:{seconds:00}";
        if (t >= TimeLimit)
        {
            _timerText.fontStyle = FontStyles.Italic;
            _timerText.color = Color.gray;
            _timerText.text = "Выполнено!";
            _timerSlider.fillRect.GetComponent<Image>().color = Color.green;
        }
    }

    private string GetRussianName(GeometryFigures type)
    {
        switch (type)
        {
            case GeometryFigures.Cube: return "Кубы";
            case GeometryFigures.Sphere: return "Сферы";
            case GeometryFigures.Cylinder: return "Цилиндры";
            default: return type.ToString();
        }
    }

    public void ActivateRestartText()
    {
        _restartText.gameObject.SetActive(true);
    }
}
