using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    private EventInstance gameplayMusicInstance;

    private void Start()
    {
       
        gameplayMusicInstance = RuntimeManager.CreateInstance(FModEvents.Instance.GameplayMusic);

        gameplayMusicInstance.start();
        gameplayMusicInstance.release();
    }

    private void OnDestroy()
    {
        // Stop the music when this object is destroyed
        if (gameplayMusicInstance.isValid())
        {
            gameplayMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            gameplayMusicInstance.release();
        }
    }
}