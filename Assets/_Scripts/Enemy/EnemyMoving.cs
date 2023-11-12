using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float speed;

    private Vector3 direction;
    private GameManager gameManager;
    private bool pauseGame;
    private bool inGameProcess;
    private float speedRate;

    private void OnEnable()
    {
        speedRate = 1f;
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        gameManager = GameManager._instance;
    }

    private void Update()
    {
        MovementBehaviour();
    }

    private void MovementBehaviour()
    {
        Rotation();

        pauseGame = gameManager.GetGamePause();
        inGameProcess = gameManager.GetGameStart();
        if (!pauseGame && inGameProcess)
        {
            transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * speed * Time.deltaTime;
        }
            
    }

    public void SpeedUp(float increment)
    {
        speedRate += increment * .01f;
    }

    private void Rotation()
    {
        Quaternion currentRotation = transform.rotation;

        if (player)
        {
            direction = player.position - transform.position;
            Quaternion target = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(currentRotation, target, 12f * Time.deltaTime);
        }
    }
}
