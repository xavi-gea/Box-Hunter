using UnityEngine;

/// <summary>
/// Spawn or move the player when this loads
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    public SpawnLocation defaultSpawnLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerManager.Instance.SpawnPlayer(defaultSpawnLocation);
    }
}
