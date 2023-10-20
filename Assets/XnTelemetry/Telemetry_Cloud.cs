using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;


namespace XnTelemetry {
    public class Telemetry_Cloud : MonoBehaviour {
        static public float LOG_INTERVAL = 0.25f;
        static protected bool DEBUG_FORM_DATA = false;

        static Telemetry_Cloud S;
        static float TIME_START;
        static public Vector3 V3_INVALID = new Vector3(-999, -999, -999);

        public bool recordTelemetry = true;

        [NaughtyAttributes.Expandable]
        public SO_TelemetrySettings telemetrySettings;
        [Tooltip("If you wish the rotation to be based on a Transform other than the this one, put it here.")]
        public Transform rotationTransform = null;

        protected List<Telem> telems = new List<Telem>();

        // Start is called before the first frame update
        void Start() {
            if (!recordTelemetry) return;
            S = this;
            Reset();
            Log("Start");
            StartCoroutine(LogEveryInterval());
        }

        public virtual void Log(string tag = "_", Transform trans = null ) {
            // trans is completely ignored here. It only exists to allow the override to use it.
            if (rotationTransform == null) {
                telems.Add(new Telem(elapsed, transform.position, transform.rotation, tag));
            } else {
                telems.Add(new Telem(elapsed, transform.position, rotationTransform.rotation, tag));
            }
        }

        public virtual void CustomLog(string tag)
        {
            Telem telem = new Telem(elapsed, transform.position, transform.rotation, tag);
            if (telemetrySettings.customEventDict.ContainsKey(tag))
            {
                telems.Add(telem);
                print( tag + " Drawn");
            }
                
        }

        IEnumerator LogEveryInterval() {
            while (true) {
                Log();
                yield return new WaitForSeconds( Telemetry_Cloud.LOG_INTERVAL );
            }
        }

        public void Reset() {
            StopAllCoroutines();
            TIME_START = Time.time;
            telems.Clear();
        }

        public float elapsed { get { return Time.time - TIME_START; } }

        protected void OnDestroy() {
            if (!recordTelemetry) return;


            if (telemetrySettings.sceneName == "") {
                telemetrySettings.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            }

            if (Application.isEditor) {
                if ( (telemetrySettings.editorSaveTo & SO_TelemetrySettings.eSaveOptions.Cloud ) == SO_TelemetrySettings.eSaveOptions.Cloud ) {
                    SaveToCloud();
                }
                if ( ( telemetrySettings.editorSaveTo & SO_TelemetrySettings.eSaveOptions.Local ) == SO_TelemetrySettings.eSaveOptions.Local ) {
                    SaveToLocal();
                }
            } else {
                if ( ( telemetrySettings.playerSaveTo & SO_TelemetrySettings.eSaveOptions.Cloud ) == SO_TelemetrySettings.eSaveOptions.Cloud ) {
                    SaveToCloud();
                }
                if ( ( telemetrySettings.playerSaveTo & SO_TelemetrySettings.eSaveOptions.Local ) == SO_TelemetrySettings.eSaveOptions.Local ) {
                    SaveToLocal();
                }
            }
        }


        protected void SaveToCloud() {
            WWWForm form = new WWWForm();
            form.AddField("project", telemetrySettings.project);
            form.AddField("semester", telemetrySettings.semester);
            form.AddField("version", telemetrySettings.version);
            form.AddField("sceneName", telemetrySettings.sceneName);
            form.AddField("playerName", telemetrySettings.playerName);

            form.AddField("data", EntriesToString());

            if ( DEBUG_FORM_DATA ) Debug.Log( "Form data\n" + System.Text.ASCIIEncoding.ASCII.GetString( form.data ) );

            // Post to WWW
            if (Application.isEditor) {
                // In editor, the Telemetry_CloudHelper can make a post after playback is halted
                new Telemetry_CloudHelper( telemetrySettings.serverURLPost, form);
            } else {
                // In runtime, the Telemetry_CloudHelper_MonoBehaviour can be called when returning to the main menu or completing a level
                // However, nothing will post data if Alt-F4 is pressed
                Telemetry_CloudHelper_MonoBehaviour.POST( telemetrySettings.serverURLPost, form );
            }
            //StartCoroutine( POST_CR( "http://telemetry.prototools.net/postTelemetry.php", form ) );
        }


        protected void SaveToLocal( string fileName = "" ) {
            string log = EntriesToString();
            Debug.Log( log );
            if ( fileName == "" ) {
                string sceneName = ( telemetrySettings.sceneName == "" )
                    ? UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
                    : telemetrySettings.sceneName;
                fileName = $"Telemetry_{sceneName}_{telemetrySettings.playerName}_";
            }
            fileName += System.DateTime.Now.ToDateTimeStamp();
            string path = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Desktop );
            path += "/" + fileName + ".txt";
            File.WriteAllText( path, log );
            Debug.Log( "Wrote telemetry to: " + path );
        }

        //IEnumerator POST_CR(string url, WWWForm form) {
        //    using ( UnityWebRequest www = UnityWebRequest.Post( url, form ) ) {
        //        yield return www.SendWebRequest();

        //        if ( www.result != UnityWebRequest.Result.Success ) {
        //            Debug.Log( www.error );
        //        } else {
        //            Debug.Log( "Form upload complete!" );
        //        }
        //    }
        //}

        /*
        /// <summary>
        /// From https://stackoverflow.com/questions/38109658/unity-post-request-using-www-class-using-json
        /// </summary>
        /// <returns></returns>
        public WWW POST(string jsonStr) {
            WWW www;
            Hashtable postHeader = new Hashtable();
            postHeader.Add( "Content-Type", "application/json" );

            // convert json string to byte
            var formData = System.Text.Encoding.UTF8.GetBytes( jsonStr );

            www = new WWW( POSTAddUserURL, formData, postHeader );
            StartCoroutine( WaitForRequest( www ) );
            return www;
        }

        IEnumerator WaitForRequest( WWW data ) {
            yield return data; // Wait until the download is done
            if ( data.error != null ) {
                Debug.LogError( "There was an error sending Telemetry_Local POST request: " + data.error );
            } else {
                Debug.Log( "Telemetry_Local POST Request: " + data.text );
            }
        }
        */

        public virtual string EntriesToString() {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Tag\tTime\tx\ty\tz\trX\trY\trZ");
            for (int i = 0; i < telems.Count; i++) {
                sb.AppendLine(telems[i].ToString());
            }
            return sb.ToString();
        }

        static public void LOG(string tag) {
            if (S != null) S.Log(tag);
        }

        static public void CUSTOMEVENTLOG(string tag)
        {
            if(S != null) S.CustomLog(tag);
        }



    }
}
