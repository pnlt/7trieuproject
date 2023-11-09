using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class TrackingPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform player;
    private GameManager gameManager;
    private bool pauseGame;
    private bool inGameProcess;
    private Vector3 direction;
    private Vector3 dm;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();

    }
    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
    }

    private void Start()
    {
        gameManager = GameManager._instance;
        dm = Vector3.zero;
    }

    private void Update()
    {
        MovementBehaviour();
        if (transform.position.z < player.position.z)
        {
            //Debug.Log("cc");
        }
    }

    private void MovementBehaviour()
    {
        Rotation();

        pauseGame = gameManager.GetGamePause();
        inGameProcess = gameManager.GetGameStart();
        if (!pauseGame && inGameProcess)
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void Rotation()
    {
        Quaternion currentRotation = transform.rotation;

        if (player)
        {
            direction = player.position - transform.position;
            Quaternion target = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(currentRotation, target, 5f * Time.deltaTime);
        }
    }
}