using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace ServiceLayer.MusicService
{
    [CreateAssetMenu(menuName = "Merge/Data/Sfx Database", fileName = "SfxDatabase")]
    public class SfxDatabase : ScriptableObject
    {
        [Serializable]
        public class SfxEntry
        {
            public SfxType sfxType;
            public EventReference fmodEvent;
        }
        
        public List<SfxEntry> entries = new();

        public Dictionary<SfxType, EventReference> ToDictionary()
        {
            var dict = new Dictionary<SfxType, EventReference>();
            foreach (var entry in entries)
            {
                if (!dict.ContainsKey(entry.sfxType))
                {
                    dict.Add(entry.sfxType, entry.fmodEvent);
                }
            }
            return dict;
        }
        
        
    }
}