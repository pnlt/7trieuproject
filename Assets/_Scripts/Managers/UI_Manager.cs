using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Header ("UI components references")]

    public static UI_Manager _instance;
    [SerializeField] private GameObject effectNotifyPanel;
    [SerializeField] private TextMeshProUGUI congratText;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Image[] hearts;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject visitPanel;
    [SerializeField] private GameObject pausePanel;  

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetEffectNotifyPanel()
    {
        return effectNotifyPanel;
    }

    public void ShowUpEffectNotify()
    {
        effectNotifyPanel.SetActive(true);
    }

    public void HideEffectPanel()
    {
        effectNotifyPanel.SetActive(false);
    }

    public void ShowTutorialPanel()
    {
        tutorialPanel.SetActive(true);
    }

    public void ShowUpGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowUpVisitPanel()
    {
        visitPanel.SetActive(true);
    }

    public void ShowUpPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void SetTextEffectPanel(string effect)
    {
        congratText.text = "Congratulation! You gained " + effect;
    }

    public void UpdateHeartsUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }

    private void Start()
    {
        tutorialPanel.SetActive(true);
    }
}
