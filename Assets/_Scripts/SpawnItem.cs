using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnITem : MonoBehaviour
{
    [SerializeField] private float zOffsetEnd; //base on the length of our map
    [SerializeField] private float _spawnTime;
    [SerializeField] private float zOffset;
    public GameObject xValues;

    [SerializeField] private GameObject poolManager;

    private float spawnTimer;
    private int objectTobeSpawned;

    private Vector3 spawnPos;
    private float[] xPosValues = new float[3];
    private List<Transform> getListX_Values = new List<Transform>();
    private float zPos;
    private float xPos;

    // Start is called before the first frame update
    void Start()
    {
        xValues = GameObject.Find("SetXValues");
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
            xPos = xPosValues[Mathf.RoundToInt(Random.Range(0, xPosValues.Length))];
            for (int i = 0; i < Random.Range(2, 5); i++)
            {
                spawnPos = new Vector3(xPos, transform.position.y, zPos);
                zPos += (zOffset / 2);
                GameObject newItem = poolManager.GetComponent<ObjectPooling>().TakeObject();
                if (newItem)
                {
                    newItem.transform.position = spawnPos;
                    newItem.SetActive(true);
                }
            }

            spawnTimer = Time.time;
            //zPos += zOffset;
        }
    }

    //Random object in certain gap
    private void RandomWithGap()
    {
        /*
         * we need the time defining when we will continue spawning
         * -----------------------------------------------------------------
         * Our Ideas is:
         * we will track the zPos where we start spawn object, then in process of spawning object randomly
         * we need to change zPos respectively
         * ------------------------------------------------------------------
         * for xPos, we will choose randomly its values based on the width of our map 
         * for yPos, we maintain it
         * 
         */

        if (Time.time - spawnTimer > _spawnTime)
        {
            xPos = xPosValues[Mathf.RoundToInt(Random.Range(0, xPosValues.Length))];
            spawnPos = new Vector3(xPos, transform.position.y, zPos);
            zPos += zOffset;
            GameObject newItem = poolManager.GetComponent<ObjectPooling>().TakeObject();
            newItem.transform.position = spawnPos;
            newItem.SetActive(true);
            spawnTimer = Time.time;
        }

    }
}

