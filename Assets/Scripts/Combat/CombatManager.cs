using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Effectiveness
{
    Strong,
    Weak,
    Normal
}

/// <summary>
/// Used to generate combat data for the <see cref="Creature"/> provided
/// </summary>
public class CreatureCombatData
{
    public string Name { get; }
    public Affinity Affinity { get; }
    public float Health { get; private set; }
    public float MaxHealth { get; }
    public List<CombatMove> CombatMoves { get; }

    public CreatureCombatData(Creature creature)
    {
        Name = creature.title;
        Affinity = creature.affinity;
        Health = creature.hitPoints;
        MaxHealth = creature.hitPoints;
        CombatMoves = new List<CombatMove>(creature.combatMoves);
    }

    public void DecreaseHealth(Affinity targetAffinity, CombatMove combatMove) 
    {
        float amountToDecrease = combatMove.amount;

        if (GetMoveEffectiveness(targetAffinity, combatMove).Equals(Effectiveness.Strong))
        {
            amountToDecrease *= 1.5f;

        }else if (GetMoveEffectiveness(targetAffinity, combatMove).Equals(Effectiveness.Weak))
        {
            amountToDecrease *= 0.5f;
        }

        Health = Mathf.Clamp(Health - amountToDecrease, 0, Health);
    }

    public static Effectiveness GetMoveEffectiveness(Affinity targetAffinity, CombatMove combatMove)
    {
        if (targetAffinity.weakAgainst.Contains(combatMove.affinity))
        {
            return Effectiveness.Strong;
        }
        else if (targetAffinity.strongAgainst.Contains(combatMove.affinity))
        {
            return Effectiveness.Weak;
        }
        else
        {
            return Effectiveness.Normal;
        }
    }
}

public class CombatManager : MonoBehaviour
{
    // must be inside combat scene!
    //[SerializeField]
    //private Button attackButton;
    //[SerializeField]
    //private Button fleeButton;
    //[SerializeField]
    //private Button returnButton;

    CombatScreenManager screenManager;

    [Header("Field Screen")]
    [SerializeField]
    private GameObject playerNameObject;
    [SerializeField]
    private GameObject playerSliderObject;
    private Slider playerHealth;

    [SerializeField]
    private GameObject enemyNameObject;
    [SerializeField]
    private GameObject enemySliderObject;
    private Slider enemyHealth;

    private CreatureCombatData playerCombatData;
    private CreatureCombatData enemyCombatData;

    [SerializeField]
    private GameObject buttonScreen;
    [Header("Main Screen")]
    [SerializeField]
    private GameObject mainScreen;
    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private Button fleeButton;

    [Header("Moves Screen")]
    [SerializeField]
    private GameObject movesScreen;
    [SerializeField]
    private GameObject moveButtonPrefab;
    [Header("Move Details Screen")]
    [SerializeField]
    private GameObject moveDetailsScreen;
    [SerializeField]
    private Button moveDetailsAttackButton;
    [SerializeField]
    private GameObject moveDetailsDescription;
    [SerializeField]
    private GameObject moveDetailsAffinity;
    [SerializeField]
    private GameObject moveDetailsAmount;

    private Dialogue currentDialogue;

    //private Creature playerCreature;
    //private List<CombatMove> playerCombatMoves;

    //private Creature opponentCreature;

    private bool isPlayerTurn = false;
    private bool combatEnded = false;
    private bool isPlayerFleeing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenManager = GetComponent<CombatScreenManager>();

        // Initially Inactive
        mainScreen.SetActive(false);
        movesScreen.SetActive(false);
        moveDetailsScreen.SetActive(false);

        SetCombatData();
        // animations

