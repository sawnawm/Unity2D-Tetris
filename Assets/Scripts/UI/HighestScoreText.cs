using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighestScoreText : TextUpdate
{
    void Start()
    {
        ScoreManager.Instance.OnHighestScored += UpdateText;
    }
}
