using UnityEditor;
using UnityEditor.SceneManagement;

namespace EckTechGames
{
    [InitializeOnLoad]
    public class AutoSaveExtension
    {
        static AutoSaveExtension()
        {
            EditorApplication.playModeStateChanged += AutoSaveWhenPlaymodeStarts;
        }

        private static void AutoSaveWhenPlaymodeStarts(PlayModeStateChange state)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                var scene = EditorSceneManager.GetActiveScene();
                if (scene.isDirty)
                {
                    EditorSceneManager.SaveScene(scene, null, false);
                }
                AssetDatabase.SaveAssets();
            }
        }
    }
}