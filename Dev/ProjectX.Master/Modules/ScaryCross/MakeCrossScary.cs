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
        public GameObject _fireAll;
        public List<Light> _lightbulbs = new List<Light>();
        public List<ParticleSystem> _FireParticleSystems = new List<ParticleSystem>();
        
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
        }

        private void Start()
        {
            InitReflection();
            ReloadSettings();

            if (this._demonDetector == null)
            {
                this._demonDetector = base.transform.gameObject.AddComponent<DemonDetector>();
            }
            if (this._effigyStimuli == null)
            {
                try
                {
                    var vailType = typeof(VailWorldSimulation);
                    var instProp = vailType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                    if (instProp != null)
                    {
                        var rawInst = instProp.GetValue(null);
                        // If it's a Func/Delegate, invoke it
                        if (rawInst != null && rawInst.GetType().Name.Contains("Func"))
                        {
                            var invokeM = rawInst.GetType().GetMethod("Invoke");
                            if (invokeM != null) rawInst = invokeM.Invoke(rawInst, null);
                        }

                        if (rawInst != null)
                        {
                            var getStimuli = AccessTools.Method(rawInst.GetType(), "GetPlayerEffigyStimuli");
                            if (getStimuli != null)
                            {
                                var playerEffigyStimuli = getStimuli.Invoke(rawInst, null) as PlayerEffigyStimuli;
                                if (playerEffigyStimuli)
                                {
                                    this._effigyStimuli = Object.Instantiate<PlayerEffigyStimuli>(playerEffigyStimuli, base.transform);
                                    this._effigyStimuli.enabled = true;
                                    this._effigyStimuli.Initialize(10);
                                    
                                    this._effigyAuraInfluence = f_aura.GetValue(this._effigyStimuli) as AuraInfluence;
                                    if (_effigyAuraInfluence != null)
                                    {
                                        this._effigyEventDescription = f_value.GetValue(this._effigyAuraInfluence) as EventDescription;
                                        
                                        if (_effigyEventDescription != null)
                                            f_range.SetValue(_effigyEventDescription, this.effigyPowerDisabledRange);
                                            
                                        f_strengthMult.SetValue(this._effigyAuraInfluence, this.effigyPowerDisabledStrength);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception) {}
            }
            if (this._scaryObject == null)
            {
                if (ScaryCrossModule._heldCrossPrefab != null)
                {
                    // Stimuli is likely a child
                    Transform stimuliTrans = ScaryCrossModule._heldCrossPrefab.transform.Find("Stimuli");
                    if (stimuliTrans)
                    {
                        Transform transform = Object.Instantiate<Transform>(stimuliTrans, base.transform);
                        transform.parent = base.transform;
                        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                        this._scaryObject = transform.GetComponent<ScaryObject>();
                        this._scaryObject.transform.SetParent(base.transform); // Set parent instead of _transform field?
                        
                        this._eventDescription = this._scaryObject.GetDescription();
                        if (_eventDescription != null)
                        {
                            f_range.SetValue(_eventDescription, this.BurnDemonEffectRangeMin);
                            f_directionalDegrees.SetValue(_eventDescription, BurnDemonAngle);
                        }
                        f_maxBurnDemonSeconds.SetValue(this._scaryObject, this.BurnDemonTimePerTrigger);
                    }
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
                    }
                }
            }
            
            Transform transform2 = base.transform.Find("Renderables");
            if (transform2 != null)
            {
                for (int i = 0; i < transform2.childCount; i++)
                {
                    foreach (Transform transform3 in transform2.GetChild(i).GetComponentsInChildren<Transform>())
                    {
                        if (transform3.name.StartsWith("Light") && transform3.GetComponent<Light>() != null)
                        {
                            this._lightbulbs.Add(transform3.GetComponent<Light>());
                            break;
                        }
                    }
                    this._lightbulbs.Shuffle();
                }
            }
            this._damageTicker = Random.Range(0f, this._damageAfterSecond);
        }

        private void Update()
        {
            this.ReloadSettings();
            
            if (_electricLight == null || _powerFlowIndicator == null) return;

            if (!this.HasPower())
            {
                this._electricLight.ToggleState(false);
            }
            
            bool lightsEnabled = AreLightsEnabled();
            
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
            
            if (lightsEnabled && this.AreDemonsInRange())
            {
                this.BurnDemons();
                this._lightIntensityInternal = Mathf.Clamp(this._lightIntensityInternal + this._LightIntensityRisePerSecond * Time.deltaTime, 0f, 100f);
            }
            else
            {
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
                    this.ToggleFire(true);
                }
            }
            else if (this._isBurning && this._temperatureInternal <= this._burnThreshhold - this._temperatureReducePerSecond * 4f)
            {
                this.ToggleFire(false);
            }
            
            if (lightsEnabled)
            {
                this.SetLightbulbStrength(this._lightIntensityInternal);
                this.SetDemonDetectionRangeHeat(this._temperatureInternal);
            }
            else
            {
                this.TurnOffLightLightbulb();
            }
            
            this.SetBurnRange(this._temperatureInternal);
            
            if (this._isBurning)
            {
                this.SetFireIntensity(this._temperatureInternal);
                
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
            if (_electricLight == null) return false;
            if (f_isOn != null) return (bool)f_isOn.GetValue(_electricLight);
            return false;
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
                        transform3.localPosition = new Vector3(-0.2f, 0.85f, 0.3f);
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
            strength = Mathf.Clamp(strength, 0f, 100f);
            float intensityMin = 4096f;
            float intensityMax = 32768f;
            float rangeMin = 6f;
            float rangeMax = 24f;

            float num = Mathf.Lerp(intensityMin, intensityMax, strength / 100f);
            float num2 = Mathf.Lerp(rangeMin, rangeMax, strength / 100f);
            
            int currentHp = 6; // Default max hp
            if (_screwStructureDestruction != null && f_currentHp != null)
            {
                currentHp = (int)f_currentHp.GetValue(_screwStructureDestruction);
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
            foreach (Light light in this._lightbulbs)
            {
                if (light)
                {
                    light.intensity = 0f;
                    light.range = 0f;
                }
            }
        }

        public void BurnDemons()
        {
            if (this._scaryObject && !this._scaryObject.enabled)
            {
                this._scaryObject.enabled = true;
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
            if (_screwStructureDestruction && f_currentHp != null)
            {
                int hp = (int)f_currentHp.GetValue(_screwStructureDestruction);
                hp--;
                f_currentHp.SetValue(_screwStructureDestruction, hp);
                
                if (hp <= 0)
                {
                    // Assuming DestroyStructure is public method?
                    this._screwStructureDestruction.DestroyStructure();
                }
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
