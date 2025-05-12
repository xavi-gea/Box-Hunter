using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manage the started combat encounter, eventually ending it
/// </summary>
public class CombatManager : MonoBehaviour
{
    // must be inside combat scene!

    private CombatScreenManager screenManager;

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

    private bool isPlayerTurn = false;
    private bool combatEnded = false;
    private bool isPlayerFleeing = false;

    /// <summary>
    /// Set's up the combat field and shows a <see cref="Dialogue"/> with the enemy's name
    /// </summary>
    void Start()
    {
        screenManager = GetComponent<CombatScreenManager>();

        // Initially Inactive
        mainScreen.SetActive(false);
        movesScreen.SetActive(false);
        moveDetailsScreen.SetActive(false);

        SetCombatData();
        // animations

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(CombatEncounterManager.Instance.combatSceneName));

        DialogueEvents.dialogueDone.AddListener(DialogueDone);

        ShowDialogue("Te has topado con un " + enemyCombatData.Name + "!");
    }

    /// <summary>
    /// Instantiates a <see cref="Dialogue"/> with the provided text
    /// </summary>
    /// <param name="dialogueText"></param>
    private void ShowDialogue(string dialogueText)
    {
        currentDialogue = ScriptableObject.CreateInstance<Dialogue>();
        currentDialogue.dialogueLines = new string[] { dialogueText };
        DialogueManager.Instance.StartDialogue(currentDialogue);
    }

    /// <summary>
    /// Set the combat data that the variables <see cref="playerCombatData"/> and <see cref="enemyCombatData"/> will use
    /// </summary>
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

        // player combat moves

        foreach (CombatMove move in playerCombatData.CombatMoves)
        {
            GameObject moveButton = Instantiate(moveButtonPrefab, movesScreen.transform);

            moveButton.GetComponent<CombatMoveContainer>().combatMove = move;
            moveButton.GetComponentInChildren<TextMeshProUGUI>().text = move.title;

            moveButton.GetComponent<Button>().onClick.AddListener(() => OpenMoveDetails(move));
        }
    }

    /// <summary>
    /// Updates the health of the player and enemy from the values contained in their <see cref="CreatureCombatData"/>
    /// </summary>
    private void UpdateHealth()
    {
        playerHealth.value = playerCombatData.Health;
        enemyHealth.value = enemyCombatData.Health;
    }

    /// <summary>
    /// Opens a screen with the details of the provided <see cref="CombatMove"/>
    /// It also sets up a listener that triggers when the <paramref name="move"/> is confirmed by the player
    /// </summary>
    /// <param name="move"></param>
    private void OpenMoveDetails(CombatMove move)
    {
        moveDetailsDescription.GetComponent<TextMeshProUGUI>().text = move.description;
        moveDetailsAffinity.GetComponent<TextMeshProUGUI>().text = move.affinity.title;
        moveDetailsAmount.GetComponent<TextMeshProUGUI>().text = (move.amount.ToString() + " PV");

        moveDetailsAttackButton.onClick.RemoveAllListeners();
        moveDetailsAttackButton.onClick.AddListener(() => StartMove(move));

        screenManager.OpenPanel(moveDetailsScreen);
    }

    /// <summary>
    /// Executes the provided move, taking into account the source and target <see cref="CreatureCombatData"/>
    /// After it executes, update their health
    /// </summary>
    /// <param name="move"></param>
    private void StartMove(CombatMove move)
    {
        screenManager.CloseCurrent();

        CreatureCombatData targetCombatData = isPlayerTurn ? enemyCombatData : playerCombatData;
        CreatureCombatData sourceCombatData = isPlayerTurn ? playerCombatData : enemyCombatData;

        targetCombatData.DecreaseHealth(targetCombatData.Affinity, move);

        UpdateHealth();

        ShowResultDialogue(sourceCombatData, targetCombatData, move);
    }

    /// <summary>
    /// Shows a <see cref="Dialogue"/> with contextual data that depends on the <see cref="CombatMove"/> used and it's <see cref="Effectiveness"/> against a target
    /// </summary>
    /// <param name="sourceCombatData"></param>
    /// <param name="targetCombatData"></param>
    /// <param name="move"></param>
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

    /// <summary>
    /// Handles what happens when a dialogue is done
    /// If the player is fleeing or the combat has ended, <see cref="CombatEncounterManager.Instance.EndCombatEncounter()"/>
    /// If the health of both the player and enemy is above 0, go to the <see cref="NextCombatTurn"/>
    /// Else, <see cref="ShowCombatResult"/>
    /// </summary>
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

    /// <summary>
    /// Handles the action of fleeing from combat
    /// Closes the current screen and create a <see cref="Dialogue"/> with the escape message
    /// </summary>
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

        CombatEncounterManager.Instance.playerLost = loserCombatData.Equals(playerCombatData);

        ShowDialogue($"{loserCombatData.Name} ha sido derrotado!");
    }

    /// <summary>
    /// Handles the next turn taking into account if it is for the player
    /// If not, execute a <see cref="CombatMove"/> pertaining to the enemy
    /// </summary>
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
        float currentDamageAmount = 0;
        CombatMove combatMove = null;

        foreach (CombatMove move in enemyCombatData.CombatMoves)
        {
            if (move.amount > currentDamageAmount)
            {
                currentDamageAmount = move.amount;
                combatMove = move;
            }
        }

        return combatMove;
    }
}
