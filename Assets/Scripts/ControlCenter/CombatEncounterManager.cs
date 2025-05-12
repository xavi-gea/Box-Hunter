using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using SystemRandom = System.Random;

/// <summary>
/// Manage the potential combat encounter, starting combat if necessary
/// </summary>
public class CombatEncounterManager : MonoBehaviour
{
    public static CombatEncounterManager Instance { get; private set; }

    [Tooltip("Name of the combat scene. Must be in the build index. Case insensitive")]
    public string combatSceneName;
    private Scene mainScene;

    public float currentCombatChance;

    public float combatChanceIncrease;

    public bool isInCombatZone;
    public bool isInCombat;
    public bool playerLost = false;

    public List<CreaturePool> creaturePool;

    public Creature CreatureToFight { get; set; }

    private void Awake()
    {
        Instance = this;

        // remember that this will be added to the Control center because
        // it can be called from outside of the additive combat scene
        //canvas = GameObject.Find("Canvas");
    }

    /// <summary>
    /// Every time this is called, increase de current combat chance by <see cref="combatChanceIncrease"/>
    /// If the roll succedes, suffle the <see cref="creaturePool"/>, get a random creature taking into account it's chance and start the combat
    /// </summary>
    public void IncreaseCombatChance()
    {
        currentCombatChance += combatChanceIncrease;

        if (currentCombatChance >= Random.Range(0, 100))
        {
            SuffleCreaturePool();

            float generatedPoolChance = Random.Range(0, 100);

            foreach (CreaturePool pool in creaturePool)
            {
                if (pool.chance >= generatedPoolChance)
                {
                    CreatureToFight = pool.creature;
                    break;
                }
                else
                {
                    generatedPoolChance -= pool.chance;
                }
            }

            StartCombat();
        }
    }

    /// <summary>
    /// Suffle the <see cref="creaturePool"/> using the fisher–yates shuffle, 
    /// </summary>
    private void SuffleCreaturePool()
    {
        SystemRandom randomCreatureIndex = new();
        int numberOfCreatures = creaturePool.Count;

        // fisher–yates shuffle

        while (numberOfCreatures > 1)
        {
            numberOfCreatures--;

            int newCreatureIndex = randomCreatureIndex.Next(numberOfCreatures + 1);

            (creaturePool[numberOfCreatures], creaturePool[newCreatureIndex]) = (creaturePool[newCreatureIndex], creaturePool[numberOfCreatures]);
        }
    }

    /// <summary>
    /// Instance the combat scene and start the combat with the set <see cref="CreatureToFight"/>
    /// </summary>
    public void StartCombat()
    {
        isInCombat = true;

        InputManager.Instance.SetInputMap(ActionMap.UI);

        // transition here or inside SceneController

        mainScene = SceneManager.GetActiveScene();

        SceneManager.LoadSceneAsync(combatSceneName,LoadSceneMode.Additive);

        Debug.Log("Combat started with: " + CreatureToFight.title);
    }

    /// <summary>
    /// Set the main scene as the active one and unload the currently opened combat scene
    /// If the player lost, move it to the spawn point
    /// </summary>
    public void EndCombatEncounter()
    {
        currentCombatChance = 0;

        // visual transition here or inside SceneController

        SceneManager.SetActiveScene(mainScene);

        SceneManager.UnloadSceneAsync(combatSceneName);

        InputManager.Instance.SetInputMap(ActionMap.Player);

        if (playerLost)
        {
            PlayerManager.Instance.MovePlayerToSpawn();
        }

        isInCombat = false;
    }
}
