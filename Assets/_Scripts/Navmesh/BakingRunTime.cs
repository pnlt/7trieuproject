using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BakingRunTime : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

    public float _timeToBake;

    private float bakeTimer;
    private GameManager gameManager;
    private NavMeshData _data;
    // Start is called before the first frame update
    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        gameManager = GameManager._instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameStart())
        {
            bakeTimer += Time.deltaTime;
            if (bakeTimer > _timeToBake)
            {
                surface.BuildNavMesh();
                bakeTimer = 0;
            }
        }
    }
}
