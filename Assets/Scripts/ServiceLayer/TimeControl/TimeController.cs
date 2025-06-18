using UnityEngine;

namespace ServiceLayer.TimeControl
{
    public class TimeController : ITimeController
    {
        #region Methods

        public void PauseGameplay()
        {
            Debug.Log("Pause Gameplay");
            Time.timeScale = 0f;
        }

        public void UnpauseGameplay()
        {
            Time.timeScale = 1f;
        }

        #endregion
    }
}