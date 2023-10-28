using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BakingRunTime : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

    public float _timeToBake;

    private float bakeTimer;
    // Start is called before the first frame update
    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        bakeTimer += Time.deltaTime;
        if (bakeTimer > _timeToBake)
        {
            surface.BuildNavMesh();
            bakeTimer = 0;
        }
    }
}
