using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XnTools {
    // Originally from https://assetstore.unity.com/packages/tools/physics/kinematic-character-controller-99131
    /// <summary>
    /// Causes a field to be shown grayed-out and uneditable.
    /// However, the field *can* be edited when the Inspector is in Debug mode. 
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer( typeof(ReadOnlyAttribute) )]
    public class ReadOnlyPropertyDrawer : PropertyDrawer {
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
            return EditorGUI.GetPropertyHeight( property, label, true );
        }

        public override void OnGUI( Rect position, SerializedProperty property,
                                    GUIContent label ) {
            GUI.enabled = false;
            EditorGUI.PropertyField( position, property, label, true );
            GUI.enabled = true;
        }
    }
#endif
}
