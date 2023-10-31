using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TargetToken))]
public class TargetTokenDrawerUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            property.GetObjectFromReflection(out TargetToken token);

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Headers.
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position, label);
            position.Translate(0, position.height);

            // Everything is readonly here.
            EditorGUI.BeginDisabledGroup(true);

            // Display the target tile/transform.
            EditorGUI.indentLevel++;
            EditorGUI.LabelField(position, "Target", $"{token.Position}");
            position.Translate(0, position.height);
            if (PathfindingManager.Instance)
            {
                EditorGUI.LabelField(position, "Grid Target", $"{token.GridPosition}");
                position.Translate(0, position.height);
            }

            // Use reflection to get value of the tracking object.
            if (token.TryGetFieldValue(
                "dynamicTarget",
                BindingFlags.NonPublic | BindingFlags.Instance,
                out Transform dynamicTarget
            ) && dynamicTarget)
            {
                position.Translate(0, position.height);

                EditorGUI.LabelField(
                    position,
                    "Dynamic Target",
                    $"{dynamicTarget}"
                );
            }
            if (token.TryGetFieldValue(
                "tileTarget",
                BindingFlags.NonPublic | BindingFlags.Instance,
                out Vector3 tileTarget))
            {
                position.Translate(0, position.height);

                EditorGUI.LabelField(
                    position,
                    "Tile Target",
                    $"{tileTarget}"
                );
            }

            EditorGUI.indentLevel--;

            EditorGUI.EndDisabledGroup();
            EditorGUI.EndProperty();
        }
        catch (System.ArgumentNullException)
        {
            base.OnGUI(position, property, label);
            return;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 4;
    }
}