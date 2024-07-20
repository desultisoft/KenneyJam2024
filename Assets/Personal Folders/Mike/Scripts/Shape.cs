using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PentagramShape", menuName = "ScriptableObjects/Shape", order = 1)]
public class Shape : ScriptableObject
{
    public List<Vector2Int> totalRequiredConnections = new List<Vector2Int>();
}
