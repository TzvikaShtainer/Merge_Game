using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VisualLayer.GamePlay.Handlers
{
    public class HudBackClickHandler : IHudBackClickHandler
    {
        public async UniTask Execute()
        {
            Debug.Log("HudBackClickHandler");
        }
    }
}