using static Player_Controller;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThreeOptionsPanel : MonoBehaviour
{
    public Button[] optionButtons;

    void Start()
    {
        // Initialize optionButtons array
        optionButtons = new Button[3];

        optionButtons[0] = GameObject.Find("Button1").GetComponent<Button>();
        optionButtons[1] = GameObject.Find("Button2").GetComponent<Button>();
        optionButtons[2] = GameObject.Find("Button3").GetComponent<Button>();
        gameObject.SetActive(false);
    }

    public void ShowOptions(List<PowerUp> powerUps)
    {
        if (powerUps.Count != 3)
        {
            Debug.LogError("Invalid number of power-ups provided. Expected 3 power-ups.");
            return;
        }
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = powerUps[i].ToString();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(powerUps[index]));
        }
    }

    
    public void ShowUp()
    {
        gameObject.SetActive(true);
    }

    public void OnOptionSelected(PowerUp selectedPowerUp)
    {
        // Notify Player_Controller of the selected power-up
        Player_Controller playerController = FindObjectOfType<Player_Controller>();
        if (playerController != null)
        {
            playerController.OnOptionSelected(selectedPowerUp);
        }
    }
}
