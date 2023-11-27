#define EXPLICIT_RECTS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XnTelemetry {


    [System.Serializable]
    public class TelemetryEntry {
        static string[] HEADERS;

        public bool show = false;
        public int recNum;
        public string project;
        public string semester;
        public string version;
        public string sceneName;
        public string playerName;
        public string dateTime;
        public string data;
        public Dictionary<int, Telem[]> telemsDict = new Dictionary<int, Telem[]>();
        public Dictionary<int, Color> colorsDict = new Dictionary<int, Color>();
        [HideInInspector]
        public float maxTime = -1f;

        static public void SET_HEADERS( string headerString ) {
            string[] bits = headerString.Split( '\t' );
            for ( int i = 0; i < bits.Length; i++ ) {
                bits[i] = bits[i].Trim();
            }
            HEADERS = bits;
        }

        static public TelemetryEntry MAKE( string str ) {
            TelemetryEntry entry = new TelemetryEntry();

            string[] bits = str.Split( '\t' );
            if ( bits.Length == 0 || bits[0] == "" ) return null;
            if ( HEADERS == null || HEADERS.Length < bits.Length ) {
                Debug.LogError( "HEADERS has not been set properly!\n"
                    + ( ( HEADERS == null ) ? "HEADERS is null" : "[ " + string.Join( "\t", HEADERS ) + " ]" ) );
                return null;
            }
            for ( int i = 0; i < bits.Length; i++ ) {
                bits[i] = bits[i].Trim();
                switch ( HEADERS[i] ) {
                case "recNum":
                    int.TryParse( bits[i], out entry.recNum );
                    break;

                case "project":
                    entry.project = bits[i];
                    break;

                case "semester":
                    entry.semester = bits[i];
                    break;

                case "version":
                    entry.version = bits[i];
                    break;

                case "sceneName":
                    entry.sceneName = bits[i];
                    break;

                case "playerName":
                    entry.playerName = bits[i];
                    break;

                case "dateTime":
                    entry.dateTime = bits[i];
                    break;

                case "data":
                    entry.data = bits[i];
                    break;
                }
            }

            entry.ParseDataToTelems();

            return entry;
        }

        internal void ParseDataToTelems() {
            string[] lines;
            Telem telem;
            data = data.Replace( "\\n", "\n" ).Replace( "\\t", "\t" ).Replace("\\r","").Replace("\r","");
            lines = data.Split( '\n' );
            // The first line is the legend, so it is skipped. - JGB
            int num = lines.Length - 1;
            Dictionary<int, List<Telem>> telemListDict = new Dictionary<int, List<Telem>>();
            colorsDict.Clear();
            for ( int i = 1; i < lines.Length; i++ ) {
                if ( lines[i].Length == 0 ) continue;
                telem = Telem.FromString( lines[i].Trim() );
                if ( telem.tag == "ERROR" ) continue;
                if ( telem.tag == "Color" ) {
                    // This is setting the color of one of the paths
                    Color col = new Color( telem.position.x, telem.position.y, telem.position.z, 1 );
                    colorsDict.Add( telem.id, col );
                    continue; // Once we have the color, ignore this Telem
                }
                if (!telemListDict.ContainsKey(telem.id)) {
                    telemListDict.Add( telem.id, new List<Telem>() );
                }
                telemListDict[telem.id].Add( telem );
                if ( telem.time > maxTime ) maxTime = telem.time;
            }
            // Parse all of the Lists into arrays
            telemsDict.Clear();
            foreach (int id in telemListDict.Keys) {
                telemsDict.Add( id, telemListDict[id].ToArray() );
            }
        }

        internal float GetMaxTime() {
            if ( telemsDict == null || telemsDict.Count == 0 ) ParseDataToTelems();
            // If it's STILL null or Length 0, return 0;
            if ( telemsDict == null || telemsDict.Count == 0 ) return 0;
            // Search all of the Telem[] arrays in telemsDict to find the last time
            float lastTime = 0;
            foreach (int id in telemsDict.Keys) {
                lastTime = Mathf.Max( lastTime, telemsDict[id][telemsDict[id].Length - 1].time );
            }
            return lastTime;
        }

        /// <summary>
        /// This is a simple binary search of the Telem[] in telemsDict[id]
        /// </summary>
        /// <param name="time">The playback time in seconds</param>
        /// <param name="id">The ID of the character (key for telemsDict)</param>
        /// <returns></returns>
        internal int FindTelemAtTime( float time, int id = -1 ) {
            if ( telemsDict == null || telemsDict.Count == 0 ) return 0;
            if ( !telemsDict.ContainsKey( id ) ) return 0; // If this id is invalid, return 0

            // Always return at least 2 if possible
            if ( time == 0 && telemsDict[id].Length > 1 ) return 2;

            int nL = 0;
            int nR = telemsDict[id].Length - 1;
            int nM;

            while ( nL <= nR ) {
                nM = ( nL + nR ) / 2;
                switch ( CompareTelemToTime( nM, time, id ) ) {
                case -1: nL = nM + 1; break;
                case 0:
                    // Always return at least 2 if possible
                    if ( nM < 2 && telemsDict[id].Length > 1 ) return 2;
                    return nM;
                case 1: nR = nM - 1; break;
                }
            }
            return -1;
        }

        int CompareTelemToTime( int ndx, float time, int id ) {
            if ( telemsDict == null || telemsDict.Count == 0 ) return 0;
            if ( !telemsDict.ContainsKey( id ) ) return 0; // If this id is invalid, return 0

            if ( telemsDict[id][ndx].time > time ) return 1;
            if ( ndx == telemsDict[id].Length - 1 ) return 0; // If this is the last element, and its time is < time, then this is the one we're looking for.
            // The following case should work for multiplayer where there are several Telems with the same time - JGB 2022-10-24
            if ( telemsDict[id][ndx + 1].time < time ) return -1;
            //if (telems[ndx].time <= time && (ndx == telems.Length - 1 || telems[ndx + 1].time >= time)) return 0;
            return 0;
        }

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer( typeof( TelemetryEntry ) )]
    public class TelemetryEntry_Drawer : PropertyDrawer {
        SerializedProperty m_show, m_recNum, m_playerName, m_dateTime;

        // Draw the property inside the given rect
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
            // Init the SerializedProperty fields
            //if ( m_show == null ) m_show = property.FindPropertyRelative( "show" );
            //if ( m_recNum == null ) m_recNum = property.FindPropertyRelative( "recNum" );
            //if ( m_playerName == null ) m_playerName = property.FindPropertyRelative( "playerName" );
            //if ( m_dateTime == null ) m_dateTime = property.FindPropertyRelative( "dateTime" );
            m_show = property.FindPropertyRelative( "show" );
            m_recNum = property.FindPropertyRelative( "recNum" );
            m_playerName = property.FindPropertyRelative( "playerName" );
            m_dateTime = property.FindPropertyRelative( "dateTime" );

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty( position, label, property );

            // Draw label
            position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), GUIContent.none );// label );

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

