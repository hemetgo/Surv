using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private bool paused;
    private bool onMenu;

    public static Singleton Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PauseGame() { paused = true; }
    public void ResumeGame() { paused = false; }
    public bool IsPaused() { return paused; }

    public void OpenMenu() { onMenu = true; }
    public void CloseMenu() { onMenu = false; }
    public bool OnMenu() { return onMenu; }
}
