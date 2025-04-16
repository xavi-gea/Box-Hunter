using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "Scriptable Objects/Creature")]
public class Creature : ScriptableObject
{
    // title might be meaningless if I can use the name
    public string title;
    public Affinity affinity;
    public int hitPoints;
    public List<CombatMove> combatMoves;
}
