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
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SpeedUp(float increment)
    {
        speed += increment * Time.deltaTime;
    }

    private void Rotation()
    {
        Quaternion currentRotation = transform.rotation;

        if (player)
        {
            direction = player.position - transform.position;
            Quaternion target = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(currentRotation, target, 7f * Time.deltaTime);
        }
    }
}
