using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            GenerateObject();
        }
    }

    public GameObject TakeObject()
    {
        int totalCount = objectInPool.Count;
        if (totalCount == 0 && !expendable) return null;
        else if (totalCount == 0) 
            GenerateObject();

        
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

    private void GenerateObject()
    {
        GameObject go = Instantiate(objectPool);
        go.transform.parent = transform;
        go.SetActive(false);
        objectInPool.Add(go);
    }
}


/* For ObjectPooling used for list gameobject
     * public class ObjectPooling : MonoBehaviour
        {
            public bool expendable;
            public GameObject[] objectPool;
            public int[] poolSize;

            private List<GameObject> poolList;
            private List<GameObject> usedList;

            private void Awake()
            {
                poolList = new List<GameObject>();
                usedList = new List<GameObject>();
        
                for (int i = 0; i < poolSize.Length; i++)
                {
                    GenerateNewObject(i);
                }
            }

            public GameObject TakeObject(int objectReference)
            {
        
                int totalFree = poolList.Count;
                if (totalFree == 0 && !expendable)
                    return null;
                else if (totalFree == 0) GenerateNewObject(objectReference);

                GameObject newObject = ObjectTaken(objectReference);
                if (newObject == null) return null;
                else
                {
                    GameObject obj = newObject;
                    poolList.RemoveAt(totalFree - 1);
                    usedList.Add(obj);
                    return obj;
                }
          
            
            }

            private GameObject ObjectTaken(int index)
            {
                GameObject objTook = null;
                if (index == 0)
                {
                    for (int i = 0; i < poolSize[index]; i++)
                    {
                        objTook = poolList[i];
                    }
                    poolSize[index] -= 1;
                }  
                else
                {
                    int index1 = poolSize[index - 1];
                    int index2 = index1 + poolSize[index];
                    for (int i = index1; i < index2; i++)
                    {
                        objTook = poolList[i];
                    }
                    poolSize[index] -= 1;
                }    

                return objTook;
            } 

            public void ReturnObject(GameObject objReturn)
            {
                objReturn.SetActive(false);
                usedList.Remove(objReturn);
                poolList.Add(objReturn);
                poolSize[poolSize.Length - 1] += 1;
            }
        
            private void GenerateNewObject(int objectIndex)
            {
                for (int i = 0; i < poolSize[objectIndex]; i++)
                {
                    GameObject go = Instantiate(objectPool[objectIndex]);
                    go.transform.parent = transform;
                    go.SetActive(false);
                    poolList.Add(go);
                }    
            }

    
        
        }
 * 
 */
