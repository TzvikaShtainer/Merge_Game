using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Score
{
    [CreateAssetMenu(menuName = "Merge/Score/Score Calculator", fileName = "Score Calculator")]
    public class GameScoreCalculatorInstaller : ScriptableObjectInstaller<GameScoreCalculatorInstaller>
    {
        [SerializeField]
        private ScoreCalculationParams _params;
        
        public override void InstallBindings()
        {
            Container
                .Bind<IInitializable>()
                .To<SignalBasedGameScoreCalculator>()
                .AsSingle();

            Container
                .Bind<ScoreCalculationParams>()
                .FromInstance(_params)
                .AsSingle();
        }
    }
}