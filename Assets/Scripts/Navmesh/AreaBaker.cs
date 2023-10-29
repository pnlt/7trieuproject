using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AreaBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private float updateRate = 0.1f;
    [SerializeField] private float moveThreshold;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 navmeshSize = new Vector3(584, 20, 700);

    private Vector3 worldAnchor;
    private NavMeshData navmeshData;
    private List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

    private void Start()
    {
        navmeshData = new NavMeshData();
        NavMesh.AddNavMeshData(navmeshData);
        BuildNavMesh(false);
        StartCoroutine(CheckPlayerMovement());

    }

    private IEnumerator CheckPlayerMovement()
    {
        while (true)
        {
            if (Vector3.Distance(worldAnchor, player.position) > moveThreshold)
            {
                BuildNavMesh(true);
                worldAnchor = player.position;
            }
            yield return new WaitForSeconds(updateRate);
        }


    }

    private void BuildNavMesh(bool async)
    {
        Bounds navmeshBounds = new Bounds(player.position, navmeshSize);
        List<NavMeshBuildMarkup> navMeshBuildMarkups = new List<NavMeshBuildMarkup>();

        List<NavMeshModifier> modifiers;
        if (surface.collectObjects == CollectObjects.Children)
        {
            modifiers = new List<NavMeshModifier>(surface.GetComponentsInChildren<NavMeshModifier>());
        }
        else
        {
            modifiers = NavMeshModifier.activeModifiers;
        }

        for (int i = 0; i < modifiers.Count; i++)
        {
            if ((surface.layerMask & (1 << modifiers[i].gameObject.layer)) == 1
                && modifiers[i].AffectsAgentType(surface.agentTypeID))
            {
                navMeshBuildMarkups.Add(new NavMeshBuildMarkup()
                {
                    root = modifiers[i].transform,
                    overrideArea = modifiers[i].overrideArea,
                    area = modifiers[i].area,
                    ignoreFromBuild = modifiers[i].ignoreFromBuild
                });
            }
        }

        if (surface.collectObjects == CollectObjects.Children)
        {
            NavMeshBuilder.CollectSources(surface.transform, surface.layerMask, surface.useGeometry,
                                        surface.defaultArea, navMeshBuildMarkups, sources);
        }
        else
        {
            NavMeshBuilder.CollectSources(navmeshBounds, surface.layerMask, surface.useGeometry,
                        surface.defaultArea, navMeshBuildMarkups, sources);
        }

        if (async)
        {
            NavMeshBuilder.UpdateNavMeshDataAsync(navmeshData, surface.GetBuildSettings(), sources,
                                        new Bounds(player.transform.position, navmeshSize));
        }
        else
        {
            NavMeshBuilder.UpdateNavMeshData(navmeshData, surface.GetBuildSettings(), sources,
                                        new Bounds(player.transform.position, navmeshSize));
        }

    }

}