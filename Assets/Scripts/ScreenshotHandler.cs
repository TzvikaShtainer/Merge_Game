using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string folderName = "Screenshots";
    [SerializeField] private int screenshotUpscale = 1; 

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Capture();
        }
    }

    public void Capture()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"Screenshot_{timestamp}.png";
        
        string fullPath = System.IO.Path.Combine(Application.dataPath, "..", folderName);
        
        if (!System.IO.Directory.Exists(fullPath))
        {
            System.IO.Directory.CreateDirectory(fullPath);
        }

        string finalDestination = System.IO.Path.Combine(fullPath, fileName);
        
        ScreenCapture.CaptureScreenshot(finalDestination, screenshotUpscale);
        
        Debug.Log($"<color=green>Screenshot saved to: {finalDestination}</color>");
    }
}