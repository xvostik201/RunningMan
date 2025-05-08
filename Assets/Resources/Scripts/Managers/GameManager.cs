using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Animator _victoryAnimator;
    [SerializeField] private UIManager _uiManager;

    private int _tasksCompleted;
    private bool _hasWon;
    public bool HasWon => _hasWon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); return; }
    }

    private void OnEnable()
    {
        CollectManager.Instance.OnCountChanged += CountChanged;
        InputManager.Instance.OnRestart += RestartGame;
    }

    private void OnDisable()
    {
        if (CollectManager.Instance != null)
            CollectManager.Instance.OnCountChanged -= CountChanged;
        InputManager.Instance.OnRestart -= RestartGame;
    }

    private void CountChanged(GeometryFigures type, int newCount)
    {
        if (_hasWon) return;

        _tasksCompleted++;
        int total = FiguresSpawnManager.Instance.TotalToSpawn;
        if (_tasksCompleted >= total) Win();
    }

    private void Win()
    {
        _hasWon = true;
        _uiManager.ActivateRestartText();
        _playerController.enabled = false;
        _victoryAnimator.SetTrigger("Victory");
    }

    private void RestartGame()
    {
        if (!_hasWon) return;

        _hasWon = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
