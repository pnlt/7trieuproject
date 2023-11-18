using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask_Script : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject mask;

    private void Start()
    {
        gameManager = GameManager._instance;
        mask = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") 
        {
            PlayerPrefs.SetInt("total_masks", PlayerPrefs.GetInt("total_masks", 0) + 1);
            if (PlayerPrefs.GetInt("total_masks", 0) == gameManager.GetMaskToVisit())
                gameManager.SetSwitch(true);
            StartCoroutine(Reset());
        }
    }

    private IEnumerator Reset()
    {
        mask.SetActive(false);
        yield return new WaitForSeconds(1.3f);
        mask.SetActive(true);
    }
}