#if EXPLICIT_RECTS
            // Calculate rects
            float left = 0, spacing = 4;
            Rect showR = new Rect( position.x + left, position.y, position.height, position.height );
            left += showR.width + spacing;
            //Rect recNumR = new Rect( position.x + left, position.y, 40, position.height );
            //left += recNumR.width + spacing;
            Rect labelR = new Rect( position.x + left, position.y, position.width - left, position.height );
            //Rect playerName = new Rect( position.x+70, position.y, position.width-70, position.height );;

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField( showR, m_show, GUIContent.none );
            EditorGUI.LabelField( labelR, $"{m_recNum.intValue}\t{m_playerName.stringValue}\t{m_dateTime.stringValue}" );
            //EditorGUI.LabelField(recNumR, m_recNum.intValue.ToString());
            //EditorGUI.LabelField(labelR, m_playerName.stringValue
            //    + "\t" + m_dateTime.stringValue);
            //EditorGUI.PropertyField( recNumR, property.FindPropertyRelative( "recNum" ), GUIContent.none );
            //EditorGUI.PropertyField( playerName, property.FindPropertyRelative( "playerName" ), GUIContent.none );
#else
            EditorGUILayout.PropertyField(m_show, GUIContent.none);
            EditorGUILayout.LabelField(m_recNum.intValue.ToString());
            EditorGUILayout.LabelField(m_playerName.stringValue + "\t" + m_dateTime.stringValue);
