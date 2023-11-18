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

    public bool isGameStarted { get; private set; }
    public bool isGameOver { get; private set; }
    public bool isShieldActive { get; private set; }
    public bool pauseGame;
    public bool isTutorial;
    private bool inSwitchMode;

    //Touch motions in game
    private static bool swipeLeft, swipeRight, swipeTop;
    private bool swipeLeftTuto, swipeTopTuto;
    private Vector2 diff;
    private int desiredLane = 1; //0: left 1:center 2:right
    private bool isGrounded;
    private bool isSpeedBoostActive = false;
    protected Vector2 m_StartingTouch;
    protected bool m_IsSwiping = false;

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
    private float distancePerSecond;

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
        distancePerSecond = gameManager.distanceContainer;
        powerUpType = PowerUp.Default;
        isTutorial = gameManager.GetIsTutorialGamePlay();
        OnTutorial();
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
        if (!isTutorial && isGameStarted)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
            swipeLeft = swipeRight = swipeTop = false;

            if (Input.GetMouseButtonDown(0))
            {
                m_IsSwiping = true;
                m_StartingTouch = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_IsSwiping = false;
            }

            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    m_IsSwiping = true;
                    m_StartingTouch = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    m_IsSwiping = false;
                }
            }

            diff = Vector2.zero;
            if (m_IsSwiping)
            {
                if (Input.touches.Length < 0)
                    diff = Input.GetTouch(0).position - m_StartingTouch;
                else if (Input.GetMouseButton(0))
                    diff = (Vector2)Input.mousePosition - m_StartingTouch;

                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);

                if (diff.magnitude > 0.01f)
                {
                    if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                    {
                        if (diff.y < 0 && !isGrounded)
                        {
                            //player_Animator.SetInteger("isJump", 0);
                            //rigid.velocity = Vector3.up * -5;
                        }
                        else if (diff.y > 0 && !swipeTopTuto && isGrounded)
                        {
                            swipeTop = true;
                            rigid.velocity = Vector3.up * jump_Force;
                            StartCoroutine(Jump());
                        }
                    }
                    else
                    {
                        if (diff.x < 0 && !swipeLeftTuto)
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

                    m_IsSwiping = false;
                }
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


    private void ApproachSwitchMap()
    {
        inSwitchMode = gameManager.GetSwitchMap();
        if (inSwitchMode)
        {
            running_Speed = 0;
            gameManager.SetGamePause(true);
            gameManager.SetGameStart(false);
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
                timePass = 0.5f - (0.1f * accumulatedSpeedIncrease);
            }
        }
       
        return distancePerSecond;
    }

    private void GameOver()
    {
        isGameOver = gameManager.GetGameOver();
        if (isGameOver)
        {
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


