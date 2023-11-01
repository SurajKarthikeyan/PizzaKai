using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace XnBugReport {
    /// <summary>
    /// Provides a simple, modal dialog forkyr2d with up to three buttons as responses.
    /// The buttons and buttonTexts arrays and the UnityEvents on each Button must be set in the Inspector.
    /// </summary>
    public class XnModalPanel : MonoBehaviour {
        static public XnModalPanel S;

        [SerializeField]
        private bool _showing = false;
        [Header( "Inscribed" )]
        public GameObject panel;
        public TMP_Text       textArea;
        public List<Button>   buttons;
        public List<TMP_Text> buttonTexts;

        System.Action<int> _callback;

        // Start is called before the first frame update
        void Start() {
            S = this;
            showing = false;
        }


        public string text {
            get { return textArea.text; }
            set { textArea.text = value; }
        }

        public bool showing {
            get { return _showing; }
            private set {
                _showing = value;
                panel.SetActive( _showing );
                if ( !_showing ) text = "";
            }
        }

        public void ButtonPress( Button bPressed ) {
            // Get the button number
            int num = buttons.IndexOf( bPressed );
            if ( num == -1 ) {
                Debug.LogError(
                    "Button pressed that was not in List<> buttons. You probably need to set up the buttons again." );

                return;
            }

            showing = false;
            _callback?.Invoke( num );
        }

        public void Show( string msg, System.Action<int> callback = null, string b0text = "Ok",
                          string b1text = "", string b2text = "" ) {
            text = msg;
            _callback = callback;
            string[] inStrings = { b0text, b1text, b2text };
            for ( int i = 0; i < buttons.Count; i++ ) {
                if ( inStrings[i] != "" ) {
                    // button 0 is always visible by default with the b0text = "Ok" default parameter
                    buttonTexts[i].text = inStrings[i];
                    buttons[i].gameObject.SetActive( true );
                } else { buttons[i].gameObject.SetActive( false ); }
            }

            showing = true;
        }


        static public void SHOW( string msg, System.Action<int> callback = null,
                                 string b0text = "Ok", string b1text = "", string b2text = "" ) {
            if ( S == null ) {
                Debug.LogError( "Call to XnModalPanel.SHOW() when S was null!" );
                return;
            }

            S.Show( msg, callback, b0text, b1text, b2text );
        }
    }
}