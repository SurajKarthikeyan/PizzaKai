using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace XnTelemetry
{
    public class Telemetry_CloudHelper_MonoBehaviour : MonoBehaviour {
        static public Telemetry_CloudHelper_MonoBehaviour _S;
        static public bool ACTIVE = false;

        const bool ALWAYS_DEBUG = true;

        public UnityWebRequest uwReq;
        public string result = "";

        public string url;
        public WWWForm form;

        static public void POST( string _url, WWWForm _form, System.Action<string> callback = null ) {
            if ( !ACTIVE ) {
                Debug.LogError( "Telemetry_CloudHelper_MonoBehaviour called before it was ACTIVE" );
                return;
            }
            _S.Post( _url, _form, callback );
        }

        private void Post( string _url, WWWForm _form, System.Action<string> callback = null ) {
            StartCoroutine( URL_Request( _url, _form, callback ) );
        }


        private IEnumerator URL_Request( string url, WWWForm form, System.Action<string> callback = null ) {
            using ( uwReq = UnityWebRequest.Post( url, form ) ) {
                yield return uwReq.SendWebRequest();

                while ( uwReq.isDone == false )
                    yield return null;

                result = uwReq.downloadHandler.text;
                callback?.Invoke( result );
                Debug.Log( uwReq.downloadHandler.text );
            }
        }

        private void Awake() {
            _S = this;
            ACTIVE = true;
        }

    }
}