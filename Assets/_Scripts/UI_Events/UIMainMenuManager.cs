using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderBoardPanel;
    public void ShowUpLeaderBoardPanel()
    {
        leaderBoardPanel.SetActive(true);
    }

    public void HideLeaderBoardPanel()
    {
        leaderBoardPanel.SetActive(false);
    }
}
