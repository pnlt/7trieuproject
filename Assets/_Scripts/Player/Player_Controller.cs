using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

#region EffectsType
public enum PowerUp
{
    Default,
    SpeedBoost,
    ExtraLife,
    Shield
}
#endregion

public class Player_Controller : MonoBehaviour
{
    [Header ("Components Reference")]
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator player_Animator;

    [Header ("Motion Parameters")]
    [SerializeField] private float laneDistance = 3;
    [SerializeField] private float side_speed;
    [SerializeField] private float running_Speed;
    [SerializeField] private float jump_Force;
    [SerializeField] private float speedIncreaseRate = 0.1f;

    [Header ("Gameobjects reference")]
    [SerializeField] private GameObject shieldPrefab;

    [Space (4)]

    [Header ("Others")]
    [SerializeField] private float shieldDuration;

    //Objects References
    private PlayerHealth health;
    private UI_Manager uiManager;
    private GameManager gameManager;
    private PowerUp powerUpType;
    private GameObject target;

    public bool isGameStarted { get; private set; }
    public bool isGameOver { get; private set; }
    public bool isShieldActive { get; private set; }
    public bool pauseGame;
    public bool isTutorial;

    //Touch motions in game
    private static bool tap, swipeLeft, swipeRight, swipeTop;
    private bool swipeLeftTuto, swipeTopTuto;
    private Vector2 startTouch, swipeDelta;
    private bool isDraging = false;
    private int desiredLane = 1; //0: left 1:center 2:right
    private bool isGrounded;
    private bool isSpeedBoostActive = false;

    //Effects in game
    private string POWERUP_TAG = "PowerUp";

//Speed boost's parameters effect
    private float originalRunningSpeed;
    private float speedBoostMultiplier = 1.5f;
    private float speedBoostDuration = 5.0f;
    private float accumulatedSpeedIncrease = 0;
    //Shield paremeters effect
    private Coroutine getShield; 
    private bool hasShield;
    private float timePass = .5f;

    private float distance;
    private float distancePerSecond = 0;

    //ObstacleLayerIndex
    private string Obstacle_Tag = "Obstacle";

    private void OnTutorial()
    {
        if (isTutorial)
        {
            swipeTopTuto = swipeLeftTuto = true;
        }
        else
        {
            swipeTopTuto = swipeLeftTuto = false;
        }
    }

    private void SetUp()
    {
        isGameStarted = false;
        isGameOver = false;
        isShieldActive = false;
        hasShield = false;
        powerUpType = PowerUp.Default;
        isTutorial = gameManager.GetIsTutorialGamePlay();
        OnTutorial();
        PlayerPrefs.SetInt("total_masks", 0);
    }

    private void Start()
    {
        uiManager = UI_Manager._instance;
        gameManager = GameManager._instance;
        rigid = GetComponent<Rigidbody>();
        health = GetComponent<PlayerHealth>();

        SetUp();
    }

