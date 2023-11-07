using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static LeaderBoardManager;

public class GameManager : MonoBehaviour
{
    [Header ("parameters")]
    public static GameManager _instance;
    [SerializeField] private int maskToVisit;
    [SerializeField] private Vector3 offsetLocation;

    [Header ("GameObject Prefab")]
    [SerializeField] private GameObject koreaPrefab;
    [SerializeField] private Transform player;

    public HighscoreEntry highscoreEntry;

    public float distanceTravese { get; set; }

    private UI_Manager uiManager;

    #region Encapsulation
    private bool isGameOVer;
    private bool isGameStarted;
    private bool isPaused;
    private bool isTutorialGamePlay;
    private bool startSwitchMap;

    public bool GetGameOver()
    {
        return this.isGameOVer;
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
        return this.maskToVisit;
    }

    public bool GetSwitchMap()
    {
        return this.startSwitchMap;  
    }

    public void SetGameOver(bool status)
    {
        this.isGameOVer = status;
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
        uiManager = UI_Manager._instance;
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
    }

    private void StartEvent()
    {
        if (Input.GetMouseButtonDown(0) && !isGameOVer)
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
        if (startSwitchMap)
        {
            MapConnect();
            startSwitchMap = false;
            //starts timeline
            uiManager.ShowUptimelineOBPanel();
        }
    }

    private void MapConnect()
    {
        GameObject hanbok = Instantiate(koreaPrefab);
        hanbok.transform.position += new Vector3(offsetLocation.x, offsetLocation.y, player.position.z + offsetLocation.z);
        player.GetComponent<Player_Controller>().HanbokHolder(hanbok);
    }
    #endregion
}
