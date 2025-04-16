using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject playerPrefab;

    public SpawnLocation spawnLocation = null;
    // or SpawnLocation

    public Creature CurrentCreature;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnPlayer(SpawnLocation defaultSpawnLocation)
    {
        if (Instance == this)
        {
            spawnLocation = (spawnLocation == null) ? defaultSpawnLocation : spawnLocation;

            GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");

            if (playerGameObject == null)
            {
                Instantiate(playerPrefab, spawnLocation.position, Quaternion.identity);
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
}
