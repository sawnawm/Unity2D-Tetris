using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominosController : MonoBehaviour
{
    public static TetrominosController Instance;

    private bool _dropped = false;
    private Tetromino _controllingTetromino;
    private const float MIN_SPEED = 1f;
    private float _gameSpeed = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ActivateInputListeners();
        ScoreManager.Instance.OnLevelUpgraded += UpdateDecisionTime;
    }

    private void ActivateInputListeners()
    {
        InputManager.Instance.HorizontalPress += OnMoveHorizontal;
        InputManager.Instance.PressedUp += OnRotate;
        InputManager.Instance.PressedDown += OnMoveDown;
        InputManager.Instance.PressedSpace += OnDropTetromino;
    }

    private void OnRotate()
    {
        if (!_dropped)
        {
            _controllingTetromino.Rotate();
        }
    }

    private void OnMoveDown()
    {
        if (!_dropped)
        {
            _controllingTetromino.ValidateAndMove(Vector3.down);
        }
    }

    private void OnMoveHorizontal(int input)
    {
        if (!_dropped)
        {
            _controllingTetromino.ValidateAndMove(input * Vector3.right);
        }
    }

    private void OnDropTetromino()
    {
        if (!_dropped)
        {
            _dropped = true;
            _controllingTetromino.StraightDrop();
            _controllingTetromino = null;
        }
    }

    private IEnumerator CustomUpdate()
    {
        _dropped = false;

        while (!_dropped)
        {
            yield return new WaitForSeconds(_gameSpeed);

            if (!_dropped)
            {
                _dropped = !_controllingTetromino.ValidateAndMove(Vector3.down);
                if (_dropped)
                {
                    _controllingTetromino.DropBlocks();
                    _controllingTetromino = null;
                }
            }
        }

        if (!GameManager.Instance.GameOver)
        {
            SpawnTetromino();
        }
    }

    public void SpawnTetromino()
    {
        _controllingTetromino = ObjectPool.Instance.GetTetrominoToControl();
        if (_controllingTetromino.InstantiateTetrominos())
        {
            StartCoroutine(CustomUpdate());
        }
        else
        {
            _controllingTetromino = null;
        }
    }

    private void UpdateDecisionTime(float level)
    {
        _gameSpeed = level > 5 ? _gameSpeed : MIN_SPEED - ((level - 1) / 10);
    }
}
