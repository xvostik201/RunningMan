using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private CursorLockMode _startLockMode = CursorLockMode.Locked;
    [SerializeField] private bool _cursorVisible = false;
    void Awake()
    {
        Cursor.lockState = _startLockMode;
        Cursor.visible = _cursorVisible;
    }

    public static void ChangeCursorMode(CursorLockMode mode, bool visible)
    {
        Cursor.lockState = mode;
        Cursor.visible = visible;
    }

}
