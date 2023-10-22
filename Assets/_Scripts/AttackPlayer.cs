using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float radius;

    private ObjectPooling pool;
    RaycastHit hit;


    private void Start()
    {
        pool = transform.parent.GetComponent<ObjectPooling>();
    }

    private void Update()
    {
        PlayerSensor();
    }

    private void PlayerSensor()
    {
        
        
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, playerMask))
        {
            hit.collider.gameObject.GetComponent<PLayerHealth>().GainDamage(damage);
            pool.ReturnObject(gameObject);
        }
         
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
