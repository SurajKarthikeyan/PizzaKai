//#define ENABLED

using System.Collections.Generic;
using UnityEngine;



namespace XnTelemetry
{
    /// <summary>
    /// The Telemetry_Local class is included here in case you want to implement
    /// a local solution in the future. It is no longer used.
    /// </summary>
    public class Telemetry_Local : MonoBehaviour {
        const float LOG_INTERVAL = 0.25f;

        static Telemetry_Local S;
        static List<Telem> ENTRIES = new List<Telem>();
        static float TIME_START;
        static public Vector3 V3_INVALID = new Vector3(-999, -999, -999);
#if ENABLED

        [Tooltip("If you wish the rotation to be based on a Transform other than the this one, put it here.")]
        public Transform rotationTransform = null;

        // Start is called before the first frame update
        void Start() {
            S = this;
            Reset();
            Log("Start");
            StartCoroutine(LogEveryInterval());
        }

        public void Log(string tag = "_") {
            if (rotationTransform == null) {
                ENTRIES.Add(new Telem(elapsed, transform.position, transform.rotation, tag));
            } else {
                ENTRIES.Add(new Telem(elapsed, transform.position, rotationTransform.rotation, tag));
            }
        }

        IEnumerator LogEveryInterval() {
            while (true) {
                Log();
                yield return new WaitForSeconds(LOG_INTERVAL);
            }
        }

        // Update is called once per frame
        void Update() {

        }

        public void Reset() {
            StopAllCoroutines();
            TIME_START = Time.time;
            ENTRIES.Clear();
        }

        public float elapsed { get { return Time.time - TIME_START; } }

        void OnDestroy() {
            SaveLocal();
        }

        void SaveLocal(string fileName = "") {
            string log = EntriesToString();
            Debug.Log(log);
            if (fileName == "") fileName = "Telemetry_";
            if (StudentLevelSelector.INITED) {
                fileName += StudentLevelSelector.SCENE_NAME + "_";
                fileName += StudentLevelSelector.STUDENT_NAME.Replace(" ", "") + "_";
                //if ( StudentLevelSelector.SCENE_NAME != "" ) {
                //    fileName += StudentLevelSelector.SCENE_NAME + "_";
                //} else {
                //    fileName += UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_";
                //}
                //if ( StudentLevelSelector.STUDENT_NAME != "" ) {
                //    string sn = StudentLevelSelector.STUDENT_NAME.Replace( " ", "" );
                //    fileName += sn + "_";
                //}
            } else {
                fileName += UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_";
                fileName += "[Enter Your Name]_";
            }
            fileName += System.DateTime.Now.ToDateTimeStamp();
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            path += "/" + fileName + ".txt";
            File.WriteAllText(path, log);
            Debug.Log("Wrote telemetry to: " + path);
        }

        static public string EntriesToString() {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Tag\tTime\tx\ty\tz\trX\trY\trZ");
            for (int i = 0; i < ENTRIES.Count; i++) {
                sb.AppendLine(ENTRIES[i].ToString());
            }
            return sb.ToString();
        }

        static public void LOG(string tag) {
            if (S != null) S.Log(tag);
        }


#endif
    }
}