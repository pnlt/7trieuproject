using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackChecking : MonoBehaviour
{
    [SerializeField] private LayerMask virusMask;
    [SerializeField] private float length;

    [SerializeField] private float speedRate;

    private void Update()
    {
        Debug.DrawRay(transform.position, -transform.forward * length, Color.red);
        CheckVirusBackward();
    }

    //maybe adjusting more and more
    private void CheckVirusBackward()
    {
        RaycastHit hit;
        Vector3 dir = transform.TransformDirection(-transform.forward) * length;
        Ray rayBackward = new Ray(transform.position, dir);

        if (Physics.Raycast(rayBackward, out hit, 10f, virusMask, QueryTriggerInteraction.Collide))
        {
            
            if (hit.collider.gameObject.GetComponent<EnemyReturn>() 
                || hit.collider.gameObject.GetComponent<EnemyMoving>())
            {
                hit.collider.gameObject.GetComponent<EnemyReturn>().LifeCoolDown();
                hit.collider.gameObject.GetComponent<EnemyMoving>().SpeedUp(speedRate);
            }


        }
    }
}
