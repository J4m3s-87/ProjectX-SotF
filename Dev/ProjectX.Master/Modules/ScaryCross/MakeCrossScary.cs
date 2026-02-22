using System;
using System.Collections.Generic;
using System.Reflection;
using RedLoader;
using Sons.Ai.Vail;
using Sons.Ai.Vail.StimuliTypes;
using Sons.Crafting.Structures;
using Sons.Electricity;
using UnityEngine;
using ProjectX.Master; 
using HarmonyLib;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ProjectX.Master.Modules.ScaryCross
{
    [RegisterTypeInIl2Cpp]
    public class MakeCrossScary : MonoBehaviour
    {
        public PlayerEffigyStimuli _effigyStimuli;
        public AuraInfluence _effigyAuraInfluence;
        public EventDescription _effigyEventDescription;
#if !SERVER
        public GameObject _fireAll;
        public List<Light> _lightbulbs = new List<Light>();
        public List<ParticleSystem> _FireParticleSystems = new List<ParticleSystem>();
#endif
        
        public ScaryObject _scaryObject;
        
        public ScrewStructureDestruction _screwStructureDestruction;
        public EventDescription _eventDescription;
        public ElectricLight _electricLight;
        public DemonDetector _demonDetector;
        public PowerFlowIndicator _powerFlowIndicator;

        private float _lightIntensityInternal;
        private float _temperatureInternal;
        private float _damageTicker;
        private bool _isBurning;
        private float _burnChainDiagTimer;
        private int _burnChainDiagCount;
        public bool _initialized = false;

        // Validated against Config logic
        private float _temperatureRisingThreshhold;
        private float _burnThreshhold;
        private float _damageThreshold;
        private float _damageAfterSecond;
        private float _LightIntensityRisePerSecond;
        private float _LightIntensityReducePerSecond;
        private float _temperatureRisePerSecond;
        private float _temperatureReducePerSecond;
        private float effigyMinRange;
        private float effigyMaxRange;
        private float effigyMinStrength;
        private float effigyMaxStrength;
        private float effigyPowerDisabledRange;
        private float effigyPowerDisabledStrength;
        private float BurnDemonEffectRangeMin;
        private float BurnDemonEffectRangeMax;
        private float BurnDemonTimePerTrigger;
        private float DemonDetectionRadiusMin;
        private float DemonDetectionRadiusMax;
        
        public static float BurnDemonAngle = 360f;

        // Reflection Fields
        private static FieldInfo f_aura;
        private static FieldInfo f_value;
        private static FieldInfo f_range;
        private static FieldInfo f_strengthMult;
        private static FieldInfo f_adjustmentsByClass;
        private static FieldInfo f_directionalDegrees;
        private static FieldInfo f_maxBurnDemonSeconds;
        private static FieldInfo f_isOn;
        private static FieldInfo f_currentHp;
        private static FieldInfo f_scaryTransform;
        private static FieldInfo f_autoFire;      // EventStimuli._autoFire
        private static FieldInfo f_autoDisable;   // EventStimuli._autoDisable
        private static FieldInfo f_autoDestruct;  // EventStimuli._autoDestruct
        private static FieldInfo f_hasFired;      // EventStimuli._hasFired

        private void InitReflection()
        {
            if (f_aura != null) return;
            f_aura = AccessTools.Field(typeof(PlayerEffigyStimuli), "_aura");
            f_value = AccessTools.Field(typeof(AuraInfluence), "_value");
            f_range = AccessTools.Field(typeof(EventDescription), "_range");
            f_strengthMult = AccessTools.Field(typeof(AuraInfluence), "_strengthMult");
            f_adjustmentsByClass = AccessTools.Field(typeof(EventDescription), "_adjustmentsByClass");
            f_directionalDegrees = AccessTools.Field(typeof(EventDescription), "_directionalDegrees");
            f_maxBurnDemonSeconds = AccessTools.Field(typeof(ScaryObject), "_maxBurnDemonSeconds");
            f_isOn = AccessTools.Field(typeof(ElectricLight), "_isOn");
            f_currentHp = AccessTools.Field(typeof(ScrewStructureDestruction), "_currentHp");
            f_scaryTransform = AccessTools.Field(typeof(ScaryObject).BaseType, "_transform"); // MonoBehaviourStimuli._transform
            f_autoFire = AccessTools.Field(typeof(EventStimuli), "_autoFire");
            f_autoDisable = AccessTools.Field(typeof(EventStimuli), "_autoDisable");
            f_autoDestruct = AccessTools.Field(typeof(EventStimuli), "_autoDestruct");
            f_hasFired = AccessTools.Field(typeof(EventStimuli), "_hasFired");
        }

        public void Initialize()
        {
            InitReflection();
            ReloadSettings();

            if (this._demonDetector == null)
            {
                this._demonDetector = base.transform.gameObject.AddComponent<DemonDetector>();
                this._demonDetector.Initialize(); // IL2CPP: Start() never fires
            }
            
            // === Effigy Stimuli Setup ===
            if (this._effigyStimuli == null)
            {
                try
                {
                    var sim = VailWorldSimulation.Instance();
                    if (sim != null)
                    {
                        PlayerEffigyStimuli playerEffigyStimuli = sim.GetPlayerEffigyStimuli();
                        if (playerEffigyStimuli)
                        {
                            this._effigyStimuli = Object.Instantiate<PlayerEffigyStimuli>(playerEffigyStimuli, base.transform);
                            this._effigyStimuli.enabled = true;
                            this._effigyStimuli.Initialize(10);
                            
                            // Get _aura -> _value chain via reflection
                            if (f_aura != null)
                                this._effigyAuraInfluence = f_aura.GetValue(this._effigyStimuli) as AuraInfluence;
                            
                            if (_effigyAuraInfluence != null)
                            {
                                if (f_value != null)
                                    this._effigyEventDescription = f_value.GetValue(this._effigyAuraInfluence) as EventDescription;
                                
                                if (_effigyEventDescription != null)
                                {
                                    // Set initial range
                                    if (f_range != null)
                                        f_range.SetValue(_effigyEventDescription, this.effigyPowerDisabledRange);
                                    
                                    // CRITICAL FIX: Add cannibals (class 2) as targets, remove initial class (1)
                                    // This is what makes the effigy actually scare enemies!
                                    // Original: _adjustmentsByClass[0]._affectsActorClasses.Add(2); .Remove(1);
                                    if (f_adjustmentsByClass != null)
                                    {
                                        try
                                        {
                                            var adjustments = f_adjustmentsByClass.GetValue(_effigyEventDescription);
                                            if (adjustments != null)
                                            {
                                                // adjustments is an Il2CppSystem.Collections.Generic.List or array
                                                // Get the first element [0]
                                                var indexer = adjustments.GetType().GetProperty("Item");
                                                if (indexer != null)
                                                {
                                                    var adj0 = indexer.GetValue(adjustments, new object[] { 0 });
                                                    if (adj0 != null)
                                                    {
                                                        var f_affects = AccessTools.Field(adj0.GetType(), "_affectsActorClasses");
                                                        if (f_affects != null)
                                                        {
                                                            var actorClassList = f_affects.GetValue(adj0);
                                                            if (actorClassList != null)
                                                            {
                                                                var addMethod = actorClassList.GetType().GetMethod("Add");
                                                                var removeMethod = actorClassList.GetType().GetMethod("Remove");
                                                                if (addMethod != null) addMethod.Invoke(actorClassList, new object[] { 2 });
                                                                if (removeMethod != null) removeMethod.Invoke(actorClassList, new object[] { 1 });
                                                                RLog.Msg("[ScaryCross] Added cannibal targeting to effigy.");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception adjEx)
                                        {
                                            RLog.Warning($"[ScaryCross] _adjustmentsByClass setup failed: {adjEx.Message}");
                                        }
                                    }
                                }
                                
                                // Set strength multiplier
                                if (f_strengthMult != null)
                                    f_strengthMult.SetValue(this._effigyAuraInfluence, this.effigyPowerDisabledStrength);
                            }
                            
                            RLog.Msg("[ScaryCross] Effigy stimuli initialized.");
                        }
                        else
                        {
                            RLog.Warning("[ScaryCross] GetPlayerEffigyStimuli() returned null.");
                        }
                    }
                    else
                    {
                        RLog.Warning("[ScaryCross] VailWorldSimulation.Instance() returned null.");
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[ScaryCross] Effigy setup failed: {ex.Message}");
                }
            }
            
            // === ScaryObject Setup ===
            if (this._scaryObject == null)
            {
                if (ScaryCrossModule._heldCrossPrefab != null)
                {
                    Transform stimuliTrans = ScaryCrossModule._heldCrossPrefab.transform.Find("Stimuli");
                    if (stimuliTrans)
                    {
                        Transform transform = Object.Instantiate<Transform>(stimuliTrans, base.transform);
                        transform.parent = base.transform;
                        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                        this._scaryObject = transform.GetComponent<ScaryObject>();
                        
                        // CRITICAL FIX: Set _transform via reflection (original: _scaryObject._transform = transform)
                        // MonoBehaviourStimuli._transform is private/protected in IL2CPP stubs
                        if (f_scaryTransform != null)
                            f_scaryTransform.SetValue(this._scaryObject, transform);
                        
                        this._eventDescription = this._scaryObject.GetDescription();
                        if (_eventDescription != null)
                        {
                            if (f_range != null)
                                f_range.SetValue(_eventDescription, this.BurnDemonEffectRangeMin);
                            if (f_directionalDegrees != null)
                                f_directionalDegrees.SetValue(_eventDescription, BurnDemonAngle);
                        }
                        
                        if (f_maxBurnDemonSeconds != null)
                            f_maxBurnDemonSeconds.SetValue(this._scaryObject, this.BurnDemonTimePerTrigger);
                        
                        
                        // CRITICAL: Prevent auto-disable/destruct so stimuli stays active
                        if (f_autoDisable != null)
                            f_autoDisable.SetValue(this._scaryObject, false);
                        if (f_autoDestruct != null)
                            f_autoDestruct.SetValue(this._scaryObject, false);
                        // _autoFire=FALSE: We control when Fire() is called.
                        // If true, OnEnable fires before enemies exist, wasting the broadcast.
                        if (f_autoFire != null)
                            f_autoFire.SetValue(this._scaryObject, false);
                        
                        // CRITICAL: Initialize the stimuli so it has a source and registers
                        // with the Vail stimuli grid. Without this, Fire() broadcasts but
                        // the AI system ignores the event because it has no source context.
                        try
                        {
                            // Use effigyStimuli as the owner (links ScaryObject to the cross's effigy)
                            if (this._effigyStimuli != null)
                            {
                                this._scaryObject.SetOwner(this._effigyStimuli);
                                this._scaryObject.Initialize(null, this._effigyStimuli);
                                RLog.Msg("[ScaryCross] ScaryObject.Initialize(null, effigyStimuli) — source linked.");
                            }
                            else
                            {
                                RLog.Warning("[ScaryCross] _effigyStimuli is null — cannot initialize ScaryObject source.");
                            }
                            
                            // Mark stimuli as valid so OnEnable registers with the grid
                            this._scaryObject.SetValid(true);
                        }
                        catch (Exception initEx)
                        {
                            RLog.Warning($"[ScaryCross] ScaryObject Initialize/SetValid error: {initEx.Message}");
                        }
                        
                        // Start disabled — BurnDemons() enables and fires when enemies detected
                        this._scaryObject.enabled = false;
                        this._scaryFireCooldown = 0f;
                        
                        RLog.Msg($"[ScaryCross] ScaryObject initialized (autoFire=false, autoDisable=false, valid={this._scaryObject.IsValid()}).");
                    }
                    else
                    {
                        RLog.Warning("[ScaryCross] 'Stimuli' child not found on held cross prefab.");
                    }
                }
                else
                {
                    RLog.Warning("[ScaryCross] _heldCrossPrefab is null — cross item not found.");
                }
            }
            
            if (this._screwStructureDestruction == null)
            {
                this._screwStructureDestruction = base.transform.GetComponent<ScrewStructureDestruction>();
            }
            if (this._electricLight == null)
            {
                this._electricLight = base.transform.GetComponent<ElectricLight>();
            }
            if (this._powerFlowIndicator == null)
            {
                this._powerFlowIndicator = base.transform.GetComponent<PowerFlowIndicator>();
            }
#if !SERVER
            if (this._fireAll == null)
            {
                if (ScaryCrossModule._bonFireElementPrefab != null)
                {
                    Transform fireAllTrans = ScaryCrossModule._bonFireElementPrefab.transform.Find("FireAll");
                    if (fireAllTrans)
                    {
                        GameObject gameObject = fireAllTrans.gameObject;
                        this._fireAll = Object.Instantiate<GameObject>(gameObject);
                        this._fireAll.transform.parent = base.transform;
                        this._fireAll.transform.localPosition = new Vector3(0f, 1f, 0f);
                        this._fireAll.SetActive(false); // Start inactive — only enabled when burning
                    }
                }
            }
            
            Transform transform2 = base.transform.Find("Renderables");
            if (transform2 != null)
            {
                for (int i = 0; i < transform2.childCount; i++)
                {
                    // IL2CPP-safe: manual child traversal instead of stripped GetComponentsInChildren<T>()
                    FindLightsRecursive(transform2.GetChild(i));
                    this._lightbulbs.Shuffle();
                }
            }
            else
            {
                RLog.Warning("[ScaryCross] Renderables child not found!");
            }
#endif
            this._damageTicker = Random.Range(0f, this._damageAfterSecond);
            
            RLog.Msg($"[ScaryCross] Start complete. ElectricLight={_electricLight != null}, Power={_powerFlowIndicator != null}, Effigy={_effigyStimuli != null}, ScaryObj={_scaryObject != null}");
            _initialized = true;
        }

#if !SERVER
        /// <summary>
        /// IL2CPP-safe recursive child traversal — replaces stripped GetComponentsInChildren.
        /// Finds Light components on children whose names start with "Light".
        /// </summary>
        private void FindLightsRecursive(Transform parent)
        {
            if (parent == null) return;
            
            if (parent.name.StartsWith("Light"))
            {
                var light = parent.GetComponent<Light>();
                if (light != null)
                {
                    _lightbulbs.Add(light);
                }
            }
            
            for (int i = 0; i < parent.childCount; i++)
            {
                FindLightsRecursive(parent.GetChild(i));
            }
        }
#endif

        /// <summary>
        /// IL2CPP does NOT call Update() on [RegisterTypeInIl2Cpp] types.
        /// This is called manually by ScaryCrossModule.OnInWorldUpdate().
        /// </summary>
        public void ManualUpdate()
        {
            this.ReloadSettings();
            
            // Tick DemonDetector (IL2CPP: Update() never fires)
            if (_demonDetector != null) _demonDetector.ManualUpdate();
            
            // Original has NO null guard here — effigy/burn logic runs regardless of power state
            bool hasPower = this.HasPower();
            if (!hasPower && _electricLight != null)
            {
                this._electricLight.ToggleState(false);
            }
            
            // AreLightsEnabled() uses f_isOn reflection which is broken in IL2CPP.
            // Use HasPower() as the primary gate — it uses the working ._hasPower property.
            bool lightsEnabled = hasPower;
            
            if (!lightsEnabled && !this._isBurning)
            {
                this.SetEffigyStrengthMultiplierOnDisabled();
                this.SetEffigyRangeOnDisabled();
            }
            else
            {
                this.SetEffigyStrengthMultiplierHeat(this._temperatureInternal);
                this.SetEffigyRangeHeat(this._temperatureInternal);
            }
            
            bool demonsInRange = this.AreDemonsInRange();
            
            // Burn chain diagnostic — log every 5 seconds (no cap — needed for persistence debugging)
            _burnChainDiagTimer += Time.deltaTime;
            if (_burnChainDiagTimer >= 5f)
            {
                _burnChainDiagTimer = 0f;
                _burnChainDiagCount++;
                RLog.Msg($"[ScaryCross] BurnChain #{_burnChainDiagCount}: power={hasPower}, pfi={_powerFlowIndicator != null}, demons={demonsInRange}, intensity={_lightIntensityInternal:F1}, temp={_temperatureInternal:F1}, burning={_isBurning}, scaryObj={_scaryObject != null}"
#if !SERVER
                    + $", fireAll={_fireAll != null}"
#endif
                );
            }
            
            if (lightsEnabled && demonsInRange)
            {
                this.BurnDemons();
                this._lightIntensityInternal = Mathf.Clamp(this._lightIntensityInternal + this._LightIntensityRisePerSecond * Time.deltaTime, 0f, 100f);
            }
            else
            {
                this.StopBurningDemons();
                this._lightIntensityInternal = Mathf.Clamp(this._lightIntensityInternal - this._LightIntensityReducePerSecond * Time.deltaTime, 0f, 100f);
            }
            
            if (this._lightIntensityInternal >= this._temperatureRisingThreshhold)
            {
                this._temperatureInternal = Mathf.Clamp(this._temperatureInternal + this._temperatureRisePerSecond * Time.deltaTime, 0f, 100f);
            }
            else
            {
                this._temperatureInternal = Mathf.Clamp(this._temperatureInternal - this._temperatureReducePerSecond * Time.deltaTime, 0f, 100f);
            }
            
            if (this._temperatureInternal >= this._burnThreshhold)
            {
                if (!this._isBurning)
                {
#if !SERVER
                    this.ToggleFire(true);
#else
                    this._isBurning = true;
#endif
                }
            }
            else if (this._isBurning && this._temperatureInternal <= this._burnThreshhold - this._temperatureReducePerSecond * 4f)
            {
#if !SERVER
                this.ToggleFire(false);
#else
                this._isBurning = false;
#endif
            }
            
            if (lightsEnabled)
            {
#if !SERVER
                this.SetLightbulbStrength(this._lightIntensityInternal);
#endif
                this.SetDemonDetectionRangeHeat(this._temperatureInternal);
            }
            else
            {
#if !SERVER
                this.TurnOffLightLightbulb();
#endif
            }
            
            this.SetBurnRange(this._temperatureInternal);
            
            if (this._isBurning)
            {
#if !SERVER
                this.SetFireIntensity(this._temperatureInternal);
#endif
                
                if (this._temperatureInternal >= this._damageThreshold)
                {
                    this._damageTicker += Time.deltaTime;
                    if (this._damageTicker > this._damageAfterSecond)
                    {
                        this.ApplyStructureDamage();
                        this._damageTicker = 0f;
                    }
                }
            }
        }

        private void ReloadSettings()
        {
            if (Config.SC_TempRiseThreshold != null) this._temperatureRisingThreshhold = Config.SC_TempRiseThreshold.Value;
            if (Config.SC_BurnThreshold != null) this._burnThreshhold = Config.SC_BurnThreshold.Value;
            if (Config.SC_DamageThreshold != null) this._damageThreshold = Config.SC_DamageThreshold.Value;
            if (Config.SC_DamageAfterSec != null) this._damageAfterSecond = Config.SC_DamageAfterSec.Value;
            if (Config.SC_LightIntensityRise != null) this._LightIntensityRisePerSecond = Config.SC_LightIntensityRise.Value;
            if (Config.SC_LightIntensityReduce != null) this._LightIntensityReducePerSecond = Config.SC_LightIntensityReduce.Value;
            if (Config.SC_TempRisePerSec != null) this._temperatureRisePerSecond = Config.SC_TempRisePerSec.Value;
            if (Config.SC_TempReducePerSec != null) this._temperatureReducePerSecond = Config.SC_TempReducePerSec.Value;
            
            if (Config.SC_EffigyMinRange != null) this.effigyMinRange = Config.SC_EffigyMinRange.Value;
            if (Config.SC_EffigyMaxRange != null) this.effigyMaxRange = Config.SC_EffigyMaxRange.Value;
            if (Config.SC_EffigyMinStrength != null) this.effigyMinStrength = Config.SC_EffigyMinStrength.Value;
            if (Config.SC_EffigyMaxStrength != null) this.effigyMaxStrength = Config.SC_EffigyMaxStrength.Value;
            if (Config.SC_EffigyDisabledRange != null) this.effigyPowerDisabledRange = Config.SC_EffigyDisabledRange.Value;
            if (Config.SC_EffigyDisabledStrength != null) this.effigyPowerDisabledStrength = Config.SC_EffigyDisabledStrength.Value;
            
            if (Config.SC_BurnDemonRangeMin != null) this.BurnDemonEffectRangeMin = Config.SC_BurnDemonRangeMin.Value;
            if (Config.SC_BurnDemonRangeMax != null) this.BurnDemonEffectRangeMax = Config.SC_BurnDemonRangeMax.Value;
            if (Config.SC_BurnDemonTimeConfig != null) this.BurnDemonTimePerTrigger = Config.SC_BurnDemonTimeConfig.Value;
            
            if (Config.SC_DemonDetectRadiusMin != null) this.DemonDetectionRadiusMin = Config.SC_DemonDetectRadiusMin.Value;
            if (Config.SC_DemonDetectRadiusMax != null) this.DemonDetectionRadiusMax = Config.SC_DemonDetectRadiusMax.Value;
        }

        public bool AreLightsEnabled()
        {
            if (_electricLight == null || f_isOn == null) return false;
            return (bool)f_isOn.GetValue(_electricLight);
        }

        public bool HasPower()
        {
            if (_powerFlowIndicator == null) return false;
            // Assuming _hasPower is public property or field. Decomp showed it as public.
            return this._powerFlowIndicator._hasPower;
        }

        public bool AreDemonsInRange()
        {
            if (_demonDetector == null) return false;
            return this._demonDetector.IsEnemyInRange;
        }

        public void SetDemonDetectionRangeHeat(float strength)
        {
            strength = Mathf.Clamp(strength, 0f, 100f);
            float num = Mathf.Lerp(this.DemonDetectionRadiusMin, this.DemonDetectionRadiusMax, strength / 100f);
            if (_demonDetector != null)
                this._demonDetector.SetTriggerRadius(num);
        }

        public void SetEffigyStrengthMultiplierOnDisabled()
        {
            if (_effigyAuraInfluence != null && f_strengthMult != null)
                f_strengthMult.SetValue(this._effigyAuraInfluence, this.effigyPowerDisabledStrength);
        }

        public void SetEffigyRangeOnDisabled()
        {
            if (_effigyEventDescription != null && f_range != null)
                f_range.SetValue(this._effigyEventDescription, this.effigyPowerDisabledRange);
        }

        public void SetEffigyStrengthMultiplierHeat(float strength)
        {
            if (_effigyAuraInfluence == null || f_strengthMult == null) return;
            strength = Mathf.Clamp(strength, 0f, 100f);
            float num = Mathf.Lerp(this.effigyMinStrength, this.effigyMaxStrength, strength / 100f);
            f_strengthMult.SetValue(this._effigyAuraInfluence, num);
        }

        public void SetEffigyRangeHeat(float strength)
        {
            if (_effigyEventDescription == null || f_range == null) return;
            strength = Mathf.Clamp(strength, 0f, 100f);
            float num = Mathf.Lerp(this.effigyMinRange, this.effigyMaxRange, strength / 100f);
            f_range.SetValue(this._effigyEventDescription, num);
        }

#if !SERVER
        public void ToggleFire(bool toggle)
        {
            if (_fireAll == null) return;

            this._fireAll.SetActive(toggle);
            this._isBurning = toggle;
            
            if (toggle)
            {
                Transform transform = this._fireAll.transform.Find("Interaction/FireSphere/BonFireParticles(Clone)/FireParticlesSourceA");
                if (transform != null)
                {
                    Transform transform2 = transform.Find("RisingFlamesA40");
                    if (transform2 != null)
                    {
                        transform2.localPosition = new Vector3(0f, -0.18f, 0f);
                    }
                    
                    var p2 = transform.Find("FireParticlesSourceA (2)");
                    if (p2) Object.Destroy(p2.gameObject);
                    
                    Transform transform3 = transform.Find("FireParticlesSourceA (3)");
                    if (transform3 != null)
                    {
                        transform3.localPosition = new Vector3(0.2f, 0.85f, 0.3f);
                        transform3.localRotation = Quaternion.Euler(0f, 320f, 0f);
                    }
                    Transform transform4 = transform.Find("FireParticlesSourceA (4)");
                    if (transform4 != null)
                    {
                        transform4.localPosition = new Vector3(0.2f, 0.85f, -0.3f);
                        transform4.localRotation = Quaternion.Euler(0f, 140f, 0f);
                    }
                    
                    this._FireParticleSystems.Clear();
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Transform child = transform.GetChild(i);
                        var ps = child.GetComponent<ParticleSystem>();
                        if (ps) this._FireParticleSystems.Add(ps);
                    }
                }
            }
            else
            {
                this._FireParticleSystems.Clear();
            }
        }

        public void SetFireIntensity(float strength)
        {
            if (this._FireParticleSystems.Count == 0) return;
            
            if (strength < this._burnThreshhold)
            {
                foreach (ParticleSystem particleSystem in this._FireParticleSystems)
                {
                    if (particleSystem != null && particleSystem.isPlaying)
                    {
                        particleSystem.Stop();
                    }
                }
            }
        }

        public void SetLightbulbStrength(float strength)
        {
            if (this._lightbulbs.Count == 0) return;
            
            strength = Mathf.Clamp(strength, 0f, 100f);
            float intensityMin = 4096f;
            float intensityMax = 32768f;
            float rangeMin = 6f;
            float rangeMax = 24f;

            float num = Mathf.Lerp(intensityMin, intensityMax, strength / 100f);
            float num2 = Mathf.Lerp(rangeMin, rangeMax, strength / 100f);
            
            int currentHp = 6; // Default max hp
            if (_screwStructureDestruction != null)
            {
                try { currentHp = this._screwStructureDestruction._currentHp; }
                catch { if (f_currentHp != null) currentHp = (int)f_currentHp.GetValue(_screwStructureDestruction); }
            }

            int maxBulbs = 0;
            switch (currentHp)
            {
                case 1: maxBulbs = 8; break;
                case 2: maxBulbs = 6; break;
                case 3: maxBulbs = 5; break;
                case 4: maxBulbs = 4; break;
                case 5: maxBulbs = 2; break;
            }
            
            for (int i = 0; i < this._lightbulbs.Count; i++)
            {
                if (this._lightbulbs[i] == null) continue;
                
                var parent = this._lightbulbs[i].transform.parent;
                var overloaded = parent ? parent.Find("LedOverloaded") : null;
                
                if (maxBulbs > 0)
                {
                    this._lightbulbs[i].intensity = 0f;
                    this._lightbulbs[i].range = 0f;
                    if (overloaded) overloaded.gameObject.SetActive(true);
                    maxBulbs--;
                }
                else
                {
                    this._lightbulbs[i].intensity = num;
                    this._lightbulbs[i].range = num2;
                    if (overloaded) overloaded.gameObject.SetActive(false);
                }
            }
        }

        public void TurnOffLightLightbulb()
        {
            for (int i = 0; i < this._lightbulbs.Count; i++)
            {
                if (this._lightbulbs[i] != null)
                {
                    this._lightbulbs[i].intensity = 0f;
                    this._lightbulbs[i].range = 0f;
                }
            }
        }
#endif

        /// <summary>
        /// How often to re-fire the ScaryObject stimuli while enemies are in range.
        /// EventStimuli.Fire() is a one-shot broadcast — it needs to be repeated
        /// so enemies that arrive after the initial fire still get affected.
        /// </summary>
        private const float SCARY_FIRE_INTERVAL = 2f;
        private float _scaryFireCooldown;
        private int _scaryFireCount; // diagnostic counter
        
        /// <summary>
        /// Track which actors have been ignited this burn cycle to avoid spam.
        /// Cleared when enemies leave range (StopBurningDemons) or periodically.
        /// </summary>
        private readonly HashSet<int> _ignitedActors = new HashSet<int>();
        
        public void BurnDemons()
        {
            if (this._scaryObject == null) return;
            
            // Ensure ScaryObject is enabled (for scare aura / effigy effects)
            if (!this._scaryObject.enabled)
                this._scaryObject.enabled = true;
            
            // Fire stimuli periodically (for scare/flee behavior)
            _scaryFireCooldown -= Time.deltaTime;
            if (_scaryFireCooldown <= 0f)
            {
                if (f_hasFired != null)
                    f_hasFired.SetValue(this._scaryObject, false);
                this._scaryObject.Fire();
                _scaryFireCooldown = SCARY_FIRE_INTERVAL;
                _scaryFireCount++;
            }
            
            // DIRECT BURN: Only ignite enemies when the cross itself is burning.
            // This ensures the cross catches fire FIRST (temperature threshold),
            // then enemies catch fire — matching the original visual sequence.
            // Each actor is ignited ONCE per burn cycle — _ignitedActors only
            // clears when enemies leave range (StopBurningDemons), not on timer.
            if (_isBurning && _demonDetector != null)
            {
                for (int i = 0; i < _demonDetector.InRangeActors.Count; i++)
                {
                    try
                    {
                        var actor = _demonDetector.InRangeActors[i];
                        if (actor == null) continue;
                        
                        int actorId = actor.GetInstanceID();
                        if (_ignitedActors.Contains(actorId)) continue;
                        
                        actor.IgniteSelf(this.BurnDemonTimePerTrigger);
                        _ignitedActors.Add(actorId);
                        
                        RLog.Msg($"[ScaryCross] IGNITE! IgniteSelf({this.BurnDemonTimePerTrigger}s) on '{actor.gameObject.name}' (typeId={actor.TypeId})");
                    }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[ScaryCross] IgniteSelf error: {ex.Message}");
                    }
                }
            }
        }
        
        /// <summary>
        /// Disables the ScaryObject when enemies leave range.
        /// Resets fire cooldown so next detection fires immediately.
        /// </summary>
        public void StopBurningDemons()
        {
            if (this._scaryObject != null && this._scaryObject.enabled)
            {
                this._scaryObject.enabled = false;
                _scaryFireCooldown = 0f; // fire immediately on next detection
                _ignitedActors.Clear();   // allow re-igniting on next approach
            }
        }

        public void SetBurnRange(float strength)
        {
            if (_eventDescription == null || f_range == null) return;
            strength = Mathf.Clamp(strength, 0f, 100f);
            float num = Mathf.Lerp(this.BurnDemonEffectRangeMin, this.BurnDemonEffectRangeMax, strength / 100f);
            f_range.SetValue(this._eventDescription, num);
        }

        public void ApplyStructureDamage()
        {
            if (_screwStructureDestruction == null || f_currentHp == null) return;
            int hp = (int)f_currentHp.GetValue(_screwStructureDestruction);
            hp--;
            f_currentHp.SetValue(_screwStructureDestruction, hp);
            if (hp <= 0)
            {
                this._screwStructureDestruction.DestroyStructure();
            }
        }
    }
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            int i = list.Count;
            while (i > 1)
            {
                i--;
                int num = Random.Range(0, i + 1);
                T t = list[i];
                list[i] = list[num];
                list[num] = t;
            }
        }
    }
}
