using UnityEngine;
using UnityEngine.UI;

public class ReportButtonClick : MonoBehaviour
{
    public Text successField;

    public void Clicked() {
        // Generate a report
        WWWForm form = new WWWForm();
        form.AddField( "ButtonClicked", gameObject.name );
        form.AddField( "Time.time", $"{Time.time:0.00}" );
        form.AddField( "udid", SystemInfo.deviceUniqueIdentifier );

        XnAnalytics.POST( form, ReportCallback );
    }


    void ReportCallback( bool success, string note ) {
        Debug.LogWarning( $"ReportCallback: Success: {success} Note: {note}" );
        successField.text = $"{success} {note}";
    }
}
