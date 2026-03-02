using System;
using System.Globalization;
using RedLoader;
using Sons.Ai.Vail;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Modifies enemy stats based on config multipliers.
    /// Mirrors the original RaidCustomizer mod's approach:
    ///   - HP: WorldSimActor.Spawn postfix → StatManager.GetStat<HealthStat>().SetMax()
    ///   - Damage: VailActor.OnActorEnabled postfix → typed _gameSettingsDamageMultiplier field
    ///   - Aggression: VailActor.OnActorEnabled postfix → typed _gameSettingsAngerMultiplier field
    /// All use typed IL2CPP field access (NOT AccessTools.Field reflection).
    /// </summary>
    public static class StatsMultiplierModifier
    {
        /// <summary>
        /// Get health multiplier for actor type.
        /// Called from WorldSimActor.Spawn postfix as: stat.GetMax() * GetHealthMultiplier(typeId, ProjectSettings.GetEnemyHealthMultiplier(typeId))
        /// </summary>
        public static float GetHealthMultiplier(VailActorTypeId typeId, float result)
        {
            try
            {
                var classId = VailTypes.GetActorClass(typeId);
                
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
        
        /// <summary>
        /// Get damage multiplier for actor type.
        /// Called from VailActor.OnActorEnabled postfix as: __instance._gameSettingsDamageMultiplier = GetDamageMultiplier(typeId, __instance._gameSettingsDamageMultiplier)
        /// </summary>
        public static float GetDamageMultiplier(VailActorTypeId typeId, float result)
        {
            var classId = VailTypes.GetActorClass(typeId);
            
            if ((int)classId == 1) // Cannibal
                return GetConfigStat(result, RaidConfig.CannibalDamageMultiplier.Value);
            if ((int)classId == 2) // Creepy
                return GetDamageMultiplierForCreep(result, typeId);
            return result;
        }
        
        /// <summary>
        /// Get aggression multiplier for actor type.
        /// Called from VailActor.OnActorEnabled postfix as: __instance._gameSettingsAngerMultiplier = GetAggressionMultiplier(typeId, __instance._gameSettingsAngerMultiplier)
        /// </summary>
        public static float GetAggressionMultiplier(VailActorTypeId typeId, float result)
        {
            var classId = VailTypes.GetActorClass(typeId);
            
            if ((int)classId == 1) // Cannibal
                return GetConfigStat(result, RaidConfig.CannibalAggressionMultiplier.Value);
            if ((int)classId == 2) // Creepy
                return GetAggressionMultiplierForCreep(result, typeId);
            return result;
        }
        
        private static float GetHealthMultiplierForCreep(VailActorTypeId typeId, float result)
        {
            if (IsBoss(typeId))
                return GetConfigStat(result, RaidConfig.BossHealthMultiplier.Value);
            return GetConfigStat(result, RaidConfig.CreepHealthMultiplier.Value);
        }
        
        private static float GetHealthMultiplierForFollower(VailActorTypeId typeId, float result)
        {
            int id = (int)typeId;
            if (id == 10) // Virginia
                return GetConfigStat(result, RaidConfig.VirginiaHealthMultiplier.Value);
            if (id == 9 || id == 100) // Kelvin
                return GetConfigStat(result, RaidConfig.KelvinHealthMultiplier.Value);
            return result;
        }
        
        private static float GetDamageMultiplierForCreep(float result, VailActorTypeId typeId)
        {
            if (IsBoss(typeId))
                return GetConfigStat(result, RaidConfig.BossDamageMultiplier.Value);
            return GetConfigStat(result, RaidConfig.CreepDamageMultiplier.Value);
        }
        
        private static float GetAggressionMultiplierForCreep(float result, VailActorTypeId typeId)
        {
            if (IsBoss(typeId))
                return GetConfigStat(result, RaidConfig.BossAggressionMultiplier.Value);
            return GetConfigStat(result, RaidConfig.CreepAggressionMultiplier.Value);
        }
        
        /// <summary>
        /// Boss TypeIds are 45-48 (same as original mod: typeId - 45 <= 3)
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
