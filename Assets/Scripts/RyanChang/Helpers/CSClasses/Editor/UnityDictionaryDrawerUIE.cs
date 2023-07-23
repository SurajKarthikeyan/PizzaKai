using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityDictionary<,>))]
public class UnityDictionaryDrawerUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            // First drop into the first child, which will be the keyValuePairs.
            property.Next(true);

            // Then, draw only the keyValuePairs.
            EditorGUI.PropertyField(position, property, label, true);
        }
        catch (System.ArgumentNullException)
        {
            base.OnGUI(position, property, label);
            return;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // First drop into the first child, which will be the keyValuePairs.
        property.Next(true);

        // Then get height.
        return EditorGUI.GetPropertyHeight(property, label);
    }
}