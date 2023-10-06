using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace XnTelemetry {
    [CreateAssetMenu( fileName = "TelemetrySettings", menuName = "ScriptableObjects/TelemetrySettings" )]
    public class SO_TelemetrySettings : ScriptableObject {
        public const string DIVESTRING = "Dive";
        public const string JUMPSTRING = "Jump";

        public static string[] PROJECTS = { "grappler", "grabbie", "3PLE", "UnicycleSamurai", "DemolitionExpress", "ScreenTear", "RockTheHouse", "Chamber8", "FPS_Demo" };
        [System.Flags]
        public enum eSaveOptions { None=0, Local=1, Cloud=2 };


        [XnTools.Hidden]
        public string serverURLQuery = "http://telemetry.prototools.net/queryTelemetryEntries.php";
        [XnTools.Hidden]
        public string serverURLPost = "http://telemetry.prototools.net/postTelemetry.php";


        [Header("Project Information")]
        [XnTools.ReadOnly]
        [Tooltip( "Semester must be in the format: 2022.1 (i.e., 4 digits, a decimal, 1 digit, because the DB needs it that way)!" )]
        public string semester = "2022.4";
        [XnTools.ReadOnly]
        public string project = "FPS_Demo";
        public bool autoVersion = true;
        public string versionOverride = "";
        public string version {
            get {
                if ( !autoVersion && versionOverride != "" ) return versionOverride;
                return Application.version;
            }
        }


        [Header( "Save Configuration" )]
        public eSaveOptions editorSaveTo = eSaveOptions.None;
        public eSaveOptions playerSaveTo = eSaveOptions.None;


        //[Header( "Custom Events" )]
        public List<TelemetryCustomEvent> customEvents;
        private int numCustomEvents = 0;
        private Dictionary<string, TelemetryCustomEvent> _customEventDict;
        public Dictionary<string, TelemetryCustomEvent> customEventDict {
            get {
                if (_customEventDict == null) {
                    _customEventDict = new Dictionary<string, TelemetryCustomEvent>();
                    foreach (TelemetryCustomEvent tCE in customEvents) {
                        if (_customEventDict.ContainsKey(tCE.name)) {
                            // The Dict already contains this key, which usually means that the user
                            //  just made a new entry, and it still has the same name.
                            _customEventDict = null;
                            return null;
                        }
                        _customEventDict.Add( tCE.name, tCE );
                    }
                }
                return _customEventDict;
            }
        }

        [Header( "Current Play Session Info" )]
        public string playerName = "UNKNOWN";
        public string sceneName = "";

        private void OnValidate() {
            if (customEvents.Count != numCustomEvents) {
                _customEventDict = null; // Reset this so that new TelemetryCustomEvents are added to the Dict on the next Editor Update
                numCustomEvents = customEvents.Count;
            }
        }

    }


#if UNITY_EDITOR
    [CustomEditor( typeof( SO_TelemetrySettings ) )]
    public class SO_TelemetrySettings_Editor : Editor {

        private SO_TelemetrySettings soTS;
        private SerializedProperty sp_project, sp_versionOverride, sp_autoVersion, sp_semester, sp_playerName, sp_sceneName; //, sp_serverURL, 
        private SerializedProperty sp_editorSaveTo, sp_playerSaveTo, sp_customEvents;
        private int selectedProject = -1;
        private bool attemptOnInspectorGUI = true;
        private GUIContent gc_autoLabel;

        void OnEnable() {
            soTS = (SO_TelemetrySettings) target;
            sp_editorSaveTo = serializedObject.FindProperty( "editorSaveTo" );
            sp_playerSaveTo = serializedObject.FindProperty( "playerSaveTo" );
            sp_project = serializedObject.FindProperty( "project" );
            sp_versionOverride = serializedObject.FindProperty( "versionOverride" );
            sp_autoVersion = serializedObject.FindProperty( "autoVersion" );
            sp_semester = serializedObject.FindProperty( "semester" );
            sp_playerName = serializedObject.FindProperty( "playerName" );
            sp_sceneName = serializedObject.FindProperty( "sceneName" );
            sp_customEvents = serializedObject.FindProperty( "customEvents" );
            //sp_serverURL = serializedObject.FindProperty( "serverURL" );

            gc_autoLabel = new GUIContent( "Auto", "When this is checked, Application.version is read from Edit > Project Settings > Player." );
        }


        public override void OnInspectorGUI() {
            if ( !attemptOnInspectorGUI ) {
                // Only happens if something has failed that makes drawing the Inspector problematic.
                GUILayout.Label( "ERROR in SO_TelemetrySettings_Editor.OnInspectorGUI()" );
                DrawDefaultInspector();
                serializedObject.ApplyModifiedProperties();
                return;
            }

            EditorGUILayout.PropertyField( sp_semester );

            if ( selectedProject == -1 ) selectedProject = System.Array.IndexOf( SO_TelemetrySettings.PROJECTS, soTS.project );
            if ( selectedProject == -1 ) { // selectedProject is not actually in the PROJECTS list!
                attemptOnInspectorGUI = false;
                Debug.LogError( $"project \"{soTS}\" is not a valid member of the SO_TelemetrySettings.PROJECTS array!" +
                    "\nThis means that it will not be able to be added to the cloud database!" );
                return;
            }

            int selectedProjectNew = EditorGUILayout.Popup( "Project", selectedProject, SO_TelemetrySettings.PROJECTS );
            if ( selectedProjectNew != selectedProject ) {
                selectedProject = selectedProjectNew;
                soTS.project = SO_TelemetrySettings.PROJECTS[selectedProject];
                sp_project.stringValue = soTS.project;
            }

            GUILayout.BeginHorizontal();
            {
                string versionString = soTS.autoVersion ? Application.version : soTS.versionOverride;
                string versionLabel = soTS.autoVersion ? "Version (auto)" : "Version (override)";
                string versionStringNew = EditorGUILayout.TextField( versionLabel, versionString );
                GUILayout.Label( gc_autoLabel, GUILayout.Width( 32 ) );
                EditorGUILayout.PropertyField( sp_autoVersion, GUIContent.none, GUILayout.Width(16) );
                if (!sp_autoVersion.boolValue) {
                    if (versionStringNew != versionString) {
                        sp_versionOverride.stringValue = versionStringNew;
                    }
                }
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField( sp_editorSaveTo );
            EditorGUILayout.PropertyField( sp_playerSaveTo );


            EditorGUILayout.PropertyField( sp_playerName );
            EditorGUILayout.PropertyField( sp_sceneName );

            GUILayout.Space( 10 );
            EditorGUILayout.PropertyField( sp_customEvents );
            

            //EditorGUILayout.PropertyField( sp_project );

            //DrawPropertiesExcluding( serializedObject, "m_Script", "saveToCloud", "project", "autoVersion", "versionOverride" );

            //EditorGUILayout.PropertyField( sp_semester );
            //EditorGUILayout.PropertyField( sp_serverURL );
            //EditorGUILayout.PropertyField( sp_versionOverride );

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif


    public static class DateTimeExtension {
        static public string ToTimeStamp( this System.DateTime time ) {
            string str = $"{time.Hour:00}{time.Minute:00}";
            return str;
        }

        static public string ToDateStamp( this System.DateTime time ) {
            string str = $"{time.Year:0000}-{time.Month:00}-{time.Day:00}";
            return str;
        }

        static public string ToDateTimeStamp( this System.DateTime time ) {
            return $"{time.ToDateStamp()}_{time.ToTimeStamp()}";
        }
    }


    //#region Telem
    //public struct Telem {
    //    const char TAG_SPLIT_CHAR = ' ';

    //    static public Vector3 V3_INVALID = new Vector3( -999, -999, -999 );

    //    public float time;
    //    public Vector3 position, p0, p1, p2;
    //    public Quaternion rotation;
    //    public string tag;
    //    public string[] splitTag;

    //    public Telem( float t, Vector3 pos, Quaternion rot, string tg = "" ) {
    //        this.time = t;
    //        this.position = pos;
    //        this.rotation = rot;
    //        this.tag = tg;
    //        this.splitTag = tg.Split( TAG_SPLIT_CHAR );
    //        p0 = p1 = p2 = V3_INVALID; // Give these a base value that can be replaced in the Telemetry_LocalViewer
    //    }

    //    override public string ToString() {
    //        Vector3 rot = rotation.eulerAngles;
    //        return $"{tag}\t{time:0.00}\t{position.x:0.00}\t{position.y:0.00}\t{position.z:0.00}\t{rot.x:0.00}\t{rot.y:0.00}\t{rot.z:0.00}";
    //    }

    //    static public Telem FromString( string s ) {
    //        string[] bits = s.Split( '\t' );
    //        if ( bits.Length != 5 && bits.Length != 8 ) {
    //            Debug.LogError( $"Telem string parsed into wrong number of bits: {bits.Length}.\n{s}" );
    //            return new Telem( -1, Vector3.zero, Quaternion.identity, "ERROR" );
    //        }
    //        string tag = bits[0];
    //        float time;
    //        Vector3 pos = Vector3.zero;
    //        Vector3 rot = Vector3.zero;
    //        try {
    //            time = float.Parse( bits[1] );
    //            pos.x = float.Parse( bits[2] );
    //            pos.y = float.Parse( bits[3] );
    //            pos.z = float.Parse( bits[4] );
    //        }
    //        catch {
    //            Debug.LogError( $"Telem floats did not parse correctly.\n{s}" );
    //            return new Telem( -1, Vector3.zero, Quaternion.identity, "ERROR" );
    //        }
    //        try {
    //            rot.x = float.Parse( bits[5] );
    //            rot.y = float.Parse( bits[6] );
    //            rot.z = float.Parse( bits[7] );
    //        }
    //        catch {
    //            // Don't actually need to do anything here, just ignore missing rotation. - JGB
    //        }
    //        return new Telem( time, pos, Quaternion.Euler( rot ), tag );
    //    }
    //}
    //#endregion
}