// Original file was created by Unity.com
// Small modifications to allow \n \t in text by Jeremy G. Bond <jeremy@exninja.com>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
#endif

namespace XnTools {
	[CustomEditor( typeof(ProjectInfo_SO) )]
	[InitializeOnLoad]
	public class ProjectInfoEditor : Editor {
		const string ProjectMenuHeader = "Tools";

		static string kShowedProjectInfoSessionStateName = "ProjectInfoEditor.showedProjectInfo";

		static float kSpace = 16f;

		static ProjectInfoEditor() {
			EditorApplication.delayCall += SelectProjectInfoAutomatically;
		}

		static void SelectProjectInfoAutomatically() {
			if ( !SessionState.GetBool( kShowedProjectInfoSessionStateName, false ) ) {
				var pInfo = SelectProjectInfo();
				SessionState.SetBool( kShowedProjectInfoSessionStateName, true );

				//if ( pInfo && !pInfo.loadedLayout ) {
				//	// LoadLayout();
				//	pInfo.loadedLayout = true;
				//}
			}
		}

		// static void LoadLayout() {
		// 	var assembly = typeof( EditorApplication ).Assembly;
		// 	var windowLayoutType = assembly.GetType( "UnityEditor.WindowLayout", true );
		// 	var method = windowLayoutType.GetMethod( "LoadWindowLayout", BindingFlags.Public | BindingFlags.Static );
		// 	method.Invoke( null, new object[] { Path.Combine( Application.dataPath, "TutorialInfo/Layout.wlt" ), false } );
		// }

		[MenuItem( ProjectMenuHeader + "/Show Project Info", false, 1 )]
		static ProjectInfo_SO SelectProjectInfo() {
			var ids = AssetDatabase.FindAssets( "t:ProjectInfo_SO" );
			if ( ids.Length == 1 ) {
				var pInfoObject =
					AssetDatabase.LoadMainAssetAtPath( AssetDatabase.GUIDToAssetPath( ids[0] ) );

				Selection.objects = new UnityEngine.Object[] { pInfoObject };

				return (ProjectInfo_SO)pInfoObject;
			} else if ( ids.Length == 0 ) {
				Debug.Log( "Couldn't find a ProjectInfo" );
				return null;
			} else {
				Debug.Log( "Found more than 1 ProjectInfo file" );
				return null;
			}
		}

		/// <summary>
		/// Adds an "Edit..." item to the context menu for InfoComponent
		/// </summary>
		/// <param name="command"></param>
		[MenuItem( "CONTEXT/ProjectInfo/Edit Project Info..." )]
		static void EnableEdit( MenuCommand command ) {
			ProjectInfo_SO info = (ProjectInfo_SO)command.context;
			info.showDefaultInspector = !info.showDefaultInspector;
		}

		protected override void OnHeaderGUI() {
			var pInfo = (ProjectInfo_SO)target;
			Init();

			var iconWidth = Mathf.Min( EditorGUIUtility.currentViewWidth / 3f - 20f,
				pInfo.iconMaxWidth );

			GUILayout.BeginHorizontal( "In BigTitle" );
			{
				GUILayout.Label( pInfo.icon, GUILayout.Width( iconWidth ),
					GUILayout.Height( iconWidth ) );

				if ( pInfo.title != null ) {
					string titleString = pInfo.title.Replace( "\\n", "\n" );
					titleString = titleString.Replace( "\\t", "\t" );
					GUILayout.Label( titleString, TitleStyle );
				} else { GUILayout.Label( "You must set this title", TitleStyle ); }
			}

			GUILayout.EndHorizontal();
		}

