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

    /// <summary>
    /// Get the spawn location, and if the player gameObject exists, move it to that spawn location
    /// If not, instantiate a prefab of the player in the spawn location
    /// </summary>
    /// <param name="defaultSpawnLocation"></param>
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

    /// <summary>
    /// Using the <see cref="spawnLocation"/>, move the player to it
    /// In this case, the player gameObject should already exist
    /// </summary>
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
