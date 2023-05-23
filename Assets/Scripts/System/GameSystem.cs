using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    [SerializeField] private GameObject _surface;

    [Header("UI")]
    [SerializeField] private GameObject _canvasUI;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private float _stopTimeWhenLose;

    [Header("Selected Button")]
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _pauseButton;

    [Header("Difficulty")]
    [SerializeField] private MovingBackground[] _applyBackground;
    [SerializeField] private ObstacleSpawner _applySpawner;
    public float IncreaseEverySeconds = 10f;
    public float Multiplier = 2f;
    public int CappedAtWave = 5;

    public bool IsGameRunning { get; private set; }
    public bool IsGameEnded { get; private set; }

    public GameObject Surface { get { return _surface; } }

    private float _timer;
    private float _timeForNextWave;
    private int _currentWave;

    void Awake()
    {
        if (Instance != null) {
            Destroy(this);
        }

        Instance = this;
    }

    void Start()
    {
        PlayerBehaviour.OnPlayerDied += EndGame;

        SetupGameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameEnded) {
            if (MyInputAction.Pause) {
                if (IsGameRunning) {
                    Pause();
                } else {
                    Resume();
                }
            }
        } else {
            if (Input.anyKeyDown) {
                SetupGameStart();
            }
        }

        if (IsGameEnded || !IsGameRunning) {
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _timeForNextWave && _currentWave < CappedAtWave) {
            _timer = 0f;
            _timeForNextWave *= Multiplier;
            _currentWave++;

            foreach(var b in _applyBackground) {
                b?.IncreaseSpeed();
            }
            _applySpawner?.IncreaseSpeed();
        }
    }

    public void Resume() {
        _startPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _losePanel.SetActive(false);

        StartCoroutine(SetGameState(true));
    }

    public void Pause() {
        _startPanel.SetActive(false);
        _pausePanel.SetActive(true);
        _losePanel.SetActive(false);

        StartCoroutine(SetGameState(false));
        StartCoroutine(SelectButton(_pauseButton));
    }

    public void SetupGameStart() {
        IsGameRunning = false;
        Time.timeScale = 0f;

        _canvasUI.SetActive(true);
        _startPanel.SetActive(true);
        _pausePanel.SetActive(false);
        _losePanel.SetActive(false);
        IsGameEnded = false;

        StartCoroutine(SelectButton(_startButton));

        _timer = 0f;
        _timeForNextWave = IncreaseEverySeconds;
        _currentWave = 0;

        _applySpawner.Restart();

        foreach(var b in _applyBackground) {
            b?.Reset();
        }
    }

    public async void EndGame(GameObject player) {
        IsGameEnded = true;
        IsGameRunning = false;
        Time.timeScale = 0f;

        await Task.Delay(TimeSpan.FromSeconds(_stopTimeWhenLose));

        _startPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _losePanel.SetActive(true);
    }

    private IEnumerator SetGameState(bool isResuming) {
        yield return new WaitForEndOfFrame();

        if (isResuming) {
            IsGameRunning = true;
            Time.timeScale = 1f;
        } else {
            IsGameRunning = false;
            Time.timeScale = 0f;
        }
    }

    private IEnumerator SelectButton(GameObject button) {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}
