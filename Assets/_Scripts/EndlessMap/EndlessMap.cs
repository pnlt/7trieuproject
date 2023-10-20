using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EndlessMap : MonoBehaviour
{

    [SerializeField] private GameObject mapPrefab;
    private bool isDestroy;
    private bool isGenerated;
    public bool mark { get; private set; }

    private Vector3 modelLength;

    private void Start()
    {
        isDestroy = false;
        isGenerated = false;

        modelLength = GetComponent<Renderer>().bounds.size;
    }

    private void Update()
    {
        isDestroy = GetComponentInChildren<SpawnNotify>().destroyed;
        isGenerated = GetComponentInChildren<SpawnNotify>().generated;

        BuildMap();
        DestroyMap();
    }

    private void DestroyMap()
    {
        if (isDestroy)
            Destroy(gameObject, 18);
    }

    private void BuildMap()
    {
        if (isGenerated)
        {
            Instantiate(mapPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + modelLength.z), Quaternion.identity);
            mark = true;
        }
    }


}
