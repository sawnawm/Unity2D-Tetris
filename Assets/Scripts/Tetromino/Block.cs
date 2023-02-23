using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameReset += DestroyFromBoard;
    }

    public void DestroyFromBoard()
    {
        // put some animation
        GameManager.Instance.OnGameReset -= DestroyFromBoard;
        Destroy(gameObject);
    }
}
