using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrackingPlayer : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.Find("Capsule").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = player.position;
    }
}
