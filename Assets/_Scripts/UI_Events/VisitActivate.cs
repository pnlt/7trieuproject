using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisitActivate : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
    }

    // Update is called once per frame
    void Update()
    {
               
    }

    public void OnClickAccept()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickDecline()
    {
        gameObject.SetActive(false);
        
    }
}
