using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Interval))]
public class IntervalDrawerUIE : PropertyDrawer
{
    private Interval GetIntervalObject(SerializedProperty property)
    {
        return (Interval)property.GetObjectFromReflection();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            Interval interval = GetIntervalObject(property);
            Range range = interval.range;

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
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
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;

            // Calculate rects
            var patternRect = new Rect(position.x, position.y + lnHeight, position.width, lnHeight);
            var value1Rect = new Rect(position.x, position.y + 2 * lnHeight, position.width, lnHeight);
            var value2Rect = new Rect(position.x, position.y + 3 * lnHeight, position.width, lnHeight);
            var value3Rect = new Rect(position.x, position.y + 4 * lnHeight, position.width, lnHeight);

            // Draw fields
            string rangeAddition = nameof(Interval.range) + ".";
            

            using (new EditorGUI.DisabledScope(range.fixedSelectionInInspector))
            {
                GUIContent intervalPattern = new("Interval Pattern");
                EditorGUI.PropertyField(patternRect, property.FindPropertyRelative(rangeAddition + nameof(Range.rangePattern)), intervalPattern);
            }

            switch (range.rangePattern)
            {
                case Range.RangePattern.Single:
                    EditorGUI.PropertyField(value1Rect, property.FindPropertyRelative(rangeAddition + nameof(Range.singleValue)));
                    EditorGUI.PropertyField(value2Rect, property.FindPropertyRelative(nameof(Interval.timer)));
                    break;
                case Range.RangePattern.Linear:
                    EditorGUI.PropertyField(value1Rect, property.FindPropertyRelative(rangeAddition + nameof(Range.scalarMin)));
                    EditorGUI.PropertyField(value2Rect, property.FindPropertyRelative(rangeAddition + nameof(Range.scalarMax)));
                    EditorGUI.PropertyField(value3Rect, property.FindPropertyRelative(nameof(Interval.timer)));
                    break;
                case Range.RangePattern.Curves:
                    EditorGUI.PropertyField(value1Rect, property.FindPropertyRelative(rangeAddition + nameof(Range.curve)));
                    EditorGUI.PropertyField(value2Rect, property.FindPropertyRelative(rangeAddition + nameof(Range.modifer)));
                    EditorGUI.PropertyField(value3Rect, property.FindPropertyRelative(nameof(Interval.timer)));
                    break;
                case Range.RangePattern.Perlin:
                    EditorGUI.PropertyField(value1Rect, property.FindPropertyRelative(rangeAddition + nameof(Range.perlinCrawlSpeed)));
                    EditorGUI.PropertyField(value2Rect, property.FindPropertyRelative(rangeAddition + nameof(Range.modifer)));
                    EditorGUI.PropertyField(value3Rect, property.FindPropertyRelative(nameof(Interval.timer)));
                    break;
            }
            

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
        catch (System.ArgumentNullException e)
        {
            Debug.LogError(e);
            base.OnGUI(position, property, label);
            return;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        try
        {
            Interval interval = GetIntervalObject(property);
            Range range = interval.range;

            if (!range.unfoldedInInspector)
                return EditorGUIUtility.singleLineHeight;
    
            int lines = 0;
    
            switch (range.rangePattern)
            {
                case Range.RangePattern.Single:
                    lines = 4;
                    break;
                case Range.RangePattern.Linear:
                    lines = 5;
                    break;
                case Range.RangePattern.Curves:
                    lines = 5;
                    break;
                case Range.RangePattern.Perlin:
                    lines = 5;
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