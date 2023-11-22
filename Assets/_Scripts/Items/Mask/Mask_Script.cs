using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Mask_Script : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject mask;

    private bool isMaskActive;

    private void Start()
    {
        gameManager = GameManager._instance;
        mask = gameObject.transform.GetChild(0).gameObject;
        isMaskActive = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") 
        {
            PlayerPrefs.SetInt("total_masks", PlayerPrefs.GetInt("total_masks", 0) + 1);
            if (PlayerPrefs.GetInt("total_masks", 0) == gameManager.GetMaskToVisit())
               gameManager.SetSwitch(true);
            gameObject.SetActive(false);
            GetComponentInParent<MaskManager>().masksPool.Add(this);
        }
    }

} 