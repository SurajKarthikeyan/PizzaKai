using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XnTools {
    /// <summary>
    /// Causes a field to be invisible in Normal mode but editable in Debug mode.
    /// </summary>
    public class HiddenAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer( typeof( HiddenAttribute ) )]
    public class HiddenPropertyDrawer : PropertyDrawer {
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
            return 0;
            //return EditorGUI.GetPropertyHeight( property, label, true );
        }

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
            //GUI.enabled = false;
            //EditorGUI.PropertyField( position, property, label, true );
            //GUI.enabled = true;
        }
    }
#endif
}
