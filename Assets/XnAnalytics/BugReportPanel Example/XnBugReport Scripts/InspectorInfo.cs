//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Needs the Serializable attribute otherwise the CustomPropertyDrawer wont be used
[System.Serializable]
public class InspectorInfo {
    //public bool showDefaultInspector = true;
    public string title = "InspectorInfo Title";
    [TextArea(1, 10)]
    public string text = "Set the Inspector to <b>Debug mode</b> \\n to edit this text.";
    public bool showAsFoldout = true;
    public bool foldoutOpenByDefault = false;
    
    public InspectorInfo() { }
    public InspectorInfo(string _title="", string _text="", bool _showAsFoldout=true, bool _foldoutOpenByDefault=false) {
        if (_title != "") title = _title;
        if (_text != "") text = _text;
        showAsFoldout = _showAsFoldout;
        foldoutOpenByDefault = _foldoutOpenByDefault;;
    }

    public int CountLines() {
        int num = 1;
        string t2 = text.Replace("\\n", "\n").Replace("\\t", "\t");
        for (int i = 0; i < t2.Length; i++) {
            if (t2[i] == '\n' || t2[i] == '\r') num++;
        }
        return num;
    }

    // public static explicit operator InspectorInfo(SerializedProperty sp) {
    //     InspectorInfo info = new InspectorInfo();
    //     //info.icon = sp.FindPropertyRelative("icon").val
    //     //info.showDefaultInspector = sp.FindPropertyRelative("showDefaultInspector").boolValue;
    //     info.title = sp.FindPropertyRelative("title").stringValue;
    //     info.text = sp.FindPropertyRelative("text").stringValue;
    //     info.showAsFoldout = sp.FindPropertyRelative("showAsFoldout").boolValue;
    //     info.foldoutOpenByDefault = sp.FindPropertyRelative("foldoutOpenByDefault").boolValue;
    //     return info;
    // }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorInfo))]
public class InspectorInfoDrawer : PropertyDrawer {
    private float totalPropertyHeight = EditorGUIUtility.singleLineHeight;
    private bool  foldoutOpen;
    // bool                  showFoldout;
    // private InspectorInfo info;
    
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return totalPropertyHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        totalPropertyHeight = 0;
        
        if (!m_Initialized) Init( property.FindPropertyRelative( "foldoutOpenByDefault" ).boolValue );
        // info = (InspectorInfo) property;
        
        //if (info.showDefaultInspector) {
        //    //base.OnGUI(position, property, label);
        //    MethodInfo defaultDraw = typeof(EditorGUI).GetMethod("DefaultPropertyField", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        //    defaultDraw.Invoke(null, new object[3] { position, property, label });
        //    return;
        //}
        SerializedProperty spTitle = property.FindPropertyRelative( "title" );
        SerializedProperty spShowAsFoldout = property.FindPropertyRelative("showAsFoldout");
        
        SerializedProperty spText = property.FindPropertyRelative("text");
        string replacedText = spText.stringValue.Replace( "\\n", "\n" ).Replace( "\\t", "\t" );
        GUIContent GUIContent_spText = new GUIContent (replacedText);

        EditorGUI.BeginProperty(position, label, property);

        if (spShowAsFoldout.boolValue) {
            //showFoldout = EditorGUILayout.Foldout(showFoldout, new GUIContent(info.title), true, BoldFoldoutStyle);
            Rect rFoldout = new Rect(position.x, position.y, position.width, BoldFoldoutStyle.lineHeight);
            totalPropertyHeight += rFoldout.height;
            foldoutOpen = EditorGUI.Foldout(rFoldout, foldoutOpen, spTitle.stringValue, true, BoldFoldoutStyle);
            if (foldoutOpen) {
                
                float textHeight = m_BodyStyle.CalcHeight (GUIContent_spText, EditorGUIUtility.currentViewWidth);

                var textRect = new Rect( position.x,
                    position.y + totalPropertyHeight,
                    position.width,
                    textHeight ); //info.CountLines() * m_BodyStyle.lineHeight);

                totalPropertyHeight += textHeight;
                
                EditorGUI.LabelField(textRect, replacedText, m_BodyStyle);
            }
        } else {
            {
                var titleRect = new Rect(position.x, position.y + EditorGUIUtility.standardVerticalSpacing, position.width, m_HeadingStyle.lineHeight);
                EditorGUI.LabelField(titleRect, spTitle.stringValue, m_HeadingStyle);

                totalPropertyHeight += titleRect.height;
                
                float textHeight = m_BodyStyle.CalcHeight (GUIContent_spText, EditorGUIUtility.currentViewWidth);

                var textRect = new Rect( position.x,
                    position.y + totalPropertyHeight,
                    position.width,
                    textHeight ); //info.CountLines() * m_BodyStyle.lineHeight);

                totalPropertyHeight += textHeight;
                
                EditorGUI.LabelField(textRect, replacedText, m_BodyStyle);
            }
        }

