using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif


namespace XnTelemetry
{
    public class Telemetry_CloudViewer : MonoBehaviour {
        //static public List<string> SCENE_NAMES;

        public bool showTelemetry = false;

        
        public SO_TelemetrySettings telemetrySettings;
        [Space(10)]
        [Range(0.25f, 4f)]
        public float coneScale = 0.5f;

        //[Dropdown( "GetSceneNames" )]
        //public string sceneName;

        //private List<string> GetSceneNames() {
        //    if (SCENE_NAMES == null) {
        //        return new List<string> { "You Must LoadSceneNames" };
        //    }
        //    return SCENE_NAMES;
        //}


        //public enum eCommand { SelectACommand, QueryRecs };
        //[Space(10)]
        //public eCommand chooseCommand = eCommand.SelectACommand;

        public List<TelemetryEntry> telemetryEntries;


        //[HideInInspector]
        //public List<TextAsset> telemetryFiles;
        //public bool checkToReloadFiles = false;

        //public List<Telemetry_Cloud.Telem[]> telemetries;


        //public void LoadSceneNames() {
        //    TextAsset sceneNamesTA = Resources.Load<TextAsset>( "SceneNames" );
        //    if ( sceneNamesTA == null ) {
        //        Debug.LogWarning( "Could not find Assets/Resources/SceneNames" );
        //        return;
        //    }
        //    string[] lines = sceneNamesTA.text.Split( '\n' );
        //    SCENE_NAMES = new List<string>();
        //    string str;
        //    for ( int l = 0; l < lines.Length; l++ ) {
        //        str = lines[l].Trim();
        //        SCENE_NAMES.Add( str );
        //    }
        //}
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Telemetry_CloudViewer))]
    public class Telemetry_CloudViewer_Editor : Editor {
        static private bool DEBUG_QUERIES       = true;
        //static private bool DEBUG               = true;
        static private bool DEBUG_ENTIRE_RESULT = true;

        Telemetry_CloudViewer tv;
        static Color C0 = Color.cyan;
        static Color C1 = Color.yellow;
        static Color CJump = Color.white;
        static Color CGrapple = Color.blue;

        float telemetryTime, telemetryTimeMax;
        bool _telemetryPlaying;
        float[] playRates = { 0.25f, 0.5f, 1, 2, 4, 8, 16 };
        string[] playRatesStr = { "0.25x", "0.5x", "1x", "2x", "4x", "8x", "16x" };
        static int playRateIndex = 5; // 1x is the 2nd element
        const string strPlay = "►", strPause = "■";
        float timeLastUpdate, previewStartTime;
        int numTelemetriesSelected = -1;

        bool telemetryPlaying {
            get { return _telemetryPlaying; }
            set {
                if (value != _telemetryPlaying) {
                    if (value) {
                        timeLastUpdate = Time.realtimeSinceStartup;

                        previewStartTime = (float)EditorApplication.timeSinceStartup;
                        previewStartTime -= telemetryTime / playRates[playRateIndex];
                        EditorApplication.update += EditorPreviewUpdate;
                    } else {
                        EditorApplication.update -= EditorPreviewUpdate;
                    }
                    _telemetryPlaying = value;
                }
            }
        }

        void EditorPreviewUpdate() {
            if (telemetryPlaying) {
                telemetryTime = (float)EditorApplication.timeSinceStartup - previewStartTime;
                telemetryTime = (telemetryTime * playRates[playRateIndex]) % telemetryTimeMax;
                SceneView.RepaintAll(); // Forces a call to OnSceneGUI() next Editor update.
                Repaint(); // Forces a call to OnInspectorGUI() next Editor update.
            }
        }

        public enum eConnectionProgress { NotConnected, Fail1, GotVersionsAndScenes, Fail2, GotTelemetries };
        public eConnectionProgress connectionProgress = eConnectionProgress.NotConnected;

        private SerializedProperty m_telemetrySettings;
        private SerializedProperty m_telemetryEntries;
        private SerializedProperty m_coneScale;
        
        void OnEnable() {
            tv = (Telemetry_CloudViewer)target;
            m_telemetrySettings = serializedObject.FindProperty( "telemetrySettings" );
            m_telemetryEntries = serializedObject.FindProperty("telemetryEntries");
            m_coneScale = serializedObject.FindProperty("coneScale");
        }

        void SelectAll(bool show) {
            telemetryTimeMax = 1;
            for (int i = 0; i < tv.telemetryEntries.Count; i++) {
                tv.telemetryEntries[i].show = show;
                if (show) telemetryTimeMax = Mathf.Max(telemetryTimeMax, tv.telemetryEntries[i].GetMaxTime());
            }
            numTelemetriesSelected = show ? tv.telemetryEntries.Count : 0;
            if (show) {
                telemetryTime = telemetryTimeMax;
            }
            SceneView.RepaintAll();
            Repaint();
        }

        // "m_Script", "showTelemetry", "telemetrySettings", "coneScale", "sceneName", "chooseCommand", "telemetryEntries", "telemetries", "checkToReloadFiles", "telemetryFiles"  
        public override void OnInspectorGUI() {
            // DrawPropertiesExcluding( serializedObject, "m_Script", "showTelemetry", "coneScale",
            //     "sceneName", "chooseCommand", "telemetryEntries", "telemetries",
            //     "checkToReloadFiles", "telemetryFiles" );

            EditorGUILayout.PropertyField( m_telemetrySettings );
            
            GUILayout.Space(10  );
            
            switch ( connectionProgress ) {
            case eConnectionProgress.NotConnected:
                if ( GUILayout.Button( "To Load Telemetry Data:\n1. Connect to Server..." ) ) ConnectToServer();
                break;
            
            case eConnectionProgress.Fail1:
                if ( GUILayout.Button( "1. Connect to Server..." ) ) ConnectToServer();
                GUILayout.Label("Connection to Server Failed");
                break;
            
            case eConnectionProgress.GotVersionsAndScenes:
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label( "Proj:   "  + tv.telemetrySettings.project );
                    GUILayout.Label( "\tSem:   " + tv.telemetrySettings.semester );
                    GUILayout.Label( "\tVers:" );
                    versionsIndex = EditorGUILayout.Popup( versionsIndex, versions );
                }
                GUILayout.EndHorizontal();
                sceneIndex =
                    EditorGUILayout.Popup( sceneIndex, versionSceneDict[versions[versionsIndex]] );

                GUILayout.Space( 10 );
                if ( GUILayout.Button( "2. Get Telemetries for this Scene" ) ) GetSceneTelemetries();
                
                break;
            
            case eConnectionProgress.Fail2:
                if ( GUILayout.Button( "1. Connect to Server..." ) ) ConnectToServer();
                GUILayout.Label("Failed to Collect Telemetries");
                break;
            
            case eConnectionProgress.GotTelemetries:
                if ( GUILayout.Button( "1. Connect to Server..." ) ) ConnectToServer();
                GUILayout.Label("Successfully Found Telemetries!");
                break;
            
            }

            GUILayout.Space( 10 );
            
            EditorGUILayout.PropertyField( m_telemetryEntries );

            int numTelemetriesSelectedPrev = numTelemetriesSelected;
            if (tv.telemetryEntries != null && tv.telemetryEntries.Count > 0) {
                // Find the number of TelemetryEntries that are selected
                numTelemetriesSelected = 0;
                telemetryTimeMax = 1;
                foreach (TelemetryEntry te in tv.telemetryEntries) {
                    if (te.show) {
                        numTelemetriesSelected++;
                        telemetryTimeMax = Mathf.Max(telemetryTimeMax, te.maxTime);
                    }
                }
                if (numTelemetriesSelectedPrev != numTelemetriesSelected) {
                    SceneView.RepaintAll();
                    if (numTelemetriesSelectedPrev == 0) telemetryTime = telemetryTimeMax;
                }

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Select:", GUILayout.MaxWidth(80));
                    if (GUILayout.Button("None")) SelectAll(false);
                    if (GUILayout.Button("All")) SelectAll(true);
                    GUILayout.Space(20);
                    GUILayout.Label("Play Rate:", GUILayout.MaxWidth(80));
                    // Show Play rate
                    playRateIndex = EditorGUILayout.Popup(playRateIndex, playRatesStr, GUILayout.MaxWidth(60));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    // Play button
                    if (telemetryPlaying) {
                        if (GUILayout.Button(strPause)) telemetryPlaying = false;
                    } else {
                        if (GUILayout.Button(strPlay)) telemetryPlaying = true;
                    }
                    // Show time slider
                    if (telemetryTime > telemetryTimeMax) telemetryTime = telemetryTimeMax;
                    float telemetryTime0 = telemetryTime;
                    telemetryTime = EditorGUILayout.Slider(telemetryTime, 0, telemetryTimeMax);
                    if (telemetryTime0 != telemetryTime) {
                        telemetryPlaying = false;
                        SceneView.RepaintAll();
                    }

                }
                EditorGUILayout.EndHorizontal();

                // Show play button and play rate
            }

            EditorGUILayout.PropertyField(m_coneScale);


            serializedObject.ApplyModifiedProperties();
            
            // DrawPropertiesExcluding( serializedObject, "m_Script", "showTelemetry",
            //     "telemetrySettings", "coneScale", "sceneName", "chooseCommand",
            //     "telemetries", "checkToReloadFiles", "telemetryFiles" );
            
        }

        /// <summary>
        /// Handles splitting the result into lines, filtering out and debugging
        /// the first line if it is an echo of the query, and trimming all lines.
        /// Also eliminates all empty lines. 
        /// </summary>
        /// <param name="result">Clean result lines as a string List</param>
        /// <param name="expectedHeader">A string of the header we should get
        /// from the server to check for version changes</param>
        /// <param name="removeHeader">Default false</param>
        /// <returns></returns>
        List<string> QueryResultToLines( string result, string expectedHeader="", bool removeHeader=false ) {
            if (DEBUG_ENTIRE_RESULT) Debug.LogWarning(result);
            
            // Split into lines
            string[] lineArray = result.Split( '\n' );
            List<string> lines = new List<string>();
            
            // Trim leading and trailing spaces and damn \r chars
            string l;
            for (int i = 0; i < lineArray.Length; i++) {
                l = lineArray[i].Trim();
                // Only keep lines with a length longer than 0
                if ( l.Length > 0 ) lines.Add( l );
            }
            
            // Usually, the first line will be a query
            string query;
            if (lines[0].Contains("Query:")) {
                query = lines[0];
                lines.RemoveAt( 0 );
                if (!DEBUG_ENTIRE_RESULT && DEBUG_QUERIES) Debug.LogWarning( query );
            }
            
            // Usually, the second line is a header
            if ( lines[0] != expectedHeader ) {
                Debug.LogError("SERVER RESULT HEADER MISMATCH\n" +
                               $"Expected: '{expectedHeader}'\n" +
                               $"Received: '{lines[0]}'");

                return null;
            }

            if ( removeHeader ) lines.RemoveAt( 0 );

            return lines;
        }

        
        private string[]                     versions      = {""};
        private int                          versionsIndex = 0;
        private Dictionary<string, string[]> versionSceneDict;
        private int sceneIndex = 0;
        const   string                       connectToServerHeader = "project\tsemester\tversion\tsceneName";
        void GetSceneTelemetries() {
            WWWForm form = new WWWForm();
            form.AddField("op", "ListAndGetEntries");
            form.AddField("project", tv.telemetrySettings.project);
            form.AddField("semester", tv.telemetrySettings.semester);
            form.AddField("sceneName", versionSceneDict[versions[versionsIndex]][sceneIndex]);
            new Telemetry_CloudHelper(tv.telemetrySettings.serverURLQuery, form, GetSceneTelemetries_CB);
        }

        void ConnectToServer() {
            tv.telemetryEntries.Clear();

            WWWForm form = new WWWForm();
            form.AddField("op", "ListVersions");
            form.AddField("project", tv.telemetrySettings.project);
            form.AddField("semester", tv.telemetrySettings.semester);
            new Telemetry_CloudHelper(tv.telemetrySettings.serverURLQuery, form, ConnectToServer_CB);
        }
        void ConnectToServer_CB( string result ) {
            // Assume failure
            connectionProgress = eConnectionProgress.Fail1;
            Repaint();
            
            List<string> lines = QueryResultToLines( result, connectToServerHeader, true );
            if ( lines == null ) return; // Connection failure
            int expectedBits = connectToServerHeader.Split( '\t' ).Length;
            
            string[] bits;
            List<string> tempVersions = new List<string>();
            string version, sceneName;
            Dictionary<string, List<string>> tempVersionSceneDict =
                new Dictionary<string, List<string>>();

            for ( int i = 0; i < lines.Count; i++ ) {
                bits = lines[i].Split( '\t' );
                if ( bits.Length != expectedBits ) {
                    Debug.LogWarning(
                        $"Expected {expectedBits} bits but got {bits.Length}:\n{bits}\n{lines[i]}" );
                    return; // bit length failure
                }
                version = bits[2];
                if ( !tempVersions.Contains( version ) ) {
                    tempVersions.Add( version );
                }
                sceneName = bits[3];
                if ( tempVersionSceneDict.ContainsKey( version ) ) {
                    tempVersionSceneDict[version].Add( sceneName );
                } else {
                    tempVersionSceneDict.Add( version, new List<string> { sceneName } );
                }
            }

            versions = tempVersions.ToArray();
            versionSceneDict = new Dictionary<string, string[]>();
            foreach ( string ver in versions ) {
                versionSceneDict.Add( ver, tempVersionSceneDict[ver].ToArray() );
            }
            versionsIndex = versions.Length - 1;
            string activeSceneName = SceneManager.GetActiveScene().name;
            string[] scenesArray = versionSceneDict[versions[versionsIndex]];
            sceneIndex = 0;
            for ( int sNum = 0; sNum < scenesArray.Length; sNum++ ) {
                if ( scenesArray[sNum] == activeSceneName ) {
                    sceneIndex = sNum;
                    break;
                }
            }
            
            connectionProgress = eConnectionProgress.GotVersionsAndScenes;
        }

        private const string getTelemetriesHeader =
            "recNum\tproject\tsemester\tversion\tsceneName\tplayerName\tdateTime\tdata";
        void GetSceneTelemetries_CB( string result ) {
            // Assume failure
            connectionProgress = eConnectionProgress.Fail2;
            Repaint();
              
            List<string> lines = QueryResultToLines( result, getTelemetriesHeader, false );
            if ( lines == null ) return; // Connection failure
            int expectedBits = getTelemetriesHeader.Split( '\t' ).Length;
            
            TelemetryEntry.SET_HEADERS(lines[0]);
            tv.telemetryEntries.Clear();
            for (int i = 1; i < lines.Count; i++) {
                TelemetryEntry entry = TelemetryEntry.MAKE(lines[i]);
                if (entry != null) tv.telemetryEntries.Add(entry);
            }

            connectionProgress = eConnectionProgress.GotTelemetries;
        }
        

        // public void LoadTelemetries() {
        //     tv.telemetries = new List<Telemetry_Cloud.Telem[]>();
        //     List<Telemetry_Cloud.Telem> telemList;
        //     //Telemetry_Local.Telem[] telems;
        //     string[] lines;
        //     Telemetry_Cloud.Telem telem;
        //     foreach (TelemetryEntry tEnt in tv.telemetryEntries) {
        //         lines = tEnt.data.Split('\n');
        //         // The first line is the legend, so it is skipped. - JGB
        //         int num = lines.Length - 1;
        //         telemList = new List<Telemetry_Cloud.Telem>();
        //         for (int i = 1; i < lines.Length; i++) {
        //             if (lines[i].Length == 0) continue;
        //             telem = Telemetry_Cloud.Telem.FromString( lines[i].Trim() );
        //             if (telem.tag == "ERROR") continue;
        //             telemList.Add(telem);
        //         }
        //         tv.telemetries.Add(telemList.ToArray());
        //     }
        //     Debug.Log($"{tv.telemetries.Count} Telemetry files loaded.");
        // }

        void OnSceneGUI() {
            // HandleCommand();


            // if (tv.checkToReloadFiles) {
            //     tv.checkToReloadFiles = false;
            //     LoadTelemetries();
            // }

            // if (!tv.showTelemetry) return;

            Quaternion rotUp = Quaternion.LookRotation(Vector3.up);
            Quaternion rotDown = Quaternion.LookRotation(Vector3.down);
            float coneSize = tv.coneScale;
            float cubeSize = coneSize * 0.5f;
            Vector3 coneOffset = new Vector3(0, -.5f, 0);
            Vector3 camOffset = new Vector3(0, 1.44f, 0); // 1.44 comes from the MainCamera position within __Player_2022
                                                          //float grappleDist = 0;


            // if (tv.telemetries == null) LoadTelemetries();
            Telem telem;
            Telem[] telems;
            Color telemColor, telemColorTrans;
            float tENum = -1;
            for (int j = 0; j < tv.telemetryEntries.Count; j++) {
                if ( !tv.telemetryEntries[j].show ) continue;
                if ( tv.telemetryEntries[j].telemsDict == null || tv.telemetryEntries[j].telemsDict.Count == 0 ) {
                    if ( tv.telemetryEntries[j].data.Length > 1 ) {
                        tv.telemetryEntries[j].ParseDataToTelems();
                    } else {
                        continue;
                    }
                }
                tENum++;
                telems = tv.telemetryEntries[j].telemsDict[-1]; // -1 is the default id
                if (numTelemetriesSelected <= 1) {
                    telemColor = C0;
                } else {
                    telemColor = Color.Lerp(C0, C1, tENum / (numTelemetriesSelected-1) );
                }
                telemColorTrans = telemColor;
                telemColorTrans.a = 0.25f;

                // Find how many telems to draw
                int numPointsToDraw = tv.telemetryEntries[j].FindTelemAtTime(telemetryTime);
                if (numPointsToDraw < 2) continue;

                Vector3[] points = new Vector3[numPointsToDraw]; //[telems.Length];
                //List<Vector3> grapplePoints = new List<Vector3>();
                //bool grapplePointsTracking = false;
                for (int i = 0; i < numPointsToDraw; i++) { // telems.Length; i++) {
                    telem = telems[i];

                    //// Only draw up to the total telemetryTime
                    //if (telem.time > telemetryTime) {
                    //    break;
                    //}

                    points[i] = telem.position;
                    string tag0 = telem.splitTag[0];
                    
                    // Check for TelemetryCustomEvents. This also allows TCEs to override Jump and Dive here.
                    if ( tv.telemetrySettings.customEventDict.ContainsKey( tag0 ) ) {
                        // Let the custom event handle it
                        tv.telemetrySettings.customEventDict[tag0].Draw( telem );
                    } else {
                        switch ( tag0 ) {
                        case "Jump":
                            Handles.color = Color.white;
                            Handles.ConeHandleCap( 0, telem.position + coneOffset, rotUp, coneSize, EventType.Repaint );
                            break;

                        case "Dive":
                            Handles.color = Color.green;
                            Handles.ConeHandleCap( 0, telem.position, telem.rotation, coneSize, EventType.Repaint );
                            break;

                        default: // Including "_"
                            Handles.color = telemColor;
                            Handles.ConeHandleCap( 0, telem.position, telem.rotation, coneSize, EventType.Repaint );
                            break;


                        /*
                        case "Jump":
                            Handles.color = CJump;
                            Handles.ConeHandleCap( 0, telem.position+coneOffset, rotUp, coneSize, EventType.Repaint );
                            break;
                        case "Grapple":
                            Handles.color = telemColor;
                            if ( telem.splitTag.Length < 2 ) break; // Just don't show it if there's an issue
                            switch (telem.splitTag[1]) {
                            case "Hit":
                                grapplePoints.Clear();
                                Handles.ConeHandleCap( 0, telem.position + camOffset, telem.rotation, coneSize, EventType.Repaint );
                                if ( telem.p0 == Telemetry_Cloud.V3_INVALID ) {
                                    try {
                                        // Get the position of the GrapplerPoint that was grabbed
                                        telem.p0 = Vector3.zero;
                                        telem.p0.x = float.Parse( telem.splitTag[2] );
                                        telem.p0.y = float.Parse( telem.splitTag[3] );
                                        telem.p0.z = float.Parse( telem.splitTag[4] );
                                        Handles.DrawLine( telem.position + camOffset, telem.p0 );
                                        grapplePoints.Add( telem.p0 );
                                        grapplePoints.Add( telem.position + camOffset );
                                        grapplePointsTracking = true;
                                    }
                                    catch { }
                                } else {
                                    Handles.DrawLine( telem.position + camOffset, telem.p0 );
                                }
                                break;
                            case "Miss":
                                Handles.ConeHandleCap( 0, telem.position+camOffset, telem.rotation, coneSize, EventType.Repaint );
                                if (grappleDist == 0) {
                                    try {
                                        grappleDist = float.Parse( telem.splitTag[2] );
                                    }
                                    catch {
                                        grappleDist = 20; // Default
                                    }
                                }
                                if (telem.p0 == Telemetry_Cloud.V3_INVALID ) {
                                    telem.p0 = telem.position + camOffset + ( telem.rotation * ( Vector3.forward * grappleDist ) );
                                }
                                Handles.DrawDottedLine( telem.position + camOffset, telem.p0, 4f );
                                break;
                            case "Release":
                                Handles.CubeHandleCap( 0, telem.position + camOffset, rotDown, cubeSize, EventType.Repaint );
                                if ( telem.p0 == Telemetry_Cloud.V3_INVALID ) {
                                    try {
                                        // Get the position of the GrapplerPoint that was grabbed
                                        telem.p0 = Vector3.zero;
                                        telem.p0.x = float.Parse( telem.splitTag[2] );
                                        telem.p0.y = float.Parse( telem.splitTag[3] );
                                        telem.p0.z = float.Parse( telem.splitTag[4] );
                                        Handles.DrawLine( telem.position + camOffset, telem.p0 );
                                        if (grapplePointsTracking) {
                                            grapplePoints.Add( telem.position + camOffset );
                                            grapplePoints.Add( telem.p0 );

                                            Handles.color = telemColorTrans;
                                            Handles.DrawAAConvexPolygon( grapplePoints.ToArray() );
                                            grapplePointsTracking = false;
                                            grapplePoints.Clear();
                                        }
                                    }
                                    catch { }
                                } else {
                                    Handles.DrawLine( telem.position + camOffset, telem.p0 );
                                }
                                break;
                            }
                            break;
                        default: // Including "_"
                            Handles.color = telemColor;
                            Handles.ConeHandleCap( 0, telem.position, telem.rotation, coneSize, EventType.Repaint );
                            if ( grapplePointsTracking ) grapplePoints.Add( telem.position + camOffset );
                            break;
                        */
                        }
                    }
                }
                Handles.color = telemColor;
                Handles.DrawAAPolyLine(4, points);
            }
        }


        // void HandleCommand() {
        //     switch (tv.chooseCommand) {
        //         case Telemetry_CloudViewer.eCommand.SelectACommand:
        //             return; // Do nothing
        //
        //         case Telemetry_CloudViewer.eCommand.QueryRecs:
        //             tv.chooseCommand = Telemetry_CloudViewer.eCommand.SelectACommand;
        //             // Query all the records that match specific data
        //             WWWForm form = new WWWForm();
        //             form.AddField("op", "ListEntries");
        //             form.AddField("project", tv.telemetrySettings.project);
        //             form.AddField("semester", tv.telemetrySettings.semester);
        //             if ( tv.sceneName != "You Must LoadSceneNames" ) {
        //                 form.AddField( "sceneName", tv.sceneName );
        //             }
        //         //form.AddField( "version", tv.telemetrySettings.version );
        //
        //         if (DEBUG) {
        //                 //Dictionary<string, string> headers = form.headers;
        //                 //System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //                 //foreach (string key in headers.Keys) {
        //                 //    sb.Append( key );
        //                 //    sb.Append( ": " );
        //                 //    sb.AppendLine( headers[key] );
        //                 //}
        //                 //Debug.Log( sb.ToString() );
        //             }
        //
        //             new Telemetry_CloudHelper("http://telemetry.prototools.net/queryTelemetryEntries.php", form, QueryCallback);
        //             break;
        //     }
        // }

        //void QueryCallback(string result) {
        //    Debug.Log(result);
        //    // Parse the results into TelemetryEntries
        //    List<string> lines = new List<string>( result.Split( '\n' ) );
        //    for (int i = 0; i < lines.Count; i++) {
        //        lines[i] = lines[i].Trim();
        //    }
        //    string query;
        //    if (lines[0].Contains("Query:")) {
        //        query = lines[0];
        //        lines.RemoveAt( 0 );
        //    }
        //    if (lines.Count == 0) {
        //        Debug.LogError("Query returned without any entries or Headers\n" + result);
        //        return;
        //    }
        //    if (lines.Count == 1) {
        //        Debug.LogError("Query returned no entries\n" + result);
        //        return;
        //    }

        //    TelemetryEntry.SET_HEADERS(lines[0]);
        //    tv.telemetryEntries.Clear();
        //    for (int i = 1; i < lines.Count; i++) {
        //        TelemetryEntry entry = TelemetryEntry.MAKE(lines[i]);
        //        if (entry != null) tv.telemetryEntries.Add(entry);
        //    }
        //}
    }
#endif

}

// From: https://docs.unity3d.com/2020.3/Documentation/ScriptReference/PropertyDrawer.html
/*
// IngredientDrawer
[CustomPropertyDrawer( typeof( Ingredient ) )]
public class IngredientDrawer : PropertyDrawer {
    // Draw the property inside the given rect
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty( position, label, property );

        // Draw label
        position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), label );

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect( position.x, position.y, 30, position.height );
        var unitRect = new Rect( position.x + 35, position.y, 50, position.height );
        var nameRect = new Rect( position.x + 90, position.y, position.width - 90, position.height );

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField( amountRect, property.FindPropertyRelative( "amount" ), GUIContent.none );
        EditorGUI.PropertyField( unitRect, property.FindPropertyRelative( "unit" ), GUIContent.none );
        EditorGUI.PropertyField( nameRect, property.FindPropertyRelative( "name" ), GUIContent.none );

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
*/