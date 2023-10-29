using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Score_Masks_Counter_Script : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI total_masks_text;
    [SerializeField] TextMeshProUGUI current_score_text;

    [SerializeField] Transform Player;

    [SerializeField] GameObject visitPanel;
    private const int masksThreshold = 20;
    private bool panelActivated = false;

    void Start()
    {

        if (visitPanel != null)
        {
            visitPanel.SetActive(false);
        }
    }


    void Update()
    {
        total_masks_text.text = PlayerPrefs.GetInt("total_masks", 0).ToString("00");
        current_score_text.text = Player.transform.position.z.ToString("00.0") + "m";

        if (Player.transform.position.z >= PlayerPrefs.GetFloat("high_score", 0f))
        {
            PlayerPrefs.SetFloat("high_score", Player.transform.position.z);
        }

        // Check if the total masks reach the threshold
        if (PlayerPrefs.GetInt("total_masks", 0) >= masksThreshold && !panelActivated)
        {

            // Activate the popup panel
            if (visitPanel != null)
            {
                Time.timeScale = 0;
                visitPanel.SetActive(true);
                panelActivated = true;
            }
        }

    }






    public void MoveForward()
    {

        SceneManager.LoadScene(2);
    }


    public void HidePanel()
    {
        if (visitPanel != null)
        {
            visitPanel.SetActive(false);
            Time.timeScale = 1;
        }

    }





}
