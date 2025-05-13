using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    [SerializeField] List<LobbyWindow> windows = new List<LobbyWindow>();

    public void SetWindow(int window)
    {
        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].gameObject.SetActive(i == window);
        }
    }

    public void SwitchWindow(LobbyWindow window)
    {
        if (windows.Contains(window))
        {
            SetWindow(windows.IndexOf(window));
        }
    }
}
