using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtonScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score_text;
    [SerializeField] TextMeshProUGUI best_score_text;

    private GameManager gameManager;

    Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        gameManager = GameManager._instance;
    }

    // Update is called once per frame
    void Update()
    {
        score_text.text = gameManager.distanceTravese.ToString() + " m";

        best_score_text.text = PlayerPrefs.GetInt("BestScore").ToString("00.0") + "m";
    }

    public void Restart_Button() {
        PlayerPrefs.SetInt("Distance_Score", 0);
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Home_Button() {
        PlayerPrefs.SetInt("Distance_Score", 0);
        SceneController.instance.NextLevel("backHome");
        Time.timeScale = 1;
    }
}
