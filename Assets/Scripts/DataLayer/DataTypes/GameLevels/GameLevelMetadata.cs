using System;
using UnityEngine;

namespace DataLayer.DataTypes
{
    [Serializable]
    public class GameLevelMetadata
    {
        #region Editor

        [SerializeField]
        private GameLevelType _levelType;
		
        [SerializeField]
        private string _levelName;

        [SerializeField]
        private int _levelSceneBuildIndex;
		
        #endregion
        
		
        #region Properties

        public GameLevelType LevelType => _levelType;
		
        public string LevelName => _levelName;

        public int LevelSceneBuildIndex => _levelSceneBuildIndex;
        
        #endregion
    }
}