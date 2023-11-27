//#define DEBUG_TEXT

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XnBugReport
{
    /// <summary>
    /// Allows a quick, static check on whether the mouse is over an element of the UI. 
    /// This can be used to prevent clicks on non-uGUI GameObjects in the scene.
    /// This must be attached to the Canvas.
    /// Should also be placed earlier than Default in Script Execution Order
    /// </summary>
    [RequireComponent( typeof(Canvas) )]
    [RequireComponent( typeof(GraphicRaycaster) )]
    public class XnPointerOverUI : MonoBehaviour {
        // TODO: Possibly add ability to ignore certain UI elements, but for now, I'm using the Raycast Target setting for this.
        static public bool             OVER_UI          = false;
        static public List<GameObject> OVER_GameObjects = new List<GameObject>();

        public int overCount = 0;
#if DEBUG_TEXT
    [Multiline(5)]
    public string overStrings;
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
#endif

        GraphicRaycaster m_Raycaster;
        PointerEventData m_PointerEventData;
        EventSystem      m_EventSystem;

        void Start() {
            //Fetch the Raycaster from the GameObject (the Canvas)
            m_Raycaster = GetComponent<GraphicRaycaster>();
            //Fetch the Event System from the Scene. For some reason, this doesn't need a GameObject reference.
            m_EventSystem = GetComponent<EventSystem>();
        }

        void Update() {
            //Set up a new Pointer Event
            m_PointerEventData = new PointerEventData( m_EventSystem );
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast( m_PointerEventData, results );

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            OVER_GameObjects.Clear();
            foreach ( RaycastResult result in results ) {
                OVER_GameObjects.Add( result.gameObject );
                //Debug.Log("Hit " + result.gameObject.name);
                //sb.AppendLine(result.gameObject.name);
            }

            overCount = OVER_GameObjects.Count;
            OVER_UI = overCount > 0;

#if DEBUG_TEXT
        if (OVER_UI) {
            sb.Clear();
            foreach (RaycastResult result in results) {
                sb.AppendLine(result.gameObject.name);
            }
            overStrings = sb.ToString();
        } else {
            overStrings = "";
        }
#endif
        }
    }
}