#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace XnTools {
    /// <summary>
    /// Allows the retrieval of the internal Dictionary\<Type,string\> that holds the headers for each Component in the Inspector
    /// From: https://forum.unity.com/threads/modifying-component-inspector-headers-for-component-folders.504247/#post-7929337
    /// Usage:
    ///    var inspectorHeaders = InspectorHeadersUtility.GetInternalInspectorTitlesCache();
    ///    inspectorHeaders[typeof(Transform)] = "New Header Text";
    /// </summary>
    public static class InspectorHeadersUtility {
        public static Dictionary<Type, string> GetInternalInspectorTitlesCache() {
            Type inspectorTitlesType = typeof( ObjectNames ).GetNestedType( "InspectorTitles", BindingFlags.Static | BindingFlags.NonPublic );
            var inspectorTitlesField = inspectorTitlesType.GetField( "s_InspectorTitles", BindingFlags.Static | BindingFlags.NonPublic );
            return (Dictionary<Type, string>) inspectorTitlesField.GetValue( null );
        }
    }
}
#endif