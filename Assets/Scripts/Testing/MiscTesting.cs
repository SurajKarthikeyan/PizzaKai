using System.Linq;
using NaughtyAttributes;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

public class MiscTesting : MonoBehaviour
{
    public PathNode.NodeFlags enumValue;

    public PathNode.NodeFlags enumFlags;

    [Button]
    private void TestFlags()
    {
        print(enumValue.AllFlagsSet(enumFlags));
        print(enumValue.SomeFlagsSet(enumFlags));
    }
}