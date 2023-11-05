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
    public void NextLevel()
    {
        if (canTransition)
        {
            StartCoroutine(LoadLevel());
        }
    }

    IEnumerator LoadLevel()
    {
        canTransition = false;
        transitionCanvas.sortingOrder = 1;
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        transitionAnim.SetTrigger("Start");

        yield return new WaitForSeconds(1);
        canTransition = true;
        transitionCanvas.sortingOrder = 0;
    }
    
}
