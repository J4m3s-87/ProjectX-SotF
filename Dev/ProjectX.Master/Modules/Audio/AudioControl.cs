#if !SERVER
using System;
using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.Audio
{
    /// <summary>
    /// Controls audio levels for various game elements
    /// </summary>
    public static class AudioControl
    {
        private static float _waterfallVolume = 1.0f;

        /// <summary>
        /// Volume multiplier for waterfall sounds (0.0 - 3.0)
        /// </summary>
        public static float WaterfallVolume
        {
            get => _waterfallVolume;
            set
            {
                _waterfallVolume = Mathf.Clamp(value, 0f, 3f);
                ApplyWaterfallVolume();
            }
        }

        private static void ApplyWaterfallVolume()
        {
            try
            {
                // Find all waterfall audio sources and adjust volume
                var waterfallSources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
                int adjusted = 0;
                
                foreach (var source in waterfallSources)
                {
                    if (source == null) continue;
                    
                    // Check if this is a waterfall audio source by looking for "water" in the name
                    if (source.gameObject.name.ToLower().Contains("waterfall") ||
                        source.gameObject.name.ToLower().Contains("water"))
                    {
                        source.volume = _waterfallVolume;
                        adjusted++;
                    }
                }
                
                RLog.Msg($"[AudioControl] Waterfall volume: {_waterfallVolume:F1} (adjusted {adjusted} sources)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AudioControl] Failed to apply waterfall volume: {ex.Message}");
            }
        }
    }
}
#endif
