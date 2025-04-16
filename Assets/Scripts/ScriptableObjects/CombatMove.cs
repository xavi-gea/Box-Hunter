using UnityEngine;

public enum CombatMoveType
{
    Offensive,
    Defensive
}

[CreateAssetMenu(fileName = "CombatMove", menuName = "Scriptable Objects/CombatMove")]
public class CombatMove : ScriptableObject
{
    public string title;
    public string description;
    public Affinity affinity;

    public CombatMoveType moveType;
    public int amount;
}
