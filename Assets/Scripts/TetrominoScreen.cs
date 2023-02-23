using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Number
{
    _1,
    _2
}

public class TetrominoScreen : MonoBehaviour
{
    public GameObject[] Tetrominos;
    public Number Screen;

    private GameObject _showingTetromino;

    private void Start()
    {
        ObjectPool.Instance.TetrominoCollected += ShowTetromino;
    }

    private void ShowTetromino(List<Tetromino> collected)
    {
        if (collected.Count > (int)Screen)
        {
            HideTetromino();
            _showingTetromino = Tetrominos[(int)collected[(int)Screen].TetrominoType];
            _showingTetromino.SetActive(true);
        }
    }

    private void HideTetromino()
    {
        _showingTetromino?.SetActive(false);
        _showingTetromino = null;
    }
}
