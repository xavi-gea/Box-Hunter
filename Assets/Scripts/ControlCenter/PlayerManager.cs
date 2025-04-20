using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject playerPrefab;
    public GameObject playerGameObject;

    public SpawnLocation spawnLocation = null;
    public SpawnLocation saveSpawnLocation = null;

    public Creature CurrentCreature;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnPlayer(SpawnLocation defaultSpawnLocation)
    {
        if (Instance == this)
        {
            if (saveSpawnLocation == null)
            {
                spawnLocation = (spawnLocation == null) ? defaultSpawnLocation : spawnLocation;
            }
            else
            {
                spawnLocation = saveSpawnLocation;
                saveSpawnLocation = null;
            }

            playerGameObject = GameObject.FindGameObjectWithTag("Player");

            if (playerGameObject == null)
            {
                playerGameObject = Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
            }
            else
            {
                playerGameObject.transform.position = spawnLocation.position;
            }
        }
        else
        {
            Instance.SpawnPlayer(defaultSpawnLocation);
        }
    }
    public void MovePlayerToSpawn()
    {
        if (Instance == this)
        {
            SpawnPlayer(spawnLocation);
        }
        else
        {
            Instance.MovePlayerToSpawn();
        }
    }
}
