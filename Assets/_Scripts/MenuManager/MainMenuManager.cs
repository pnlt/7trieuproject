using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI total_masks_text;
    [SerializeField] TextMeshProUGUI distance_text;
    [SerializeField] private Button pauseMenu; 

    private GameManager gameManager;
    private UI_Manager uiManager;
    private bool inStartMode;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
        uiManager = UI_Manager._instance;
        pauseMenu = GetComponentInChildren<Button>();
        pauseMenu.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        inStartMode = gameManager.GetGameStart();
        if (inStartMode)
            pauseMenu.gameObject.SetActive(true);
        total_masks_text.text = PlayerPrefs.GetInt("total_masks").ToString();
        distance_text.text = gameManager.distanceTravese.ToString() + " m";
    }

    public void DisplayPausePanel()
    {
        uiManager.ShowUpPausePanel();
        Time.timeScale = 0;
    }
}
