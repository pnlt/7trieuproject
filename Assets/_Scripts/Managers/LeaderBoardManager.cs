using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager instance;
    public Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntriesList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        instance = this;
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        //if (PlayerPrefs.GetString("highscoreTable") != null)
        //{
        //    Debug.Log("OK");
        //    string jsonString = PlayerPrefs.GetString("highscoreTable");
        //    Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //    for (int i = 0; i < highscores.highscoreEntriesList.Count; i++)
        //    {
        //        for (int j = i + 1; j < highscores.highscoreEntriesList.Count; j++)
        //        {
        //            if (highscores.highscoreEntriesList[j].score > highscores.highscoreEntriesList[i].score)
        //            {
        //                HighscoreEntry tmp = highscores.highscoreEntriesList[i];
        //                highscores.highscoreEntriesList[i] = highscores.highscoreEntriesList[j];
        //                highscores.highscoreEntriesList[j] = tmp;
        //            }
        //        }
        //    }

        //    highscoreEntryTransformList = new List<Transform>();
        //    foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntriesList)
        //    {
        //        CreateHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        //    }
        //}
        //else
        //{
        //    Debug.Log("OK2");
        //    highscoreEntriesList = new List<HighscoreEntry>()
        //    {
        //        new HighscoreEntry{score = 0, name ="Player"},
        //    };

        //    //Sort entry list by score
        //    for (int i = 0; i < highscoreEntriesList.Count; i++)
        //    {
        //        for (int j = i + 1; j < highscoreEntriesList.Count; j++)
        //        {
        //            if (highscoreEntriesList[j].score > highscoreEntriesList[i].score)
        //            {
        //                HighscoreEntry tmp = highscoreEntriesList[i];
        //                highscoreEntriesList[i] = highscoreEntriesList[j];
        //                highscoreEntriesList[j] = tmp;
        //            }
        //        }
        //    }

        //    highscoreEntryTransformList = new List<Transform>();
        //    foreach (HighscoreEntry highscoreEntry in highscoreEntriesList)
        //    {
        //        CreateHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        //    }

        //    Highscores highscore = new Highscores { highscoreEntriesList = highscoreEntriesList };
        //    string json = JsonUtility.ToJson(highscore);
        //    PlayerPrefs.SetString("highscoreTable", json);
        //    PlayerPrefs.Save();
        //}
        PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("highscoreTable"))
        {
            string jsonString = PlayerPrefs.GetString("highscoreTable");
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

            highscoreEntriesList = highscores.highscoreEntriesList;

            // Sort the list by score
            highscoreEntriesList.Sort((a, b) => b.score.CompareTo(a.score));

            highscoreEntryTransformList = new List<Transform>();
            int maxEntriesToShow = Mathf.Min(highscoreEntriesList.Count, 10); // Limit to the top 10 entries
            for (int i = 0; i < maxEntriesToShow; i++)
            {
                CreateHighScoreEntryTransform(highscoreEntriesList[i], entryContainer, highscoreEntryTransformList);
            }
        }
        else
        {
            Debug.Log("No high scores found.");
            highscoreEntriesList = new List<HighscoreEntry>()
            {
                new HighscoreEntry { score = 0, name = "Player" },
                new HighscoreEntry { score = 1, name = "Player" },
                new HighscoreEntry { score = 3, name = "Player" },
                new HighscoreEntry { score = 4, name = "Player" },
                new HighscoreEntry { score = 5, name = "Player" },
                new HighscoreEntry { score = 6, name = "Player" },
                new HighscoreEntry { score = 7, name = "Player" },
                new HighscoreEntry { score = 8, name = "Player" },
                new HighscoreEntry { score = 9, name = "Player" },
                new HighscoreEntry { score = 19, name = "Player" },
                new HighscoreEntry { score = 29, name = "Player" },
                new HighscoreEntry { score = 39, name = "Player" },
                new HighscoreEntry { score = 49, name = "Player" },
            };

            highscoreEntryTransformList = new List<Transform>();
            foreach (HighscoreEntry highscoreEntry in highscoreEntriesList)
            {
                CreateHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }

            Highscores highscore = new Highscores { highscoreEntriesList = highscoreEntriesList };
            string json = JsonUtility.ToJson(highscore);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
        }   
}

    private void CreateHighScoreEntryTransform(HighscoreEntry highscore, Transform container, List<Transform> transformList)
    {
        float templateHeight = 70f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        rankString = rank.ToString();

        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().SetText(rankString);

        int score = highscore.score;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().SetText(score.ToString());

        string name = highscore.name;
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(name);

        transformList.Add(entryTransform);
    }

    public class Highscores
    {
        public List<HighscoreEntry> highscoreEntriesList;
    }

    [System.Serializable]
    public class HighscoreEntry
    {
        public int score;
        public string name;
    }
}