		public override void OnInspectorGUI() {
			var pInfo = (ProjectInfo_SO)target;
			Init();

			bool altHeld = ( Event.current.modifiers == EventModifiers.Alt );
			if ( pInfo.showDefaultInspector || altHeld ) {
				pInfo.showDefaultInspector = EditorGUILayout.ToggleLeft( "Show Default Inspector…",
					pInfo.showDefaultInspector );
			}

			if ( pInfo.sections != null ) {
				foreach ( var section in pInfo.sections ) {
					if ( !string.IsNullOrEmpty( section.heading ) ) {
						GUILayout.Label( section.heading, HeadingStyle );
					}

					if ( !string.IsNullOrEmpty( section.text ) ) {
						string sTxt = section.text.Replace( "\\n", "\n" );
						sTxt = sTxt.Replace( "\\t", "\t" );
						GUILayout.Label( sTxt, BodyStyle );
					}

					if ( !string.IsNullOrEmpty( section.linkText ) ) {
						if ( LinkLabel( new GUIContent( section.linkText ) ) ) {
							Application.OpenURL( section.url );
						}
					}

					GUILayout.Space( kSpace );
				}
			}

			if ( pInfo.showDefaultInspector ) {
				GUILayout.Space( 10 );
				if ( GUILayout.Button( "Hide Default Inspector" ) )
					pInfo.showDefaultInspector = false;

				DrawDefaultInspector();
			}
		}


		bool m_Initialized;

		GUIStyle LinkStyle {
			get { return m_LinkStyle; }
		}

		[SerializeField]
		GUIStyle m_LinkStyle;

		GUIStyle TitleStyle {
			get { return m_TitleStyle; }
		}

		[SerializeField]
		GUIStyle m_TitleStyle;

		GUIStyle HeadingStyle {
			get { return m_HeadingStyle; }
		}

		[SerializeField]
		GUIStyle m_HeadingStyle;

		GUIStyle BodyStyle {
			get { return m_BodyStyle; }
		}

		[SerializeField]
		GUIStyle m_BodyStyle;

		void Init() {
			if ( m_Initialized )
				return;

			m_BodyStyle = new GUIStyle( EditorStyles.label );
			m_BodyStyle.wordWrap = true;
			m_BodyStyle.fontSize = 14;
			m_BodyStyle.richText = true;

			m_TitleStyle = new GUIStyle( m_BodyStyle );
			m_TitleStyle.fontSize = 26;
			m_TitleStyle.alignment = TextAnchor.MiddleCenter;

			m_HeadingStyle = new GUIStyle( m_BodyStyle );
			m_HeadingStyle.fontSize = 18;

			m_LinkStyle = new GUIStyle( m_BodyStyle );
			m_LinkStyle.wordWrap = false;
			// Match selection color which works nicely for both light and dark skins
			m_LinkStyle.normal.textColor = new Color( 0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f );
			m_LinkStyle.stretchWidth = false;

			m_Initialized = true;
		}

		bool LinkLabel( GUIContent label, params GUILayoutOption[] options ) {
			var position = GUILayoutUtility.GetRect( label, LinkStyle, options );

			Handles.BeginGUI();
			Handles.color = LinkStyle.normal.textColor;
			Handles.DrawLine( new Vector3( position.xMin, position.yMax ),
				new Vector3( position.xMax, position.yMax ) );

			Handles.color = Color.white;
			Handles.EndGUI();

			EditorGUIUtility.AddCursorRect( position, MouseCursor.Link );

			return GUI.Button( position, label, LinkStyle );
		}
	}
}

