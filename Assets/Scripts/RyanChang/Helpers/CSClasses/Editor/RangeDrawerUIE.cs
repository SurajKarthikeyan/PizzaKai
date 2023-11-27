using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Range))]
public class RangeDrawerUIE : PropertyDrawer
{
    private Range GetRangeObject(SerializedProperty property)
    {
        return (Range)property.GetObjectFromReflection();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            Range range = GetRangeObject(property);

            // Using BeginProperty / EndProperty on the parent property means
            // that prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Define heights
            float lnHeight = EditorGUIUtility.singleLineHeight;
            float tHeight = GetPropertyHeight(property, label);

            // Dropdown
            range.unfoldedInInspector = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width - 6, lnHeight),
                range.unfoldedInInspector,
                label);

            if (!range.unfoldedInInspector)
                return;

            // Define indent
            var oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            // Calculate rects
            var patternRect = new Rect(position.x, position.y + lnHeight, position.width, lnHeight);
            var value1Rect = new Rect(position.x, position.y + 2 * lnHeight, position.width, lnHeight);
            var value2Rect = new Rect(position.x, position.y + 3 * lnHeight, position.width, lnHeight);
            var value3Rect = new Rect(position.x, position.y + 4 * lnHeight, position.width, lnHeight);

            // Draw fields
            using (new EditorGUI.DisabledScope(range.fixedSelectionInInspector))
            {
                EditorGUI.PropertyField(patternRect, property.FindPropertyRelative(nameof(Range.rangePattern)));
            }
            
            switch (range.rangePattern)
            {
                case Range.RangePattern.Single:
                    EditorGUI.PropertyField(value1Rect, property.FindPropertyRelative(nameof(Range.singleValue)));
                    break;
                case Range.RangePattern.Linear:
                    EditorGUI.PropertyField(value1Rect, property.FindPropertyRelative(nameof(Range.scalarMin)));
                    EditorGUI.PropertyField(value2Rect, property.FindPropertyRelative(nameof(Range.scalarMax)));
                    break;
                case Range.RangePattern.Curves:
                    EditorGUI.PropertyField(value1Rect, property.FindPropertyRelative(nameof(Range.curve)));
                    EditorGUI.PropertyField(value2Rect, property.FindPropertyRelative(nameof(Range.modifer)));
                    break;
                case Range.RangePattern.Perlin:
                    EditorGUI.PropertyField(value1Rect, property.FindPropertyRelative(nameof(Range.perlinCrawlSpeed)));
                    EditorGUI.PropertyField(value2Rect, property.FindPropertyRelative(nameof(Range.modifer)));
                    break;
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = oldIndent;

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
        try
        {
            Range range = GetRangeObject(property);
    
            if (!range.unfoldedInInspector)
                return EditorGUIUtility.singleLineHeight;
    
            int lines = 0;
    
            switch (range.rangePattern)
            {
                case Range.RangePattern.Single:
                    lines = 3;
                    break;
                case Range.RangePattern.Linear:
                    lines = 4;
                    break;
                case Range.RangePattern.Curves:
                    lines = 4;
                    break;
                case Range.RangePattern.Perlin:
                    lines = 4;
                    break;
            }
    
            return EditorGUIUtility.singleLineHeight * lines;
        }
        catch (System.ArgumentNullException)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}