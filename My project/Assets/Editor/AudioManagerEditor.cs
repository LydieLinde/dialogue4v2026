using UnityEditor;
using UnityEngine;

// Custom inspector para controlar o AudioManager em tempo de edição e Play Mode.
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AudioManager manager = (AudioManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Playback Controls", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play"))
        {
            // Se estiver em modo Play, use singleton para controlar runtime
            if (EditorApplication.isPlaying)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayCurrentCollection();
                }
            }
            else
            {
                // Em edit mode, instruir o usuário a entrar em Play Mode
                Debug.Log("Enter Play Mode to play music via AudioManager singleton.");
            }
        }

        if (GUILayout.Button("Pause"))
        {
            if (EditorApplication.isPlaying)
            {
                AudioManager.Instance?.PauseSound();
            }
            else
            {
                Debug.Log("Pause is only available in Play Mode.");
            }
        }

        if (GUILayout.Button("Stop"))
        {
            if (EditorApplication.isPlaying)
            {
                AudioManager.Instance?.StopSound();
            }
            else
            {
                Debug.Log("Stop is only available in Play Mode.");
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Runtime Collection Control", EditorStyles.boldLabel);

        EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
        int runtimeIndex = EditorGUILayout.IntField("Play Index (runtime)", manager.collectionIndex);
        if (GUILayout.Button("Set Runtime Index via Singleton"))
        {
            if (EditorApplication.isPlaying)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetCollectionIndex(runtimeIndex);
                    // opcional: tocar imediatamente
                    AudioManager.Instance.PlayCurrentCollection();
                }
                else
                {
                    Debug.LogWarning("AudioManager.Instance is null. Ensure the AudioManager is present in the scene.");
                }
            }
            else
            {
                Debug.LogWarning("This action only works in Play Mode.");
            }
        }
        EditorGUI.EndDisabledGroup();
    }
}




