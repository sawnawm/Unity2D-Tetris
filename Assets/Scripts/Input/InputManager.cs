using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject inputManager = new GameObject("Input Manager");
                _instance = inputManager.AddComponent<InputManager>();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    public delegate void InputCode(int code);
    public InputCode HorizontalPress;
    public delegate void Notify();
    public Notify PressedDown;
    public Notify PressedUp;
    public Notify PressedSpace;
    public Notify PressedEnter;

    private static InputManager _instance;
    [SerializeField]
    private float _horizontal = 0;
    [SerializeField]
    private float _vertical = 0;

    private void Update()
    {
        if(Time.timeScale != 0)
        {
            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            int vertical = (int)Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Return))
            {
                PressedEnter?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                PressedSpace?.Invoke();
            }
            else if (horizontal != _horizontal)
            {
                _horizontal = horizontal;
                _vertical = 0;
                if (_horizontal != 0)
                {
                    HorizontalPress?.Invoke(horizontal);
                }
            }
            else if (vertical != _vertical)
            {
                _vertical = vertical;
                _horizontal = 0;
                if (_vertical == 1)
                {
                    PressedUp?.Invoke();
                }
                else if (_vertical == -1)
                {
                    PressedDown?.Invoke();
                }
            }
        }
    }
}
