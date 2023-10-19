using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNotify : MonoBehaviour
{
    public bool destroyed { get; private set; }
    public bool generated { get; private set; }

    private bool mark;

    private void Start()
    {
        mark = false;
    }

    private void Update()
    {
        mark = GetComponentInParent<EndlessMap>().mark;
        if (mark)
        {
            destroyed = false;
            generated = false;
        }    
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            destroyed = true;
            generated = true;
   
        }    
    }
}
