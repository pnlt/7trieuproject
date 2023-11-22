using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public string name;
    public GameObject prefabEnemy;
    [Range(0f, 100f)] public int chance;
    [HideInInspector] public float _weight;
}

public class ObjectPoolingWithProbability : MonoBehaviour
{
    [SerializeField] private bool expendable;
    [SerializeField] private int count;
    [SerializeField] private EnemyType[] objectSpawnRandom;

    private List<GameObject> listObjectInPool;
    private List<GameObject> usedList;
    private float lengthPrefab;

    private float accumulatedWeight;
    private System.Random random = new System.Random();

    private void Awake()
    {
        listObjectInPool = new List<GameObject>();
        usedList = new List<GameObject>();
        lengthPrefab = objectSpawnRandom.Length;
        CalculateWeight();

        for (int i = 0; i < count; i++)
        {
            GenerateObject(1);
        }
    }

    public GameObject TakeObject(int enemyLack)
    {
        int totalCount = listObjectInPool.Count;
        if (totalCount == 0 && !expendable) return null;
        else if (totalCount == 0)
        {
            GenerateObject(enemyLack);
            totalCount = listObjectInPool.Count;
        }

        GameObject obj = listObjectInPool[totalCount - 1];
        listObjectInPool.RemoveAt(totalCount - 1);
        usedList.Add(obj);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        usedList.Remove(obj);
        listObjectInPool.Add(obj);
        obj.SetActive(false);
    }

    private int GetRandomEnemy()
    {
        if (lengthPrefab >= 2)
        {
            double rand = random.NextDouble() * accumulatedWeight;
            for (int i = 0; i < objectSpawnRandom.Length; i++)
            {
                if (objectSpawnRandom[i]._weight >= rand)
                {
                    return i;
                }
            }
        }

        return 0;
    }

    private void CalculateWeight()
    {
        if (lengthPrefab >= 2)
        {
            accumulatedWeight = 0f;
            foreach (var enemy in objectSpawnRandom)
            {
                accumulatedWeight += enemy.chance;
                enemy._weight = accumulatedWeight;
            }
        }
    }

    private void GenerateObject(int enemyExtra)
    {
        for (int i = 0; i < enemyExtra; i++)
        {
            GameObject go = Instantiate(objectSpawnRandom[GetRandomEnemy()].prefabEnemy);
            go.transform.parent = transform;
            go.SetActive(false);
            listObjectInPool.Add(go);
        }
    }
}
