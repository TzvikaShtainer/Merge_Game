using UnityEditor;
using UnityEngine;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using Zenject;

namespace Editor.Debug
{
    public class DebugMenuItems
    {
        [MenuItem("Merge/Popups/YesNoPopup")]
        public static void ShowYesNoPopup()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            var context = Object.FindObjectOfType<SceneContext>();
            if (context == null)
            {
                return;
            }
			
            var popupFactory = context.Container.Resolve<YesNoPopup.Factory>();
            //popupFactory.Create();
        }
    }
}