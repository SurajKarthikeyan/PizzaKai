using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InfoComponent : MonoBehaviour {
	//[HideInInspector]
	//public bool turnOnDefaultInspector = false;
	public Texture2D icon;
	//public float iconMaxWidth = 64f;
	public string title = "InfoComponent Title";
	[TextArea(1,10)]
	public string text = "Set the Inspector to Debug mode to edit this text.";
}



#if UNITY_EDITOR
// Initially based on ProjectInfoEditor.cs, which was originally created by Unity.com
// By Jeremy G. Bond <jeremy@exninja.com>
[CustomEditor(typeof(InfoComponent))]
//[CanEditMultipleObjects]
public class InfoComponentEditor : Editor {
	static bool TURN_ON_DEFAULT_INSPECTORS = false;

	//static float kSpace = 16f;
	InfoComponent info;
	bool showDefaultInspector = false;

	//protected override void OnHeaderGUI() {
	//	var info = (ProjectInfo)target;
	//	Init();

	//	var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

	//	GUILayout.BeginHorizontal("In BigTitle");
	//	{
	//		GUILayout.Label(info.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
	//		string titleString = info.title.Replace("\\n", "\n");
	//		titleString = titleString.Replace("\\t", "\t");
	//		GUILayout.Label(titleString, TitleStyle);
	//	}
	//	GUILayout.EndHorizontal();
	//}


	void OnEnable() {
		info = (InfoComponent)target;
		Init();
	}

	/// <summary>
	/// Adds an "Edit..." item to the context menu for InfoComponent
	/// </summary>
	/// <param name="command"></param>
	[MenuItem("CONTEXT/InfoComponent/Edit InfoComponent...")]
	static void EnableEdit(MenuCommand command) {
		//InfoComponent info = (InfoComponent)command.context;
		TURN_ON_DEFAULT_INSPECTORS = true;
	}

	public override void OnInspectorGUI() {
		if (info == null) {
			info = (InfoComponent)target;
			Init();
		}

		if (TURN_ON_DEFAULT_INSPECTORS) {
			showDefaultInspector = true;
			TURN_ON_DEFAULT_INSPECTORS = false;
		}
		
		var inspectorHeaders = XnTools.InspectorHeadersUtility.GetInternalInspectorTitlesCache();
		inspectorHeaders[typeof(InfoComponent)] = $"{info.title} (InfoComponent)";

		bool altHeld = ( Event.current.modifiers == EventModifiers.Alt );
		if ( showDefaultInspector || altHeld ) {
			showDefaultInspector = EditorGUILayout.ToggleLeft( "Show Default Inspector…", showDefaultInspector );
		    
		}

		//float iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 4f - 20f, info.iconMaxWidth);
		GUILayout.BeginVertical();
		{
			GUILayout.BeginHorizontal();// "In BigTitle");
			{
				if (info.icon != null) {
					GUILayout.Label(info.icon, GUILayout.Width(m_TitleStyle.lineHeight), GUILayout.Height(m_TitleStyle.lineHeight));
				}
				if (!string.IsNullOrEmpty(info.title)) {
					string titleString = info.title.Replace("\\n", "\n").Replace("\\t", "\t");
					GUILayout.Label(titleString, TitleStyle);
				}
			}
			GUILayout.EndHorizontal();

			if (!string.IsNullOrEmpty(info.text)) {
				GUILayout.BeginHorizontal();// "In BigTitle");
				{
					GUILayout.Space(m_TitleStyle.lineHeight / 2);
					string infoString = info.text.Replace("\\n", "\n").Replace("\\t", "\t");
					GUILayout.Label(infoString, BodyStyle);
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndVertical();


		if (showDefaultInspector) {
			GUILayout.Space(10);
			if (GUILayout.Button("Hide Default Inspector")) showDefaultInspector = false;
			DrawDefaultInspector();
		}
		//foreach (var section in info.sections) {
		//	if (!string.IsNullOrEmpty(section.heading)) {
		//		GUILayout.Label(section.heading, HeadingStyle);
		//	}
		//	if (!string.IsNullOrEmpty(section.text)) {
		//		string sTxt = section.text.Replace("\\n", "\n");
		//		sTxt = sTxt.Replace("\\t", "\t");
		//		GUILayout.Label(sTxt, BodyStyle);
		//	}
		//	if (!string.IsNullOrEmpty(section.linkText)) {
		//		if (LinkLabel(new GUIContent(section.linkText))) {
		//			Application.OpenURL(section.url);
		//		}
		//	}
		//	GUILayout.Space(kSpace);
		//}
	}


	bool m_Initialized;

	GUIStyle TitleStyle { get { return m_TitleStyle; } }
	[SerializeField] GUIStyle m_TitleStyle;

	GUIStyle TextAreaStyle { get { return m_TextAreaStyle; } }
	[SerializeField] GUIStyle m_TextAreaStyle;

	GUIStyle LinkStyle { get { return m_LinkStyle; } }
	[SerializeField] GUIStyle m_LinkStyle;

	GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
	[SerializeField] GUIStyle m_HeadingStyle;

	GUIStyle BodyStyle { get { return m_BodyStyle; } }
	[SerializeField] GUIStyle m_BodyStyle;


	void Init() {
		if (m_Initialized) return;

		m_BodyStyle = new GUIStyle(EditorStyles.label);
		m_BodyStyle.wordWrap = true;
		m_BodyStyle.fontSize = 14;
		m_BodyStyle.richText = true;

		m_TitleStyle = new GUIStyle(m_BodyStyle);
		m_TitleStyle.fontSize = 24;
		m_TitleStyle.richText = true;

		m_TextAreaStyle = new GUIStyle(EditorStyles.textArea);
		m_TextAreaStyle.wordWrap = true;
		m_TextAreaStyle.fontSize = 14;
		m_TextAreaStyle.richText = true;

		m_LinkStyle = new GUIStyle(m_BodyStyle);
		m_LinkStyle.wordWrap = false;
		// Match selection color which works nicely for both light and dark skins
		m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
		m_LinkStyle.stretchWidth = false;


		m_HeadingStyle = new GUIStyle(m_BodyStyle);
		m_HeadingStyle.fontSize = 18;


		m_Initialized = true;
	}

	//bool LinkLabel(GUIContent label, params GUILayoutOption[] options) {
	//	var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

	//	Handles.BeginGUI();
	//	Handles.color = LinkStyle.normal.textColor;
	//	Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
	//	Handles.color = Color.white;
	//	Handles.EndGUI();

	//	EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

	//	return GUI.Button(position, label, LinkStyle);
	//}

}
#endif

/* May have finally figured out the height of TextArea issue.
https://answers.unity.com/questions/509725/format-text-area-in-propertydrawer.html
[CustomPropertyDrawer (typeof (Texto))]
public class ScaledCurveDrawer : PropertyDrawer 
{
	float textHeight;
 
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) 
	{
		// Custom style
		GUIStyle myStyle = new GUIStyle (EditorStyles.textArea);
		myStyle.fontSize = 14;
 
		// Our text
		SerializedProperty myTextProperty = prop.FindPropertyRelative ("myTextProperty");
		myTextProperty.stringValue = EditorGUI.TextArea (pos, contenido.stringValue, myStyle);
 
		// Text height
		GUIContent guiContent = new GUIContent (myTextProperty.stringValue);
		textHeight = myStyle.CalcHeight (guiContent, EditorGUIUtility.currentViewWidth);
	}
 
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return textHeight;
	}
}
*/