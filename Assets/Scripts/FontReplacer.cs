#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FontReplacer : MonoBehaviour
{
    [Header("Chose your font")]
    public TMP_FontAsset newTMPFont; 

    [ContextMenu("Replace All Fonts In Scene")]
    public void ReplaceFonts()
    {
        var uiTexts = FindObjectsOfType<Text>(true);
        foreach (var text in uiTexts)
        {
            Undo.RecordObject(text, "Change Font");
            EditorUtility.SetDirty(text);
        }

        // החלפה ל-TMP_Text
        var tmpTexts = FindObjectsOfType<TMP_Text>(true);
        foreach (var tmp in tmpTexts)
        {
            Undo.RecordObject(tmp, "Change TMP Font");
            tmp.font = newTMPFont;
            EditorUtility.SetDirty(tmp);
        }

        Debug.Log($"שונה הפונט של {uiTexts.Length} UI Texts ו־{tmpTexts.Length} TMP Texts");
    }
}
#endif