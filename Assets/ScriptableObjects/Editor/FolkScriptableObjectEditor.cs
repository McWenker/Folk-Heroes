using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FolkScriptableObject))]
public class FolkScriptableObjectEditor : Editor
{
    FolkScriptableObject folk;
    string[] days;
    bool showSchedules = true;
    public void OnEnable()
    {
        folk = (FolkScriptableObject)target;
        folk.schedules = new Schedule[7];
        days = new string[]{"Montir", "Denst", "Mittwik", "Doorsday", "Freeday", "Sattir", "Sonnst"};
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorStyles.textField.wordWrap = true;
        
        folk.convoSprite = (Sprite)EditorGUILayout.ObjectField("Conversation Sprite", folk.convoSprite, typeof(Sprite), false);
        GUILayout.Label ("Character's in-game description:");
        folk.description = EditorGUILayout.TextArea(folk.description, GUILayout.Height(100));

        SerializedProperty likedItems = serializedObject.FindProperty ("likedItems");
        EditorGUILayout.PropertyField(likedItems, true);

        SerializedProperty dislikedItems = serializedObject.FindProperty ("dislikedItems");
        EditorGUILayout.PropertyField(dislikedItems, true);

        showSchedules = EditorGUILayout.BeginFoldoutHeaderGroup(showSchedules, "Default Schedule");
        if(showSchedules)
        {
            SerializedProperty schedules = serializedObject.FindProperty ("schedules");   
            for(int i = 0; i < schedules.arraySize; ++i)
            {
                SerializedProperty schedule = schedules.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginVertical();
                GUILayout.Label(days[i]);   
                EditorGUILayout.PropertyField(schedule, true);
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        

        serializedObject.ApplyModifiedProperties();
    }
}