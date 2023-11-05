using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private ParticleSystem poisonInfected;
    [SerializeField] private int maxHealth;

    private GameManager gameManager;
    private UI_Manager uiManager;

    public int currentHealth { get; set; }

    public int GetHealth()
    {
        return this.maxHealth;
    }

    public void SetHealth(int health)
    { 
        this.maxHealth = health;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        gameManager = GameManager._instance;
        uiManager = UI_Manager._instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameManager.SetGameOver(true);
        }
    }

    public void DiminishHealth()
    {
        currentHealth--;
        uiManager.UpdateHeartsUI(currentHealth);
    }

    private IEnumerator PoisonEffect()
    {
        poisonInfected.gameObject.SetActive(true);
        poisonInfected.Play(true);

        yield return new WaitUntil(() => poisonInfected.isStopped);

        poisonInfected.gameObject.SetActive(false);
    }

    public void GainDamage(float damage)
    {
        if (!GetComponentInParent<Player_Controller>().isShieldActive)
        {
            StartCoroutine(PoisonEffect());
            DiminishHealth();
        }
        
    }
}
