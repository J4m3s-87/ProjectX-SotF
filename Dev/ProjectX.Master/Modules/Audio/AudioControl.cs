#if !SERVER && !CLIENT
using System;
using System.Collections.Generic;
using HarmonyLib;
using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.Audio
{
    /// <summary>
    /// Controls waterfall volume using FMOD event emitters.
    /// Based on WaterfallSoundControl 1.0.0 by the original author.
    /// 
    /// Original mod uses Scene.GetRootGameObjects() + GetComponentsInChildren — both stripped by IL2CPP.
    /// This rewrite uses Harmony Postfix on SonsFMODEventEmitter.Awake to track emitters as they load,
    /// filtering by root parent name "WaterfallSounds".
    /// </summary>
    public static class AudioControl
    {
        private static float _waterfallVolume = 1.0f;
        private static bool _initialized;
        private static HarmonyLib.Harmony _harmony;

        /// <summary>Tracked waterfall FMOD emitters, discovered via Harmony Postfix</summary>
        private static readonly List<SonsFMODEventEmitter> _emitters = new();

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

        /// <summary>
        /// Initialize Harmony patch to track SonsFMODEventEmitters as they Awake.
        /// Called from MasterPlugin.OnSdkInitialized().
        /// </summary>
        public static void Init()
        {
            if (_initialized) return;

            try
            {
                _harmony = new HarmonyLib.Harmony("ProjectX.AudioControl");
                
                // Patch SonsFMODEventEmitter.Awake to track waterfall emitters
                var awakeMethod = AccessTools.Method(typeof(SonsFMODEventEmitter), "Awake");
                if (awakeMethod != null)
                {
                    var postfix = new HarmonyMethod(typeof(AudioControl), nameof(OnFMODEmitterAwake));
                    _harmony.Patch(awakeMethod, postfix: postfix);
                    RLog.Msg("[AudioControl] Harmony POSTFIX on SonsFMODEventEmitter.Awake — PATCHED OK");
                }
                else
                {
                    RLog.Warning("[AudioControl] SonsFMODEventEmitter.Awake not found — trying OnEnable fallback");
                    
                    // Fallback: try OnEnable
                    var onEnableMethod = AccessTools.Method(typeof(SonsFMODEventEmitter), "OnEnable");
                    if (onEnableMethod != null)
                    {
                        var postfix = new HarmonyMethod(typeof(AudioControl), nameof(OnFMODEmitterAwake));
                        _harmony.Patch(onEnableMethod, postfix: postfix);
                        RLog.Msg("[AudioControl] Harmony POSTFIX on SonsFMODEventEmitter.OnEnable — PATCHED OK (fallback)");
                    }
                    else
                    {
                        RLog.Warning("[AudioControl] No hookable method found on SonsFMODEventEmitter");
                    }
                }

                _initialized = true;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AudioControl] Init failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Harmony Postfix — called when any SonsFMODEventEmitter.Awake fires.
        /// Checks if the emitter belongs to the "WaterfallSounds" hierarchy and tracks it.
        /// 
        /// Original mod finds: Scene("Site02StreamsAndLakes") → root "WaterfallSounds" → GetComponentsInChildren.
        /// We replicate this by walking up the transform hierarchy to check for the root name.
        /// </summary>
        private static void OnFMODEmitterAwake(SonsFMODEventEmitter __instance)
        {
            try
            {
                if (__instance == null) return;

                // Walk up to the root transform to check if this belongs to "WaterfallSounds"
                Transform root = __instance.transform;
                while (root.parent != null)
                {
                    root = root.parent;
                }

                string rootName = root.gameObject.name;
                if (rootName == "WaterfallSounds")
                {
                    _emitters.Add(__instance);

                    // Apply current volume immediately
                    try
                    {
                        __instance.SetVolume(_waterfallVolume);
                    }
                    catch { /* _baseVolume may not be accessible yet during Awake */ }
                }
            }
            catch { /* Emitter may be in partial state during Awake */ }
        }

        /// <summary>
        /// Apply volume to all tracked waterfall emitters.
        /// Uses the same Stop/Play restart cycle as the original mod to force FMOD to pick up the new volume.
        /// </summary>
        private static void ApplyWaterfallVolume()
        {
            int adjusted = 0;

            // Prune destroyed emitters and apply volume
            for (int i = _emitters.Count - 1; i >= 0; i--)
            {
                if (_emitters[i] == null)
                {
                    _emitters.RemoveAt(i);
                    continue;
                }

                try
                {
                    _emitters[i].SetVolume(_waterfallVolume);

                    // Stop/Play restart cycle — same as original mod
                    // Forces FMOD to pick up the new _baseVolume value
                    if (_emitters[i].IsPlaying())
                    {
                        _emitters[i].Stop();
                        _emitters[i].Play();
                    }

                    adjusted++;
                }
                catch { /* Individual emitter may have been destroyed between null check and access */ }
            }

            RLog.Msg($"[AudioControl] Waterfall volume: {_waterfallVolume:F1} (adjusted {adjusted}/{_emitters.Count} FMOD emitters)");
        }

        /// <summary>
        /// Clear tracked emitters on world exit.
        /// </summary>
        public static void OnWorldExited()
        {
            _emitters.Clear();
        }
    }
}
#endif
