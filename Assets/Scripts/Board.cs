using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;
    public Block[,] Blocks = new Block[10,20];
    public delegate void Notify(int count);
    public Notify OnLinesCleared;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameReset += ClearData;
    }

    public void FillTetroInBoard(Block[] Tetrominos)
    {
        foreach (Block block in Tetrominos)
        {
            Blocks[(int)block.transform.position.x, (int)block.transform.position.y] = block;
        }
        RemoveLines();
    }

    // check if input allows to move tetromino or is blocked
    public bool FollowsMoveRulesAndBlockFree(List<Vector2> blockPositions)
    {
        bool valid = true;

        foreach (Vector2 pos in blockPositions)
        {
            valid &= FollowsBasicMoveRulesInBoard(pos);

            // array bounds error if doesn't follow basic rules
            if (valid)
            {
                // check if there is another block on its new position
                valid &= HasEmptyCell(pos);
            }
            else
            {
                return false;
            }
        }
        return valid;
    }

    public bool HasEmptyCell(Vector2 pos)
    {
        return Blocks[(int)pos.x, (int)pos.y] == null;
    }

    public bool FollowsBasicMoveRulesInBoard(Vector2 pos)
    {
        // can't go below board, or leave right and left as well
        if (pos.x >= 0 && pos.y >= 0 && pos.x < Blocks.GetLength(0))
        {
            return true;
        }
        return false;
    }

    private void RemoveLines()
    {
        List<int> clearedRows = new List<int>();
        for (int j = 0; j < Blocks.GetLength(1); j++)
        {
            List<Block> row = new List<Block>();
            for (int i = Blocks.GetLength(0) - 1; i >= 0; i--)
            {
                if (Blocks[i, j] == null)
                {
                    break;
                }
                row.Add(Blocks[i, j]);
                if(i == 0)
                {
                    ClearLine(row);
                    clearedRows.Add(j);
                }
            }
        }
        if (clearedRows.Count > 0)
        {
            OnLinesCleared?.Invoke(clearedRows.Count);
            AdjustLines(clearedRows);
        }

    }

    private void ClearLine(List<Block> rows)
    {
        rows.ForEach(cell => cell.DestroyFromBoard());
    }

    private void AdjustLines(List<int> clearedRows)
    {
        for (int j = 0; j < Blocks.GetLength(1); j++)
        {
            int drop = 0;
            if (!clearedRows.Contains(j))
            {
                clearedRows.ForEach(row => drop += (j > row) ? 1 : 0);
                if (drop != 0)
                {
                    for (int i = 0; i < Blocks.GetLength(0); i++)
                    {
                        Block block = Blocks[i, j];
                        if(block != null)
                        {
                            Blocks[i, j - drop] = block;
                            Blocks[i, j] = null;
                            block.transform.position = new Vector2(i, j - drop);
                        }
                    }
                }
            }
        }
    }

    private void ClearData()
    {
        Array.Clear(Blocks, 0, Blocks.Length);
    }

}