using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnim;
    [SerializeField] private Canvas transitionCanvas;
    private bool canTransition = true;

    private void Awake()
    {
        instance = this;
    }
    public void NextLevel(string text)
    {
        if (canTransition)
        {
            StartCoroutine(LoadLevel(text));
        }
    }

    IEnumerator LoadLevel(string text)
    {
        canTransition = false;
        transitionCanvas.sortingOrder = 1;
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        canTransition = true;
        transitionCanvas.sortingOrder = 0;
        if (text == "backHome") {
            SceneManager.LoadScene(0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
    
}
