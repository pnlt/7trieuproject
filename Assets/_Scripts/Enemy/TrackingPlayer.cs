using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class TrackingPlayer : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private GameManager gameManager;
    private bool pauseGame;
    private bool inGameProcess;
    private Vector3 direction;
    private Vector3 dm;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        gameManager = GameManager._instance;
        dm = Vector3.zero;
    }

    private void Update()
    {
       
        inGameProcess = gameManager.GetGameStart();
        pauseGame = gameManager.GetGamePause();

        if (pauseGame || !inGameProcess) { agent.speed = 0; }
        else
            agent.speed = 30;

        if (gameObject.activeSelf == true && agent.isOnNavMesh)
        {
            agent.destination = player.position;
        }
        else
        {
            agent.Warp(player.position + new Vector3(0, 0, -100f));
        }

        
    }

    private bool IsOnNM()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(agent.transform.position, out hit, 10f, NavMesh.AllAreas);
    }
}
