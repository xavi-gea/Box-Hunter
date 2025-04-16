using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Affinity", menuName = "Scriptable Objects/Affinity")]
public class Affinity : ScriptableObject
{
    public string title;
    public List<Affinity> strongAgainst;
    public List<Affinity> weakAgainst;
}
