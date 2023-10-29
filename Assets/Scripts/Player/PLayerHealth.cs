using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private ParticleSystem poisonInfected;
    // Start is called before the first frame update
    void Start()
    {
        //poisonInfected = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (maxHealth == 0)
            Debug.Log("Die");
    }

    private IEnumerator PoisonEffect()
    {
        poisonInfected.gameObject.SetActive(true);
        poisonInfected.Play(true);

        yield return new WaitUntil(() => poisonInfected.isStopped);
        poisonInfected.gameObject.SetActive(false);
        //poisonInfected.gameObject.SetActive(false);
    }

    public void GainDamage(float damage)
    {
        StartCoroutine(PoisonEffect());
        maxHealth -= damage;
    }
}
