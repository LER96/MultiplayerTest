using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> windows = new List<GameObject>();

    public void SetWindow(int window)
    {
        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].SetActive(i == window);
        }
    }
}
