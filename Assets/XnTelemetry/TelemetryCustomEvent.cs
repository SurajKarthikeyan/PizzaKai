using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XnTelemetry {
    public enum eHandleType { arrow, cone, cube, cylinder, dot, icon, text, sphere };
    public enum eRotationOverride { rotation, up, down, left, right, forward_none, back };

    [System.Serializable]
    public class TelemetryCustomEvent {
        static bool inited = false;
        static Quaternion rotUp, rotDown, rotRight, rotLeft, rotForward, rotBack;
        static public float coneScale = 1; // NOTE: This is set by Telemetry_Cloud_MultiplayerViewer
        static GUIStyle labelStyle;

        public string name;

        public eHandleType type = eHandleType.text;

        [Tooltip( "The rotation of the Handle drawn.\n\"rotation\" will rotate with the Telemetry\n\"forward_none\" is the identity rotation" )]
        public eRotationOverride rot = eRotationOverride.rotation;

        public bool overrideColor = false;
        public Color color = new Color( 1, 1, 1, 1 );

        public Texture icon;
        public float iconSize = 20;

        public Vector3 positionOffset = Vector3.zero;
        public float scaleMultiplier = 1;

        //public TelemetryCustomEvent() {
        //    color = Color.white; // Fix the default clear color bug?...No, it doesn't fix the bug!
        //}

        public void InitStatics() {
            if ( inited ) return;

            rotUp = Quaternion.LookRotation( Vector3.up, Vector3.back );
            rotDown = Quaternion.LookRotation( Vector3.down, Vector3.back );
            rotRight = Quaternion.LookRotation( Vector3.right, Vector3.back );
            rotLeft = Quaternion.LookRotation( Vector3.left, Vector3.back );
            rotForward = Quaternion.identity;
            rotBack = Quaternion.LookRotation( Vector3.back, Vector3.up );

            labelStyle = new GUIStyle();
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.imagePosition = ImagePosition.ImageAbove;
        }


        /// <summary>
        /// Returns the correct rotation for this TelemetryCustomEvent based on the Telem
        /// </summary>
        public Quaternion Rotation( Telem telem ) {
            switch ( rot ) {
            case eRotationOverride.up:
                return rotUp;
            case eRotationOverride.down:
                return rotDown;
            case eRotationOverride.right:
                return rotRight;
            case eRotationOverride.left:
                return rotLeft;
            case eRotationOverride.forward_none:
                return rotForward;
            case eRotationOverride.back:
                return rotBack;
            }
            // The final case is eRotationOverride.rotation
            return telem.rotation;
        }

        public void Draw( Telem telem ) {
            InitStatics();
#if UNITY_EDITOR
            Color handleColor = Handles.color;
            if ( overrideColor ) Handles.color = color;
            float scale = coneScale * scaleMultiplier;

            switch ( type ) {
            case eHandleType.arrow:
                Handles.ArrowHandleCap( 0, telem.position + positionOffset, Rotation( telem ), scale, EventType.Repaint );
                break;

            case eHandleType.cone:
                Handles.ConeHandleCap( 0, telem.position + positionOffset, Rotation( telem ), scale, EventType.Repaint );
                break;

            case eHandleType.cube:
                Handles.CubeHandleCap( 0, telem.position + positionOffset, Rotation( telem ), scale, EventType.Repaint );
                break;

            case eHandleType.cylinder:
                Handles.CylinderHandleCap( 0, telem.position + positionOffset, Rotation( telem ), scale, EventType.Repaint );
                break;

            case eHandleType.dot:
                Handles.DotHandleCap( 0, telem.position + positionOffset, Rotation( telem ), scale, EventType.Repaint );
                break;

            case eHandleType.sphere:
                Handles.SphereHandleCap( 0, telem.position + positionOffset, Rotation( telem ), scale, EventType.Repaint );
                break;

            case eHandleType.text:
                Handles.Label( telem.position + positionOffset, name, labelStyle );
                break;

            case eHandleType.icon:
                Vector2 prevIconSize = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize( Vector2.one * iconSize );
                Handles.Label( telem.position + positionOffset, new GUIContent( icon ), labelStyle );
                EditorGUIUtility.SetIconSize( prevIconSize );
                break;
            }

            //if ( overrideColor ) Handles.color = handleColor;
#endif
        }
    }

//#if UNITY_EDITOR
//    [CustomPropertyDrawer( typeof( TelemetryCustomEvent ) )]
//    public class TelemetryCustomEvent_PropertyDrawer : PropertyDrawer {
//        TelemetryCustomEvent tCE;

//        public override void OnGUI( Rect pos, SerializedProperty prop, GUIContent label ) {
//            // Using BeginProperty / EndProperty on the parent property means that
//            // prefab override logic works on the entire property.
//            EditorGUI.BeginProperty( pos, label, prop );

//            // Prevent the default clear color bug
//            SerializedProperty sp_color = prop.FindPropertyRelative( "color" );
//            if ( sp_color.colorValue == Color.clear ) sp_color.colorValue = Color.white;

//            EditorGUI.PropertyField( pos, prop, label, true );

//            EditorGUI.EndProperty();
//        }
//    }
//#endif
}