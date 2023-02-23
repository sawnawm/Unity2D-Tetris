using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool GameOver { get; private set; } = true;
    public delegate void Notify();
    public Notify OnGameReset;
    public Notify OnGameOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.OnGameStartRequest += StartGame;
    }

    public void TetrominoSpaceNotAvailable()
    {
        GameOver = true;
        OnGameOver?.Invoke();
    }

    private void StartGame()
    {
        GameOver = false;
        OnGameReset?.Invoke();
        TetrominosController.Instance.SpawnTetromino();
    }

}
