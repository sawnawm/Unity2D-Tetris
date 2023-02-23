using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public GameObject[] TetrominoPrefabs;
    public List<Tetromino> NextTetrominos = new List<Tetromino>();
    public delegate void Notify(List<Tetromino> tetrominos);
    public Notify TetrominoCollected;

    [SerializeField]
    private List<Tetromino> TetrominoPool = new List<Tetromino>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (NextTetrominos.Count < 2)
        {
            CollectNextTetrominos();
        }
    }

    private void CollectNextTetrominos()
    {
        Tetromino tetromino = SpawnTetromino();
        NextTetrominos.Add(tetromino);
        TetrominoCollected?.Invoke(NextTetrominos);
    }

    private Tetromino SpawnTetromino()
    {
        int spawnIndex = Random.Range(0, TetrominoPrefabs.Length * 1000) / 1000;

        Tetromino tetrominoSelected = TetrominoPrefabs[spawnIndex].GetComponent<Tetromino>();

        tetrominoSelected = TetrominoPool.Find( tetromino => 
            tetromino.TetrominoType == tetrominoSelected.TetrominoType 
            && 
            !tetromino.Used());

        if (tetrominoSelected == null)
        {
            tetrominoSelected = Instantiate(TetrominoPrefabs[spawnIndex], null).GetComponent<Tetromino>();
        }
        else
        {
            TetrominoPool.Remove(tetrominoSelected);
        }
        return tetrominoSelected;
    }

    public Tetromino GetTetrominoToControl()
    {
        Tetromino tetromino = NextTetrominos[0];
        NextTetrominos.Remove(tetromino);
        TetrominoPool.Add(tetromino);
        return tetromino;
    }


}
