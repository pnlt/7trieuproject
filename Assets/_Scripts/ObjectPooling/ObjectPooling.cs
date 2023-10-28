using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int count;

    private List<GameObject> freeList;
    private List<GameObject> usedList;

    private void Awake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        for (int i = 0; i < count; i++)
            GenerateObject();
    }

    private GameObject GetObject()
    {
        int totalObject = freeList.Count;
        if (totalObject == 0) GenerateObject();

        GameObject objectChildren = null;

        GameObject obj = freeList[totalObject - 1];
        freeList.RemoveAt(totalObject - 1);
        usedList.Add(obj);
        return objectChildren;
    }   

    private void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        freeList.Add(obj);
        usedList.Remove(obj);
    }    
    
    private void GenerateObject()
    {
        GameObject go = Instantiate(prefabObject);
        go.transform.parent = transform.parent;
        freeList.Add(go);
       
    }    
}
