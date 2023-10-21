using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;
    private bool expendable;
    [SerializeField] private int count;
    [SerializeField] private GameObject objectPool;

    public List<GameObject> objectInPool;
    public List<GameObject> usedList;

    private void Awake()
    {
        Instance = this;
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
        objectInPool.Add(obj);
        usedList.Remove(obj);
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
