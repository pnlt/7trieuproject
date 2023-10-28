using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask_Script : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            PlayerPrefs.SetInt("total_masks", PlayerPrefs.GetInt("total_masks", 0) + 1);
            Destroy(this.gameObject);
        }
    }
}