/*


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;

[CustomEditor(typeof(ReadMe))]
[InitializeOnLoad]
public class ReadMeEditor : Editor {
	
	static string kShowedReadMeSessionStateName = "ReadMeEditor.showedReadMe";
	
	static float kSpace = 16f;
	
	static ReadMeEditor()
	{
		EditorApplication.delayCall += SelectReadMeAutomatically;
	}
	
	static void SelectReadMeAutomatically()
	{
		if (!SessionState.GetBool(kShowedReadMeSessionStateName, false ))
		{
			var pInfo = SelectReadMe();
			SessionState.SetBool(kShowedReadMeSessionStateName, true);
			
			if (pInfo && !pInfo.loadedLayout)
			{
				LoadLayout();
				pInfo.loadedLayout = true;
			}
		} 
	}
	
	static void LoadLayout()
	{
		EditorUtility.LoadWindowLayout(Path.Combine(Application.dataPath, "Utilities/»ReadMe/ReadMe.wlt"));
	}
	
	[MenuItem("Tutorial/ReadMe")]
	static ReadMe SelectReadMe() 
	{
		var ids = AssetDatabase.FindAssets("ReadMe t:ReadMe");
		if (ids.Length == 1)
		{
			var pInfoObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
			
			Selection.objects = new UnityEngine.Object[]{pInfoObject};
			
			return (ReadMe)pInfoObject;
		}
		else
		{
			Debug.Log("Couldn't find a pInfo");
			return null;
		}
	}
	
	protected override void OnHeaderGUI()
	{
		var pInfo = (ReadMe)target;
		Init();
		
		var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth/3f - 20f, pInfo.iconMaxWidth);
		
		GUILayout.BeginHorizontal("In BigTitle");
		{
			GUILayout.Label(pInfo.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
			GUILayout.Label(pInfo.title, TitleStyle);
		}
		GUILayout.EndHorizontal();
	}
	
	public override void OnInspectorGUI()
	{
		var pInfo = (ReadMe)target;
		Init();
		
		foreach (var section in pInfo.sections)
		{
			if (!string.IsNullOrEmpty(section.heading))
			{
				GUILayout.Label(section.heading, HeadingStyle);
			}
			if (!string.IsNullOrEmpty(section.text))
			{
				GUILayout.Label(section.text, BodyStyle);
			}
			if (!string.IsNullOrEmpty(section.linkText))
			{
				GUILayout.Space(kSpace / 2);
				if (LinkLabel(new GUIContent(section.linkText)))
				{
					Application.OpenURL(section.url);
				}
			}
			GUILayout.Space(kSpace);
		}
	}
	
	
	bool m_Initialized;
	
	GUIStyle LinkStyle { get { return m_LinkStyle; } }
	[SerializeField] GUIStyle m_LinkStyle;
	
	GUIStyle TitleStyle { get { return m_TitleStyle; } }
	[SerializeField] GUIStyle m_TitleStyle;
	
	GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
	[SerializeField] GUIStyle m_HeadingStyle;
	
	GUIStyle BodyStyle { get { return m_BodyStyle; } }
	[SerializeField] GUIStyle m_BodyStyle;
	
	void Init()
	{
		if (m_Initialized)
			return;
		m_BodyStyle = new GUIStyle(EditorStyles.label);
		m_BodyStyle.wordWrap = true;
		m_BodyStyle.fontSize = 14;
		
		m_TitleStyle = new GUIStyle(m_BodyStyle);
		m_TitleStyle.fontSize = 26;

		m_HeadingStyle = new GUIStyle(m_BodyStyle);
		m_HeadingStyle.fontSize = 18;
		m_HeadingStyle.fontStyle = FontStyle.Bold;
		
		m_LinkStyle = new GUIStyle(m_BodyStyle);
		// Match selection color which works nicely for both light and dark skins
		m_LinkStyle.normal.textColor = new Color (0x00/255f, 0x78/255f, 0xDA/255f, 1f);
		m_LinkStyle.stretchWidth = false;
		
		m_Initialized = true;
	}
	
	bool LinkLabel (GUIContent label, params GUILayoutOption[] options)
	{
		var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

		Handles.BeginGUI ();
		Handles.color = LinkStyle.normal.textColor;
		Handles.DrawLine (new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
		Handles.color = Color.white;
		Handles.EndGUI ();

		EditorGUIUtility.AddCursorRect (position, MouseCursor.Link);

		return GUI.Button (position, label, LinkStyle);
	}
}

*/