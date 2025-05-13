using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabHandler : MonoBehaviour
{
    [SerializeField] List<LobbyTab> tabs = new List<LobbyTab>();
    
    public void SetTab(int tab)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].TabPanel.SetActive(i == tab);
            tabs[i].TabButton.enabled= i != tab;
        }
    }

    public void SwitchTab(LobbyTab tab)
    {
        if (tabs.Contains(tab))
        {
            SetTab(tabs.IndexOf(tab));
        }
    }
}
