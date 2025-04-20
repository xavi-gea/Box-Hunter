using System.Collections.Generic;
using UnityEngine;

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

        }
        else if (GetMoveEffectiveness(targetAffinity, combatMove).Equals(Effectiveness.Weak))
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
