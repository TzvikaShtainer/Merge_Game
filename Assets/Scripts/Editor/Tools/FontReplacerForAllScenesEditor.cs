#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class FontReplacerTool : EditorWindow
{
    private TMP_FontAsset newTMPFont;

    [MenuItem("Tools/Replace Fonts/In All Scenes")]
    public static void ShowWindow()
    {
        GetWindow<FontReplacerTool>("Font Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace Fonts In All Scenes", EditorStyles.boldLabel);
        GUILayout.Space(10);

        newTMPFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font", newTMPFont, typeof(TMP_FontAsset), false);

        GUILayout.Space(10);

        if (GUILayout.Button("Replace Font In All Scenes"))
        {
            ReplaceFontsInAllScenes();
        }
    }

    private void ReplaceFontsInAllScenes()
    {
        if (newTMPFont == null)
        {
            EditorUtility.DisplayDialog("Error", "Please select a TMP Font before replacing.", "OK");
            return;
        }

        string[] scenePaths = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);

        int totalTMPTexts = 0;
        int totalUIText = 0;

        foreach (string scenePath in scenePaths)
        {
            Debug.Log($"🔄 Loading scene: {scenePath}");

            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            var uiTexts = Object.FindObjectsOfType<Text>(true);
            var tmpTexts = Object.FindObjectsOfType<TMP_Text>(true);

            foreach (var text in uiTexts)
            {
                Undo.RecordObject(text, "Change UI Font");
                EditorUtility.SetDirty(text);
                totalUIText++;
            }

            foreach (var tmp in tmpTexts)
            {
                Undo.RecordObject(tmp, "Change TMP Font");
                tmp.font = newTMPFont;
                EditorUtility.SetDirty(tmp);
                totalTMPTexts++;
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        EditorUtility.DisplayDialog(
            "Font Replacement Complete",
            $"Replaced fonts in {totalUIText} UI Texts and {totalTMPTexts} TMP Texts across all scenes.",
            "OK"
        );
    }
}
#endif
