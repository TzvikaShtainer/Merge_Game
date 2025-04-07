using UnityEngine;
using TMPro;
using UnityEditor;

public class FontReplacer : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Replace Fonts In Scene")]
    private static void ReplaceFonts()
    {
        TMP_FontAsset newFont = Resources.Load<TMP_FontAsset>("Fonts/ChalkboardSE-Regular"); 
        if (newFont == null)
        {
            Debug.LogError("Couldn't find the font! Make sure it's in Resources/Fonts/MyNewFont");
            return;
        }

        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true); 

        
        foreach (var text in allTexts)
        {
            text.font = newFont;
            EditorUtility.SetDirty(text);
        }

        Debug.Log($"✅ Replaced fonts on {allTexts.Length} text objects!");
    }
#endif
}