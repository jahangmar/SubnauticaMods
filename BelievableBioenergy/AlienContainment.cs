using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BelievableBioenergy
{
    class AlienContainment
    {
        private const float energyPerSecond = 0.5f;

        private static Dictionary<int, float> lastTime = new Dictionary<int, float>();
        
        private static float GetLastTime(int id)
        {
            if (!lastTime.ContainsKey(id))
                SetLastTime(id);    
            
            return lastTime[id];
        }

        private static void SetLastTime(int id)
        {
            lastTime[id] = Time.time;
        }


        [HarmonyPatch(typeof(WaterPark))]
        [HarmonyPatch("Update")]
        internal class WaterparkUpdate
        {
            [HarmonyPrefix]
            public static bool Prefix(WaterPark __instance)
            {
                PowerRelay powerRelay = __instance.gameObject.GetComponentInParent<PowerRelay>();
                
                if (powerRelay != null && powerRelay.IsPowered())
                {
                    if (Time.time > GetLastTime(__instance.GetInstanceID()) + 1f)
                    {
                        powerRelay.ConsumeEnergy(energyPerSecond, out _);
                        SetLastTime(__instance.GetInstanceID());
                    }
                }
                
                return true;
            }    
        }

        [HarmonyPatch(typeof(WaterPark))]
        [HarmonyPatch ("TryBreed")]
        internal class WaterparkTryBreed
        {
            [HarmonyPrefix]
            public static bool Prefix(WaterParkCreature creature, WaterPark __instance)
            {
                PowerRelay powerRelay = __instance.gameObject.GetComponentInParent<PowerRelay>();                
                return powerRelay != null && powerRelay.IsPowered();
            }
        }

    }
}