        EditorGUI.EndProperty();
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

    GUIStyle                  BoldFoldoutStyle { get { return m_BoldFoldoutStyle; } }
    [SerializeField] GUIStyle m_BoldFoldoutStyle;
    
    GUIStyle                  TextAreaStyle { get { return m_TextAreaStyle; } }
    [SerializeField] GUIStyle m_TextAreaStyle;

    void Init(bool _openByDefault) {
        if (m_Initialized)
            return;
        m_BodyStyle = new GUIStyle(EditorStyles.label);
        m_BodyStyle.wordWrap = true;
        m_BodyStyle.fontSize = 12;
        m_BodyStyle.alignment = TextAnchor.UpperLeft;
        m_BodyStyle.richText = true;

        m_TitleStyle = new GUIStyle(m_BodyStyle);
        m_TitleStyle.fontSize = 26;

        m_TextAreaStyle = new GUIStyle( EditorStyles.textArea );
        m_TextAreaStyle.wordWrap = true;
        m_TextAreaStyle.fontSize = 14;

        m_HeadingStyle = new GUIStyle(m_BodyStyle);
        m_HeadingStyle.fontSize = 18;

        m_LinkStyle = new GUIStyle(m_BodyStyle);
        m_LinkStyle.wordWrap = false;
        // Match selection color which works nicely for both light and dark skins
        m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
        m_LinkStyle.stretchWidth = false;

        // Initialize GUI styles
        m_BoldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        m_BoldFoldoutStyle.fontStyle = FontStyle.Bold;

        foldoutOpen = _openByDefault;
        
        m_Initialized = true;
    }
}
#endif

/* May have finally figured out the height of TextArea issue.
https://answers.unity.com/questions/509725/format-text-area-in-propertydrawer.html
[CustomPropertyDrawer (typeof (Text))]
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
		myTextProperty.stringValue = EditorGUI.TextArea (pos, myTextProperty.stringValue, myStyle);
 
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




/*
// _____________________ Everything in comments under here is largely broken, but I wanted it in case I tried this craziness again - JGB 2022-10-04
[CustomPropertyDrawer(typeof(InspectorInfo))]
public class InspectorInfoDrawer : PropertyDrawer {
    private InspectorInfo info;
    float propertyHeight = EditorGUIUtility.singleLineHeight;

    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
    //    // The 6 comes from extra spacing between the fields (2px each)
    //    return EditorGUIUtility.singleLineHeight * 4 + 6;
    //}
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (info == null) {
            info = (InspectorInfo)property;
            Init();
        }

        if (info.showDefaultInspector) {
            return base.GetPropertyHeight(property, label);
        }

        float totalHeight = m_HeadingStyle.lineHeight + EditorGUIUtility.standardVerticalSpacing;
        totalHeight += info.CountLines() * m_BodyStyle.lineHeight;


        ////SerializedObject childObj = new UnityEditor.SerializedObject(property.objectReferenceValue as InspectorInfo);
        ////SerializedProperty ite = childObj.GetIterator();
        //var enumerator = property.GetEnumerator();
        //float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;


        ////while (ite.NextVisible(true)) {
        //while (enumerator.MoveNext()) { 
        //    totalHeight += EditorGUI.GetPropertyHeight(enumerator.Current as SerializedProperty, label, true) + EditorGUIUtility.standardVerticalSpacing;
        //}

        return totalHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (info == null) {
            info = (InspectorInfo)property;
            Init();
        }

        EditorGUI.BeginProperty(position, label, property);


        var titleRect = new Rect(position.x, position.y + EditorGUIUtility.standardVerticalSpacing, position.width, m_HeadingStyle.lineHeight);
        EditorGUI.LabelField(titleRect, property.FindPropertyRelative("title").stringValue, m_HeadingStyle);

        SerializedProperty spText = property.FindPropertyRelative("text");
        var textRect = new Rect(position.x,
            position.y + titleRect.height,
            position.width,
            info.CountLines() * m_BodyStyle.lineHeight);
        EditorGUI.LabelField(textRect, spText.stringValue.Replace("\\n", "\n").Replace("\\t", "\t"));


        //var enumerator = property.GetEnumerator();
        //SerializedProperty sp;
        //Rect rec;
        //while (enumerator.MoveNext()) {
        //    sp = enumerator.Current as SerializedProperty;
        //    rec = 
        //    totalHeight += EditorGUI.GetPropertyHeight(sp, label, true) + EditorGUIUtility.standardVerticalSpacing;
        //}


        //Rect rLabel = new Rect(position.x, position.y, position.width - 20, 16);
        //Rect rEditButton = new Rect(position.x + position.width - 20, position.y, 20, 16);


        //Rect rTextArea = 
        //EditorGUI.LabelField(new Rect(3, 3, position.width, 20),
        //    "Time since start: ",
        //    EditorApplication.timeSinceStartup.ToString());


        //string titleString = info.title.Replace("\\n", "\n").Replace("\\t", "\t");
        //GUILayout.Label(titleString, HeadingStyle);
        //if (!string.IsNullOrEmpty(info.text)) {
        //    string infoString = info.text.Replace("\\n", "\n").Replace("\\t", "\t");
        //    GUILayout.Label(infoString, BodyStyle);
        //}

        //this.Repaint();

        //float iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 4f - 20f, info.iconMaxWidth);
        //GUILayout.BeginHorizontal();// "In BigTitle");
        //{
        //    if (info.icon != null) {
        //        GUILayout.Label(info.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
        //    }

        //EditorGUILayout.BeginVertical();
        //{
        //    if (!string.IsNullOrEmpty(info.title)) {
        //        string titleStr = info.title.Replace("\\n", "\n").Replace("\\t", "\t");
        //        GUILayout.Label(titleStr, HeadingStyle);
        //    }
        //    if (!string.IsNullOrEmpty(info.text)) {
        //        string infoString = info.text.Replace("\\n", "\n").Replace("\\t", "\t");
        //        GUILayout.Label(infoString, BodyStyle);
        //    }
        //}
        //EditorGUILayout.EndVertical();

        //}
        //GUILayout.EndHorizontal();

        // ---- This part actually (kind of) worked.
        //if (info.showDefaultInspector) {
        //    //GUILayout.Space(10);
        //    //    if (GUILayout.Button("Hide Default Inspector")) info.showDefaultInspector = false;
        //    //    DrawDefaultProperty();

        //    var titleRect = new Rect(position.x, position.y, position.width, m_HeadingStyle.fontSize+2);
        //    SerializedProperty spText = property.FindPropertyRelative("text");
        //    var textRect = new Rect(position.x, position.y + titleRect.height, position.width, EditorGUI.GetPropertyHeight(spText, false));
        //    EditorGUI.LabelField(titleRect, property.FindPropertyRelative("title").stringValue, m_HeadingStyle);
        //    EditorGUI.LabelField(textRect, spText.stringValue);
        //    //EditorGUI.PropertyField(titleRect, property.FindPropertyRelative("title"));
        //    //EditorGUI.PropertyField(textRect, property.FindPropertyRelative("text"));

        //}


        //EditorGUI.LabelField(position, label);

        //var nameRect = new Rect(position.x, position.y + 18, position.width, 16);
        //var ageRect = new Rect(position.x, position.y + 36, position.width, 16);
        //var genderRect = new Rect(position.x, position.y + 54, position.width, 16);

        //EditorGUI.indentLevel++;

        //EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"));
        //EditorGUI.PropertyField(ageRect, property.FindPropertyRelative("Age"));
        //EditorGUI.PropertyField(genderRect, property.FindPropertyRelative("Gender"));

        //EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
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

    void Init() {
        if (m_Initialized)
            return;
        m_BodyStyle = new GUIStyle(EditorStyles.label);
        m_BodyStyle.wordWrap = true;
        m_BodyStyle.fontSize = 14;

        m_TitleStyle = new GUIStyle(m_BodyStyle);
        m_TitleStyle.fontSize = 26;

        m_HeadingStyle = new GUIStyle(m_BodyStyle);
        m_HeadingStyle.fontSize = 18;

        m_LinkStyle = new GUIStyle(m_BodyStyle);
        m_LinkStyle.wordWrap = false;
        // Match selection color which works nicely for both light and dark skins
        m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
        m_LinkStyle.stretchWidth = false;

        m_Initialized = true;
    }
}
*/

