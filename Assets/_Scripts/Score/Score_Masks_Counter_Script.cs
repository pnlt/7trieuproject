using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score_Masks_Counter_Script : MonoBehaviour
{

   
    [SerializeField] TextMeshProUGUI total_masks_text;
    [SerializeField] TextMeshProUGUI current_score_text;

    [SerializeField] Transform Player;
    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        total_masks_text.text = PlayerPrefs.GetInt("total_masks", 0).ToString("00");
        current_score_text.text = Player.transform.position.z.ToString("00.0") + "m";

        if (Player.transform.position.z >= PlayerPrefs.GetFloat("high_score", 0f)) {
            PlayerPrefs.SetFloat("high_score", Player.transform.position.z);
        }
    }

   
}
