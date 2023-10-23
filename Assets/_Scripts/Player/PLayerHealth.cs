using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (maxHealth == 0)
            Debug.Log("Die");
    }

    public void GainDamage(float damage)
    {
        maxHealth -= damage;
    }
}

