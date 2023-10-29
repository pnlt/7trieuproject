using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceRolling : MonoBehaviour //IPointerClickHandler
{
    
 
    private bool isRolling = false;
    public Player_Controller playerController;
    public GameObject powerUpPanel;
    public Transform diceTransform;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isRolling)
        {
            RollDice();
        }
    }
    public void RollDice()
    {
        if (!isRolling)
        {
            isRolling = true;

         

 
            LeanTween.rotateAroundLocal(diceTransform.gameObject, Vector3.forward, 360f, 1f)     
                .setEase(LeanTweenType.easeOutQuad)
               
                .setOnComplete(() =>
                {
                   diceTransform.transform.localRotation = Quaternion.identity;
                    isRolling = false;
                
                    playerController.ApplyRandomPowerUp();
             
                        powerUpPanel.SetActive(false);
                  
                });
        }
    }
    }
