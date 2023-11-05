using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPoolingRoad : MonoBehaviour
{
    [SerializeField] private GameObject[] Road_Default;
    [SerializeField] private GameObject[] pooledPrefabs;
    [SerializeField] private int[] maskThreshold;
    public static ObjectPoolingRoad instance;
    

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 5;

    private int currentMapIdx;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMapIdx = 0;
        for (int i = 0; i < amountToPool; i++)
        {
            pooledObjects.Add(Road_Default[i]);
        }
    }

    public GameObject GetPooledObject()
    {
        UpdateMapSpawned(ref currentMapIdx);
        int totalPoolObject = pooledObjects.Count;
        if (totalPoolObject == 0)
            CreateObjects(pooledPrefabs, currentMapIdx);

        GameObject objectTaken = null;
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                objectTaken = pooledObjects[i];
                pooledObjects.RemoveAt(i);
                break;
            }
        }

        /*if (objectTaken == null)
        {
            objectTaken = Instantiate(pooledPrefabs[currentMapIdx]);
            objectTaken.transform.parent = GameObject.Find(pooledPrefabs[currentMapIdx].name).transform;
            objectTaken.SetActive(false);
        }*/
        return objectTaken;
    }

    private void UpdateMapSpawned(ref int currentMapIdx)
    {
        int currentMask = PlayerPrefs.GetInt("total_masks");
        if (currentMask >= maskThreshold[currentMapIdx])
        {
            if (currentMapIdx >= pooledPrefabs.Length - 1)
                currentMapIdx = pooledPrefabs.Length - 1;
            else
            {
                currentMapIdx += 1;
                pooledObjects.Clear();
            }
            
        }
    }

    private void CreateObjects(GameObject[] mapPrefabs, int currentIdx)
    {
        GameObject go = Instantiate(mapPrefabs[currentIdx]);
        go.transform.parent = GameObject.Find(mapPrefabs[currentIdx].name).transform;
        go.SetActive(false);
        pooledObjects.Add(go);

    }
}
