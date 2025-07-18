using System.Collections.Generic;
using FMODUnity;

namespace ServiceLayer.MusicService
{
    public class FMODSfxService : ISfxService
    {
        private bool _sfxEnabled = true;
        private readonly Dictionary<SfxType, EventReference> _sfxMap;

        public bool IsSfxEnabled => _sfxEnabled;

        public FMODSfxService(SfxDatabase eventMap)
        {
            _sfxMap = eventMap.ToDictionary();
        }

        public void PlaySfxType(SfxType type)
        {
            if (!_sfxEnabled || !_sfxMap.ContainsKey(type)) return;

            var evt = _sfxMap[type];
            if (!evt.IsNull)
                RuntimeManager.PlayOneShot(evt);
        }

        public void SetSfxEnabled(bool enabled)
        {
            _sfxEnabled = enabled;
        }
    }
}