    private void InputChecking()
    {
        if (!isTutorial)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
            swipeLeft = swipeRight = swipeTop = false;

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
                    if (x < 0 && !swipeLeftTuto)
                    {
                        swipeLeft = true;
                        desiredLane--;
                        if (desiredLane == -1) desiredLane = 0;
                    }
                    else
                    {
                        swipeRight = true;
                        desiredLane++;
                        if (desiredLane == 3) desiredLane = 2;
                    }
                }
                else
                {
                        //Swipe Up or Down
                        if (y < 0 && !isGrounded )
                        {
                            player_Animator.SetInteger("isJump", 0);
                            rigid.velocity = Vector3.up * -5;
                        }
                        else if (y > 0 && !swipeTopTuto && isGrounded)
                        {
                            swipeTop = true;
                            rigid.velocity = Vector3.up * jump_Force;
                            StartCoroutine(Jump());
                      

                        }
                    //    else if (!isGrounded && rigid.velocity.y < 0)
                    //{
                       
                    //        rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y * -0.8f, rigid.velocity.z);
                        
                    //}
                }
                Reset();
            }
        }
    }

    private void StartMotion()
    {
        isGameStarted = gameManager.GetGameStart();
        pauseGame = gameManager.GetGamePause();

        if (pauseGame)
            player_Animator.SetInteger("isRunning", 0);
            
        if (isGameStarted && !pauseGame)
        {
            SetUpAnimStart();
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
            Vector3 newPosition = Vector3.Lerp(rigid.position, targetPosition, 10 * Time.deltaTime);
            rigid.MovePosition(newPosition += -Vector3.back * Time.deltaTime * running_Speed);        

            distance = CalculateDistance();
            gameManager.distanceTravese = distance;

            if (!isSpeedBoostActive)
            {

                float currentIncrease = speedIncreaseRate * Time.deltaTime;
                running_Speed += currentIncrease;
                accumulatedSpeedIncrease += currentIncrease;

            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        InputChecking();
        StartMotion();
        ApproachSwitchMap();
        GameOver();
    }    

    public void HanbokHolder(GameObject hanbok)
    {
        if (hanbok)
        {
            target = hanbok;
        }
    }

    private void ApproachSwitchMap()
    {
        if (target)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < 15)
            {
                running_Speed = 0;
                gameManager.SetGamePause(true);
                //kick off some events
            }
        }
    }

    private float CalculateDistance()
    {
        if (!pauseGame)
        {
            timePass -= Time.deltaTime;

            if (timePass < 0)
            {
                distancePerSecond += 1;
                timePass = 0.5f - (0.2f * accumulatedSpeedIncrease);
            }
        }
       
        return distancePerSecond;
    }

    private void GameOver()
    {
        isGameOver = gameManager.GetGameOver();
        if (isGameOver)
        {
            gameManager.SetGameOver(false);
            gameManager.SetGameStart(false);
            gameManager.SetGamePause(true);
            gameManager.SetBestScore((int)distance);
            uiManager.ShowUpGameOverPanel();
            health.currentHealth = 1;
            SetUpAnimEnd();
        }
    }

    private void SetUpAnimStart()
    {
        player_Animator.SetInteger("isRunning", 1);
        player_Animator.speed = 1.3f;
    }

    private void SetUpAnimEnd()
    {
        player_Animator.applyRootMotion = true;
        player_Animator.SetInteger("isDied", 1);
    }

    IEnumerator Jump()
    {

        player_Animator.SetInteger("isJump", 1);
        yield return new WaitForSeconds(0.4f);
        if (!isGrounded)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y * -0.1f, rigid.velocity.z);
        }
        player_Animator.SetInteger("isJump", 0);
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(POWERUP_TAG) && !pauseGame)
        {
            StartCoroutine(ShowUpEffect());
        }
        else if (other.gameObject.CompareTag(Obstacle_Tag) && !isShieldActive)
        {
            player_Animator.SetInteger("isJump", 0);
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * -.5f);
            gameManager.SetGamePause(true);
            other.enabled = false;

            Obstacle ob = other.gameObject.GetComponent<Obstacle>();
            if (ob != null)
            {
                rigid.velocity = -Vector3.forward * jump_Force;

                health.DiminishHealth();
                ob.Impacted();
            }
            player_Animator.SetTrigger("hitTrigger");
        }
        else if (other.gameObject.CompareTag("Tutorial"))
        {
            if (other.gameObject.name == "Right")
            {
                isTutorial = false;
                gameManager.SetGamePause(true);
                uiManager.ShowUpswipeRightPanel();
            }
            if (other.gameObject.name == "Left")
            {
                swipeLeftTuto = false;
                gameManager.SetGamePause(true);
                uiManager.ShowUpswipeLeftPanel();
            }
            if (other.gameObject.name == "Top")
            {
                swipeTopTuto = false;
                gameManager.SetGamePause(true);
                uiManager.ShowUpswipeTopPanel();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tutorial"))
        {
            if (other.gameObject.name == "Right")
            {
                if (swipeRight)
                {
                    uiManager.HideswipeRightPanel();
                    gameManager.SetGamePause(false);
                }
            }
            if (other.gameObject.name == "Left")
            {
                if (swipeLeft)
                {
                    uiManager.HideswipeLeftPanel();
                    gameManager.SetGamePause(false);
                }
            }
            if (other.gameObject.name == "Top")
            {
                if (swipeTop)
                {
                    player_Animator.SetInteger("isJump", 1);
                    uiManager.HideswipeTopPanel();
                    gameManager.SetGamePause(false);
                }
            }
        }
    }

    #region EffectsHandler  

    public IEnumerator ShowUpEffect()
    {
        ApplyRandomPowerUp();
        gameManager.SetGamePause(true);
        uiManager.SetTextEffectPanel(powerUpType.ToString());
        uiManager.SetSpriteEffects(powerUpType);
        uiManager.ShowUpEffectNotify();

        yield return new WaitForSeconds(1f);

        StopCoroutine(ShowUpEffect());
        gameManager.SetGamePause(false);
        uiManager.HideEffectPanel();
        OnPowerUpSelected(powerUpType);
    }

    public void ApplyRandomPowerUp()
    {
        if (!pauseGame)
        {
            float randomValue = UnityEngine.Random.value;

            if (randomValue > 0.5f)  // 60% chance for SpeedBoost
            {
                powerUpType = PowerUp.SpeedBoost;
            }
            else if (randomValue > 0.2f)  // 20% chance for ExtraLife
            {
                powerUpType = PowerUp.ExtraLife;
            }
            else  // 20% chance for Shield
            {
                powerUpType = PowerUp.Shield;
            }
        }
    }

    public void OnPowerUpSelected(PowerUp selectedPowerUp)
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
        if (!isSpeedBoostActive)
        {
            // Increase running speed during speed boost
            originalRunningSpeed = running_Speed;
            running_Speed *= speedBoostMultiplier;

            // Set the speed boost as active
            isSpeedBoostActive = true;

            // Restore the original running speed after a short delay
            StartCoroutine(RestoreOriginalRunningSpeedAfterDelay(speedBoostDuration));
        }
        powerUpType = PowerUp.Default;

    }
    private IEnumerator RestoreOriginalRunningSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restore the original running speed
        running_Speed = originalRunningSpeed + accumulatedSpeedIncrease;

        // Set the speed boost as inactive
        isSpeedBoostActive = false;
    }

    public void AddExtraLife()
    {
        if (health.currentHealth < health.GetHealth())
        {
            health.currentHealth++;
            uiManager.UpdateHeartsUI(health.currentHealth);
        }
        else
        {
            Debug.Log("Maximum health reached. Cannot gain more hearts.");
        }
        powerUpType = PowerUp.Default;
    }

    public void ActivateShield()
    {
        if (hasShield)
        {
            StopCoroutine(getShield);
            getShield = StartCoroutine(DeactivateShieldAfterDuration(shieldDuration));
        }
        else
        {
            powerUpType = PowerUp.Default;
            hasShield = true;
            getShield = StartCoroutine(DeactivateShieldAfterDuration(shieldDuration));
        }
    }

    private IEnumerator DeactivateShieldAfterDuration(float duration)
    {
        shieldPrefab.SetActive(true);
        isShieldActive = true;

        yield return new WaitForSeconds(duration);

        hasShield = false;
        isShieldActive = false;
        shieldPrefab.SetActive(false);
    }

    #endregion
}


