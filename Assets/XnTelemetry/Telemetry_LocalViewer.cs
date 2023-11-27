//#define ENABLED

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XnTelemetry {
    /// <summary>
    /// The Telemetry_LocalViewer class is included here in case you want to
    /// implement a local solution in the future. It is no longer used, and it
    /// is drastically behind the Telemetry_CloudViewer class.
    /// </summary>
    public class Telemetry_LocalViewer : MonoBehaviour {
#if ENABLED
        public bool showTelemetry = false;
        [Range(0.25f, 4f)]
        public float coneScale = 0.5f;
        public List<TextAsset> telemetryFiles;
        public bool checkToReloadFiles = false;

        public List<Telem[]> telemetries;
#endif
    }

#if ENABLED
#if UNITY_EDITOR
    [CustomEditor(typeof(Telemetry_LocalViewer))]
    public class Telemetry_LocalViewer_Editor : Editor {
        Telemetry_LocalViewer tv;
        static Color C0 = Color.cyan;
        static Color C1 = Color.yellow;
        static Color CJump = Color.white;
        static Color CGrapple = Color.blue;

        void OnEnable() {
            tv = (Telemetry_LocalViewer)target;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }

        public void LoadTelemetries() {
            tv.telemetries = new List<Telem[]>();
            List<Telem> telemList;
            //Telemetry.Telem[] telems;
            string[] lines;
            Telem telem;
            foreach (TextAsset ta in tv.telemetryFiles) {
                lines = ta.text.Split('\n');
                // The first line is the legend, so it is skipped. - JGB
                int num = lines.Length - 1;
                telemList = new List<Telem>();
                for (int i = 1; i < lines.Length; i++) {
                    if (lines[i].Length == 0) continue;
                    telem = Telem.FromString(lines[i]);
                    if (telem.tag == "ERROR") continue;
                    telemList.Add(telem);
                }
                tv.telemetries.Add(telemList.ToArray());
            }
            Debug.Log($"{tv.telemetries.Count} Telemetry files loaded.");
        }

        void OnSceneGUI() {
            if (tv.checkToReloadFiles) {
                tv.checkToReloadFiles = false;
                LoadTelemetries();
            }

            if (!tv.showTelemetry) return;

            Quaternion rotUp = Quaternion.LookRotation(Vector3.up);
            Quaternion rotDown = Quaternion.LookRotation(Vector3.down);
            float coneSize = tv.coneScale;
            float cubeSize = coneSize * 0.5f;
            Vector3 coneOffset = new Vector3(0, -.5f, 0);
            Vector3 camOffset = new Vector3(0, 1.44f, 0); // 1.44 comes from the MainCamera position within __Player_2022
            float grappleDist = 0;

            if (tv.telemetries == null) LoadTelemetries();
            Telem telem;
            Telem[] telems;
            Color telemColor, telemColorTrans;
            for (int j = 0; j < tv.telemetries.Count; j++) {
                telems = tv.telemetries[j];
                if (tv.telemetries.Count <= 1) {
                    telemColor = C0;
                } else {
                    telemColor = Color.Lerp(C0, C1, ((float)j / (tv.telemetries.Count - 1)));
                }
                telemColorTrans = telemColor;
                telemColorTrans.a = 0.25f;
                Vector3[] points = new Vector3[telems.Length];
                List<Vector3> grapplePoints = new List<Vector3>();
                bool grapplePointsTracking = false;
                for (int i = 0; i < telems.Length; i++) {
                    telem = telems[i];
                    points[i] = telem.position;
                    switch (telem.splitTag[0]) {
                        case "Jump":
                            Handles.color = CJump;
                            Handles.ConeHandleCap(0, telem.position + coneOffset, rotUp, coneSize, EventType.Repaint);
                            break;
                        case "Grapple":
                            Handles.color = telemColor;
                            if (telem.splitTag.Length < 2) break; // Just don't show it if there's an issue
                            switch (telem.splitTag[1]) {
                                case "Hit":
                                    grapplePoints.Clear();
                                    Handles.ConeHandleCap(0, telem.position + camOffset, telem.rotation, coneSize, EventType.Repaint);
                                    if (telem.p0 == Telemetry_Local.V3_INVALID) {
                                        try {
                                            // Get the position of the GrapplerPoint that was grabbed
                                            telem.p0 = Vector3.zero;
                                            telem.p0.x = float.Parse(telem.splitTag[2]);
                                            telem.p0.y = float.Parse(telem.splitTag[3]);
                                            telem.p0.z = float.Parse(telem.splitTag[4]);
                                            Handles.DrawLine(telem.position + camOffset, telem.p0);
                                            grapplePoints.Add(telem.p0);
                                            grapplePoints.Add(telem.position + camOffset);
                                            grapplePointsTracking = true;
                                        } catch { }
                                    } else {
                                        Handles.DrawLine(telem.position + camOffset, telem.p0);
                                    }
                                    break;
                                case "Miss":
                                    Handles.ConeHandleCap(0, telem.position + camOffset, telem.rotation, coneSize, EventType.Repaint);
                                    if (grappleDist == 0) {
                                        try {
                                            grappleDist = float.Parse(telem.splitTag[2]);
                                        } catch {
                                            grappleDist = 20; // Default
                                        }
                                    }
                                    if (telem.p0 == Telemetry_Local.V3_INVALID) {
                                        telem.p0 = telem.position + camOffset + (telem.rotation * (Vector3.forward * grappleDist));
                                    }
                                    Handles.DrawDottedLine(telem.position + camOffset, telem.p0, 4f);
                                    break;
                                case "Release":
                                    Handles.CubeHandleCap(0, telem.position + camOffset, rotDown, cubeSize, EventType.Repaint);
                                    if (telem.p0 == Telemetry_Local.V3_INVALID) {
                                        try {
                                            // Get the position of the GrapplerPoint that was grabbed
                                            telem.p0 = Vector3.zero;
                                            telem.p0.x = float.Parse(telem.splitTag[2]);
                                            telem.p0.y = float.Parse(telem.splitTag[3]);
                                            telem.p0.z = float.Parse(telem.splitTag[4]);
                                            Handles.DrawLine(telem.position + camOffset, telem.p0);
                                            if (grapplePointsTracking) {
                                                grapplePoints.Add(telem.position + camOffset);
                                                grapplePoints.Add(telem.p0);

                                                Handles.color = telemColorTrans;
                                                Handles.DrawAAConvexPolygon(grapplePoints.ToArray());
                                                grapplePointsTracking = false;
                                                grapplePoints.Clear();
                                            }
                                        } catch { }
                                    } else {
                                        Handles.DrawLine(telem.position + camOffset, telem.p0);
                                    }
                                    break;
                            }
                            break;
                        default: // Including "_"
                            Handles.color = telemColor;
                            Handles.ConeHandleCap(0, telem.position, telem.rotation, coneSize, EventType.Repaint);
                            if (grapplePointsTracking) grapplePoints.Add(telem.position + camOffset);
                            break;
                    }
                }
                Handles.color = telemColor;
                Handles.DrawAAPolyLine(4, points);
            }
        }
    }
#endif
#endif
}