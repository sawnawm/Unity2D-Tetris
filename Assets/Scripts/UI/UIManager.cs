using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject RetryPanel;
    public GameObject ResumeButton;
    public delegate void Notify();
    public Notify OnGameStartRequest;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.PressedEnter += PauseGame;
        GameManager.Instance.OnGameOver += OnGameOver;
    }

    private void PauseGame()
    {
        if (!GameManager.Instance.GameOver)
        {
            ResumeButton.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        OnGameStartRequest?.Invoke();
    }

    private void OnGameOver()
    {
        RetryPanel.SetActive(true);
    }
}
