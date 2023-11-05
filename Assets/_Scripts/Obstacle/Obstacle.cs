using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Animator anim;
    Collider colliderObject;

    private void Start()
    {
        anim = GetComponent<Animator>();
        colliderObject = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        anim.SetInteger("IsHit", 0);
        colliderObject.enabled = true;
    }

    public virtual void Impacted()
    {
        if (anim != null)
        {
            anim.SetInteger("IsHit", 1);
        }
    }
}
