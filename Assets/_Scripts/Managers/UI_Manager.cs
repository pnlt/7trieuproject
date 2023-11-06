using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Localization.LocalizationTableCollection;

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
    [SerializeField] private GameObject tutorialGamePlay;
    [SerializeField] private Sprite[] effectSprites;
    [SerializeField] private Image effectSymbol;

    [Space (5)]
    [Header ("TutorialUI")]
    [SerializeField] private GameObject swipeRightPanel;
    [SerializeField] private GameObject swipeLeftPanel;
    [SerializeField] private GameObject swipeTopPanel;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowUpswipeRightPanel()
    {
        swipeRightPanel.SetActive(true);
    }

    public void HideswipeRightPanel()
    {
        swipeRightPanel.SetActive(false);
    }
    public void ShowUpswipeLeftPanel()
    {
        swipeLeftPanel.SetActive(true);
    }

    public void HideswipeLeftPanel()
    {
        swipeLeftPanel.SetActive(false);
    }
    public void ShowUpswipeTopPanel()
    {
        swipeTopPanel.SetActive(true);
    }

    public void HideswipeTopPanel()
    {
        swipeTopPanel.SetActive(false);
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
    public void ShowUpTutorialGamePlay()
    {
        tutorialGamePlay.SetActive(true);
    }

    public void HideTutorialGamePlay()
    {
        tutorialGamePlay.SetActive(false);
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
        congratText.text = effect;
    }

    public void SetSpriteEffects(PowerUp effectTypes)
    {
        switch (effectTypes)
        {
            case PowerUp.SpeedBoost:
                effectSymbol.sprite = effectSprites[0];
                break;
            case PowerUp.ExtraLife:
                effectSymbol.sprite = effectSprites[1];
                break;
            case PowerUp.Shield:
                effectSymbol.sprite = effectSprites[2];
                break;
        }
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
