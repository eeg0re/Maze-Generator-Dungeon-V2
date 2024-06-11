using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefab;
    int randomSpawnPoint; 
    int randomEnemy;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SpawnEnemy(){
        randomSpawnPoint = Random.Range(0, spawnPoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
