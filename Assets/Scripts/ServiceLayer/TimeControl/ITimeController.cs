namespace ServiceLayer.TimeControl
{
    public interface ITimeController
    {
        #region Methods

        void PauseGameplay();

        void UnpauseGameplay();

        #endregion
    }
}