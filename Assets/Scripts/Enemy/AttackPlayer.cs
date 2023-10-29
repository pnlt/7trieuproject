using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float radius;

    [SerializeField] private Transform player;

    private ObjectPoolingWithProbability pool;


    private void Start()
    {
        pool = transform.parent.GetComponent<ObjectPoolingWithProbability>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        PlayerSensor();
    }

    private void PlayerSensor()
    {
        RaycastHit hit;
        Vector3 direction = (player.position - transform.position).normalized;
        if (Physics.SphereCast(transform.position, radius, direction, out hit, .3f, playerMask))
        {
            hit.collider.gameObject.GetComponent<PLayerHealth>().GainDamage(damage);
            pool.ReturnObject(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}