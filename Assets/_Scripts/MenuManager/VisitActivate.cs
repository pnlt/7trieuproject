using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisitActivate : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI notifyText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
    }

    // Update is called once per frame

    public void OnClickAccept()
    {
        SceneController.instance.NextLevel("NextScene");
    }

    public void SetTextNotifySwitch(string text)
    {
        notifyText.text = text;
    }
}
