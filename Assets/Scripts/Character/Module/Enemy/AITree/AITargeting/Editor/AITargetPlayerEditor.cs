using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for <see cref="AITargetPlayer"/>.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
[CustomEditor(typeof(AITargetPlayer))]
public class AITargetPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Box(
            "Automatically targets the player.",
            GUIStyleExt.LeftAlignBox
        );

        base.OnInspectorGUI();
    }
}