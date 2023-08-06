using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityDictionary<,>))]
public class UnityDictionaryDrawerUIE : PropertyDrawer
{
    private float errorBoxHeight = EditorGUIUtility.singleLineHeight * 2;

    private const float LIST_ERROR_SPACING = 5;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            // First, load the unityDictionary. This enables us to check for
            // errors.
            dynamic unityDict = property.GetObjectFromReflection();
            UnityDictionaryErrorCode ec = unityDict.ValidateKVPs();
            // First drop into the first child, which will be the keyValuePairs.
            property.Next(true);

            // Then, draw only the keyValuePairs.
            EditorGUI.PropertyField(position, property, label, true);

            if (ec != UnityDictionaryErrorCode.None)
            {
                position.y += EditorGUI.GetPropertyHeight(property, label)
                    + LIST_ERROR_SPACING;
            }

            if (ec.HasFlag(UnityDictionaryErrorCode.DuplicateKeys))
            {
                EditorGUI.HelpBox(
                    new(position.x, position.y, position.width, errorBoxHeight),
                    "Duplicate keys detected in the Unity Dictionary. " +
                    "Duplicate keys are not allowed in dictionaries.",
                    MessageType.Error
                );
                position.y += errorBoxHeight;
            }

            if (ec.HasFlag(UnityDictionaryErrorCode.NullKeys))
            {
                EditorGUI.HelpBox(
                    new(position.x, position.y, position.width, errorBoxHeight),
                    "Null keys detected in the Unity Dictionary. " +
                    "Null keys are not allowed in dictionaries.",
                    MessageType.Error
                );
                position.y += errorBoxHeight;
            }
        }
        catch (System.ArgumentNullException)
        {
            base.OnGUI(position, property, label);
            return;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // First, load the unityDictionary. This enables us to check for
        // errors.
        dynamic unityDict = property.GetObjectFromReflection();
        UnityDictionaryErrorCode ec = unityDict.ValidateKVPs();

        float heightAddition = 0;

        if (ec != UnityDictionaryErrorCode.None)
        {
            heightAddition += LIST_ERROR_SPACING * 2;
        }

        if (ec == UnityDictionaryErrorCode.DuplicateKeys)
        {
            heightAddition += errorBoxHeight;
        }

        if (ec == UnityDictionaryErrorCode.NullKeys)
        {
            heightAddition += errorBoxHeight;
        }

        // Then drop into the first child, which will be the keyValuePairs.
        property.Next(true);

        // Then get height.
        return EditorGUI.GetPropertyHeight(property, label) + heightAddition;
    }
}