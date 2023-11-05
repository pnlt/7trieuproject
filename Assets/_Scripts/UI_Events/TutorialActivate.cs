using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActivate : MonoBehaviour
{
    private bool isGameStarted;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager._instance;    
    }

    // Update is called once per frame
    void Update()
    {
        isGameStarted = gameManager.GetGameStart();
        if (isGameStarted)
            gameObject.SetActive(false);
    }
}