        //NextCombatTurn(); // if player turn, show combat UI?

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(CombatEncounterManager.Instance.combatSceneName));

        DialogueEvents.dialogueDone.AddListener(DialogueDone);

        ShowDialogue("Te has topado con un " + enemyCombatData.Name + "!");
    }

    private void ShowDialogue(string dialogueText)
    {
        currentDialogue = ScriptableObject.CreateInstance<Dialogue>();
        currentDialogue.dialogueLines = new string[] { dialogueText };
        DialogueManager.Instance.StartDialogue(currentDialogue);
    }

    private void SetCombatData()
    {
        // get player creature
        playerCombatData = new CreatureCombatData(PlayerManager.Instance.CurrentCreature);

        // get opponent creature
        enemyCombatData = new CreatureCombatData(CombatEncounterManager.Instance.CreatureToFight);

        // place relevant data in gameobjects

        playerNameObject.GetComponent<TextMeshProUGUI>().text = playerCombatData.Name;
        playerHealth = playerSliderObject.GetComponent<Slider>();
        playerHealth.maxValue = playerCombatData.MaxHealth;

        enemyNameObject.GetComponent<TextMeshProUGUI>().text = enemyCombatData.Name;
        enemyHealth = enemySliderObject.GetComponent<Slider>();
        enemyHealth.maxValue = enemyCombatData.MaxHealth;

        UpdateHealth();

        // combat moves

        foreach (CombatMove move in playerCombatData.CombatMoves)
        {
            GameObject moveButton = Instantiate(moveButtonPrefab, movesScreen.transform);

            moveButton.GetComponent<CombatMoveContainer>().combatMove = move;
            moveButton.GetComponentInChildren<TextMeshProUGUI>().text = move.title;

            moveButton.GetComponent<Button>().onClick.AddListener(() => OpenMoveDetails(move));
        }
    }

    private void UpdateHealth()
    {
        playerHealth.value = playerCombatData.Health;
        enemyHealth.value = enemyCombatData.Health;
    }

    private void OpenMoveDetails(CombatMove move)
    {
        moveDetailsDescription.GetComponent<TextMeshProUGUI>().text = move.description;
        moveDetailsAffinity.GetComponent<TextMeshProUGUI>().text = move.affinity.title;
        moveDetailsAmount.GetComponent<TextMeshProUGUI>().text = (move.amount.ToString() + " PV");

        moveDetailsAttackButton.onClick.AddListener(() => StartMove(move));

        screenManager.OpenPanel(moveDetailsScreen);
    }

    private void StartMove(CombatMove move)
    {
        moveDetailsAttackButton.onClick.RemoveAllListeners();
        screenManager.CloseCurrent();

        CreatureCombatData targetCombatData = isPlayerTurn ? enemyCombatData : playerCombatData;
        CreatureCombatData sourceCombatData = isPlayerTurn ? playerCombatData : enemyCombatData;

        targetCombatData.DecreaseHealth(targetCombatData.Affinity, move);

        UpdateHealth();

        ShowResultDialogue(sourceCombatData, targetCombatData, move);
    }

    private void ShowResultDialogue(CreatureCombatData sourceCombatData, CreatureCombatData targetCombatData, CombatMove move)
    {
        string dialogueText = $"{sourceCombatData.Name} ha utilizado {move.title}.";

        if (CreatureCombatData.GetMoveEffectiveness(targetCombatData.Affinity, move).Equals(Effectiveness.Strong))
        {
            dialogueText += " Ha sido muy efectivo!";
        }
        else if (CreatureCombatData.GetMoveEffectiveness(targetCombatData.Affinity, move).Equals(Effectiveness.Weak))
        {
            dialogueText += " No ha sido muy efectivo...";
        }

        ShowDialogue(dialogueText);
    }

    private void DialogueDone()
    {
        if (isPlayerFleeing || combatEnded)
        {
            CombatEncounterManager.Instance.EndCombatEncounter();
        }
        else if (playerCombatData.Health > 0 && enemyCombatData.Health > 0)
        {
            NextCombatTurn();
        }
        else
        {
            ShowCombatResult();
        }
    }

    public void FleeCombat()
    {
        screenManager.CloseCurrent();
        isPlayerFleeing = true;
        ShowDialogue($"Has escapado del combate!");
    }

    private void ShowCombatResult()
    {
        combatEnded = true;

        CreatureCombatData loserCombatData = playerCombatData.Health <= 0 ? playerCombatData : enemyCombatData;

        ShowDialogue($"{loserCombatData.Name} ha sido derrotado!");
    }

    private void NextCombatTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
        {
            screenManager.OpenPanel(mainScreen);
        }
        else
        {
            StartMove(GetEnemyMove());
        }
    }

    /// <summary>
    /// Return move that the enemy will use. It will choose the strongest one
    /// </summary>
    /// <returns><see cref="CombatMove"/> that the enemy will use</returns>
    private CombatMove GetEnemyMove()
    {
        float damageAmount = 0;
        CombatMove combatMove = null;

        foreach (CombatMove move in enemyCombatData.CombatMoves)
        {
            if (move.amount > damageAmount)
            {
                damageAmount = move.amount;
                combatMove = move;
            }
        }

        return combatMove;

        // pity to avoid using the strongest move if it is going to beat the player?
        // random chance?
    }
}
