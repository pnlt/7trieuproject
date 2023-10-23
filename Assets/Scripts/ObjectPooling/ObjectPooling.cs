using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private bool expendable;
    [SerializeField] private int count;
    [SerializeField] private GameObject objectPool;

    private List<GameObject> objectInPool;
    private List<GameObject> usedList;

    private void Awake()
    {
        objectInPool = new List<GameObject>();
        usedList = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GenerateObject(1);
        }
    }

    public GameObject TakeObject(int enemyLack)
    {
        int totalCount = objectInPool.Count;
        if (totalCount == 0 && !expendable) return null;
        else if (totalCount == 0)
        {
            GenerateObject(enemyLack);
            totalCount = objectInPool.Count;
        }

        GameObject obj = objectInPool[totalCount - 1];
        objectInPool.RemoveAt(totalCount - 1);
        usedList.Add(obj);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        usedList.Remove(obj);
        objectInPool.Add(obj);
        obj.SetActive(false);
    }

    private void GenerateObject(int enemyExtra)
    {
        for (int i = 0; i < enemyExtra; i++)
        {
            GameObject go = Instantiate(objectPool);
            go.transform.parent = transform;
            go.SetActive(false);
            objectInPool.Add(go);
        }
    }
}
