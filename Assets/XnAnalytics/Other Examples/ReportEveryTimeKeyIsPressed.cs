using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReportEveryTimeKeyIsPressed : MonoBehaviour {
    public KeyCode keyCode;

    void Update() {
        if ( Input.GetKeyDown( keyCode ) ) {
            // Generate a report
            WWWForm form = new WWWForm();
            form.AddField( "keyCode", keyCode.ToString() );
            form.AddField( "Time.time", $"{Time.time:0.00}" );
            form.AddField( "udid", SystemInfo.deviceUniqueIdentifier );

            XnAnalytics.POST( form, ReportCallback );
        }
    }


    void ReportCallback(bool success, string note) {
        Debug.LogWarning( $"ReportCallback: Success: {success} Note: {note}" );
    }
}
