using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string folderName = "Screenshots";
    [SerializeField] private int screenshotUpscale = 1; // מגדיל את הרזולוציה פי X

    void Update()
    {
        // נשתמש במקש K (מלשון Keep) כדי לצלם
        if (Input.anyKeyDown)
        {
            Capture();
        }
    }

    public void Capture()
    {
        // יצירת שם קובץ ייחודי מבוסס זמן כדי שלא נדרוס קבצים
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"Screenshot_{timestamp}.png";
        
        // יצירת הנתיב - ב-Editor זה יישמר בתיקיית הפרויקט הראשית
        string fullPath = System.IO.Path.Combine(Application.dataPath, "..", folderName);
        
        // בדיקה שהתיקייה קיימת, אם לא - יוצרים אותה
        if (!System.IO.Directory.Exists(fullPath))
        {
            System.IO.Directory.CreateDirectory(fullPath);
        }

        string finalDestination = System.IO.Path.Combine(fullPath, fileName);

        // הפעולה עצמה
        ScreenCapture.CaptureScreenshot(finalDestination, screenshotUpscale);
        
        Debug.Log($"<color=green>Screenshot saved to: {finalDestination}</color>");
    }
}