using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static LeaderBoardManager;

public class GameManager : MonoBehaviour
{
    [Header ("parameters")]
    public static GameManager _instance;
    private static int maskToVisit;
    [SerializeField] private Vector3 offsetLocation;

    [Header ("GameObject Prefab")]
    [SerializeField] private GameObject koreaPrefab;
    [SerializeField] private Transform player;

    public HighscoreEntry highscoreEntry;

    public float distanceTravese { get; set; }

    private UI_Manager uiManager;

    #region Encapsulation
    private bool isGameOver;
    private bool isGameStarted;
    private bool isPaused;
    private bool isTutorialGamePlay;
    private bool startSwitchMap;
    public int distanceContainer;
    private static bool startTracking = false;

    public bool GetGameOver()
    {
        return this.isGameOver;
    }

    public bool GetGameStart()
    {
        return this.isGameStarted;
    }
    public bool GetIsTutorialGamePlay()
    {
        return this.isTutorialGamePlay;
    }

    public bool GetGamePause()
    {
        return this.isPaused;
    }

    public int GetMaskToVisit()
    {
        return GameManager.maskToVisit;
    }

    public bool GetSwitchMap()
    {
        return this.startSwitchMap;  
    }

    public void SetGameOver(bool status)
    {
        this.isGameOver = status;
    }

    public void SetGameStart(bool status)
    {
        this.isGameStarted = status;
    }

    public void SetGamePause(bool status)
    {
        this.isPaused = status;
    }

    public void SetSwitch(bool status)
    {
        this.startSwitchMap = status;
    }
    #endregion

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        PlayerPrefs.SetInt("total_masks", 0);
        distanceContainer += PlayerPrefs.GetInt("Distance_Score");
        distanceTravese = distanceContainer;
        uiManager = UI_Manager._instance;
        ShowTutorial();
        if (startTracking)
        {
            int valueIncr = Random.Range(15, 51);
            maskToVisit += valueIncr;
        }
        else
            maskToVisit = 5;
        
    }

    private void ShowTutorial()
    {
        if (!PlayerPrefs.HasKey("firstPlay"))
        {
            isTutorialGamePlay = true;
            PlayerPrefs.SetInt("firstPlay", 0);
            uiManager.ShowUpTutorialGamePlay();
        }
        else
        {
            isTutorialGamePlay = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartEvent();
        SwitchMapEvent();
        //Debug.Log(Time.unscaledTime / Time.unscaledDeltaTime);
    }

    private void StartEvent()
    {
        if (Input.GetMouseButtonDown(0) && !isGameOver)
        {
            isGameStarted = true;
        }
    }

    public void SetBestScore(int distance)
    {
        AddHighscroreEntry(distance, "Player");
        int lastBestScore = PlayerPrefs.GetInt("BestScore");
        if (distance > lastBestScore)
        {
            PlayerPrefs.SetInt("BestScore", distance);
        }
    }

    public void AddHighscroreEntry(int score, string name)
    {
        highscoreEntry = new HighscoreEntry { score = score, name = name };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscores.highscoreEntriesList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    #region SwitchMap
    private void SwitchMapEvent()
    {
        if (startSwitchMap && !isGameOver)
        {
            string text = "";
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                text = "Congratulation! You have collected enough masks.You will be transported to Korea map.";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            { 
                text = "Congratulation! You have collected enough masks.You will be transported to VietNam map.";
            }
            //starts timeline
            uiManager.SetSwitchText(text);
            uiManager.ShowUptimelineOBPanel();
        }
    }

    public void ChangeStatus()
    {
        startTracking = true;
        PlayerPrefs.SetInt("Distance_Score", (int)distanceTravese);
    }
    #endregion
}
