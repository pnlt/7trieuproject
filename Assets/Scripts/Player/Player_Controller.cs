using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
   Rigidbody rigidbody;

    public static bool tap, swipeLeft, swipeRight;
    private Vector2 startTouch, swipeDelta;
    private bool isDraging = false;

    private int desiredLane = 1; //0: left 1:center 2:right
    public float laneDistance = 3;

    public float side_speed;
    public float running_Speed;
    public float jump_Force;

    // Health System
    public int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private Image[] hearts;

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    // Speed boost

    private float originalRunningSpeed;
    private float speedBoostMultiplier = 1.5f;
    private float speedBoostDuration = 5.0f;
 
    public GameObject shieldPrefab; 
    private float shieldDuration = 6.0f;

    private const string POWERUP_TAG = "PowerUp";

    // Dice rolling controller

    [SerializeField] private GameObject powerUpPanel;
    private bool isRollingDice;
   



    public bool isGameStarted;
    
    public bool isGameOver;
    
    private bool isShieldActive;

   


    [SerializeField] Animator player_Animator;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject Tap_To_Start_Canvas;

  


    private void Start()
    {
        isGameStarted = false;
        isGameOver = false;
        isShieldActive = false;
        rigidbody = GetComponent<Rigidbody>();
       

        // Health System Initialization
        currentHealth = maxHealth;
        UpdateHeartsUI();
        // Get the dice rolling controller
       isRollingDice = false;
        powerUpPanel.SetActive(false);

        PlayerPrefs.SetInt("total_masks", 0);
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }

    // Update is called once per frame
    [Obsolete]
    void Update()
    {
      
        if (!isGameStarted || !isGameOver) {
            if (Input.GetMouseButtonDown(0) && !isGameOver) {
               // Debug.Log("Game is started");
                isGameStarted = true;
                player_Animator.SetInteger("isRunning", 1);
                player_Animator.speed = 1.3f;
                Tap_To_Start_Canvas.SetActive(false);
            }
        }

        if (isGameStarted) 
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
            swipeLeft = swipeRight = false;

            #region Standalone Inputs
            if (Input.GetMouseButtonDown(0))
            {
                isDraging = true;
                startTouch = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDraging = false;
                Reset();
            }
            #endregion

            #region Mobile Input
            if (Input.touches.Length > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    isDraging = true;
                    startTouch = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                {
                    isDraging = false;
                    Reset();
                }
            }
            #endregion

            swipeDelta = Vector2.zero;
            if (isDraging)
            {
                if (Input.touches.Length < 0)
                    swipeDelta = Input.touches[0].position - startTouch;
                else if (Input.GetMouseButton(0))
                    swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }

            if (swipeDelta.magnitude > 100)
            {
                float x = swipeDelta.x;
                float y = swipeDelta.y;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    //Swipe Left or Right
                    if (x < 0)
                    {
                        desiredLane--;
                        if (desiredLane == -1) desiredLane = 0;
                    }
                    else
                    {
                        desiredLane++;
                        if (desiredLane == 3) desiredLane = 2;
                    }
                }
                else
                {
                    if (isGrounded)
                    {
                        //Swipe Up or Down
                        if (y < 0)
                        {

                        }
                        else
                        {
                            rigidbody.velocity = Vector3.up * jump_Force;
                          
                            StartCoroutine(Jump());
                        }
                    }
                }
                Reset();
            }



            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (desiredLane == 0)
            {
                targetPosition += Vector3.left * laneDistance;
            }
            if (desiredLane == 2)
            {
                targetPosition += Vector3.right * laneDistance;
            }

            // Calculate the new position.
            Vector3 newPosition = Vector3.Lerp(rigidbody.position, targetPosition, 10 * Time.deltaTime);

            rigidbody.MovePosition(newPosition += -Vector3.back * Time.deltaTime * running_Speed);

   


        }

        if (isGameOver) {
            if (!GameOverPanel.gameObject.active){
                GameOverPanel.SetActive(true);
            }
        }

    }




    IEnumerator Jump() {
        player_Animator.SetInteger("isJump", 1);
        yield return new WaitForSeconds(0.1f);
        player_Animator.SetInteger("isJump", 0);
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }

    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.tag == "object" && !isShieldActive) {

            currentHealth--;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isGameStarted = false;
                isGameOver = true;
                player_Animator.applyRootMotion = true;
                player_Animator.SetInteger("isDied", 1);
            }
            UpdateHeartsUI();
        }
    }
  

    public enum PowerUp
    {
        SpeedBoost,
        ExtraLife,
        Shield
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == POWERUP_TAG && !isRollingDice)
        {
            // Stop the player
              isGameStarted = false;
            player_Animator.StopPlayback();
            player_Animator.speed = 0f;


            // Display the PowerUpPanel
            powerUpPanel.SetActive(true);

          
        }
    }

  

 





    public void ApplyRandomPowerUp()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue < 0.6f)  // 60% chance for SpeedBoost
        {
            ActivateSpeedBoost();
        }
        else if (randomValue < 0.8f)  // 20% chance for ExtraLife
        {
            AddExtraLife();
        }
        else  // 20% chance for Shield
        {
            ActivateShield();
        }
    }



    public void OnPowerUpSelected(PowerUp selectedPowerUp, GameObject powerUpObject)
    {

        switch (selectedPowerUp)
        {
            case PowerUp.SpeedBoost:
                ActivateSpeedBoost();
                break;
            case PowerUp.ExtraLife:
                AddExtraLife();
                break;
            case PowerUp.Shield:
                ActivateShield();
                break;
        }



    }

    public void ActivateSpeedBoost()
    {
        // Increase running speed during speed boost
        originalRunningSpeed = running_Speed;
        running_Speed *= speedBoostMultiplier;

        

        // Restore the original running speed after a short delay
        Invoke("RestoreOriginalRunningSpeed", speedBoostDuration);

    }
    private void RestoreOriginalRunningSpeed()
    {
        // Restore the original running speed
        running_Speed = originalRunningSpeed;
    }

    public void AddExtraLife()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateHeartsUI();
        }
        else
        {
            Debug.Log("Maximum health reached. Cannot gain more hearts.");
        }

    }

    public void ActivateShield()
    {
        isShieldActive = true;

        
        // Create a shield object
        GameObject shieldObject = Instantiate(shieldPrefab, transform.position, Quaternion.identity);

     
        // Calculate an offset to position the shield in front of the player
        Vector3 shieldOffset = new Vector3(0, 1, 7); 

        // Position the shield relative to the player with the calculated offset
        shieldObject.transform.parent = transform;
        shieldObject.transform.localPosition = shieldOffset;

        Vector3 shieldScale = new Vector3(0.005f, 0.005f, 0.005f);  
        shieldObject.transform.localScale = shieldScale;

        StartCoroutine(DeactivateShieldAfterDuration(shieldObject, shieldDuration));

    
    }

    private IEnumerator DeactivateShieldAfterDuration(GameObject shieldObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(shieldObject);
        isShieldActive = false;
        
    }
}


