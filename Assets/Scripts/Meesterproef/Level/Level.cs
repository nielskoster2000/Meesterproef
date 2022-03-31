using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

[System.Serializable]

public class Level : MonoBehaviour
{ 
    public string levelName;
    public Sprite thumbnail;
    [HideInInspector] public GameObject Map;
    public List<NavPoint> spawnPoints = new List<NavPoint>();
    public Material sky;
    public NavMeshData navMeshData;
    public NavMeshDataInstance navMeshDataInstance;

    private void Awake()
    {
        Map = gameObject;

        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
    }

    private void OnDestroy()
    {
        NavMesh.RemoveNavMeshData(navMeshDataInstance);
    }

    public NavPoint GetRandomSpawnPoint() //This does not check for taken spawnpoints!
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    }
}
