using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }

    protected void UpdateText(float score)
    {
        text.text = score.ToString();
    }
}
