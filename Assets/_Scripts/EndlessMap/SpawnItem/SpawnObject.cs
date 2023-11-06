using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnObject : MonoBehaviour
{
    [SerializeField] private float zOffsetSpawn; //base on the length of our map
    [SerializeField] private float _spawnTime;
    [SerializeField] private int maxEnemyNumber;
    [SerializeField] private GameObject poolManager;
    [SerializeField] public GameObject xValues;

    private Player_Controller playerController;

    private float spawnTimer;
    private Vector3 spawnPos;
    private float[] xPosValues = new float[3];
    private List<Transform> getListX_Values = new List<Transform>();
    private float zPos;
    private float xPos;
    private float yPos;

    // Start is called before the first frame update
    private void Start()
    {
        poolManager = GameObject.FindGameObjectWithTag("Pooling");
        playerController = GetComponentInParent<Player_Controller>();
        SetUpSpawn();
    }

    private void SetUpSpawn()
    {
        yPos = transform.position.y;
        spawnTimer = Time.time;
        getListX_Values = xValues.GetComponent<X_Values>().GetX_Values();

        for (int i = 0; i < xPosValues.Length; i++)
        {
            xPosValues[i] = getListX_Values[i].position.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isGameStarted)
            Invoke("SpawnItemsRandomly", 1);
    }

    private void SpawnItemsRandomly()
    {
        if (!playerController.isGameOver && !playerController.pauseGame)
        LineRandomContinuously();
    }


    //Random object continuously in one line
    private void LineRandomContinuously()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= _spawnTime)
        {
            int enemySpawned = Random.Range(1, maxEnemyNumber);
            xPos = xPosValues[Mathf.RoundToInt(Random.Range(0, xPosValues.Length))];

            if (zPos < transform.position.z)
            {
                zPos = transform.position.z + zOffsetSpawn * 0.75f;
            }
            else
                zPos = transform.position.z + zOffsetSpawn;

            for (int i = 0; i < enemySpawned; i++)
            {
                spawnPos = new Vector3(xPos, yPos, zPos);
                zPos += 3;
                GameObject newEnemy = poolManager.GetComponent<ObjectPoolingWithProbability>().TakeObject(enemySpawned - i);

                if (newEnemy)
                {
                    newEnemy.transform.position = spawnPos;
                    newEnemy.SetActive(true);
                }
            }
            spawnTimer = 0;
        }
    }
}
