using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

namespace XnBugReport {
    public class BugReportPanel : MonoBehaviour {
        static public bool DEBUG = true;

        public TMP_Dropdown   dropType;
        public TMP_InputField inName, inEmail, inComment;

        XnTogglePosition toggle;

        private void Awake() {
            toggle = GetComponent<XnTogglePosition>();
        }

        public void CancelButton() {
            if ( DEBUG ) Debug.Log( $"Bug report CancelButton()" );
            toggle.Toggle();
        }

        public void SubmitButton() {
            if ( DEBUG ) Debug.Log( $"Bug report SubmitButton()" );
            SubmitFeedback();
        }

        void SubmitFeedback() {
            WWWForm wForm = new WWWForm();
            wForm.AddField( "userName", inName.text );
            wForm.AddField( "userEmail", inEmail.text );
            wForm.AddField( "comment", inComment.text );
            string reportType = dropType.options[dropType.value].text;
            wForm.AddField( "reportType", reportType );
            wForm.AddField( "version", Application.version );
            //if (WordFlower.PUZZLE != null) {
            //    wForm.AddField("puzzleSeed", WordFlower.PUZZLE.puzzleSeed.ToString());
            //    string puzzleJson = JsonUtility.ToJson(WordFlower.PUZZLE);
            //    wForm.AddField("puzzleData", puzzleJson);
            //}

            XnAnalytics.POST( wForm, SubmitFeedbackCallback );
            toggle.Toggle( XnTogglePosition.eState.pos0 );

            XnModalPanel.SHOW( "Thank you.\n\nSending your report to the server...", null,
                "" );// "" is third argument to not show any buttons.
        }

        void SubmitFeedbackCallback( bool success, string note ) {
            inComment.text = "";
            if ( success ) {
                //inComment.text = $"Success!\n\nThank you for submitting your comment.\n\n{note}";
                XnModalPanel.SHOW( $"Success!\n\nThank you for submitting your comment." );
            } else {
                //inComment.text = $"I'm sorry, but the comment didn't make it to our servers. Please submit it again later.\n\n{note}";
                XnModalPanel.SHOW(
                    $"I'm sorry, but the comment didn't make it to our servers. Please submit it again later." );
            }
        }

    }
}
