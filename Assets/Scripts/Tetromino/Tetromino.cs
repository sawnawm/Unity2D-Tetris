using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Types
{
    O,
    I,
    J,
    L,
    T,
    S,
    Z
}

public class Tetromino : MonoBehaviour
{
    public GameObject TetrominoCell;
    public Vector2[] TetroCellOriginPosition = new Vector2[4];
    public Types TetrominoType;

    private Block[] _blocks = new Block[4];
    private Vector2[] _tetroCellCurrentPositions = new Vector2[4];
    private Vector2[] _tempTetroCellPositions = new Vector2[4];
    private Vector2 _originCellPos;
    private bool _currentlyUsed = false;
    private List<Vector2> _blocksSpawnPostions;

    public bool InstantiateTetrominos()
    {
        _blocksSpawnPostions = new List<Vector2>();
        _originCellPos = transform.position;

        for (int i = 0; i < TetroCellOriginPosition.Length; i ++)
        {
            _tetroCellCurrentPositions[i] = TetroCellOriginPosition[i];
            _blocksSpawnPostions.Add(_originCellPos + TetroCellOriginPosition[i]);
        }

        if (!Board.Instance.FollowsMoveRulesAndBlockFree(_blocksSpawnPostions))
        {
            float shift = RequiredUpwardVerticalBlockShift(_blocksSpawnPostions);
            if (shift != 0)
            {
                for (int i = 0; i < _blocksSpawnPostions.Count; i++)
                {
                    _blocksSpawnPostions[i] += Vector2.up * shift;
                }
                InstantiateBlocks(_blocksSpawnPostions);
            }
            GameManager.Instance.TetrominoSpaceNotAvailable();
            return false;
        }

        InstantiateBlocks(_blocksSpawnPostions);
        _currentlyUsed = true;

        return true;
    }

    private void InstantiateBlocks(List<Vector2> position)
    {
        for (int i = 0; i < TetroCellOriginPosition.Length; i++)
        {
            _blocks[i] = Instantiate(TetrominoCell, position[i], Quaternion.identity).GetComponent<Block>();
        }
    }

    private void SetBlockPositions(List<Vector2> newPosition)
    {
        for (int i = 0; i < newPosition.Count; i++)
        {
            _blocks[i].transform.position = newPosition[i];
        }
    }

    #region For normal moves left right and down

    public bool ValidateAndMove(Vector3 input)
    {
        List<Vector2> simulatedPositions = SimulateNormalMovementsPosition(input);

        if (Board.Instance.FollowsMoveRulesAndBlockFree(simulatedPositions))
        {
            _originCellPos += (Vector2)input;
            SetBlockPositions(simulatedPositions);
            return true;
        }
        return false;
    }

    private List<Vector2> SimulateNormalMovementsPosition(Vector3 input)
    {
        List<Vector2> blockPositions = new List<Vector2>();
        foreach (Block block in _blocks)
        {
            blockPositions.Add(block.transform.position + input);
        }
        return blockPositions;
    }

    #endregion

    #region For rotation of tetromino blocks

    public void Rotate()
    {
        if (TetrominoType != 0)
        {
            Tuple<List<Vector2>, Vector2> simulatedPositions = SimulateRotatePosition();

            if (Board.Instance.FollowsMoveRulesAndBlockFree(simulatedPositions.Item1))
            {
                _originCellPos += simulatedPositions.Item2;
                SetBlockPositions(simulatedPositions.Item1);

                _tempTetroCellPositions.CopyTo(_tetroCellCurrentPositions, 0);
            }
        }
    }
    
    private Tuple<List<Vector2>, Vector2> SimulateRotatePosition()
    {
        List<Vector2> simulatedPos = new List<Vector2>();

        for (int i = 0; i < _tetroCellCurrentPositions.Length; i++ )
        {
            Vector2 newLocalPos = new Vector2(_tetroCellCurrentPositions[i].y, -_tetroCellCurrentPositions[i].x);
            _tempTetroCellPositions[i] = newLocalPos;
            simulatedPos.Add(_originCellPos + newLocalPos);
        }
        Vector2 shift = RequiredBlockShift(simulatedPos);
        for (int i = 0; i < simulatedPos.Count; i++)
        {
            simulatedPos[i] += shift;
        }
        return new Tuple<List<Vector2>, Vector2>(simulatedPos, shift);
    }

    #endregion

    private Vector2 RequiredBlockShift(List<Vector2> currentPositions)
    {
        float x = 0;

        foreach (Vector2 pos in currentPositions)
        {
            if (pos.x > 9)
            {
                x = Mathf.Min(x, 9 - pos.x);
            }
            else if (pos.x < 0)
            {
                x = Mathf.Max(x, -pos.x);
            }
        }
        return new Vector2(x, 0);
    }

    private float RequiredUpwardVerticalBlockShift(List<Vector2> currentPositions)
    {
        float maxBlockedPos = 0;
        float lowestPos = Board.Instance.Blocks.GetLength(1);
        float y = 0;

        foreach (Vector2 pos in currentPositions)
        {
            if (!Board.Instance.HasEmptyCell(pos))
            {
                maxBlockedPos = Mathf.Max(maxBlockedPos, pos.y);
            }
            lowestPos = Mathf.Min(lowestPos, pos.y);

        }

        y = maxBlockedPos < Board.Instance.Blocks.GetLength(1) - 1 ? 1 : 0;

        if (y != 0)
        {
            y += maxBlockedPos - lowestPos;
        }

        return Mathf.Max(y, 0);
    }

    public void StraightDrop()
    {
        int shift = (int)_originCellPos.y;

        foreach (Block block in _blocks)
        {
            Vector2 pos = block.transform.position;
            int move = 0;
            for (int i = (int)pos.y - 1; i >= 0; i--)
            {
                if (Board.Instance.Blocks[(int)pos.x, i] == null)
                {
                    move += 1;
                }
                else
                {
                    break;
                }
            }
            shift = Mathf.Min(shift, move);
        }

        foreach (Block block in _blocks)
        {
            block.transform.position += Vector3.down * shift;
        }
        DropBlocks();
    }

    public void DropBlocks()
    {
        Board.Instance.FillTetroInBoard(_blocks);
        ControlDetached();
    }

    public void ControlDetached()
    {
        _currentlyUsed = false;
    }

    public bool Used()
    {
        return _currentlyUsed;
    }
}
