using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class pallet_random_script : MonoBehaviour
{
    public GameObject[] difficults;
    private GameManager gameManager;
    public bool isDefault;
    private float reducePercent = 0.5f;
    private bool inSwitchMode;

    private void Start()
    {
        gameManager = GameManager._instance;
    }

    private void OnEnable()
    {
        if (!isDefault)
        {
            int random_value = Random.Range(0, difficults.Length);

            if (random_value == 0)
            {
                if (Random.value < reducePercent)
                {
                    random_value = Random.Range(1, difficults.Length);
                }
            }

            difficults[random_value].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        inSwitchMode = gameManager.GetSwitchMap();
        if (inSwitchMode)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnDisable()
    {
        isDefault = false;
        foreach (var obstacles in difficults)
        {
            if (obstacles.activeSelf)
            {
                obstacles.gameObject.SetActive(false);
            }
        }
    }
    
}





