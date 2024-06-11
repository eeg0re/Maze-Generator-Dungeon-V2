using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    int randomSpawnPoint; 
    int randomEnemy;
    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, 5f);
        
    }

    void SpawnEnemy(){
        // get a random enemy spawn point
        randomSpawnPoint = Random.Range(0, spawnPoints.Length);

        // get random enemy prefab
        randomEnemy = Random.Range(0, enemyPrefabs.Length);

        // spawn in the enemy
        Instantiate(enemyPrefabs[randomEnemy], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
