using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelText : TextUpdate
{
    void Start()
    {
        ScoreManager.Instance.OnLevelUpgraded += UpdateText;
    }
}
