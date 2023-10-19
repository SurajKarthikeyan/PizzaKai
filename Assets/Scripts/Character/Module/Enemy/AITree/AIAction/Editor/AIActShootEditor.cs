using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for <see cref="AIActShoot"/>.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
[CustomEditor(typeof(AIActShoot))]
public class AIActShootEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Box(
            "Shoots at the AI's current target.",
            GUIStyleExt.LeftAlignBox
        );

        base.OnInspectorGUI();
    }
}