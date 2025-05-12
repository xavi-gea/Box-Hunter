using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreaturePool
{
    public Creature creature;

    [Tooltip("Chance at which this creature will appear")]
    public float chance;
}

/// <summary>
/// Set up and manage a combat zone
/// </summary>
public class CombatZone : MonoBehaviour
{
    [Tooltip("Combat chance at which the combat will start")]
    public float baseCombatChance = 0;

    [Tooltip("How much the combat chance will increase each time the player moves")]
    public float chanceIncrease;

    [Tooltip("List of creatures that will start combat in this zone")]
    public List<CreaturePool> creaturePool = new();

    /// <summary>
    /// When the player enters the collider, comunicate it to <see cref="CombatEncounterManager"/> and set up relevant data for the future encounter
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // if player detected, comunicate to CombatManager that player is in combat zone
            CombatEncounterManager.Instance.isInCombatZone = true;
            CombatEncounterManager.Instance.currentCombatChance = baseCombatChance;
            CombatEncounterManager.Instance.combatChanceIncrease = chanceIncrease;
            CombatEncounterManager.Instance.creaturePool = creaturePool;
        }
    }

    /// <summary>
    /// When the player exits the collider, comunicate it to <see cref="CombatEncounterManager"/>
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CombatEncounterManager.Instance.isInCombatZone = false;
        }
    }

    /// <summary>
    /// Calculate the total chance for each creature in <see cref="creaturePool"/>. Return an error if the total is not equal to 100
    /// </summary>
    void Start()
    {
        if (creaturePool.Count > 0)
        {
            float totalChance = 0f;

            foreach (var item in creaturePool)
            {
                totalChance += item.chance;
            }

            if (totalChance != 100f)
            {
                Debug.LogError(gameObject.name + " at " + transform.position + ": The total chance in the creature pool must be equal to 100 (currently " + totalChance + ")");
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " at "+ transform.position + ": Creature pool is empty");
        }
    }

    
}
