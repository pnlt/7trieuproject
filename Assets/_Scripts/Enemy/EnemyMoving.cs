using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float speed;

    private Vector3 direction;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        MovementBehaviour();
    }

    private void MovementBehaviour()
    {
        Rotation();
        
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
