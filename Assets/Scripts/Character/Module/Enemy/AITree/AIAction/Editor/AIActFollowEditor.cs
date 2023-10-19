using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for <see cref="AIActFollow"/>
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
[CustomEditor(typeof(AIActFollow))]
public class AIActFollowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Box(
            "Tells the AI to follow its current target.",
            GUIStyleExt.LeftAlignBox
        );

        base.OnInspectorGUI();
    }
}