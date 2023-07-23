using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Contains classes pertaining to editor stuff.
/// </summary>
public static class EditorExt
{
    #region Numbers
    /// <summary>
    /// Equal to <see cref="EditorGUIUtility.singleLineHeight"/> +
    /// <see cref="EditorGUIUtility.standardVerticalSpacing"/>.
    /// </summary>
    public static float SpacedLineHeight => EditorGUIUtility.singleLineHeight
        + EditorGUIUtility.standardVerticalSpacing;
    #endregion

    #region Reflection
    /// <summary>
    /// Gets the 
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    /// <exception cref="System.NullReferenceException"></exception>
    public static object GetObjectFromReflection(this SerializedProperty property)
    {
        // Get the target object and type. These will be modified when we are
        // traversing the path.
        object targetObject = property.serializedObject.targetObject;
        System.Type targetType = targetObject.GetType();

        string fullPath = property.propertyPath;
        fullPath = fullPath.Replace("Array.data[", "[");

        string[] path = fullPath.Split('.');

        // Path traversal.
        foreach (var name in path)
        {
            // Check if there's a bracket. If so, that means that we have an
            // array.
            int bracketPos = name.IndexOf('[');
            if (bracketPos >= 0)
            {
                // In an array
                // This is an array element we are looking for.
                int index = int.Parse(name.Substring(bracketPos + 1,
                    name.Length - bracketPos - 2));

                var enumerable = ((IEnumerable)targetObject);

                // Iterate through the enumerable, until index is reached.
                foreach (var thing in enumerable)
                {
                    if (index <= 0)
                    {
                        targetObject = thing;
                        targetType = thing.GetType();
                        break;
                    }

                    index--;
                }
            }
            else
            {
                // For each name in the path, get the reflection.
                FieldInfo field = targetType.GetField(name,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                if (field == null)
                    throw new System.NullReferenceException(
                        $"Field is null for {name}");

                targetType = field.FieldType;
                targetObject = field.GetValue(targetObject);
            }
        }

        return targetObject;
    }

    /// <summary>
    /// Calls <see cref="SerializedProperty.Next(bool)"/> until a child is found
    /// of type <paramref name="type"/>. This is useful if your property is
    /// nested somewhere.
    /// </summary>
    /// <param name="property">The serialized property to search. If it returns
    /// true, then this will be a serialized property of type <paramref
    /// name="type"/>. Otherwise, it is reset.</param>
    /// <param name="type">The type to look for.</param>
    /// <param name="visibleOnly">If true, then calls <see
    /// cref="SerializedProperty.NextVisible(bool)"/> instead.</param>
    /// <return>True on success, false otherwise.</return>
    public static bool AdvancePropertyToType(this SerializedProperty property,
        System.Type type,
        bool visibleOnly = false)
    {
        do
        {
            string typename = type.Name;
            if (property.type == typename)
                return true;
        }
        while (visibleOnly ? property.NextVisible(true) : property.Next(true));

        property.Reset();
        return false;
    }
    #endregion
    
    #region Drawing
    /// <summary>
    /// Draws a bolded title at position.
    /// </summary>
    /// <param name="position">Where to draw this.</param>
    /// <param name="label">Label to use as a title.</param>
    public static void DrawTitleLabel(ref Rect position, string label)
    {
        position.height = 24;
        EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
        position.Translate(new Vector2(0, position.height));
    }

    /// <summary>
    /// Draws the serialized property. Tries to look for custom property
    /// drawers.
    /// </summary>
    /// <param name="property">Property to draw.</param>
    /// <param name="position">Where to draw the property.</param>
    /// <param name="label">Label to use with custom property drawers, if found
    /// </param>
    public static void DrawSerializedProperty(this SerializedProperty property,
        ref Rect position, GUIContent label)
    {
        // Try to find property drawer, if we can.
        PropertyDrawer drawer = PropertyDrawerFinder.FindDrawer(property);

        if (drawer != null)
        {
            // Found a custom property drawer. Use found drawer.
            position.height = drawer.GetPropertyHeight(property, label);
            EditorGUI.BeginProperty(position, label, property);
            drawer.OnGUI(position, property, label);
            EditorGUI.EndProperty();
        }
        else
        {
            // Did not find a custom property drawer. Use property field.
            position.height = EditorGUI.GetPropertyHeight(property);
            EditorGUI.PropertyField(position, property, true);
        }

        position.Translate(new Vector2(0, position.height));
    }

    /// <summary>
    /// Draws the serialized property. Tries to look for custom property
    /// drawers.
    /// </summary>
    /// <param name="property">Property to draw.</param>
    /// <param name="position">Where to draw the property.</param>
    public static void DrawSerializedProperty(this SerializedProperty property,
        ref Rect position)
    {
        property.DrawSerializedProperty(ref position, GUIContent.none);
    }

    /// <summary>
    /// Returns the height of the serialized property, trying to look for any
    /// custom height getters. If not found, returns defaultHeight.
    /// </summary>
    /// <param name="property">Property to get the height from.</param>
    /// <param name="label">The label.</param>
    /// <param name="defaultHeight">The default height
    /// (base.GetPropertyHeight(property, label);).</param>
    /// <returns></returns>
    public static float GetSerializedPropertyHeight(
        this SerializedProperty property, GUIContent label, float defaultHeight)
    {
        // Try to find property drawer, if we can.
        PropertyDrawer drawer = PropertyDrawerFinder.FindDrawer(property);

        if (drawer != null)
        {
            // Found a custom property drawer. Use found drawer.
            return drawer.GetPropertyHeight(property, label);
        }
        else
        {
            // Did not find a custom property drawer. Use default.
            return defaultHeight;
        }
    }
    #endregion
}