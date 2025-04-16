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

public class CombatZone : MonoBehaviour
{
    [Tooltip("Combat chance at which the combat will start")]
    public float baseCombatChance = 0;

    [Tooltip("How much the combat chance will increase each time the player moves")]
    public float chanceIncrease;

    [Tooltip("List of creatures that will start combat in this zone")]
    public List<CreaturePool> creaturePool = new();

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // if player no longer detected, comunicate to CombatManager that player is NOT in combat zone
            CombatEncounterManager.Instance.isInCombatZone = false;
        }
    }

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
