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
    private const int masksThreshold = 1000;
    private bool panelActivated = false;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject powerUpPrefab;
    private bool powerUpSpawned = false;

    private bool isGamePaused = false;

    private Player_Controller playerController;
    private SpawnObject spawnItem;
    private PlayerBackChecking playerBackChecking;

    void Start()
    {

        if (visitPanel != null)
        {
            visitPanel.SetActive(false);
        }
        playerController = Player.GetComponent<Player_Controller>();
        spawnItem = Player.GetComponent<SpawnObject>();
        playerBackChecking = Player.GetComponent<PlayerBackChecking>();
    }


    void Update()
    {
        total_masks_text.text = PlayerPrefs.GetInt("total_masks", 0).ToString("00");
        current_score_text.text = Player.transform.position.z.ToString("00.0") + "m";

        if (Player.transform.position.z >= PlayerPrefs.GetFloat("high_score", 0f))
        {
            PlayerPrefs.SetFloat("high_score", Player.transform.position.z);
        }

     
        if (PlayerPrefs.GetInt("total_masks", 0) >= masksThreshold && !panelActivated)
        {
            if (visitPanel != null)
            {
                PauseGame();
              
                visitPanel.SetActive(true);
                panelActivated = true;
            }
        }

        if (Player.transform.position.z >= 300 && !powerUpSpawned)
        {
         
            SpawnPowerUp();

       
            if (dialoguePanel != null)
            {
                PauseGame();
              
                dialoguePanel.SetActive(true);
                powerUpSpawned = true;
            }

            
        }

    }

    void SpawnPowerUp()
    {
        if (powerUpPrefab != null)
        {
           
            GameObject powerUp = Instantiate(powerUpPrefab, Player.position + Vector3.up * 2f, Quaternion.identity);

         
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
       
            ResumeGame();
        }


    }

    public void HideDialoguePanel()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        
            ResumeGame();
        }
    }

    void PauseGame()
    {
        isGamePaused = true;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (spawnItem != null)
        {
            spawnItem.enabled = false;
        }

        if (playerBackChecking != null)
        {
            playerBackChecking.enabled = false;
        }

        Rigidbody playerRigidbody = Player.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }

        // Add additional logic to disable other components or systems
    }

    void ResumeGame()
    {
        isGamePaused = false;

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        if (spawnItem != null)
        {
            spawnItem.enabled = true;
        }

        if (playerBackChecking != null)
        {
            playerBackChecking.enabled = true;
        }

        Rigidbody playerRigidbody = Player.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }

        // Add additional logic to enable other components or systems
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && isGamePaused)
        {
            ResumeGame();
        }
    }





}
