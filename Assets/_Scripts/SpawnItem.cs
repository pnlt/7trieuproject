using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnITem : MonoBehaviour
{
    [SerializeField] private float zOffsetEnd; //base on the length of our map
    [SerializeField] private float _spawnTime;
    [SerializeField] private float zOffset;

    //[SerializeField] private GameObject[] itemPrefab;
    //[SerializeField] private GameObject _gameObject;
    [SerializeField] private GameObject poolManager;

    private float spawnTimer;
    private int objectTobeSpawned;

    private Vector3 spawnPos;
    private float[] xPosValues = { -4, 0, 4 };
    private float zPos;
    private float xPos;

    // Start is called before the first frame update
    void Start()
    {
        zPos = transform.position.z + zOffsetEnd;
        spawnTimer = Time.time;

        poolManager = GameObject.Find("PoolManager");

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
            xPos = xPosValues[Mathf.RoundToInt(Random.Range(0, xPosValues.Length))];
            for (int i = 0; i < Random.Range(2, 5); i++)
            {
                spawnPos = new Vector3(xPos, transform.position.y, zPos);
                zPos += (zOffset / 2);
                GameObject newItem = poolManager.GetComponent<ObjectPooling>().TakeObject();
                newItem.transform.position = spawnPos;
                newItem.SetActive(true);
                Debug.Log("cc");

            }

            spawnTimer = Time.time;
            zPos += zOffset;
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

        /* Illustrative code
         * -----------------------------------------------------------
         * if (pass the time to continue spawning):
         *      xPos = Random.Range(three equally parts of the map's x-axis)
         *      spawnPos = new Vector3 (xPos, transform.position.y, zPos);
         *      GameObject item = ObjectPooling.TakeObject();
         *      item.transform.position = spawnPos;
         *      item.transform.setActive(true);
         *      update zPos;  
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



        //GameObject item = Instantiate(itemPrefab[objectPrefab], )
    }
}

