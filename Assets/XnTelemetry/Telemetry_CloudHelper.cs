using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XnTelemetry {
    /// <summary>
    /// This manages asynchronous communication with the php-based Cloud server
    /// </summary>
    public class Telemetry_CloudHelper {
        static protected bool DEBUG_FORM_DATA = false;

        static public List<Telemetry_CloudHelper> TCH_List = new List<Telemetry_CloudHelper>();

        const bool ALWAYS_DEBUG = true;

        public UnityWebRequest uwReq;
        public string result = "";

        public string url;
        public WWWForm form;

        public Telemetry_CloudHelper(string _url, WWWForm _form, System.Action<string> callback = null) {
            form = _form;
            url = _url;
            if (ALWAYS_DEBUG) callback += DebugResult;
            if (callback == null) callback = DebugResult;
            BackgroundRequest(URL_Request(_url, _form), callback);
            TCH_List.Add(this);
        }

        public Telemetry_CloudHelper() {
        }


        public void BackgroundRequest( IEnumerator task, System.Action<string> callback = null ) {
#if UNITY_EDITOR
            EditorApplication.CallbackFunction closureCallback = null;

            closureCallback = () => {
                try {
                    if (task.MoveNext() == false) {
                        EditorApplication.update -= closureCallback;
                        TCH_List.Remove(this);
                        if (callback != null) callback(result);
                    }
                } catch (System.Exception ex) {
                    EditorApplication.update -= closureCallback;
                    TCH_List.Remove(this);
                    if (callback != null) callback(result);
                    Debug.LogException(ex);
                }
            };
            EditorApplication.update += closureCallback;
            //TODO: Add a BackgroundRequest that will work in builds!! - JGB 2022-10-19
#endif
        }

        void DebugResult(string s) {
            if ( DEBUG_FORM_DATA ) Debug.Log( $"Post URL: {url}\n\nForm Data:\n" + System.Text.ASCIIEncoding.ASCII.GetString( form.data ) );
            if ( s == "1" ) {
                Debug.Log( "Post of Telemetry data to Cloud was successful!" );
            } else {
                Debug.Log( $"Post Result:\n{s}" );
            }
        }

        //private void ClosureCallback() {
        //    try {
        //        if ( task.MoveNext() == false ) {
        //            if ( callback != null ) callback( uwReq.);
        //            EditorApplication.update -= closureCallback;
        //        }
        //    }
        //    catch ( System.Exception ex ) {
        //        if ( callback != null ) callback();
        //        Debug.LogException( ex );
        //        EditorApplication.update -= closureCallback;
        //    }

        //}

        private IEnumerator URL_Request(string url, WWWForm form) {
            using (uwReq = UnityWebRequest.Post(url, form)) {
                yield return uwReq.SendWebRequest();

                while (uwReq.isDone == false)
                    yield return null;

                result = uwReq.downloadHandler.text;
                //Debug.Log(uwReq.downloadHandler.text);
            }
        }

        /*
        static public void POST( string url, WWWForm form ) {
            StartCoroutine()
        }

        static IEnumerator POST_CR( string url, WWWForm form ) {
            using ( UnityWebRequest www = UnityWebRequest.Post( url, form ) ) {
                yield return www.SendWebRequest();

                if ( www.result != UnityWebRequest.Result.Success ) {
                    Debug.Log( www.error );
                } else {
                    Debug.Log( "Form upload complete!" );
                }
            }
        }
        */
    }
}


/*
 *

// From https://forum.unity.com/threads/using-unitywebrequest-in-editor-tools.397466/
public static Utility
{
   public static void       StartBackgroundTask(IEnumerator update, Action end = null)
   {
       EditorApplication.CallbackFunction   closureCallback = null;
 
       closureCallback = () =>
       {
           try
           {
               if (update.MoveNext() == false)
               {
                   if (end != null)
                       end();
                   EditorApplication.update -= closureCallback;
               }
           }
           catch (Exception ex)
           {
               if (end != null)
                   end();
               Debug.LogException(ex);
               EditorApplication.update -= closureCallback;
           }
       };
 
       EditorApplication.update += closureCallback;
   }
}

private IEnumerator   Test()
{
   using (UnityWebRequest w = UnityWebRequest.Get("https://www.google.com"))
   {
       yield return w.SendWebRequest();
 
       while (w.isDone == false)
           yield return null;
 
       Debug.Log(w.downloadHandler.text);
   }
}

Usage: Utility.StartBackgroundTask(Test());

*/