using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private bool expendable;
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
            GenerateObject();
        }
    }

    public GameObject TakeObject()
    {
        int totalCount = objectInPool.Count;
        if (totalCount == 0 && !expendable) return null;
        else if (totalCount == 0) GenerateObject();

        GameObject obj = objectInPool[totalCount - 1];
        objectInPool.RemoveAt(totalCount - 1);
        usedList.Add(obj);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        objectInPool.Remove(obj);
        usedList.Add(obj);
        obj.SetActive(false);
    }

    private void GenerateObject()
    {
        GameObject go = Instantiate(objectPool);
        go.transform.parent = transform;
        go.SetActive(false);
        objectInPool.Add(go);
    }
}