#endif

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
#endif



    #region TelemetryEntry.Telem
    //[System.Serializable] // Making this Serializable broke EVERYTHING! Unity completely froze with a beachball. - JGB 2022-10-22
    public struct Telem {
        const char TAG_SPLIT_CHAR = ' ';

        static public Vector3 V3_INVALID = new Vector3( -999, -999, -999 );

        public int id;
        public float time;
        public Vector3 position;
        public Quaternion rotation;
        public string tag;
        public string[] splitTag;
        // These are extra points that were used by the Telemetry_LocalViewer to draw the Grappler line in the grappler project.
        //public Vector3 p0, p1, p2; // You can uncomment them if you wish to use them in your game. - JGB 2022-10-24

        /// <summary>
        /// The optional _id parameter of the Telem constructor is for multiplayer games. - JGB 2022-10-24
        /// </summary>
        public Telem( float t, Vector3 pos, Quaternion rot, string tg = "", int _id=-1 ) {
            this.id = _id;
            this.time = t;
            this.position = pos;
            this.rotation = rot;
            this.tag = tg;
            this.splitTag = tg.Split( TAG_SPLIT_CHAR );
            //p0 = p1 = p2 = V3_INVALID; // Give these a base value that can be replaced in the Telemetry_LocalViewer
        }

        override public string ToString() {
            Vector3 rot = rotation.eulerAngles;
            if (id != -1) {
                return $"{tag}\t{time:0.00}\t{position.x:0.00}\t{position.y:0.00}\t{position.z:0.00}\t{rot.x:0.00}\t{rot.y:0.00}\t{rot.z:0.00}\t{id}";
            }
            return $"{tag}\t{time:0.00}\t{position.x:0.00}\t{position.y:0.00}\t{position.z:0.00}\t{rot.x:0.00}\t{rot.y:0.00}\t{rot.z:0.00}";
        }

        static public Telem FromString( string s ) {
            int id = -1;

            string[] bits = s.Trim().Split( '\t' );
            switch (bits.Length) {
            case 5:
            case 8:
                break; // It's the right number of bits. No action needed.
            case 9: // Parse the id
                try {
                    id = int.Parse( bits[8] );
                } catch {
                    Debug.LogError( $"Telem id did not parse correctly.\n{s}" );
                    return new Telem( -1, Vector3.zero, Quaternion.identity, "ERROR" );
                }
                break;
            default:
                Debug.LogError( $"Telem string parsed into wrong number of bits: {bits.Length}.\n{s}" );
                return new Telem( -1, Vector3.zero, Quaternion.identity, "ERROR" );
            }
            string tag = bits[0];
            float time;
            Vector3 pos = Vector3.zero;
            Vector3 rot = Vector3.zero;
            try {
                time = float.Parse( bits[1] );
                // NOTE: For "Color" tags, this will parse the r, g, & b values into pos.x, y, & z respectively. - JGB 2022-10-25
                pos.x = float.Parse( bits[2] );
                pos.y = float.Parse( bits[3] );
                pos.z = float.Parse( bits[4] );
            }
            catch {
                Debug.LogError( $"Telem floats did not parse correctly.\n{s}" );
                return new Telem( -1, Vector3.zero, Quaternion.identity, "ERROR" );
            }
            try {
                rot.x = float.Parse( bits[5] );
                rot.y = float.Parse( bits[6] );
                rot.z = float.Parse( bits[7] );
            }
            catch {
                // Don't actually need to do anything here, just ignore missing rotation. - JGB
            }
            // NOTE: id only works if there are rotation bits as well. - JGB 2022-10-24
            return new Telem( time, pos, Quaternion.Euler( rot ), tag, id );
        }
    }
    #endregion

}
