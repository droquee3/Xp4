using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestivarCursor : MonoBehaviour
{
    public bool cursorActiveInGameplay = false;

    void Start()
    {
        ToggleCursor(cursorActiveInGameplay);
    }
    public void ToggleCursor(bool isActive)
    {
        Cursor.visible = isActive;
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor(true);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCursor(false);
        }
    }
}
