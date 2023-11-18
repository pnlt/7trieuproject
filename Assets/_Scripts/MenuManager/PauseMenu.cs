using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeResumeText;
    [SerializeField] private GameObject pausePanel;
    private GameManager gameManager;
    private float timeToResume = 3f;


    private void Start()
    {
        gameManager = GameManager._instance;
    }

    public void Home()
    {
        Time.timeScale = 1.0f;
        SceneController.instance.NextLevel("backHome");
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        //StartCoroutine(ResumeGame()); 

        StartCoroutine(ResumeGame());
    }

    private IEnumerator ResumeGame()
    {
        while (true)
        {
            timeResumeText.text = Mathf.RoundToInt(timeToResume).ToString();
            timeToResume = timeToResume - Time.unscaledDeltaTime;
            
            if (timeToResume <= 0)
            {
                timeToResume = 3f;
                Time.timeScale = 1.0f;
                gameManager.SetGamePause(false);
                gameObject.SetActive(false);
                pausePanel.SetActive(true);
                break;
            }
            
            yield return null;
        }
        
    }



}
