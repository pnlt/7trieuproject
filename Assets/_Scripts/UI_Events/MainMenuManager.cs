using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI total_masks_text;
    [SerializeField] TextMeshProUGUI distance_text;

    private GameManager gameManager;
    private UI_Manager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
        uiManager = UI_Manager._instance;
    }

    // Update is called once per frame
    void Update()
    {
        total_masks_text.text = PlayerPrefs.GetInt("total_masks").ToString();
        distance_text.text = gameManager.distanceTravese.ToString() + " m";
    }

    public void DisplayPausePanel()
    {
        uiManager.ShowUpPausePanel();
        Time.timeScale = 0;
    }
}
