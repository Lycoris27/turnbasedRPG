using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovementTileScript))]
public class MovementTileScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get reference to the target script
        MovementTileScript script = (MovementTileScript)target;
        var boosts = script.GetBoostValues();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Final Boost Values", EditorStyles.boldLabel);
        EditorGUILayout.IntField("Avoid Boost", boosts[0]);
        EditorGUILayout.IntField("Def Boost", boosts[1]);
        EditorGUILayout.IntField("Heal Boost", boosts[2]);
    }
}
