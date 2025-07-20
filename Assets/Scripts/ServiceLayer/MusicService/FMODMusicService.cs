using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace ServiceLayer.MusicService
{
    public class FMODMusicService :IMusicService
    {
        private EventInstance  _currentMusic;
        private bool _isMusicEnabled = true;

        private EventReference _loadingMusicRef;
        private EventReference _gameMusicRef;

        public bool IsMusicEnabled => _isMusicEnabled;
        
        public FMODMusicService()
        {
            Debug.Log("FMODMusicService Constructor");
            _loadingMusicRef = RuntimeManager.PathToEventReference("event:/Music/Loading");
            _gameMusicRef = RuntimeManager.PathToEventReference("event:/Music/GameplayMusic");
            Debug.Log("_gameMusicRef: "+_gameMusicRef);
        }
        
        public void PlayMusic(EventReference musicRef)
        {
            StopMusic();

            if (!musicRef.IsNull)
            {
                _currentMusic = RuntimeManager.CreateInstance(musicRef);
                _currentMusic.start();
                _currentMusic.release(); 
            }
        }
        public void PlayLoadingMusic()
        {
            if(!_isMusicEnabled) return;
            
            PlayMusic(_loadingMusicRef);
        }
        
        public void PlayGameMusic()
        {
            if(!_isMusicEnabled) return;
            
            PlayMusic(_gameMusicRef);
        }

        public void StopMusic()
        {
            if (_currentMusic.isValid())
            {
                _currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        public void SetMusicEnabled(bool enabled)
        {
            _isMusicEnabled = enabled;

            if (enabled)
            {
                PlayMusic(_gameMusicRef);
            }
            else
            {
                StopMusic();
            }
        }
    }
}