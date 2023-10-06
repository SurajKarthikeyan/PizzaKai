using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XnTelemetry {
    public class Telemetry_Cloud_Multiplayer : Telemetry_Cloud {
        static Telemetry_Cloud_Multiplayer _S;

        const string tagColor = "Color";
        const string tagStart = "Start";

        public List<TelemetryTracker> trackers;

        // This Start intentionally overrides Telemetry_Cloud.Start();
        void Start() {
            if ( !recordTelemetry ) return;
            _S = this;
            Reset();
            Log( tagColor );
            Log( tagStart );
            StartCoroutine( LogEveryInterval() );
        }

        IEnumerator LogEveryInterval() {
            while ( true ) {
                Log();
                yield return new WaitForSeconds( Telemetry_Cloud.LOG_INTERVAL );
            }
        }

        private void OnValidate() {
            for ( int i = 0; i < trackers.Count; i++ ) {
                trackers[i].id = i;
            }
        }

        public override void Log( string tag = "_", Transform trans = null ) {
            foreach ( TelemetryTracker tr in trackers ) {
                // If trans == null, log all TelemetryTrackers; if it's not null, only log one of them
                if ( trans == null || trans == tr.positionTransform || trans == tr.rotationTransform ) {
                    if (tag == tagColor ) {
                        Vector3 colorVec = new Vector3( tr.color.r, tr.color.g, tr.color.b );
                        telems.Add( new Telem( elapsed, colorVec, Quaternion.identity, tag, tr.id ) ); // Inject Color information into a standard Telem
                    } else if ( tr.rotationTransform == null ) {
                        telems.Add( new Telem( elapsed, tr.positionTransform.position, tr.positionTransform.rotation, tag, tr.id ) );
                    } else {
                        telems.Add( new Telem( elapsed, tr.positionTransform.position, tr.rotationTransform.rotation, tag, tr.id ) );
                    }
                }
            }
        }

        public override string EntriesToString() {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine( "Tag\tTime\tx\ty\tz\trX\trY\trZ\tid" );
            for ( int i = 0; i < telems.Count; i++ ) {
                sb.AppendLine( telems[i].ToString() );
            }
            return sb.ToString();
        }

        static public void LOG( string tag, Transform trans=null ) {
            if ( _S != null ) _S.Log( tag, trans );
        }

        [System.Serializable]
        public class TelemetryTracker {
            [XnTools.ReadOnly] public int id;
            public Color color;
            public Transform positionTransform;
            [Tooltip( "Leave rotationTransform null if intended to be the same as positionTransform." )]
            public Transform rotationTransform;
        }
    }

#if UNITY_EDITOR
    [CustomEditor( typeof( Telemetry_Cloud_Multiplayer ) )]
    public class Telemetry_Cloud_Multiplayer_Editor : Editor {
        public override void OnInspectorGUI() {
            DrawPropertiesExcluding( serializedObject, "m_Script", "rotationTransform" );
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}