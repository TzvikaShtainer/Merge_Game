using UnityEngine.Device;

namespace ServiceLayer.NavigationService
{
    public class NavigationService : INavigationService
    {
        public void OpenUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            
            Application.OpenURL(url);
        }
    }
}