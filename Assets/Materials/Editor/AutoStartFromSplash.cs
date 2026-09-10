#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutoStartFromSplash
{
    static AutoStartFromSplash()
    {
        // Siguraduhin na ang Scene 0 (Splashscreen) ang laging unang mag-lo-load pagpindot ng Play button
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/Splashscreen.unity");
    }
}
#endif