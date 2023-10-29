using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnITem : MonoBehaviour
{
    [SerializeField] private float zOffsetEnd; //base on the length of our map
    [SerializeField] private float _spawnTime;
    public GameObject xValues;
    [SerializeField] private int maxEnemyNumber;

    //[SerializeField] private GameObject poolEnemyManager;
    [SerializeField] private GameObject poolManager;
    //[SerializeField] private GameObject poolMaskManager;

    private float spawnTimer;

    private Vector3 spawnPos;
    private float[] xPosValues = new float[3];
    private List<Transform> getListX_Values = new List<Transform>();
    private float zPos;
    private float xPos;
    private float yPos;

    // Start is called before the first frame update
    void Start()
    {
        xValues = GameObject.Find("SetXValues");
        poolManager = GameObject.Find("PoolEnemyManager");
        SetUpSpawn();
    }

    private void SetUpSpawn()
    {
        zPos = transform.position.z + zOffsetEnd;
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
        Invoke("SpawnItemsRandomly", 1);
    }

    private void SpawnItemsRandomly()
    {
        LineRandomContinuously();
    }


    //Random object continuously in one line
    private void LineRandomContinuously()
    {
        if (Time.time - spawnTimer > _spawnTime)
        {
            int enemySpawned = Random.Range(1, maxEnemyNumber);
            xPos = xPosValues[Mathf.RoundToInt(Random.Range(0, xPosValues.Length))];
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

                /*GameObject newMask = poolMaskManager.GetComponent<ObjectPooling>().TakeObject();
                if (newMask)
                {
                    newMask.transform.position = spawnPos;
                    newMask.SetActive(true);
                }*/
            }

            spawnTimer = Time.time;
            zPos = transform.position.z + zOffsetEnd * 1.5f;

        }

    }
}
