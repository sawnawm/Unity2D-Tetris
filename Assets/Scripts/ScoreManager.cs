using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public delegate void Notify(float score);
    public Notify OnScoreUpdated;
    public Notify OnLinesUpdated;
    public Notify OnLevelUpgraded;
    public Notify OnHighestScored;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject scoreManager = new GameObject("Score Manager");
                _instance = scoreManager.AddComponent<ScoreManager>();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    public float Score
    {
        get => _score;

        private set
        {
            _score = value;
            OnScoreUpdated?.Invoke(_score);
        }
    }
    public float Lines
    {
        get => _lines;

        private set
        {
            _lines = value;
            OnLinesUpdated?.Invoke(_lines);
        }
    }
    public float Level
    {
        get => _level;

        private set
        {
            _level = value;
            OnLevelUpgraded?.Invoke(_level);
        }
    }
    public float HighScore
    {
        get => _highScore;

        private set
        {
            _highScore = value;
            PlayerPrefs.SetFloat(_HIGHEST_SCORE, value);
            OnHighestScored?.Invoke(_highScore);
        }
    }

    private static ScoreManager _instance = null;
    private float _lines = 0;
    private float _score = 0;
    private float _level = 1;
    private float _highScore = 0;
    private const string _HIGHEST_SCORE = "HighestScore";

    private void Start()
    {
        GameManager.Instance.OnGameReset += ResetDetails;
        Board.Instance.OnLinesCleared += ManageScore;
    }

    private void ResetDetails()
    {
        Score = 0;
        Lines = 0;
        Level = 1;
        _highScore = PlayerPrefs.GetFloat(_HIGHEST_SCORE);
    }

    private void ManageScore(int lines)
    {
        Lines += lines;
        Score += 100 * ((2 * lines) - 1);
        ManageLevel();
        ManageHighScore();
    }

    private void ManageLevel()
    {
        if (Level == ((int)Lines / 10))
        {
            Level += 1;
        }
    }

    private void ManageHighScore()
    {
        if (_highScore < Score)
        {
            HighScore = Score;
        }
    }
}
