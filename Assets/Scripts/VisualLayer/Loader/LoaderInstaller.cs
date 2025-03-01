using UnityEngine;
using Zenject;

namespace VisualLayer.Loader
{
    public class LoaderInstaller : MonoInstaller<LoaderInstaller>
    {
        
        #region Loader

        [SerializeField]
        private Loader _loader;

        #endregion
        
        public override void InstallBindings()
        {
            Container
                .Bind<ILoader>()
                .FromInstance(_loader)
                .AsSingle();
        }
    }
}