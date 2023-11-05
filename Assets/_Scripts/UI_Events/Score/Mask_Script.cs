using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask_Script : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager._instance;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            PlayerPrefs.SetInt("total_masks", PlayerPrefs.GetInt("total_masks", 0) + 1);
            if (PlayerPrefs.GetInt("total_masks", 0) == gameManager.GetMaskToVisit())
                gameManager.SetSwitch(true);
            Destroy(this.gameObject);
        }
    }
}
