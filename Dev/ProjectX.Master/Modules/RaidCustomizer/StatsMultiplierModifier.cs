using System;
using System.Globalization;
using System.Reflection;
using HarmonyLib;
using RedLoader;
using Sons.Ai.Vail;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Modifies enemy stats based on config multipliers
    /// Uses reflection to access private fields for damage/aggression
    /// </summary>
    public static class StatsMultiplierModifier
    {
        // Cached reflection fields
        private static FieldInfo _damageMultiplierField;
        private static FieldInfo _aggressionMultiplierField;
        private static bool _reflectionCached = false;
        private static bool _damageFieldFound = false;
        private static bool _aggressionFieldFound = false;
        
        /// <summary>
        /// Initialize reflection cache for private fields
        /// Note: IL2CPP Reflection Paradox means most private fields are inaccessible
        /// </summary>
        public static void CacheReflection()
        {
            if (_reflectionCached) return;
            _reflectionCached = true;
            
            try
            {
                var vailActorType = typeof(VailActor);
                var allFields = vailActorType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                
                // Try to find the damage field with direct reflection
                var damageFieldNames = new[] { "_gameSettingsDamageMultiplier", "_damageOutputMultiplier", "_damageMultiplier", "damageMultiplier" };
                foreach (var name in damageFieldNames)
                {
                    _damageMultiplierField = vailActorType.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (_damageMultiplierField != null)
                    {
                        _damageFieldFound = true;
                        RLog.Msg($"[RaidCustomizer] Found damage field: {name}");
                        break;
                    }
                }
                
                // Try to find the aggression/anger field
                var aggressionNames = new[] { "_angerFloat", "_gameSettingsAngerMultiplier", "_aggressionMultiplier", "_aggression" };
                foreach (var name in aggressionNames)
                {
                    _aggressionMultiplierField = vailActorType.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (_aggressionMultiplierField != null)
                    {
                        _aggressionFieldFound = true;
                        RLog.Msg($"[RaidCustomizer] Found aggression field: {name}");
                        break;
                    }
                }
                
                // Only log if neither field was found (expected due to IL2CPP Reflection Paradox)
                if (!_damageFieldFound && !_aggressionFieldFound)
                {
                    RLog.Msg("[RaidCustomizer] Stat multiplier fields not accessible (IL2CPP limitation)");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Reflection cache failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Apply damage and aggression multipliers to an actor
        /// </summary>
        public static void ApplyMultipliers(VailActor actor)
        {
            if (actor == null) return;
            
            try
            {
                CacheReflection();
                
                var typeId = actor.TypeId;
                var classId = VailTypes.GetActorClass(typeId);
                
                // Only apply to enemies (Cannibals=1, Creeps=2)
                int classInt = (int)classId;
                if (classInt != 1 && classInt != 2) return;
                
                // Apply damage multiplier
                if (_damageFieldFound && _damageMultiplierField != null)
                {
                    float damageMultiplier = GetDamageMultiplier(typeId);
                    if (Math.Abs(damageMultiplier - 1.0f) > 0.01f)
                    {
                        try
                        {
                            _damageMultiplierField.SetValue(actor, damageMultiplier);
                        }
                        catch { }
                    }
                }
                
                // Apply aggression multiplier
                if (_aggressionFieldFound && _aggressionMultiplierField != null)
                {
                    float aggressionMultiplier = GetAggressionMultiplier(typeId);
                    if (Math.Abs(aggressionMultiplier - 1.0f) > 0.01f)
                    {
                        try
                        {
                            _aggressionMultiplierField.SetValue(actor, aggressionMultiplier);
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }
        
        private static float GetDamageMultiplier(VailActorTypeId typeId)
        {
            var classId = VailTypes.GetActorClass(typeId);
            int classInt = (int)classId;
            
            if (classInt == 1) // Cannibal
            {
                return RaidConfig.CannibalDamageMultiplier.Value;
            }
            else if (classInt == 2) // Creepy
            {
                if (IsBoss(typeId))
                    return RaidConfig.BossDamageMultiplier.Value;
                else
                    return RaidConfig.CreepDamageMultiplier.Value;
            }
            
            return 1.0f;
        }
        
        private static float GetAggressionMultiplier(VailActorTypeId typeId)
        {
            var classId = VailTypes.GetActorClass(typeId);
            int classInt = (int)classId;
            
            if (classInt == 1) // Cannibal
            {
                return RaidConfig.CannibalAggressionMultiplier.Value;
            }
            else if (classInt == 2) // Creepy
            {
                if (IsBoss(typeId))
                    return RaidConfig.BossAggressionMultiplier.Value;
                else
                    return RaidConfig.CreepAggressionMultiplier.Value;
            }
            
            return 1.0f;
        }
        
        /// <summary>
        /// Get health multiplier for actor type
        /// </summary>
        public static float GetHealthMultiplier(VailActorTypeId typeId, float result)
        {
            try
            {
                var classId = VailTypes.GetActorClass(typeId);
                
                // VailActorClassId: 1=Cannibal, 2=Creepy, 3=Animal, 4=Follower
                switch ((int)classId)
                {
                    case 1: // Cannibal
                        return GetConfigStat(result, RaidConfig.CannibalHealthMultiplier.Value);
                    case 2: // Creepy
                        return GetHealthMultiplierForCreep(typeId, result);
                    case 4: // Follower (Kelvin/Virginia)
                        return GetHealthMultiplierForFollower(typeId, result);
                    default:
                        return result;
                }
            }
            catch
            {
                return result;
            }
        }
        
        private static float GetHealthMultiplierForCreep(VailActorTypeId typeId, float result)
        {
            if (IsBoss(typeId))
                return GetConfigStat(result, RaidConfig.BossHealthMultiplier.Value);
            return GetConfigStat(result, RaidConfig.CreepHealthMultiplier.Value);
        }
        
        private static float GetHealthMultiplierForFollower(VailActorTypeId typeId, float result)
        {
            // TypeId 9 = Kelvin, 10 = Virginia, 100 = Kelvin (alternate?)
            int id = (int)typeId;
            if (id == 10)
                return GetConfigStat(result, RaidConfig.VirginiaHealthMultiplier.Value);
            if (id == 9 || id == 100)
                return GetConfigStat(result, RaidConfig.KelvinHealthMultiplier.Value);
            return result;
        }
        
        /// <summary>
        /// Boss TypeIds are 45-48
        /// </summary>
        private static bool IsBoss(VailActorTypeId typeId)
        {
            int id = (int)typeId;
            return id >= 45 && id <= 48;
        }
        
        private static float GetConfigStat(float result, float configValue)
        {
            if (Math.Abs(configValue - 1.0f) < 0.01f)
                return result;
            
            return configValue;
        }
    }
}
