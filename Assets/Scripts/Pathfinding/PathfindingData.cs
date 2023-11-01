using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Scriptable Objects/PathfindingData")]
public class PathfindingData : ScriptableObject
{
    public Graph<Vector3Int> pathData;
}