using System;
using UnityEngine;

namespace XnTools {
	[CreateAssetMenu( fileName = "ProjectInfo", menuName = "ScriptableObjects/ProjectInfo",
		order = 1 )]
	public class ProjectInfo_SO : ScriptableObject {
		public Texture2D icon         = null;
		public float     iconMaxWidth = 128f;
		[TextArea( 1, 10 )]
		public string title = "Replace this Title";
		public Section[] sections = new Section[] { Section.demo };
		//public bool loadedLayout;
		public bool showDefaultInspector = false;

		[Serializable]
		public class Section {
			[TextArea( 1, 10 )]
			public string heading, text, linkText, url;

			static public Section demo {
				get {
					Section sec = new Section();
					sec.heading = "Section Heading";
					sec.text =
						"<b>Section</b> <i>text</i>.\nHold <b>option/alt</b>, move the mouse, and click the \"<b>Show Default Inspector</b>\" button.";

					return sec;
				}
			}
		}
	}
}