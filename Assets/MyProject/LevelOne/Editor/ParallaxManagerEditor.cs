#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParallaxManager))]
public class ParallaxManagerEditor : Editor
{
    // We use OnEnable instead of Awake for Editors to ensure 
    // it triggers when you click the object
    private void OnEnable()
    {
        ParallaxManager manager = (ParallaxManager)target;

        // Try to find the camera if it's missing
        if (manager.gameCamera == null && Camera.main != null)
        {
            manager.gameCamera = Camera.main.transform;
            EditorUtility.SetDirty(manager);
        }
    }

    public override void OnInspectorGUI()
    {
        // Draws the default array and settings
        base.OnInspectorGUI();

        ParallaxManager manager = (ParallaxManager)target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Parallax Tools", EditorStyles.boldLabel);

        // --- BUTTONS ---

        if (GUILayout.Button("Refresh From Children", GUILayout.Height(30)))
        {
            // Record for Undo so you can Ctrl+Z if it messes up
            Undo.RecordObject(manager, "Refresh Backgrounds");
            manager.ResetBackgrounds();
        }

        if (GUILayout.Button("Distribute Intensities"))
        {
            Undo.RecordObject(manager, "Set Intensities");
            manager.ResetIntensities();
        }

        // Check if any changes were made to save them
        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
        }
    }
}
#endif