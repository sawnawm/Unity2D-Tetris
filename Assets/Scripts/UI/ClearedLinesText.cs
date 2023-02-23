using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearedLinesText : TextUpdate
{
    void Start()
    {
        ScoreManager.Instance.OnLinesUpdated += UpdateText;
    }
}
