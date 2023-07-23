using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumDictionary<,>))]
public class EnumDictionaryDrawerUIE : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            // Fix the enum dictionary if needed. See
            // https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/interop/using-type-dynamic
            // for more information on the dynamic keyword.
            dynamic enumDictionary = property.GetObjectFromReflection();
            enumDictionary.FixEditorDict();

            // Check if the base property is expanded or not. If not, don't draw
            // anything.
            property.isExpanded = EditorGUI.Foldout(position,
                property.isExpanded, label.text, EditorStyles.foldoutHeader);

            if (property.isExpanded)
            {
                // Collapsable section
                EditorGUI.indentLevel++;

                // First drop into the first child, which will be editorDict, then
                // drop into the second child, which will be keyValuePairs.
                property.Next(true);
                property.Next(true);

                // Loop through the values,
                for (int i = 0; i < property.arraySize; i++)
                {
                    var child = property.GetArrayElementAtIndex(i);

                    // Drop into the Key field and get the enum.
                    child.Next(true);
                    string[] enums = child.enumDisplayNames;
                    string enumName = enums[child.enumValueIndex];

                    // Drop into the Value field and draw that property.
                    child.Next(false);
                    EditorGUILayout.PropertyField(child, new GUIContent(enumName));
                }

                // Exit collapsable section.
                EditorGUI.indentLevel--;
            }

            // // Then, draw only the keyValuePairs.
            // EditorGUI.PropertyField(position, property, label, true);
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