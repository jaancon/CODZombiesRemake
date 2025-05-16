using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject Zombie1;
    public GameObject Zombie2;
    public int maxZombiesOnScreen;
    public int totalZombies;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int ZombiesPerSpawn;
    
    public float generatedSpawnTime = 0;
    public float currentSpawnTime = 0;
     
    [Serializable] public class SpawnPointParent
    {
        public float spawnTime = 0;
        public float timeBetweenSpawns;
        public int zombiesPerSpawn;
        public ZombieSpawner[] spawnerScripts;
    }

    public SpawnPointParent[] spawnPointParents;

    public void Update()
    {
        foreach (var spawnPointParent in spawnPointParents)
        {
            if (spawnPointParent.spawnTime < Time.time - spawnPointParent.timeBetweenSpawns)
            {
                spawnPointParent.spawnerScripts[UnityEngine.Random.Range(0, spawnPointParent.spawnerScripts.Length - 1)]
                    .SpawnZombies(spawnPointParent.zombiesPerSpawn);
                spawnPointParent.spawnTime = Time.time;
            }
        }
    }

    public void NewAreaOpened(string whichSpawnerToEnable)
    {
        GameObject zone = GameObject.Find(whichSpawnerToEnable);
        ZombieSpawner[] childrenSpawners = zone.GetComponentsInChildren<ZombieSpawner>();

        foreach (var child in childrenSpawners)
        {
            child.isSpawningZombies = true;
        }
    }

    public void EndGame()
    {
        
    }
}
