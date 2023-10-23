using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// An editor for <see cref="AIBranchModule"/> that allows for the quick
/// modification of certain fields in <see cref="AIBranchModule"/>. This
/// utilizes reflection, so there's no need to go and update this at all, even
/// if we add more classes to the AI Controls (ie <see cref="AITargeting"/>,
/// <see cref="AIDecision"/>, and <see cref="AIAction"/>).
///
/// <br/><br/>
///
/// For more information on reflection, see <see
/// cref="https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/#reflection-overview"/>.
/// Keep in mind that reflection is extremely expensive and should not be used
/// while the player is playing the game. It can also be a very deep rabbit
/// hole, so prepare to get your hands dirty. However, if you do decide to
/// explore reflection, I'll guarantee that you won't look at C# the same way
/// ever again.
///
/// <br/><br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[CustomEditor(typeof(AIBranchModule))]
public class AIBranchEditor : Editor
{
    private bool[] opened = new bool[2];

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();

        opened[0] = EditorGUILayout.BeginFoldoutHeaderGroup(opened[0],
            "Branch Documentation");
        if (opened[0])
        {
            GUILayout.Box(
                "A branch is utilizes classes derived from AITargeting, " +
                "AIDecision, and AIAction (collectively referred to as AI " +
                "Controls). " +
                "AITargeting selects a target for the AI. " +
                "AIDecision uses that target to decide if the branch should be " +
                "executed. " +
                "AIAction is responsible for the actual execution of the AI.",
                GUIStyleExt.LeftAlignBox
            );
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        base.OnInspectorGUI();

        AIBranchModule branch = (AIBranchModule)serializedObject.targetObject;

        EditorGUILayout.Separator();

        opened[1] = EditorGUILayout.BeginFoldoutHeaderGroup(opened[1],
            "Buttons Documentation");
        if (opened[1])
        {
            GUILayout.Box(
                "Select a type from the dropdowns below to quickly create and " +
                "assign it. Once assigned, you can click on the Remove button to " +
                "get rid of it. Be warned: undo does NOT work for this, and I " +
                "didn't add a confirmation box either (lmk if you want one).",
                GUIStyleExt.LeftAlignBox
            );
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditAIControl(ref branch.targeting, "Targeting");
        EditAIControl(ref branch.decision, "Decision");
        branch.actions = EditAIControl(branch.actions, "Action");

        EditorGUILayout.Separator();

        CreateAddBranchBtn();
    }

    /// <inheritdoc cref="EditAIControl{T}(ref T, string, bool)"/>
    private T[] EditAIControl<T>(T[] aiControls, string noun)
        where T : Component
    {
        T aiControl = null;
        CreateAddDropDown(ref aiControl, noun);

        if (aiControl)
        {
            return aiControls.Append(aiControl).ToArray();
        }

        return aiControls;
    }

    /// <summary>
    /// Create GUI elements that allows the user to manipulate AI controls (ie
    /// <see cref="AITargeting"/>, <see cref="AIDecision"/>, and <see
    /// cref="AIAction"/>).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="aiControl">The AI Control.</param>
    /// <param name="noun">What to call <paramref name="aiControl"/>.</param>
    private void EditAIControl<T>(ref T aiControl, string noun, bool noRemove = false)
        where T : Component
    {
        if (!aiControl)
        {
            CreateAddDropDown(ref aiControl, noun);
        }
        else if (!noRemove)
        {
            if (GUILayout.Button("Remove " + noun))
            {
                // Remove the reference and destroy its gameobject. I don't know
                // why, but Unity requires you to use DestroyImmediate outside
                // of play mode.
                DestroyImmediate(aiControl.gameObject);
                aiControl = null;
            }
        }
    }

    private void CreateAddDropDown<T>(ref T aiControl, string noun) where T : Component
    {
        // Create a list of types derived from T.
        var types = TypeExt.FindAllDerivedTypes<T>();

        // Take that list and create another one that's the names of the
        // types, along with a [Not Set] value.
        var options = types
            .Select(t => t.Name)
            .Prepend("[Not Set]")
            .ToArray();

        // Selection corresponds with the selected element in types, or -1
        // if [Not Set] is selected. Also create the popup.
        int selection = EditorGUILayout.Popup(
            "Add " + noun,
            0,
            options
        ) - 1;

        if (selection >= 0)
        {
            EditorGUI.BeginChangeCheck();

            // If we have selected a valid value, then get the selected
            // type. See above comment for more info.
            var selectedType = types.ElementAt(selection);
            Debug.Log(selectedType);

            // Create the new AI Control gameobject, make it a child of the
            // branch, then assign it to the referenced aiControl.
            var branch = (AIBranchModule)serializedObject.targetObject;

            GameObject go = new(
                $"[{noun}] {branch.id}",
                selectedType
            );
            go.transform.Localize(branch.transform);
            go.RequireComponent(out T tempCtrl);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(aiControl, "AICTRL" + noun);
                aiControl = tempCtrl;
            }
        }
    }

    /// <summary>
    /// Creates and adds a button allowing the user to add new subbranches.
    /// </summary>
    private void CreateAddBranchBtn()
    {
        if (GUILayout.Button("Add Branch"))
        {
            var branch = (AIBranchModule)serializedObject.targetObject;

            GameObject go = new("New Branch", typeof(AIBranchModule));
            go.transform.Localize(branch.transform);
            go.RequireComponent(out AIBranchModule newBranch);

            branch.branches.Add(newBranch);
        }
    }
}