using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnITem : MonoBehaviour
{
    [SerializeField] private float zOffsetEnd; //base on the length of our map
    [SerializeField] private float _spawnTime;
    [SerializeField] private float zOffset;
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

    // Start is called before the first frame update
    void Start()
    {
        xValues = GameObject.Find("SetXValues");
        //poolEnemyManager = GameObject.Find("PoolEnemyManager");
        //poolMaskManager = GameObject.Find("PoolMaskManager");
        poolManager = GameObject.Find("PoolManager");
        SetUpSpawn();
    }

    private void SetUpSpawn()
    {
        zPos = transform.position.z + zOffsetEnd;
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
        //RandomWithGap();
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
                spawnPos = new Vector3(xPos, transform.position.y, zPos);
                zPos += (zOffset / 2);
                GameObject newItem = poolManager.GetComponent<ObjectPooling>().TakeObject(enemySpawned - i);

                //GameObject newItem = poolEnemyManager.GetComponent<ObjectPooling>().TakeObject();

                if (newItem)
                {
                    newItem.transform.position = spawnPos;
                    newItem.SetActive(true);
                }

                /*GameObject newMask = poolMaskManager.GetComponent<ObjectPooling>().TakeObject();
                if (newMask)
                {
                    newMask.transform.position = spawnPos;
                    newMask.SetActive(true);
                }*/
            }

            spawnTimer = Time.time;
            //zPos += zOffset;
        }
    }
}

